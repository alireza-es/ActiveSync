using ActiveSync.Core.Requests;
using ActiveSync.RequestProcessor;
using ActiveSync.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveSync.Tests
{
    [TestClass]
    public class ItemEstimateSyncTest: BaseTest
    {
        [TestMethod]
        public void GetItemEstimate_Success()
        {
            var request = CreateCommandRequest(eRequestCommand.GetItemEstimate, "GetItemEstimate");

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(response.Content);
        }
    }
}
