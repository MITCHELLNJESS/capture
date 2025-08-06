using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace MissionPlanner.Maps
{
    [Serializable]
    public class GMapMarkerPOI : GMarkerGoogle
    {
        public GMapMarkerPOI(PointLatLng p)
            : base(p, GMarkerGoogleType.red_dot)
        {
        }
    }

    public class GMapMarkerPOI_Blue : GMarkerGoogle
    {
        public GMapMarkerPOI_Blue(PointLatLng p)
            : base(p, GMarkerGoogleType.blue_dot)
        {
        }
    }

    public class GMapMarkerPOI_Yellow : GMarkerGoogle
    {
        public GMapMarkerPOI_Yellow(PointLatLng p)
            : base(p, GMarkerGoogleType.yellow_dot)
        {
        }
    }
}