using Ict;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sci.Production.MercuryDownloadStickerQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.MercuryDownloadStickerQueue.Tests
{
    [TestClass()]
    public class MercuryDownloadStickerQueueTests
    {
        [TestMethod()]
        public void CreateTempShippingMarkPicTest()
        {
            MercuryDownloadStickerQueue mercury = new MercuryDownloadStickerQueue();
            byte[] pdfImage;
            DualResult result = mercury.CreateTempShippingMarkPic("9927", "D:\\aa\\Mercury\\", true, out pdfImage);
            Assert.Fail();
        }
    }
}