using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - AddExpense ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.InvoiceManager
{
    public class AddExpenseViewModel
    {
        public List<ExpenseCategViewModel> ExpenseCategs { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; }

        public ExpenseViewModel NewExpense { get; set; }
    }
}
