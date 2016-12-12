using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApp.Models.Home
{
    /*
     * Model Layer - IndexViewModel
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class IndexViewModel
    {
        public NETDateViewModel DateOne { get; set; }
        public NETDateViewModel DateTwo { get; set; }
        public int Diff { get; set; }
        public string Msg { get; set; }
    }
}
