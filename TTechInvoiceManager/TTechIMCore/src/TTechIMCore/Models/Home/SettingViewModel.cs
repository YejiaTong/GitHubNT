using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - Setting ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.Home
{
    public class SettingViewModel
    {
        public List<SiteMapViewModel> SiteMaps { get; set; }
    }
}
