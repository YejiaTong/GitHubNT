﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - WeekView ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.InvoiceManager
{
    public class WeekViewItemViewModel
    {
        public int UserId { get; set; }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int OrderVal { get; set; }

        [DataType(DataType.Currency)]
        public double TotalCost { get; set; }
    }

    public class WeekViewViewModel
    {
        public List<WeekViewItemViewModel> WeekViewItems { get; set; }

        public WeekViewPagerViewModel Pager { get; set; }

        [DataType(DataType.Currency)]
        public double WeekTotalCost { get; set; } = 0.0;
    }
}