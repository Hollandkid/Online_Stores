using Microsoft.AspNetCore.Hosting;
using OnlineStore1.Data;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Utility
{
    public class TestClass
    {
        private readonly ApplicationDbContext _dBContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestClass(ApplicationDbContext dBContext, IWebHostEnvironment hostingEnvironment)
        {
            _dBContext = dBContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public byte[] RetriveImage(int Id)
        {
            //NetworkThrottingAsync();

            var model = _dBContext.Products.FirstOrDefault(c => c.Id == Id);

            string path = Path.Combine(_hostingEnvironment.WebRootPath, model.Image);
            byte[] imageArray = File.ReadAllBytes(path);
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return imageArray;
        }
        public void NetworkThrottingAsync()
        {
            var driver = new ChromeDriver("selenium-webdriver");

            var networkConditions = new OpenQA.Selenium.Chromium.ChromiumNetworkConditions();
            networkConditions.Latency = new TimeSpan(150);
            networkConditions.IsOffline = false;
            networkConditions.DownloadThroughput = 3 * 1024;
            networkConditions.UploadThroughput = 5 * 1024;
            driver.NetworkConditions = networkConditions;

        }
    }
}
