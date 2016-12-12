using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.Home
{
    /*
     * Model Layer - NETDateViewModel
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class NETDateViewModel
    {
        [DisplayName("Year")]
        [Digits]
        public int Year { get; set; }

        [DisplayName("Month")]
        [Digits]
        public int Month { get; set; }

        [DisplayName("Day")]
        [Digits]
        public int Day { get; set; }
    }
}
