using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace NETDateTime.Classes
{
    /*
     * Lazy Initialization, Flyweight Global shared variables
     * In support of customized NETDate, NETDateTime objects
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public static class StaticComponent
    {
        #region [Readonly Variables]
        private static readonly int[] PreallocationMonthList =
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        private static readonly int[] PreallocationStandardMonthList =
            { 2, 4, 6, 9, 11 };

        private static readonly int[] PreallocationDayList =
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
              16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31};

        public static readonly int LeapYearInterval = 4;
        public static readonly int MinYear = 0;
        #endregion

        #region [Setter & Getter]
        private static HashSet<int> _hashSetMonths;

        private static HashSet<int> _hashSetStandardMonths;

        private static HashSet<int> _hashSetDays;

        public static HashSet<int> Months
        {
            get
            {
                if (_hashSetMonths == null)
                {
                    _hashSetMonths = new HashSet<int>(PreallocationMonthList);
                }
                return _hashSetMonths;
            }
        }

        public static HashSet<int> StandardMonths
        {
            get
            {
                if (_hashSetStandardMonths == null)
                {
                    _hashSetStandardMonths = new HashSet<int>(PreallocationStandardMonthList);
                }
                return _hashSetStandardMonths;
            }
        }

        public static HashSet<int> Days
        {
            get
            {
                if (_hashSetDays == null)
                {
                    _hashSetDays = new HashSet<int>(PreallocationDayList);
                }
                return _hashSetDays;
            }
        }
        #endregion

        public static bool IsLeapYear(int year)
        {
            return year % LeapYearInterval == 0;
        }
    }
}
