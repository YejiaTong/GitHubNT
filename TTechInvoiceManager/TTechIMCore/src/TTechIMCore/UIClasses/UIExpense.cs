using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for Expenses
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.UIClasses
{
    public class UIExpense : Expense
    {
        public UIExpense()
            : base()
        {
            Address = "Current Location";
            Time = DateTime.UtcNow;
        }

        public UIExpense(Expense expense)
        {
            this.ExpenseId = expense.ExpenseId;
            this.ExpenseCategId = expense.ExpenseCategId;
            this.UserId = expense.UserId;
            this.Name = expense.Name;
            this.Cost = expense.Cost;
            this.Address = expense.Address;
            this.Time = expense.Time;
            this.Description = expense.Description;
        }
    }
}
