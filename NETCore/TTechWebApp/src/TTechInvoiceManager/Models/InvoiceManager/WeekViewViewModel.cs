﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTWebApp.Models.InvoiceManager
{
    public class WeekViewItemViewModel
    {
        public int UserId { get; set; }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int OrderVal { get; set; }

        public double TotalCost { get; set; }
    }

    public class WeekViewViewModel
    {
        public List<WeekViewItemViewModel> WeekViewItems { get; set; }

        public WeekViewPagerViewModel Pager { get; set; }

        public double WeekTotalCost { get; set; } = 0.0;
    }
}
