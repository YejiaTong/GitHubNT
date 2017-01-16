using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for Weekly Expense Summary
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.UIClasses
{
    public class UIWeekExpense : WeekExpense
    {
        public UIWeekExpense()
            : base() { }

        public UIWeekExpense(WeekExpense weekExpense)
        {
            this.UserId = weekExpense.UserId;
            this.ExpenseCategId = weekExpense.ExpenseCategId;
            this.ExpenseCategName = weekExpense.ExpenseCategName;
            this.OrderVal = weekExpense.OrderVal;
            this.TotalCost = weekExpense.TotalCost;
        }
    }
}
