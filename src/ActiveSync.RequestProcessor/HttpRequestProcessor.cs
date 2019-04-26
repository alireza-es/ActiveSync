using ActiveSync.Core.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ActiveSync.Core.DeviceManagement;
using ActiveSync.Core.Injection;
using ActiveSync.SyncContract.Service;
using ASWBXML = ActiveSync.RequestProcessor.WBXML.ASWBXML;

namespace ActiveSync.RequestProcessor
{
    public class HttpRequestProcessor
    {
        const string PorotocolsVersion = "14.0, 14.1";
        private eRequestMethod Method { get; set; }

        private eRequestCommand Command { get; set; }

        private string RequestContent { get; set; }

        private UserDevice Device { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }

        public HttpRequestProcessor(HttpRequestMessage requestMessage)
        {
            #region Init Authentication Service

            AuthenticationService = ServiceResolver.GetService<IAuthenticationService>();

            #endregion

            #region Init Request

            switch (requestMessage.Method.Method.ToUpper())
            {
                case "POST":
                    this.Method = eRequestMethod.Post;
                    break;
                case "OPTIONS":
                    this.Method = eRequestMethod.Options;
                    break;
                default:
                    //TODO: Throw Invalid HttpMethod Name
                    break;
            }

            this.RequestContent = GetRequestContent(requestMessage.Content);

            var uri = requestMessage.RequestUri;
            var queryStrings = HttpUtility.ParseQueryString(requestMessage.RequestUri.Query);

            #region Command

            eRequestCommand command;
            if (Enum.TryParse(queryStrings["Cmd"], true, out command))
            {
                this.Command = command;
            }
            else
            {
            }


            #endregion

            #region Device

            this.Device = new UserDevice
            {
                DeviceId = queryStrings["DeviceId"],
                DeviceType = queryStrings["DeviceType"],
                User = queryStrings["User"]
            };

            #endregion

            //TODO: Check if Cmd QueryString is exists
            #region User Credential

            if (requestMessage.Headers.Authorization.Scheme == "Basic")
            {
                var basicUserPass = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(requestMessage.Headers.Authorization.Parameter));
                var splitedUserPass = basicUserPass.Split(':');
                this.Device.Credential = new UserCredential
                {
                    UserName = splitedUserPass[0],
                    Password = splitedUserPass[1]
                };
            }
            else
            {
                //TODO: return Error Message: Not Supported Auhtorization Type
            }

            #endregion

            #endregion
        }
        public HttpResponseMessage Process()
        {
            switch (this.Method)
            {
                case eRequestMethod.Post:
                    return ProcessCommand();

                case eRequestMethod.Options:
                    return ProcessOptions();
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private HttpResponseMessage ProcessOptions()
        {
            try
            {
                if (Device.Credential == null)
                    return new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

                var isAuthenticated = AuthenticationService.Authenticate(Device.Credential.UserName,
                    Device.Credential.Password);
                if (!isAuthenticated)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("MS-ASProtocolVersions", PorotocolsVersion);
            response.Headers.Add("MS-ASProtocolCommands", RequestHandlerFactory.GetSupportingCommands());

            return response;
        }

        private HttpResponseMessage ProcessCommand()
        {
            var requestHandler = RequestHandlerFactory.GetHandler(this.Command);

            if (requestHandler != null)
            {
                requestHandler.Initialize(this.Device, this.RequestContent);
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                var responseObj = requestHandler.Handle();
                var responseXml = responseObj.GetAsXML();

                var encoder = new ASWBXML();
                encoder.LoadXml(responseXml);
                var wbxmlContent = encoder.GetBytes();

                responseMessage.Content = new ByteArrayContent(wbxmlContent);
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-sync.wbxml");

                return responseMessage;
            }
            else
            {
                //TODO: Return appropriate ResponseMessage
                //throw new InvalidCommandException(string.Format("Command {0} is not supported", this.Command.ToString()));
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            //TODO: Resturn Appropriate Response
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        private string GetRequestContent(HttpContent requestContent)
        {
            if (requestContent == null)
                return string.Empty;

            var requestContentBytes = requestContent.ReadAsByteArrayAsync().Result;

            if (requestContentBytes == null || requestContentBytes.Length == 0)
                return string.Empty;

            var decoder = new ASWBXML();
            decoder.LoadBytes(requestContentBytes);

            return decoder.GetXml();
        }
    }
}
