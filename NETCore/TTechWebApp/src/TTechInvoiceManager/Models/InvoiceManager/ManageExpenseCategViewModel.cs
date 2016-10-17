using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class ManageExpenseCategViewModel
    {
        public List<ExpenseCategViewModel> ExistingExpenseCategs { get; set; }

        public List<ExpenseCategViewModel> NewExpenseCategs { get; set; }
    }
}
