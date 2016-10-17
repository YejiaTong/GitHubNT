using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }

        public int ExpenseCategId { get; set; }

        public int UserId { get; set; }

        [DisplayName("Name")]
        [ExpenseName]
        public string Name { get; set; }

        [DisplayName("Cost")]
        [DataType(DataType.Currency)]
        [ExpenseCost]
        public double Cost { get; set; } = 0.0;

        public string Address { get; set; } = "Current Location";

        [DisplayName("Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public ExpenseCategViewModel ExpenseCateg { get; set; }
    }
}
