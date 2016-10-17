using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NTWebApp.DBAccess;

namespace NTWebApp.UIClasses
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
