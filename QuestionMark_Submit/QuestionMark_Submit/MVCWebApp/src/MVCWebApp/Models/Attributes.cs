using System;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models
{
    /*
     * Customized Data Annotation Validation
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class DigitsAttribute : ValidationAttribute
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
                ErrorMessage = "Invalid digits";

                return false;
            }
            return true;
        }
    }
}
