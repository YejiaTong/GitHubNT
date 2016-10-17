using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class ExpenseCategViewModel
    {
        public int ExpenseCategId { get; set; }

        [DisplayName("Expense Category")]
        [ExpenseCategName]
        public string ExpenseCategName { get; set; }

        public int UserId { get; set; }

        public int IsDefault { get; set; }

        [DisplayName("Display Order")]
        [ExpenseOrderVal]
        public int OrderVal { get; set; }
    }
}
