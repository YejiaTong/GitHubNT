using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

namespace TTechIMCore.UIClasses
{
    public class UIExpenseCateg : ExpenseCateg
    {
        public UIExpenseCateg()
            : base() { }

        public UIExpenseCateg(ExpenseCateg expenseCateg)
        {
            this.ExpenseCategId = expenseCateg.ExpenseCategId;
            this.ExpenseCategName = expenseCateg.ExpenseCategName;
            this.UserId = expenseCateg.UserId;
            this.IsDefault = expenseCateg.IsDefault;
            this.OrderVal = expenseCateg.OrderVal;
        }

        public void Assign(ExpenseCateg expenseCateg)
        {
            this.ExpenseCategId = expenseCateg.ExpenseCategId;
            this.ExpenseCategName = expenseCateg.ExpenseCategName;
            this.UserId = expenseCateg.UserId;
            this.IsDefault = expenseCateg.IsDefault;
            this.OrderVal = expenseCateg.OrderVal;
        }
    }
}
