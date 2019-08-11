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
	/// Read KML files whith GEO points
	/// </summary>
	public class KMLWorkerClass
	{
		public KML_geo_pos[] geo_points = new KML_geo_pos[500];
		public int geo_points_cnt = 0;
		public KMLWorkerClass()
		{
		}
		
		public void load_KML(string kml_file_path)
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
					}
					if (line.Contains("<coordinates>"))
					{
						coord_line = find_coordinates(line);
						coordinates = coord_line.Split(',');
						geo_points[geo_points_cnt].Longitude = Convert.ToDouble(coordinates[0]);
						geo_points[geo_points_cnt].Latitude = Convert.ToDouble(coordinates[1]);
					}
					if (line.Contains("<name>"))
					{
						geo_points[geo_points_cnt].Name = find_name(line);
					}
				}
			}
            
			sr.Close();
			
			System.Threading.Thread.CurrentThread.CurrentCulture = tmp_culture;
		}
		
		//find coordinates in string
		string find_coordinates(string text)
		{
			int pos1 = text.IndexOf("<coord")+13;
			int pos2 = text.IndexOf("</coord");
			
			string result = text.Substring(pos1,(pos2-pos1));
			return result;
		}
		
		//find name in string
		string find_name(string text)
		{
			int pos1 = text.IndexOf("<name>")+6;
			int pos2 = text.IndexOf("</name>");
			
			string result = text.Substring(pos1,(pos2-pos1));
			return result;
		}
	}
}
