using System;
using System.IO;
using System.Net.Http;
using System.Xml;
using ActiveSync.Core.Requests;
using ActiveSync.RequestProcessor.WBXML;

namespace ActiveSync.Tests.Base
{
    public class BaseTest
    {
        const string _accessToken = "accessToken";
        private const string hostAddress = "http://localhost/";

        public BaseTest()
        {
            //TODO: Start Autofac
            //Autofac().Start();
        }

        private byte[] LoadXmlFileAsWBXML(string filePath)
        {
            var xml = LoadXmlFile(filePath);

            var encoder = new ASWBXML();
            encoder.LoadXml(xml);
            return encoder.GetBytes();
        }
        private string LoadXmlFile(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            string retXmlString;

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                retXmlString = stringWriter.GetStringBuilder().ToString();
            }


            return retXmlString;

        }
        protected HttpRequestMessage CreateCommandRequest(eRequestCommand command, string dataContainingFolder, string dataFileName = null)
        {
            if (dataFileName == null)
                dataFileName = string.Format("{0}Request.xml", command);

            var url = string.Format("{0}Microsoft-Server-ActiveSync?Cmd={1}", hostAddress, command);
            var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post
                };

            request.Headers.Add("Authorization", "Bearer " + _accessToken);

            //TODO: Add DeviceInfo

            //TODO: Add Request Header

            //Add Request Content
            var xmlRequestPath = string.Format("TestData/{0}/{1}", dataContainingFolder, dataFileName);
            var wbxmlContent = LoadXmlFileAsWBXML(xmlRequestPath);
            request.Content = new ByteArrayContent(wbxmlContent);

            return request;
        }
    }
}
