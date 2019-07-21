using System;

namespace lrpt_places1
{
	public class Time_proc
	{
		public DateTime start_time;//msk time (time only)
		public double flight_duration;//in seconds
		
		public Time_proc()
		{
			start_time = new DateTime(2000,1,1,0,0,0,0);
			flight_duration = 0;
		}
		
		/// <summary>
		/// Calculate start time and duration and place it in class start_time variable
		/// </summary>
		public int fill_time_from_string(string str_time)
		{
			string[] str_lines = str_time.Split('\n');
			
			if (str_lines.Length < 2)
			{
				return -1;
			}
			
			string line_start = str_lines[0];
			line_start = line_start.Remove(line_start.Length-1,1);
			string line_duration = str_lines[1];
			
			int start_ms = 	Convert.ToInt32(line_start.Substring(line_start.Length - 3));
			int duration_ms = 	Convert.ToInt32(line_duration.Substring(line_duration.Length - 3));
			
			start_time = new DateTime(1,1,1,Convert.ToInt32(line_start.Substring(0,2)),
			                          Convert.ToInt32(line_start.Substring(3,2)),
			                          Convert.ToInt32(line_start.Substring(6,2)),start_ms);
			
			flight_duration = Convert.ToDouble(duration_ms)*0.001;
			flight_duration+= Convert.ToDouble(line_duration.Substring(6,2));//sec
			flight_duration+= Convert.ToDouble(line_duration.Substring(3,2))*60;//min
			
			return 1;
		}
		
		/// <summary>
		/// Merge date and time values - 3 hours (msk)
		/// </summary>
		public DateTime create_full_utc(DateTime date, DateTime time, bool is_utc_time = false)
		{
			DateTime datetime_result = new DateTime(date.Year,
			                                        date.Month,
			                                        date.Day,
			                                        time.Hour,
			                                        time.Minute,
			                                        time.Second,
			                                        time.Millisecond);
			
			if (datetime_result.Year < 2002)
                return datetime_result;//can not substruct from bad date

            if (is_utc_time == false)
                datetime_result = datetime_result.Subtract(new TimeSpan(3,0,0));//get utc time

			return datetime_result;
			                                        
			                                        
		}
		
		
	}
}
