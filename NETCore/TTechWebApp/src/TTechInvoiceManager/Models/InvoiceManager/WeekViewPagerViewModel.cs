using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class WeekViewModel
    {
        [DisplayName("Week Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WeekStartTs { get; set; }

        [DisplayName("Week End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WeekEndTs { get; set; }

        public int WeekNum { get; set; }
    }

    public class WeekViewPagerViewModel
    {
        public int[] YearsOpts { get; set; } = YearGenerator.GetYears(3).ToArray();

        public WeekViewModel[] WeeksOpts { get; set; }

        public int Year { get; set; } = DateTime.Today.Year;

        public int Month { get; set; } = 0;

        public int Week { get; set; } = 0;
    }
}
