using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NTWebApp.DBAccess;

namespace NTWebApp.UIClasses
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
