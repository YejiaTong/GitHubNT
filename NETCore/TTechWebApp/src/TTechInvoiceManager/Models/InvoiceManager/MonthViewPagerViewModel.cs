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
        public DateTime MonthStartTs { get; set; }

        [DisplayName("Month End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MonthEndTs { get; set; }

        public int MonthNum { get; set; }
    }

    public class MonthViewPagerViewModel
    {
        public List<int> YearsOpts { get; set; } = YearGenerator.GetYears(10).ToList();

        public List<MonthViewModel> MonthsOpts { get; set; }

        public MonthViewModel SelectedMonth { get; set; }

        public int Year { get; set; } = DateTime.Today.Year;

        public int Month { get; set; } = 0;
    }
}
