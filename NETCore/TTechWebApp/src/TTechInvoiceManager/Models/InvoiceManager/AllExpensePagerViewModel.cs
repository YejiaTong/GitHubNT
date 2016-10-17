using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.InvoiceManager
{
    public class AllExpensePagerViewModel
    {
        public int[] PageSizeOpts { get; set; } = new int[4] {5, 10, 25, 50};

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;

        public string ExpenseNameSearch { get; set; } = String.Empty;

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTs { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndTs { get; set; } = DateTime.Now;

        public long LastItemId { get; set; } = 0;

        public string JsonConvertObject { get; set; } = String.Empty;
    }
}
