using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleASBSender.Models;
using SampleASBSender.Services;
using SampleASBShared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SampleASBSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureServiceBus _azureServiceBus;

        public HomeController(ILogger<HomeController> logger, IAzureServiceBus azureServiceBus )
        {
            _logger = logger;
            _azureServiceBus = azureServiceBus; 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Person person)
        {
            // send the message to the Queue and give the queue Name 
            await _azureServiceBus.SendMessageAsync(person, "personqueue");

            return RedirectToAction("Privacy");
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
