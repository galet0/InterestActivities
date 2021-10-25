using Microsoft.AspNetCore.Mvc;
using MyFirstMVCProject.Models;

namespace MyFirstMVCProject.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Brand"] = "BMW";
            ViewData["Model"] = "X3";

            Car car = new Car()
            {
                Brand = "BMW",
                Model = "X5"
            };

            return View(car);
        }

        public string Welcome(string name, int id)
        {
            return $"Welcome to my page, {name}!ID = {id}";
        }
    }
}
