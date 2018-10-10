using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseManager.Controllers
{
    public class ExpenseController : Controller
    {
        ExpensesDataAcessLayer objExpense = new ExpensesDataAcessLayer();

        // GET: /<controller>/
        public IActionResult Index(string searchString)
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            lstEmployee = objExpense.GetAllExpenses().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                lstEmployee = objExpense.GetSearchResult(searchString).ToList();
            }

            return View(lstEmployee);
        }

        public ActionResult AddEditExpenses(int itemId)
        {
            ExpenseReport model = new ExpenseReport();

            if (itemId > 0)
            {
                model = objExpense.GetExpenseData(itemId);
            }

            return PartialView("_expenseForm", model);
        }

        [HttpPost]
        public ActionResult Create(ExpenseReport newExpense)
        {
            if (ModelState.IsValid)
            {
                if (newExpense.ItemId > 0)
                {
                    objExpense.UpdateExpense(newExpense);
                }
                else
                {
                    objExpense.AddExpense(newExpense);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            objExpense.DeleteExpense(id);

            return RedirectToAction("Index");
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public JsonResult GetMonthlyExpense()
        {
            Dictionary<string, decimal> monthlyExpense = objExpense.CalculateMonthlyExpense();

            return new JsonResult(monthlyExpense);
        }

        public JsonResult GetWeeklyExpense()
        {
            Dictionary<string, decimal> weeklyExpense = objExpense.CalculateWeeklyExpense();

            return new JsonResult(weeklyExpense);
        }
    }
}
