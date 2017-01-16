using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for Site Map Items
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.UIClasses
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
