using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for Expense Categories
 * 
 * Noah Tong - Jan 05, 2017
 * */

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
