﻿using System;
using Zeptomoby.OrbitTools;
using Gavaghan.Geodesy;
using System.IO;

namespace lrpt_places1
{
	public struct Sat_geo_pos
	{
		public double Latitude;
		public double Longitude;
		public double Altitude;
	}
	
	public struct time_struct
	{
		public int y_pose;
		public DateTime block_start;
		public bool is_msk_time;
	}
	
   /// <summary>
   /// Satellite position calculator
   /// </summary>
	public class SatellitePosCalcClass
	{
		public Tle current_tle;
		
		/// <summary>
		/// Satellite position for corresponding image line
		/// </summary>
		public Sat_geo_pos[] satellite_positions = new Sat_geo_pos[10000];
		public time_struct[] time_info = new time_struct[10000];
		int time_info_cnt  = 0;
		public int points_cnt = 0;
		
		public DateTime table_start_time;
		public double table_fight_duration;

        /// <summary>
        /// north to south flight flag
        /// </summary>
		public bool NtoS_fight = false;
		
		/// <summary>
		/// Initialise Satellite position calculator
		/// </summary>
		/// 
		public SatellitePosCalcClass()
		{
		}
		
		/// <summary>
		/// Place current TLE in object memory
		/// </summary>
		/// <param name="tle">TLE object to load</param>
		public void Load_TLE(Tle tle)
		{
			current_tle = tle;
			System.Diagnostics.Debug.WriteLine("\n", tle.Name);
			System.Diagnostics.Debug.WriteLine("\n", tle.Line1);
			System.Diagnostics.Debug.WriteLine("\n", tle.Line2);
		}
		
		/// <summary>
		/// Calculate Lat/Long for a certain time
		/// </summary>
		/// <param name="time">Time for calculation</param>
		public Sat_geo_pos FindSatGeoPos(DateTime time)
		{
			Sat_geo_pos cur_sat_pos = new Sat_geo_pos();
			Satellite sat = new Satellite(current_tle);
			Eci sat_pos = sat.PositionEci(time);
			Julian cur_julian = new Julian(time);
			Geo sat_geo_pos = new Geo(sat_pos,cur_julian);
			
			cur_sat_pos.Latitude = sat_geo_pos.LatitudeDeg;
			cur_sat_pos.Longitude = sat_geo_pos.LongitudeDeg;
			
			if (cur_sat_pos.Longitude > 180)
            {
                cur_sat_pos.Longitude = (-360+cur_sat_pos.Longitude);
            }
			cur_sat_pos.Altitude = sat_geo_pos.Altitude;
			
			return cur_sat_pos;
		}


        /// <summary>
        /// Calculate Lat/Long satellite positions (projection to Earth) at flight time
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="flight_duration"></param>
        /// <param name="flight_points_num">Number of points during flight - Image height</param>
        /// <param name="use_timetable"></param>
        public void CalculateSatellitePositions(DateTime start_time, double flight_duration, int flight_points_num, bool use_timetable)
		{
			int i;
			DateTime cur_time;
			double time_increment_d = 0.15412*10000000;//duration of one line is 100ns
			int time_table_pos = 0;
			
			if (use_timetable) 
			{
				UpdateTimetable(start_time);
				cur_time = time_info[0].block_start;
			}
			else
			{
				cur_time = start_time;
			}
			
			TimeSpan time_increment = new TimeSpan(Convert.ToInt64(time_increment_d));
			cur_time = cur_time.Add(new TimeSpan(Convert.ToInt64(time_increment_d/2)));
			
			for (i=0; i < flight_points_num; i++)
			{
				if (((i % 8) == 0) && use_timetable)
				{
					time_table_pos = FindLineInTimeTable(i);
					if (time_table_pos > -1)
					{
						//System.Diagnostics.Debug.WriteLine("i: {0}\n", i);
						cur_time = time_info[time_table_pos].block_start;
						cur_time = cur_time.Subtract(new TimeSpan(Convert.ToInt64(time_increment_d*9)));
					}
				}
				satellite_positions[i] = FindSatGeoPos(cur_time);
				cur_time = cur_time.Add(time_increment);

			}
			points_cnt = flight_points_num;
			System.Diagnostics.Debug.WriteLine("Number of calculated points: {0}\n", flight_points_num);
			
			if ((satellite_positions[0].Latitude - satellite_positions[flight_points_num-1].Latitude) > 0)
			{
				NtoS_fight = true;//flight "down"
				System.Diagnostics.Debug.WriteLine("flight down (север-юг)\n");
			}
			else
			{
				NtoS_fight = false;//flight "up"
				System.Diagnostics.Debug.WriteLine("flight up (юг-север)\n");
			}
		}

