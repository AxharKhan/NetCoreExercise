using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetCoreExercise.Models;
using NetCoreExercise.Repositories;
using System.Diagnostics;

namespace NetCoreExercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepository _customers;

        public HomeController(ILogger<HomeController> logger, ICustomerRepository customer)
        {
            _logger = logger;
            _customers = customer;
        }

        #region Partial View Methods

        public async Task<PartialViewResult> GetEditModal(int Id)
        {
            var customer = await _customers.GetByID(Id);

            if (customer is not null)
            {
                ViewBag.CustomerTypeList = await _customers.GetAllTypesSelectList(Id);

                return PartialView("_EditModal", customer);
            }

            return PartialView("_EditModal", null);
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerTypeList = await _customers.GetAllTypesSelectList();

            return View();
        }

        public async Task<IActionResult> AddType(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                CustomerType customerType = new()
                {
                    Name = typeName,
                };

                customerType = await _customers.InsertType(customerType);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}