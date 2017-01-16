using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - ExpenseCateg ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.InvoiceManager
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
