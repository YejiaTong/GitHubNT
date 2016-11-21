using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Home
{
    public class SiteMapViewModel
    {
        public int SiteMapId { get; set; }

        public string SiteMapName { get; set; }

        public string SiteMapController { get; set; }

        public string SiteMapView { get; set; }

        public string Description { get; set; }

        public bool IsSelected { get; set; } = false;
    }
}