        /// <summary>
        /// Find the best line in image for a given mark position
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>Return number or image line where distance was shortest</returns>
        public int FindBestLine2(double latitude, double longitude)
		{
			int i;
			double min_diff_km = 100000;
			int min_diff_line  = 0;
			double diff_km = 0;
			
			for (i = 0; i < points_cnt; i++)
			{
                //Find distance between mark and stell. pos
				diff_km = Find_2pointsDistance(
                    satellite_positions[i].Latitude,satellite_positions[i].Longitude,
                    latitude,longitude);

				if (diff_km < min_diff_km)
				{
					min_diff_km = diff_km;
					min_diff_line = i;
				}
			}
			
			return min_diff_line;
		}

        /// <summary>
        /// Calculate distance between 2 points in kilometers
        /// </summary>
        /// <param name="start_lat">Point 1</param>
        /// <param name="start_lon">Point 1</param>
        /// <param name="stop_lat">Point 2</param>
        /// <param name="stop_lon">Point 2</param>
        /// <returns>Return istance value in km</returns>
        public double Find_2pointsDistance(double start_lat, double start_lon,  double stop_lat, double stop_lon)
		{
			GeodeticCalculator geoCalc = new GeodeticCalculator();
			Ellipsoid reference = Ellipsoid.WGS84;
			
			GlobalCoordinates start_pose = new GlobalCoordinates(new Angle(start_lat), new Angle(start_lon));
			GlobalCoordinates stop_pose = new GlobalCoordinates(new Angle(stop_lat), new Angle(stop_lon));
			GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, start_pose, stop_pose);
      		double ellipseKilometers = geoCurve.EllipsoidalDistance / 1000.0;
      
			return ellipseKilometers;
		}
		
		/// <summary>
		/// Search for line in timetable containing y_pose
		/// </summary>
		public int FindLineInTimeTable(int y_pose)
		{
			int i;
			for (i=0; i < time_info_cnt; i++)
			{
				if (time_info[i].y_pose == y_pose)
                    return i;
			}
			return -1;
			
		}
		
		public void LoadTimeTable(string path)
		{
			StreamReader sr3 = new StreamReader(path);
			string line;
			int i = 0;
			int pos1 = 0;
			int pos2 = 0;
			string substr1;
			int cur_y_pos = 0;
			bool search_time = false;
			time_info_cnt  = 0;
			
            while ((line = sr3.ReadLine()) != null)
            {
            	pos1 = line.IndexOf("y=");
            	if ((pos1 > -1) && (search_time == false))
            	{
            		pos2 = line.IndexOf(" y_last");
            		substr1 = line.Substring(pos1+2,(pos2-pos1-2));
            		cur_y_pos = Convert.ToInt32(substr1);
            		search_time = true;
            		//System.Diagnostics.Debug.WriteLine("pos: {0}\n", i);
            	}
            	else if (search_time == true)
            	{
            		pos1 = line.IndexOf("tt=");
            		pos2 = line.IndexOf(" x=");
            		substr1 = line.Substring(pos1+3,(pos2-pos1-2));
            		int start_ms = 	Convert.ToInt32(substr1.Substring(substr1.Length - 3));

            		DateTime start_time = new DateTime(
            			01,01,01,
            			Convert.ToInt32(substr1.Substring(0,2)),
            			Convert.ToInt32(substr1.Substring(3,2)),
            			Convert.ToInt32(substr1.Substring(6,2)),
            			start_ms);
            		time_info[time_info_cnt].block_start = start_time;
            		time_info[time_info_cnt].y_pose = cur_y_pos;
            		time_info[time_info_cnt].is_msk_time = true;
            		time_info_cnt++;
            		search_time = false;
            	}
            	i++;
            }
            sr3.Close();
			System.Diagnostics.Debug.WriteLine("Time info records: {0}\n",time_info_cnt);
			
			table_start_time = time_info[0].block_start;
			TimeSpan duration_time;
			duration_time = time_info[time_info_cnt-1].block_start.Subtract(table_start_time);
			table_fight_duration = duration_time.TotalMilliseconds/1000;
			table_fight_duration+=1.232;//last block
		}
		
		/// <summary>
		/// Update timetable with new date value
		/// </summary>
		public void UpdateTimetable(DateTime startdate)
		{
			int i;
			DateTime tmp_datetime;
			DateTime table_datetime;
			for (i=0;i<time_info_cnt;i++)
			{
				table_datetime = time_info[i].block_start;
				tmp_datetime = new DateTime(startdate.Year,
				                            startdate.Month,
				                            startdate.Day,
				                            table_datetime.Hour,
				                            table_datetime.Minute,
				                            table_datetime.Second,
				                            table_datetime.Millisecond);
				if (time_info[i].is_msk_time)
				{
					tmp_datetime = tmp_datetime.Subtract(new TimeSpan(3,0,0));//get utc time
					time_info[i].block_start = tmp_datetime;
					time_info[i].is_msk_time = false;
				}
				else
				{
					time_info[i].block_start = tmp_datetime;
				}
			}
		}
		
	}//end of class
}
