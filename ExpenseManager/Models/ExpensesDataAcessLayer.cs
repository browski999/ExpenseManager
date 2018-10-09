using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseManager.Models
{
    public class ExpensesDataAcessLayer
    {
        ExpenseDBContext db = new ExpenseDBContext();

        public IEnumerable<ExpenseReport> GetAllExpenses()
        {
            try
            {
                return db.ExpenseReport.ToList();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ExpenseReport> GetSearchResult(string searchString)
        {
            List<ExpenseReport> exp = new List<ExpenseReport>();

            try
            {
                exp = GetAllExpenses().ToList();

                return exp.Where(x => x.ItemName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            }
            catch
            {
                throw;
            }
        }

        public void AddExpense(ExpenseReport expense)
        {
            try
            {
                db.ExpenseReport.Add(expense);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public int UpdateExpense(ExpenseReport expense)
        {
            try
            {
                db.Entry(expense).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public ExpenseReport GetExpenseData(int id)
        {
            try
            {
                ExpenseReport expense = db.ExpenseReport.Find(id);

                return expense;
            }
            catch
            {
                throw;
            }
        }

        public void DeleteExpense(int id)
        {
            try
            {
                ExpenseReport expense = db.ExpenseReport.Find(id);

                db.ExpenseReport.Remove(expense);

                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<string, decimal> CalculateMonthlyExpense()
        {
            ExpensesDataAcessLayer objExpense = new ExpensesDataAcessLayer();

            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictMonthlySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReport.Where(cat => cat.Category == "Food" && (cat.ExpenseDate >
            DateTime.Now.AddMonths(-7)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal shoppingSum = db.ExpenseReport.Where(cat => cat.Category == "Shopping" && (cat.ExpenseDate >
            DateTime.Now.AddMonths(-7)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal travelSum = db.ExpenseReport.Where(cat => cat.Category == "Travel" && (cat.ExpenseDate >
            DateTime.Now.AddMonths(-7)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal healthSum = db.ExpenseReport.Where(cat => cat.Category == "Health" && (cat.ExpenseDate >
            DateTime.Now.AddMonths(-7)))
            .Select(cat => cat.Amount)
            .Sum();

            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Travel", travelSum);
            dictMonthlySum.Add("Health", healthSum);

            return dictMonthlySum;
        }

        public Dictionary<string, decimal> CalculateWeeklyExpense()
        {
            ExpensesDataAcessLayer objExpense = new ExpensesDataAcessLayer();

            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReport.Where(cat => cat.Category == "Food" && (cat.ExpenseDate >
            DateTime.Now.AddDays(-7)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal shoppingSum = db.ExpenseReport.Where(cat => cat.Category == "Shopping" && (cat.ExpenseDate >
            DateTime.Now.AddDays(-28)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal travelSum = db.ExpenseReport.Where(cat => cat.Category == "Travel" && (cat.ExpenseDate >
            DateTime.Now.AddDays(-28)))
            .Select(cat => cat.Amount)
            .Sum();

            decimal healthSum = db.ExpenseReport.Where(cat => cat.Category == "Health" && (cat.ExpenseDate >
            DateTime.Now.AddDays(-28)))
            .Select(cat => cat.Amount)
            .Sum();

            dictWeeklySum.Add("Food", foodSum);
            dictWeeklySum.Add("Shopping", shoppingSum);
            dictWeeklySum.Add("Travel", travelSum);
            dictWeeklySum.Add("Health", healthSum);

            return dictWeeklySum;

        }
    }
}
