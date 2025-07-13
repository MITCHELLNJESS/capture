using GMap.NET.WindowsForms;
using MissionPlanner.Controls;
using MissionPlanner.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.Utilities
{
    public class POI
    {
        /// <summary>
        /// Store points of interest
        /// </summary>
        static ObservableCollection<PointLatLngAlt> POIs = new ObservableCollection<PointLatLngAlt>();
        static List<int> color = new List<int>();

        private static EventHandler _POIModified;

        public static event EventHandler POIModified
        {
            add
            {
                _POIModified += value;
                try
                {
                    if (File.Exists(filename))
                        LoadFile(filename);
                }
                catch
                {
                }
            }
            remove { _POIModified -= value; }
        }

        private static string filename = Settings.GetUserDataDirectory() + "poi.txt";
        private static bool loading;

        static POI()
        {
            POIs.CollectionChanged += POIs_CollectionChanged;
        }

        private static void POIs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (loading)
                    return;
                SaveFile(filename);
            }
            catch { }
        }

        public static void POIAdd(PointLatLngAlt Point, string tag)
        {
            // local copy
            PointLatLngAlt pnt = Point;

            pnt.Tag = tag + "\n" + pnt.ToString();

            POI.POIs.Add(pnt);
            POI.color.Add(0);

            if (_POIModified != null && !loading)
                _POIModified(null, null);
        }

        public static void POIAdd(PointLatLngAlt Point, string tag, int color)
        {
            // local copy
            PointLatLngAlt pnt = Point;

            pnt.Tag = tag + "\n" + pnt.ToString();

            POI.POIs.Add(pnt);
            POI.color.Add(color);

            if (_POIModified != null && !loading)
                _POIModified(null, null);
        }

        public static void POIAdd(PointLatLngAlt Point)
        {
            if (Point == null)
                return;

            PointLatLngAlt pnt = Point;

            string output = "";

            if (DialogResult.OK != InputBox.Show("POI", "Enter ID", ref output))
                return;

            POIAdd(Point, output);
            POI.color.Add(0);
        }

        public static int POIDelete(GMapMarkerPOI Point)
        {
            if (Point == null)
                return -1;

            for (int a = 0; a < POI.POIs.Count; a++)
            {
                if (POI.POIs[a].Point() == Point.Position)
                {
                    POI.POIs.RemoveAt(a);
                    POI.color.RemoveAt(a);
                    if (_POIModified != null)
                        _POIModified(null, null);
                    return a;
                }
            }

            return -1;
        }

        public static int POIDeleteClosest(GMapMarkerPOI Point)
        {
            GMapMarkerPOI PointToDelete = null;
            double closestDist = Double.MaxValue;
            for (int a = 0; a < POI.POIs.Count; a++)
            {
                double dLat = (POI.POIs[a].Point().Lat - Point.Position.Lat) * Math.PI / 180;
                double dLng = (POI.POIs[a].Point().Lng - Point.Position.Lng) * Math.PI / 180;

                double lat1 = POI.POIs[a].Point().Lat * Math.PI / 180;
                double lat2 = Point.Position.Lat * Math.PI / 180;

                double b = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLng / 2) * Math.Sin(dLng / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                double c = 2 * Math.Atan2(Math.Sqrt(b), Math.Sqrt(1 - b));
                double distance = 6371 * c;

                if (distance < closestDist)
                {
                    closestDist = distance;
                    PointToDelete = new GMapMarkerPOI(POI.POIs[a].Point());
                }
            }
            return POIDelete(PointToDelete);
        }

        public static void POIClear()
        {
            for (int a = POI.POIs.Count - 1; a >= 0; a--)
            {
                POI.POIs.RemoveAt(a);
                POI.color.RemoveAt(a);
            }
        }

        public static void POIEdit(GMapMarkerPOI Point)
        {
            if (Point == null)
                return;

            string output = "";

            if (DialogResult.OK != InputBox.Show("POI", "Enter ID", ref output))
                return;

            for (int a = 0; a < POI.POIs.Count; a++)
            {
                if (POI.POIs[a].Point() == Point.Position)
                {
                    POI.POIs[a].Tag = output + "\n" + Point.Position.ToString();
                    if (_POIModified != null)
                        _POIModified(null, null);
                    return;
                }
            }
        }

        public static void POIMove(GMapMarkerPOI Point)
        {
            for (int a = 0; a < POI.POIs.Count; a++)
            {
                if (POIs[a].Tag == Point.ToolTipText)
                {
                    POIs[a].Lat = Point.Position.Lat;
                    POIs[a].Lng = Point.Position.Lng;
                    POIs[a].Tag = POIs[a].Tag.Substring(0, POIs[a].Tag.IndexOf('\n')) + "\n" + Point.Position.ToString();
                    break;
                }
            }

            if (_POIModified != null)
                _POIModified(null, null);
        }

        public static void POISave()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Poi File|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(sfd.FileName);
                }
            }
        }

        private static void SaveFile(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                foreach (var item in POI.POIs)
                {
                    string line = item.Lat.ToString(CultureInfo.InvariantCulture) + "\t" +
                                  item.Lng.ToString(CultureInfo.InvariantCulture) + "\t" + item.Tag.Substring(0, item.Tag.IndexOf('\n')) + "\r\n";
                    byte[] buffer = ASCIIEncoding.ASCII.GetBytes(line);
                    file.Write(buffer, 0, buffer.Length);
                }
            }
        }


        public static void POILoad()
        {
            using (OpenFileDialog sfd = new OpenFileDialog())
            {
                sfd.Filter = "Poi File|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(sfd.FileName);
                }
            }
        }

        private static void LoadFile(string fileName)
        {
            loading = true;
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] items = sr.ReadLine().Split('\t');

                        if (items.Count() < 3)
                            continue;

                        POIAdd(new PointLatLngAlt(double.Parse(items[0], CultureInfo.InvariantCulture)
                            , double.Parse(items[1], CultureInfo.InvariantCulture)), items[2]);
                    }
                }
            }
            loading = false;
            // redraw now
            if (_POIModified != null)
                _POIModified(null, null);
        }

        public static void UpdateOverlay(GMap.NET.WindowsForms.GMapOverlay poioverlay)
        {
            if (poioverlay == null)
                return;

            poioverlay.Clear();

            int i = 0;

            foreach (var pnt in POIs)
            {
                if (POI.color[i] == 1)
                {
                    poioverlay.Markers.Add(new GMapMarkerPOI_Blue(pnt)
                    {
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        ToolTipText = pnt.Tag
                    });
                }
                else if (POI.color[i] == 2)
                {
                    poioverlay.Markers.Add(new GMapMarkerPOI_Yellow(pnt)
                    {
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        ToolTipText = pnt.Tag
                    });
                }
                else
                {
                    poioverlay.Markers.Add(new GMapMarkerPOI(pnt)
                    {
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        ToolTipText = pnt.Tag
                    });
                }
                i++;
            }
        }
    }
}