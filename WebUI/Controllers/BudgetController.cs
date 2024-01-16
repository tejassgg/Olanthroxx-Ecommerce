using Models.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebAPI.Helpers;

namespace WebUI.Controllers
{
    public class BudgetController : Controller
    {
        HttpClient hc = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"].ToString())
        };

        public ActionResult Index()
        {
            BudgetData objBudget = new BudgetData();
            objBudget = GetAllBudgetData();
            return View(objBudget);
        }

        public PartialViewResult AddBudget()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddBudget(Budget budget)
        {
            budget.UserID = User.Identity.Name;

            var responseTask = hc.PostAsJsonAsync<Budget>("API/Budget/AddBudget", budget);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult AddExpense()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            expense.UserID = User.Identity.Name;

            var responseTask = hc.PostAsJsonAsync<Expense>("API/Budget/AddExpense", expense);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public BudgetData GetAllBudgetData()
        {
            BudgetData objBudget = new BudgetData();

            if (User.Identity.IsAuthenticated)
            {
                var rsTask = hc.GetAsync("API/Budget/GetAllBudgetData/" + User.Identity.Name);
                rsTask.Wait();
                var result = rsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readResult = result.Content.ReadAsAsync<BudgetData>();
                    readResult.Wait();
                    objBudget = readResult.Result;
                }
            }
            return objBudget;
        }
    }
}