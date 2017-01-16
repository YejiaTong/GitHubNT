using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - ManageExpenseCateg ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.InvoiceManager
{
    public class ManageExpenseCategViewModel
    {
        public List<ExpenseCategViewModel> ExistingExpenseCategs { get; set; }

        public List<ExpenseCategViewModel> NewExpenseCategs { get; set; }
    }
}
