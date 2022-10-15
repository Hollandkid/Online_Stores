using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using OnlineStore1.Utility;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _dBContext;
        private readonly TestClass _testClass;

        public TestController(ApplicationDbContext dBContext, TestClass testClass)
        {
            _dBContext = dBContext;
            _testClass = testClass;
        }
        public void NetworkThrottingAsync()
        {
            var driver = new ChromeDriver("selenium-webdriver");

            var networkConditions = new OpenQA.Selenium.Chromium.ChromiumNetworkConditions();
            networkConditions.Latency = new TimeSpan(0);
            networkConditions.IsOffline = false;
            networkConditions.DownloadThroughput = 130 * 1024;
            networkConditions.UploadThroughput = 150 * 1024;
            driver.NetworkConditions = networkConditions;

        }

        public ActionResult RetriveImage(int id, long delta)
        {
            NetworkThrottingAsync();
            long ticksPerSec = TimeSpan.TicksPerSecond;
            long currentTick = DateTime.Now.Ticks;

            long tickDelta = currentTick - delta;
            long ticksPer2Sec = ticksPerSec * 4;
            if (tickDelta >= ticksPer2Sec || tickDelta < 0)
            {
                return NotFound();
            }

            var image = _testClass.RetriveImage(id);
            return File(image, "image/png");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
