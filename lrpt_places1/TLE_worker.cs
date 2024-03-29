﻿using System;
using System.IO;
using Zeptomoby.OrbitTools;

namespace lrpt_places1
{
	public class TLEWorkerClass
	{
        public enum SatelliteCode
        {
            METEOR_M1 = 1,
            METEOR_M2,
            METEOR_M2_2,
            METEOR_M2_3,
            METEOR_M2_4,
        };

        public int cur_min_diff = 10000;
		public SatelliteCode meteor_code = SatelliteCode.METEOR_M2;
        int archive_lenth_days = 10000;//length of the archive in days


        public TLEWorkerClass()
		{
		}
		
		/// <summary>
		/// Select Meteor's name to work with
		/// </summary>
		public void SetMeteorCode(SatelliteCode code)
		{
			meteor_code = code;
		}
		
		/// <summary>
		/// Scan TLE archive and rename files: "archive_XXXXX"
		/// </summary>
		public void ScanArchiveAndRename(string archive_path)
		{
            if (Directory.Exists(archive_path) == false)
            {
                Directory.CreateDirectory(archive_path);
            }

			string[] dirs = Directory.GetFiles(archive_path, "*.txt");
			string cur_filename;
			string cur_filepath;
			string new_filename;
			string new_filepath;
			int files_cnt = dirs.Length;
			int i;
			
			for (i=0; i < files_cnt; i++)
			{
				cur_filename = Path.GetFileName(dirs[i]);
				cur_filepath = dirs[i];
				if ((cur_filename.Contains("archive")) || (cur_filename.Contains("skip")))
				{
					//this file should be skipped
				}
				else
				{
					//this file should be analysed and renamed
					new_filename = AnalyseFile(cur_filepath);
					new_filepath = archive_path + @"\" + new_filename;

                    // Такой файл уже есть в архиве, удаляем его
                    if (File.Exists(new_filepath))
                    {
                        File.Delete(new_filepath);
                    }
                    System.IO.File.Move(cur_filepath, new_filepath);
                }
			}
		}
		
		/// <summary>
		/// Analyse file for containing METEOR M2 TLE and create it's new name
		/// </summary>
		public string AnalyseFile(string file_path)
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
				if (meteor_code == SatelliteCode.METEOR_M1)
				{
					if (line.Contains("M1") || line.Contains("M 1"))
					{
						tle_line1 = sr.ReadLine();
						tle_line2 = sr.ReadLine();
						tle1 = new Tle(line, tle_line1, tle_line2);
						str_epoch = tle1.Epoch;
						str_epoch = RemoveFraction(str_epoch);
						new_filename = "archive_"+ str_epoch+ ".txt";
						sr.Close();
						return new_filename;
					}
				}
				if (line.Contains("METEOR"))
				{
                    if (meteor_code == SatelliteCode.METEOR_M2)
                    {
                        if (line.Contains("M2") || line.Contains("M 2"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            str_epoch = tle1.Epoch;
                            str_epoch = RemoveFraction(str_epoch);
                            new_filename = "archive_" + str_epoch + ".txt";
                            sr.Close();
                            return new_filename;
                        }
                    }
                    else if (meteor_code == SatelliteCode.METEOR_M2_2)
                    {
                        if (line.Contains("M2 2"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            str_epoch = tle1.Epoch;
                            str_epoch = RemoveFraction(str_epoch);
                            new_filename = "archive_" + str_epoch + ".txt";
                            sr.Close();
                            return new_filename;
                        }
                    }
                    else if (meteor_code == SatelliteCode.METEOR_M2_3)
                    {
                        if (line.Contains("M2 3"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            str_epoch = tle1.Epoch;
                            str_epoch = RemoveFraction(str_epoch);
                            new_filename = "archive_" + str_epoch + ".txt";
                            sr.Close();
                            return new_filename;
                        }
                    }
                    else if (meteor_code == SatelliteCode.METEOR_M2_4)
                    {
                        if (line.Contains("M2 4"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            str_epoch = tle1.Epoch;
                            str_epoch = RemoveFraction(str_epoch);
                            new_filename = "archive_" + str_epoch + ".txt";
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
		public string RemoveFraction(string text)
		{
			string new_text = "";
			int pos = text.IndexOf(".");
			new_text = text.Substring(0,pos);
			
			return new_text;
		}
		
		/// <summary>
		/// Find epoch value in string
		/// </summary>
		public int GetEpochDay(string text)
		{
			int pos = text.IndexOf("_");
			string new_text = text.Remove(0, pos+1);
			new_text = new_text.Remove(new_text.Length-4, 4);
			
			string year = new_text.Substring(0, 2);
			string day = new_text.Substring(2, 3);
			
			return Convert.ToInt32(year) * 365 + Convert.ToInt32(day);
		}
		
		/// <summary>
		/// Scan archive and find best TLE file by its name
		/// </summary>
		public string FindBest_TLE(string archive_path, DateTime image_date)
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
			
			image_epoch = ConvertDateToEpoch(image_date);
			
			for (i = 0; i < files_cnt; i++)
			{
				cur_filename = Path.GetFileName(dirs[i]);
				if (cur_filename.Contains("skip"))
				{
					//this file should be skipped
				}
				else
				{
					cur_epoch = GetEpochDay(cur_filename);
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
		
		public int ConvertDateToEpoch(DateTime start_date)
		{
			if (start_date.Year < 2002)
             return 0;
			Julian cur_julian = new Julian(start_date);
			double epoch = (cur_julian.Date - 2451545.0);
			
			return Convert.ToInt32(epoch);
		}
		
		//open file "tle_path", find METEOR TLE
		//return TLE for METEOR
		public Tle Load_TLE(string tle_path)
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
					if (meteor_code == SatelliteCode.METEOR_M1)
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
					else if (meteor_code == SatelliteCode.METEOR_M2)
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
                    else if (meteor_code == SatelliteCode.METEOR_M2_2)
                    {
                        if (line.Contains("M2 2"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            sr.Close();
                            return tle1;
                        }
                    }
                    else if (meteor_code == SatelliteCode.METEOR_M2_3)
                    {
                        if (line.Contains("M2 3"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            sr.Close();
                            return tle1;
                        }
                    }
                    else if (meteor_code == SatelliteCode.METEOR_M2_4)
                    {
                        if (line.Contains("M2 4"))
                        {
                            tle_line1 = sr.ReadLine();
                            tle_line2 = sr.ReadLine();
                            tle1 = new Tle(line, tle_line1, tle_line2);
                            sr.Close();
                            return tle1;
                        }
                    }
                }//meteor
			}
			
			sr.Close();
			return null;
		}

        /// <summary>
        /// Delete old files
        /// </summary>
        public void CleanArchive(string archive_path)
        {
            string[] dirs = Directory.GetFiles(archive_path, "*.txt");
            string cur_filename;
            int i;
            int today_epoch = ConvertDateToEpoch(DateTime.Now);
            int files_cnt = dirs.Length;

            for (i = 0; i < files_cnt; i++)
            {
                cur_filename = Path.GetFileName(dirs[i]);
                if (cur_filename.Contains("skip"))
                {
                    //this file should be skipped
                }
                else
                {
                    int cur_file_epoch = GetEpochDay(cur_filename);
                    int diff_days = today_epoch - cur_file_epoch;
                    if (diff_days > archive_lenth_days)
                    {
                        try
                        {
                            File.Delete(dirs[i]);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public void SetArchiveLength(int length)
        {
            archive_lenth_days = length;
        }
		
		
	}//end class
}
