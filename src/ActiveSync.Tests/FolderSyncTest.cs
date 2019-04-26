using ActiveSync.Core.Requests;
using ActiveSync.RequestProcessor;
using ActiveSync.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveSync.Tests
{
    [TestClass]
    public class FolderSyncTest : BaseTest
    {
        [TestMethod]
        public void FolderCreate_Success()
        {
            var request = CreateCommandRequest(eRequestCommand.FolderCreate, "FolderSync");

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void FolderDelete_Success()
        {
            var request = CreateCommandRequest(eRequestCommand.FolderDelete, "FolderSync");

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void FolderUpdate_Success()
        {
            var request = CreateCommandRequest(eRequestCommand.FolderUpdate, "FolderSync");

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(response.Content);
        }
        [TestMethod]
        public void Ping()
        {
            var request = CreateCommandRequest(eRequestCommand.Ping, "FolderSync");

            var requestProcessor = new HttpRequestProcessor(request);
            var response = requestProcessor.Process();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(response.Content);
        }
    }
}
