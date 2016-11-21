using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Home
{
    public class SettingViewModel
    {
        public List<SiteMapViewModel> SiteMaps { get; set; }
    }
}
