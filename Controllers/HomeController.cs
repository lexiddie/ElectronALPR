using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ElectronALPR.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElectronALPR.Models;
using ElectronALPR.ViewModels;
using Newtonsoft.Json;

namespace ElectronALPR.Controllers
{
      public class HomeController : Controller
      {
            private static readonly HttpClient Client = new HttpClient();

            // Replace your own OpenALPR API Key from your registered account
            private const string SecretKey = "sk_13cac913e4a817f1e5d66efe";
            private const string ApiUrl = "https://api.openalpr.com/v3/recognize_bytes?recognize_vehicle=1&country=th&secret_key=";

            private readonly ILogger<HomeController> _logger;

            public HomeController(ILogger<HomeController> logger)
            {
                  _logger = logger;
            }

            public IActionResult Index()
            {
                  return View();
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

            [HttpPost]
            [Route("UploadImageBase")]
            public async Task<IActionResult> UploadImageBase(string base64String)
            {
                  var originalBase = base64String.Substring(base64String.IndexOf('|') + 1);
                  var base64 = originalBase.Substring(originalBase.IndexOf(',') + 1);
                  var content = new StringContent(base64);
                  var response = await Client.PostAsync(ApiUrl + SecretKey, content).ConfigureAwait(false);
                  var res = await response.Content.ReadAsStringAsync();
                  var data = JsonConvert.DeserializeObject<CarDto>(res);
                  var car = LoadCar(data);
                  var viewModel = new CarViewModel
                  {
                        Base64String = base64String,
                        Car = car
                  };
                  return PartialView("_DisplayResult", viewModel);
            }

            private static Car LoadCar(CarDto item)
            {
                  var car = new Car();
                  if (item == null) return null;
                  car.Plate = item.Results[0].Plate;
                  car.Region = item.Results[0].Region;
                  car.Color = item.Results[0].Vehicle.color[0].Name;
                  car.Make = item.Results[0].Vehicle.make[0].Name;
                  car.BodyType = item.Results[0].Vehicle.body_type[0].Name;
                  car.Year = item.Results[0].Vehicle.year[0].Name;
                  car.MakeModel = item.Results[0].Vehicle.make_model[0].Name;
                  return car;
            }

            public IActionResult ErrorPage()
            {
                  return PartialView("ErrorPage");
            }
      }
}