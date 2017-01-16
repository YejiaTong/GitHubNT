using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for Monthly Expense Summary
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.UIClasses
{
    public class UIMonthExpense : MonthExpense
    {
        public UIMonthExpense()
            : base() { }

        public UIMonthExpense(MonthExpense monthExpense)
        {
            this.UserId = monthExpense.UserId;
            this.ExpenseCategId = monthExpense.ExpenseCategId;
            this.ExpenseCategName = monthExpense.ExpenseCategName;
            this.OrderVal = monthExpense.OrderVal;
            this.TotalCost = monthExpense.TotalCost;
        }
    }
}
