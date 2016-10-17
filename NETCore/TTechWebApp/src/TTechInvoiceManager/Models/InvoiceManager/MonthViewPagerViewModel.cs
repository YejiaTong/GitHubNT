using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class MonthViewModel
    {
        [DisplayName("Month Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WeekStartTs { get; set; }

        [DisplayName("Month End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WeekEndTs { get; set; }

        public int MonthNum { get; set; }
    }

    public class MonthViewPagerViewModel
    {
        public int[] YearsOpts { get; set; } = YearGenerator.GetYears(3).ToArray();

        public MonthViewModel[] MonthsOpts { get; set; }

        public int Year { get; set; } = DateTime.Today.Year;

        public int Month { get; set; } = 0;
    }
}
