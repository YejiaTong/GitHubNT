using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - AllExpense ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.InvoiceManager
{
    public class AllExpenseViewModel
    {
        public List<ExpenseViewModel> Expenses { get; set; }

        public AllExpensePagerViewModel Pager { get; set; }
    }
}
