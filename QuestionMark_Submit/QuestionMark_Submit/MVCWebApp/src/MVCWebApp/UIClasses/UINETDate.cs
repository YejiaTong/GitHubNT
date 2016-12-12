using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NETDateTime.Classes;

namespace MVCWebApp.UIClasses
{
    /*
     * UI Layer - UINETDate
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class UINETDate
    {
        private NETDate _data;

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public static NETDate Parse(string input)
        {
            return NETDate.Parse(input);
        }

        public NETDate Data
        {
            get
            {
                _data = new NETDate(Year, Month, Day);
                return _data;
            }
        }

        public int GetDiff(NETDate value)
        {
            return Data - value;
        }
    }
}
