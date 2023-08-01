using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace lrpt_places1
{
	public struct KML_geo_pos
	{
		public double Latitude;
		public double Longitude;
		public string Name;
	}
	
	/// <summary>
	/// Read KML files with GEO points
	/// </summary>
	public class KMLWorkerClass
	{
		public KML_geo_pos[] geo_points = new KML_geo_pos[3000];
		public int geo_points_cnt = 0;

		public KMLWorkerClass()
		{
		}
		
		public void Load_KML(string kml_file_path)
		{
			StreamReader sr = new StreamReader(kml_file_path);
			string line;
			bool placemark_begin = false;
			geo_points_cnt = 0;
			string coord_line;
			string[] coordinates;
			
			CultureInfo tmp_culture;
			
			tmp_culture = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			
			while ((line = sr.ReadLine()) != null)
            {
				if (placemark_begin == false)
				{
					if (line.Contains("<Placemark")) {placemark_begin = true;}
				}
				else if (placemark_begin)
				{
					if (line.Contains("</Placemark")) 
					{
						placemark_begin = false;
						geo_points_cnt++;
                        if (geo_points_cnt >= geo_points.Length) //too much points
                            break;
					}
					if (line.Contains("<coordinates>"))
					{
						coord_line = FindCoordinates(line);
						coordinates = coord_line.Split(',');
						geo_points[geo_points_cnt].Longitude = Convert.ToDouble(coordinates[0]);
						geo_points[geo_points_cnt].Latitude = Convert.ToDouble(coordinates[1]);
					}
					if (line.Contains("<name>"))
					{
						geo_points[geo_points_cnt].Name = FindName(line);
					}
				}
			}
			sr.Close();
			
			System.Threading.Thread.CurrentThread.CurrentCulture = tmp_culture;
		}

        /// <summary>
        /// Find coordinates in given string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string FindCoordinates(string text)
		{
			int pos1 = text.IndexOf("<coord") + 13;
			int pos2 = text.IndexOf("</coord");
			
			string result = text.Substring(pos1, (pos2-pos1));
			return result;
		}

        /// <summary>
        /// Find name in given string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string FindName(string text)
		{
			int pos1 = text.IndexOf("<name>") + 6;
			int pos2 = text.IndexOf("</name>");
			
			string result = text.Substring(pos1, (pos2-pos1));
			return result;
		}
	}
}
