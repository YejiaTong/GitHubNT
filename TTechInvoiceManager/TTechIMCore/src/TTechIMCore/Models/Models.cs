using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TTechIMCore.UIClasses;

using Newtonsoft.Json;

/*
 * TTech IM - App Model Layer - Base Model Class
 * Customized Front-end Model Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models
{
    public class Models
    {

    }

    public class LoginIdAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val))
                {
                    ErrorMessage = "Email or Username can not be Empty";

                    return false;
                }
                if (!UIUser.IsValidUsername(val) && !UIUser.IsValidEmail(val))
                {
                    ErrorMessage = "Invalid Email or Username";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class UserNameAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val))
                {
                    ErrorMessage = "Username can not be Empty";

                    return false;
                }
                if (!UIUser.IsValidUsername(val))
                {
                    ErrorMessage = "Invalid Username";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class PasswordAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val))
                {
                    ErrorMessage = "Password can not be Empty";

                    return false;
                }
                if (!UIUser.IsValidPassword(val))
                {
                    ErrorMessage = "Invalid Password. Please follow the password policy";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class EmailAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val))
                {
                    ErrorMessage = "Email can not be Empty";

                    return false;
                }
                if (!UIUser.IsValidEmail(val))
                {
                    ErrorMessage = "Invalid Email";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class PostalCodeAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (!UIUser.IsValidPostalCode(val))
                {
                    ErrorMessage = "Invalid Postal Code";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class ExpenseNameAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val.Trim()))
                {
                    ErrorMessage = "Empty Expense Name";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class ExpenseCostAttribute : ValidationAttribute
    {
        public double Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            double val = 0.0;
            if (!Double.TryParse(value.ToString(), out val))
            {
                ErrorMessage = "Invalid Expense Cost";

                return false;
            }
            else
            {
                if (val < 0)
                {
                    ErrorMessage = "Expense Cost >= 0.0";

                    return false;
                }
            }

            return true;
        }
    }

    public class ExpenseCategNameAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val.Trim()))
                {
                    ErrorMessage = "Empty Expense Category Name";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public class ExpenseOrderValAttribute : ValidationAttribute
    {
        public int Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            int val = 0;
            if (!Int32.TryParse(value.ToString(), out val))
            {
                ErrorMessage = "Invalid Order Number";

                return false;
            }
            else
            {
                if (val < 1 || val > 100)
                {
                    ErrorMessage = "Order Number should be within (1 ... 100)";

                    return false;
                }
            }

            return true;
        }
    }

    public class MessageBoardMsgAttribute : ValidationAttribute
    {
        public string Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            if (value is string)
            {
                string val = value as string;
                if (String.IsNullOrEmpty(val.Trim()))
                {
                    ErrorMessage = "Message is empty";

                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
