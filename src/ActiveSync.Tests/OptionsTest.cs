using System;
using System.Net.Http;
using ActiveSync.RequestProcessor;
using ActiveSync.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveSync.Tests
{
    [TestClass]
    public class OptionTest : BaseTest
    {
        private const string hostAddress = "http://localhost/";

        [TestMethod]
        public void Options_Success()
        {
            var url = string.Format("{0}Microsoft-Server-ActiveSync", hostAddress);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Options
            };

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Headers.Contains("MS-ASProtocolVersions"));
            Assert.IsTrue(response.Headers.Contains("MS-ASProtocolCommands"));
        }
    }
}
