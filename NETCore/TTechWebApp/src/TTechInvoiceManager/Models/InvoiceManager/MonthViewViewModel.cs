using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class MonthViewItemViewModel
    {
        public int UserId { get; set; }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int OrderVal { get; set; }

        [DataType(DataType.Currency)]
        public double TotalCost { get; set; }
    }

    public class MonthViewViewModel
    {
        public List<MonthViewItemViewModel> MonthViewItems { get; set; }

        public MonthViewPagerViewModel Pager { get; set; }

        [DataType(DataType.Currency)]
        public double MonthTotalCost { get; set; } = 0.0;
    }
}
