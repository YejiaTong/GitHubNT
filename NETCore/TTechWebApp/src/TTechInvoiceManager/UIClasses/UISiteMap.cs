using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NTWebApp.DBAccess;

namespace NTWebApp.UIClasses
{
    public class UISiteMap : SiteMap
    {
        public UISiteMap()
            : base() { }

        public UISiteMap(SiteMap siteMap)
        {
            this.SiteMapId = siteMap.SiteMapId;
            this.SiteMapName = siteMap.SiteMapName;
            this.SiteMapController = siteMap.SiteMapController;
            this.SiteMapView = siteMap.SiteMapView;
            this.Description = siteMap.Description;
        }
    }
}
