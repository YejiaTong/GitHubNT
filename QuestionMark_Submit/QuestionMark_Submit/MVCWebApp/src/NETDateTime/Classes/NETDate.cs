using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETDateTime.Classes
{
    /*
     * Implementation of NETDate with applicable Year, Month, Day components
     * Certain business logic is in place for the initialization of such object
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class NETDate : TimestampBase
    {
        protected int _totalNumberOfDay;

        #region [Constructors]
        public NETDate()
            : base() { }

        public NETDate(int year)
            : base()
        {
            Year = year;

            if (!IsDayValid(_Year, _Month, _Day))
            {
                throw new Exception($"Year: {_Year}, Month: {_Month}, Day: {_Day} is not a valid day");
            }
        }

        public NETDate(int year, int month)
            : base()
        {
            Year = year;
            Month = month;

            if (!IsDayValid(_Year, _Month, _Day))
            {
                throw new Exception($"Year: {_Year}, Month: {_Month}, Day: {_Day} is not a valid day");
            }
        }

        public NETDate(int year, int month, int day)
            : base()
        {
            Year = year;
            Month = month;
            Day = day;

            if (!IsDayValid(_Year, _Month, _Day))
            {
                throw new Exception($"Year: {_Year}, Month: {_Month}, Day: {_Day} is not a valid day");
            }
        }
        #endregion

        public int TotalNumberOfDay
        {
            get
            {
                _totalNumberOfDay = 0;

                // Year component
                int iteration = 0;
                if ((Year - StaticComponent.MinYear) % StaticComponent.LeapYearInterval == 0)
                {
                    iteration = Convert.ToInt32(Math.Floor(((double)(Year - StaticComponent.MinYear) / StaticComponent.LeapYearInterval)));

                    _totalNumberOfDay += iteration * (366 + 3 * 365);
                }
                else
                {
                    iteration = Convert.ToInt32(Math.Floor(((double)(Year - StaticComponent.MinYear) / StaticComponent.LeapYearInterval)));

                    _totalNumberOfDay += iteration * (366 + 3 * 365);

                    int adderYear = ((Year - StaticComponent.MinYear * iteration) % StaticComponent.LeapYearInterval);
                    _totalNumberOfDay += 366 + (adderYear - 1) * 365;
                }

                // Month component
                int m = 1;
                for(; m < Month; m++)
                {
                    if(StaticComponent.StandardMonths.Contains(m))
                    {
                        if(m != 2)
                        {
                            _totalNumberOfDay += 30;
                        }
                        else if(StaticComponent.IsLeapYear(Year))
                        {
                            _totalNumberOfDay += 29;
                        }
                        else
                        {
                            _totalNumberOfDay += 28;
                        }
                    }
                    else
                    {
                        _totalNumberOfDay += 31;
                    }
                }

                // Day component
                int d = Day;
                _totalNumberOfDay += Day;

                return _totalNumberOfDay;
            }
        }

        #region [Setters & Getters]
        public int Year
        {
            set
            {
                SetYear(value);
            }
            get
            {
                return _Year;
            }
        }

        public int Month
        {
            set
            {
                SetMonth(value);
            }
            get
            {
                return _Month;
            }
        }

        public int Day
        {
            set
            {
                SetDay(value);
            }
            get
            {
                return _Day;
            }
        }

        private void SetYear(int year)
        {
            var result = ValidateYear(year);
            if (result.Success)
            {
                _Year = year;
            }
            else
            {
                throw new ArgumentException("Year", result.Msg);
            }
        }

        private Result ValidateYear(int year)
        {
            if (year < StaticComponent.MinYear)
            {
                return new Result() { Success = false, Msg = $"Year cannot be less then {StaticComponent.MinYear}" };
            }
            return new Result() { Success = true };
        }

        private void SetMonth(int month)
        {
            var result = ValidateMonth(month);
            if (result.Success)
            {
                _Month = month;
            }
            else
            {
                throw new ArgumentException("Month", result.Msg);
            }
        }

        private Result ValidateMonth(int month)
        {
            if (!StaticComponent.Months.Contains(month))
            {
                return new Result() { Success = false, Msg = "Invalid Month value" };
            }
            return new Result() { Success = true };
        }

        private void SetDay(int day)
        {
            var result = ValidateDay(day);
            if (result.Success)
            {
                _Day = day;
            }
            else
            {
                throw new ArgumentException("Day", result.Msg);
            }
        }

        private Result ValidateDay(int day)
        {
            if (!StaticComponent.Days.Contains(day))
            {
                return new Result() { Success = false, Msg = "Invalid Day value" };
            }
            return new Result() { Success = true };
        }
        #endregion

        // ToString
        public override string ToString()
        {
            return $"{Year}-{Month.ToString().PadLeft(2, '0')}-{Day.ToString().PadLeft(2, '0')}";
        }

        // Parse from string
        public static NETDate Parse(string input)
        {
            NETDate ret;
            try
            {
                int y = Int32.Parse(input.Substring(0, 4));
                int m = Int32.Parse(input.Substring(4, 2));
                int d = Int32.Parse(input.Substring(6, 2));

                ret = new NETDate(y, m, d);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to parse date value: " + input
                    + Environment.NewLine + ex.Message);
            }

            return ret;
        }

        // Overload operator -
        public static int operator -(NETDate value1, NETDate value2)
        {
            return Math.Abs(value1.TotalNumberOfDay - value2.TotalNumberOfDay);
        }

        // Check if a day is valid
        private bool IsDayValid(int year, int month, int day)
        {
            if(StaticComponent.StandardMonths.Contains(month))
            {
                if(month != 2)
                {
                    return day <= 30;
                }
                else if(StaticComponent.IsLeapYear(year))
                {
                    return day <= 29;
                }
                else
                {
                    return day <= 28;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
