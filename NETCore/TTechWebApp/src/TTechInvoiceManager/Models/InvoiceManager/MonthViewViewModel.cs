using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTWebApp.Models.InvoiceManager
{
    public class MonthViewItemViewModel
    {
        public int UserId { get; set; }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int OrderVal { get; set; }

        public double TotalCost { get; set; }
    }

    public class MonthViewViewModel
    {
        public List<MonthViewItemViewModel> MonthViewItems { get; set; }

        public MonthViewPagerViewModel Pager { get; set; }

        public double MonthTotalCost { get; set; } = 0.0;
    }
}
