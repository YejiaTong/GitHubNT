using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETDateTime.Classes
{
    /*
     * Base class for Timestamp object
     * For future inheritance of Date and DateTime
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public abstract class TimestampBase
    {
        protected int _Year;
        protected int _Month;
        protected int _Day;
        protected int _Hour;
        protected int _Minute;
        protected int _Second;

        public TimestampBase()
        {
            _Year = 0;
            _Month = 0;
            _Day = 0;
            _Hour = 0;
            _Minute = 0;
            _Second = 0;
        }

        // public abstract T Parse<T>(string input) where T : TimestampBase;
    }
}
