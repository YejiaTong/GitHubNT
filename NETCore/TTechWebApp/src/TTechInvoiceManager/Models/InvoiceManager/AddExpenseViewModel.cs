using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class AddExpenseViewModel
    {
        public List<ExpenseCategViewModel> ExpenseCategs { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; }

        public ExpenseViewModel NewExpense { get; set; }
    }
}
