using System;
using System.IO;
using Zeptomoby.OrbitTools;

namespace lrpt_places1
{
	public class TLE_worker
	{
		public int cur_min_diff = 10000;
		public int meteor_code = 2;
		
		public TLE_worker()
		{
		}
		
		/// <summary>
		/// Select Meteor's name to work with
		/// </summary>
		public void set_meteor_code(int code)
		{
			meteor_code = code;
		}
		
		/// <summary>
		/// Scan TLE archive and rename files: "archive_XXXXX"
		/// </summary>
		public void scan_archive_and_rename(string archive_path)
		{
			string[] dirs = Directory.GetFiles(archive_path, "*.txt");
			string cur_filename;
			string cur_filepath;
			string new_filename;
			string new_filepath;
			int files_cnt = dirs.Length;
			int i;
			
			for (i=0;i<files_cnt;i++)
			{
				cur_filename = Path.GetFileName(dirs[i]);
				cur_filepath = dirs[i];
				if ((cur_filename.Contains("archive")) || (cur_filename.Contains("skip")))
				{
					//this file should be skipped
				}
				else
				{
					//this file shold be analysed and renamed
					new_filename = analyse_file(cur_filepath);
					new_filepath = archive_path + @"\" + new_filename;
					System.IO.File.Move(cur_filepath, new_filepath);
				}
			}
		}
		
		/// <summary>
		/// Analyse file for containing METEOR M2 TLE and create it's new name
		/// </summary>
		public string analyse_file(string file_path)
		{
			string new_filename = "";
			string line;
			string tle_line1;
			string tle_line2;
			string str_epoch;
		
			Tle tle1;
			
			StreamReader sr = new StreamReader(file_path);
			
			while ((line = sr.ReadLine()) != null)
            {
				if (meteor_code == 1)
				{
					if (line.Contains("M1") || line.Contains("M 1"))
					{
						tle_line1 = sr.ReadLine();
						tle_line2 = sr.ReadLine();
						tle1 = new Tle(line, tle_line1, tle_line2);
						str_epoch = tle1.Epoch;
						str_epoch = remove_fraction(str_epoch);
						//int_epoch = Convert.ToInt32(str_epoch);
						new_filename = "archive_"+ str_epoch+ ".txt";
						sr.Close();
						return new_filename;
					}
				}
				if (line.Contains("METEOR"))
				{
					if (meteor_code == 2)
					{
						if (line.Contains("M2") || line.Contains("M 2"))
						{
							tle_line1 = sr.ReadLine();
							tle_line2 = sr.ReadLine();
							tle1 = new Tle(line, tle_line1, tle_line2);
							str_epoch = tle1.Epoch;
							str_epoch = remove_fraction(str_epoch);
							//int_epoch = Convert.ToInt32(str_epoch);
							new_filename = "archive_"+ str_epoch+ ".txt";
							sr.Close();
							return new_filename;
						}
					}

				}
            }
			//meteor not found
			
			sr.Close();
			string file_name = Path.GetFileName(file_path);
			new_filename = "skip_" + file_name;
			
			return new_filename;
		}
		
		//remove fraction part from string
		public string remove_fraction(string text)
		{
			string new_text = "";
			int pos = text.IndexOf(".");
			new_text = text.Substring(0,pos);
			
			return new_text;
		}
		

		/// <summary>
		/// Find epoch value in string
		/// </summary>
		public int calc_epoch_day(string text)
		{
			int pos = text.IndexOf("_");
			string new_text = text.Remove(0,pos+1);
			new_text = new_text.Remove(new_text.Length-4,4);
			
			string year = new_text.Substring(0,2);
			string day = new_text.Substring(2,3);
			
			return Convert.ToInt32(year)*365+Convert.ToInt32(day);
		}
		
		/// <summary>
		/// Scan archive and find best TLE file by it name
		/// </summary>
		public string find_best_tle(string archive_path, DateTime image_date)
		{
			string[] dirs = Directory.GetFiles(archive_path, "*.txt");
			string cur_filename;
			int i;
			int cur_epoch = 0;
			int image_epoch = 0;
			int files_cnt = dirs.Length;
			string best_tle_path = "";
			
			int min_diff = 1000;
			int best_pos = 0;
			
			image_epoch = convert_date_to_epoch(image_date);
			
			for (i=0;i<files_cnt;i++)
			{
				cur_filename = Path.GetFileName(dirs[i]);
				if (cur_filename.Contains("skip"))
				{
					//this file should be skipped
				}
				else
				{
					cur_epoch = calc_epoch_day(cur_filename);
					if (Math.Abs(cur_epoch - image_epoch) < min_diff)
					{
						min_diff = Math.Abs(cur_epoch - image_epoch);
						best_pos = i;
					}
				}
				
			}
			cur_min_diff = min_diff;
			best_tle_path = dirs[best_pos];
			
			return best_tle_path;
		}
		
		public int convert_date_to_epoch(DateTime start_date)
		{
			if (start_date.Year < 2002) {return 0;}
			Julian cur_julian = new Julian(start_date);
			double epoch = (cur_julian.Date - 2451545.0);
			
			return Convert.ToInt32(epoch);
		}
		
		//open file "tle_path", find METEOR TLE
		//return TLE for METEOR
		public Tle load_tle(string tle_path)
		{
			StreamReader sr = new StreamReader(tle_path);
			Tle tle1;
			string tle_line1;
			string tle_line2;
			string line;
			
			while ((line = sr.ReadLine()) != null)
            {
				if (line.Contains("METEOR"))
				{
					if (meteor_code == 1)
					{
						if (line.Contains("M1") || line.Contains("M 1"))
						{
							tle_line1 = sr.ReadLine();
							tle_line2 = sr.ReadLine();
							tle1 = new Tle(line, tle_line1, tle_line2);
							sr.Close();
							return tle1;
						}
					}
					else if (meteor_code == 2)
					{
						if (line.Contains("M2") || line.Contains("M 2"))
						{
							tle_line1 = sr.ReadLine();
							tle_line2 = sr.ReadLine();
							tle1 = new Tle(line, tle_line1, tle_line2);
							sr.Close();
							return tle1;
						}
					}
				}
			}
			
			sr.Close();
			return null;
		}
		
		
	}//end class
}
