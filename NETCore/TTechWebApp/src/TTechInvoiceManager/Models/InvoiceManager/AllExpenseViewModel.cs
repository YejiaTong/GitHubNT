using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class AllExpenseViewModel
    {
        public List<ExpenseViewModel> Expenses { get; set; }

        public AllExpensePagerViewModel Pager { get; set; }
    }
}
