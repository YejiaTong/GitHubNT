using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NTWebApp.DBAccess;

namespace NTWebApp.UIClasses
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
