using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zeptomoby.OrbitTools;
using System.Threading;

namespace lrpt_places1
{
	public partial class MainForm : Form
	{
		SatellitePosCalcClass CurSatelliteCalc;
		ImageWorkerClass CurImageWorker;
		TimeProcClass CurTimeProc;
		TLEWorkerClass TLEWorker;
		KMLWorkerClass KMLWorker;
		
		string cur_tle_path = "";
		string image_path = "";
			
		DateTime start_time;
		DateTime start_date;
		DateTime full_start_time;

		double fight_duration = 0.0;//duration of image in seconds
		int cur_image_x_center;
		bool use_table_time = false;//true for log file
		int image_vert_shift = 0;
		int image_horizontal_shift = 0;

        TLEWorkerClass.SatelliteCode meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2;
		int draw_center_line = 0;
		int mark_size = 20;
		int font_size = 8;
		string html_cross_color = "#000000";
		string html_text_color  = "#000000";

        //Horizontal shift used in auto mode
        int AutoHorShiftMeteorM2 = 30;
        int AutoHorShiftMeteorM2_2 = -4;

        // Do not show not visible points
        bool FilterNotVisible = true;


        public MainForm()
		{
			InitializeComponent();
			CurImageWorker = new ImageWorkerClass();
			CurTimeProc = new TimeProcClass();
			TLEWorker = new TLEWorkerClass();
			KMLWorker = new KMLWorkerClass();
			
			full_start_time = new DateTime(1,1,1,0,0,0,0);
			start_time = new DateTime(1,1,1,0,0,0,0);
			start_date = new DateTime(2015,02,14,11,15,0,0);

            load_config_from_ini_file();

            string archive_path = Application.StartupPath + @"\Archive";
			TLEWorker.scan_archive_and_rename(archive_path);
            TLEWorker.CleanArchive(archive_path);

            textBox1.Text = "11:15:16.572\r\n00:13:38.708";
         	
         	CurSatelliteCalc = new SatellitePosCalcClass();

            string path = Application.StartupPath + @"\points1.kml";

            if (File.Exists(path) == false)
            {
                MessageBox.Show("File 'points1.kml' doesn't exist! \r\n Place it near App exe file.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            KMLWorker.load_KML(path);
         	
         	
         	set_form_controlls();

		}
		
		//process image
		void Button1Click(object sender, EventArgs e)
		{
            //DrawMarkersTask();
            Thread drawThread = new Thread(new ThreadStart(DrawMarkersTask));
            drawThread.Start();
        }

        void DrawMarkersTask()
        {
            int i;
            double latitude;
            double longitude;
            string point_name;
            bool rotate_image = false;


            if (CurImageWorker.image_loaded == false)
            {
                load_image(image_path);
            }

            image_vert_shift = Convert.ToInt32(textBox2.Text);
            image_horizontal_shift = Convert.ToInt32(textBox3.Text);

            update_program_param();
            update_full_time();

            if (cur_tle_path.Length > 5)
            {
                Tle cur_tle = TLEWorker.load_tle(cur_tle_path);
                if (cur_tle == null)
                {
                    MessageBox.Show("Problem with loading TLE info.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    CurSatelliteCalc.load_tle(cur_tle);
                }
            }
            else
            {
                MessageBox.Show("Bad TLE path.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                return;
            }

            if (CurImageWorker.cur_image_height <= 0)
                return;

            Invoke((MethodInvoker)delegate ()
            {
                label1.Text = "START";
            });
            
            Application.DoEvents();

            cur_image_x_center = CurImageWorker.cur_image_width / 2 - 1;

            CurSatelliteCalc.calculate_satellite_positions(
               full_start_time,
               fight_duration,
               CurImageWorker.cur_image_height,
               use_table_time);

            if (chkAutoHorizShift.Checked)//auto horizontal correction
            {
                int horizShiftValue = 0;//shift in pixels
                if (meteor_code == TLEWorkerClass.SatelliteCode.METEOR_M2)
                    horizShiftValue = AutoHorShiftMeteorM2;
                else if (meteor_code == TLEWorkerClass.SatelliteCode.METEOR_M2_2)
                    horizShiftValue = AutoHorShiftMeteorM2_2;

                if (CurSatelliteCalc.NtoS_fight)
                    image_horizontal_shift = -horizShiftValue;
                else
                    image_horizontal_shift = horizShiftValue;
            }

            if (CurSatelliteCalc.NtoS_fight == false)
                rotate_image = true;

            CurImageWorker.load_image(CurImageWorker.cur_image_path, rotate_image);//истинная загрузка изображения

            if (chkShowCenterLine.Checked)
                CurImageWorker.image_draw_center_line(1);

            int prev_percent = 0;
            for (i = 0; i < KMLWorker.geo_points_cnt; i++)
            {
                int percent = i * 100 / KMLWorker.geo_points_cnt;

                latitude = KMLWorker.geo_points[i].Latitude;
                longitude = KMLWorker.geo_points[i].Longitude;
                point_name = KMLWorker.geo_points[i].Name;

                //if (point_name == "Мекорьюк")
                if (point_name == "Москва")
                {
                    //point_name = "test";
                }
                draw_pos(latitude, longitude, point_name);

                if (percent != prev_percent)//if percent value has changed
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        label1.Text = $"WORK: {percent} %";
                    });
                }
                prev_percent = percent;
            }

            CurImageWorker.save_image();
            //label1.Text = "DONE";

            Invoke((MethodInvoker)delegate ()
            {
                label1.Text = "DONE";
            });
        }
		
		//draw geo point on image
		void draw_pos(double latitude, double longitude, string name)
		{
			int best_line = 0;
			int x_pose = 0;
			int y_pose = 0;
			int x_shift = 0;
			
			best_line = CurSatelliteCalc.find_best_line2(latitude,longitude);
			x_shift = calculate_x_shift_pos(best_line, latitude,longitude);
			
			//int y_shift = Convert.ToInt32(Math.Sin(0.0698)*x_shift);
			int y_shift = Convert.ToInt32(Math.Sin(0.07)*x_shift);
			
			if (CurSatelliteCalc.NtoS_fight == false)//flight "up"
			{
				y_shift = y_shift;
				x_shift = x_shift * (-1);
			}
			
			if (CurImageWorker.image_type == 1)
			{
				x_shift = Convert.ToInt32(Math.Tan(Math.PI/2 * (double)x_shift / 1102)*711);
			}
			x_pose = x_shift + cur_image_x_center;
			y_pose = best_line+y_shift+image_vert_shift;
			
			if (CurSatelliteCalc.NtoS_fight == false)//flight "up"
			{
				x_pose = CurImageWorker.cur_image_width - x_pose;
				y_pose = CurImageWorker.cur_image_height - y_pose;
			}

            // Revove marks that are outsides image
            if (FilterNotVisible)
            {
                if (x_pose < -mark_size)
                    return;

                if (x_pose > (CurImageWorker.cur_image_width + mark_size / 2))
                    return;
            }

			CurImageWorker.image_draw_cross(x_pose, y_pose, mark_size, 2, html_cross_color);

			//center cross
			if (draw_center_line == 1)
                CurImageWorker.image_draw_cross(cur_image_x_center, best_line+image_vert_shift, mark_size/2, 1, html_cross_color);

			CurImageWorker.image_draw_text(x_pose+(mark_size/2)+3,y_pose-6,name,html_text_color,font_size);
		}
				
		//load image
		void Button2Click(object sender, EventArgs e)
		{
			//image_path = "D:/METEOR_proc/MeteorEc.jpg";
			openFileDialog1.Filter = "Image Files (*.BMP;*.JPG)|*.BMP;*.JPG;";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				image_path = openFileDialog1.FileName;
				load_image(image_path);
				button5.Enabled = true;
				button1.Enabled = true;
				button3.Enabled = true;
   			}
			else
			{
				MessageBox.Show("Impossible to open file." ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
		}
		

		void load_image(string path)
		{
			//cur_image_worker.load_image(path);
			CurImageWorker.pre_load_image(path);
			label2.Text = "Image width: " + CurImageWorker.cur_image_width.ToString();
			label3.Text = "Image height: " + CurImageWorker.cur_image_height.ToString();
		
			
			label5.Text = "Image date: " + CurImageWorker.fileCreatedDate.ToShortDateString();
			label13.Text = "Image type: " + CurImageWorker.image_type_string;
			label15.Text = "Image name: " + CurImageWorker.cur_image_name;
		}
		
		//take date from calendar
		void Button4Click(object sender, EventArgs e)
		{
			start_date = monthCalendar1.SelectionStart.Date;
			update_full_time();
			label4.Text = "Image date present";
		}
		//take date from image
		void Button5Click(object sender, EventArgs e)
		{
			start_date = CurImageWorker.fileCreatedDate;
			update_full_time();
			label4.Text = "Image date present";
		}
		
		int calculate_x_shift_pos(int line, double latitude, double longitude)
		{
			Sat_geo_pos sat_pose = CurSatelliteCalc.image_positions[line];
			double dist = CurSatelliteCalc.find_2points_distance(sat_pose.Latitude,sat_pose.Longitude, latitude, longitude);//km

			double angle_rad = calculate_arc_angle(dist,sat_pose.Altitude);
			double pixel_offset_d = angle_rad/0.001225;//0.0012 - angular resolution of METEOR M2 //THIS IS WRONG, but working???
			//double pixel_offset_d = Math.Tan(angle_rad)/Math.Tan(0.0014);//0.0012 - angular resolution of METEOR M2
			
			//этот код не особо проверен!!!!!!!!
			
			if ((longitude > 0) && (sat_pose.Longitude > 0))//полностью западное полушарие
			{
				if (longitude < sat_pose.Longitude)
				{
					pixel_offset_d = pixel_offset_d*(-1);
				}
				pixel_offset_d = pixel_offset_d + image_horizontal_shift;//shift
			}
			else if ((longitude > 0) && (sat_pose.Longitude < 0))
			{
				if (longitude > sat_pose.Longitude)
				{
					pixel_offset_d = pixel_offset_d*(-1);
				}
				pixel_offset_d = pixel_offset_d + image_horizontal_shift;//shift
			}
			else if ((longitude <0) && (sat_pose.Longitude < 0))//полностью восточное полушарие
			{
				if (longitude < sat_pose.Longitude)
				{
					pixel_offset_d = pixel_offset_d*(-1);
				}
				pixel_offset_d = pixel_offset_d + image_horizontal_shift;//shift
			}
			else pixel_offset_d = 100000;//не будет отображено
			
			/*
			if (longitude < sat_pose.Longitude)//THIS IS BAD
			{
				pixel_offset_d = pixel_offset_d*(-1);
			}
			pixel_offset_d = pixel_offset_d + image_horizontal_shift;//shift
			*/

			return Convert.ToInt32(pixel_offset_d);
		}
		
		//angulal distance of "dist" at "height" on Earth
		double calculate_arc_angle(double dist, double height)
		{
			double earth_rad = 6378.1;
			double beta_angle = dist/earth_rad;//radians
			double l1 = Math.Cos(beta_angle)*earth_rad;
			double h = Math.Sin(beta_angle)*earth_rad;
			double l2 = earth_rad + height - l1;
			double alpha_angle = Math.Atan(h/l2);
			
			return alpha_angle;
		}
		
		//обновит параметры программы
		void update_program_param()
		{
			if (chkMeteorM2_2.Checked)
                meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2_2;
			else if (met_code_button2.Checked)
                meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2;
			TLEWorker.set_meteor_code(meteor_code);
			
			if (chkShowCenterLine.Checked)
                draw_center_line = 1;
            else
                draw_center_line = 0;
			
			mark_size = Convert.ToInt32(numericUpDown1.Value);//ширина креста
		}
		
		
		void update_full_time()
		{
            bool is_utc_time = false;
            

            //if (chkMeteorM2_2.Checked)//meteor 2-2
            //    is_utc_time = true;

            if (use_table_time)
			{
				full_start_time = CurTimeProc.create_full_utc(start_date, CurSatelliteCalc.table_start_time, is_utc_time);
				fight_duration = CurSatelliteCalc.table_fight_duration;
			}
			else
			{
				full_start_time = CurTimeProc.create_full_utc(start_date, start_time, is_utc_time);
				fight_duration = CurTimeProc.flight_duration;
			}
			label6.Text = "Full UTC Date and Time: " + full_start_time.ToString();
			label7.Text = "Flight duration: " + fight_duration.ToString() +" s";
		}
		
		//load time from MANUAL input
		void Button6Click(object sender, EventArgs e)
		{
			int result = CurTimeProc.fill_time_from_string(textBox1.Text);
			if (result != 1)
			{
				MessageBox.Show("Wrong time/duration data","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
			start_time = CurTimeProc.start_time;
			use_table_time = false;
			update_full_time();
		}
		
		//load time from STAT file
		void Button7Click(object sender, EventArgs e)
		{
			string stat_path = "";
			openFileDialog1.Filter = "STAT Files|*.stat";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				stat_path = openFileDialog1.FileName;
				update_time_from_stat_file(stat_path);
				label14.Text = "Image time present";
				update_full_time();
   			}
			else
			{
				MessageBox.Show("Impossible to open file: " + stat_path ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
		}
		
		
		void update_time_from_stat_file(string path)
		{
			StreamReader sr = new StreamReader(path);
			string stat_str = sr.ReadToEnd();
			int result = CurTimeProc.fill_time_from_string(stat_str);
			if (result != 1)
			{
				MessageBox.Show("Wrong time/duration data","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
			start_time = CurTimeProc.start_time;
			sr.Close();
			use_table_time = false;
		}
		
		void load_config_from_ini_file()
		{
			IniParser parser = new IniParser(Application.StartupPath+@"\config.ini");
			mark_size = Convert.ToInt32(parser.GetSetting("GRAPHICS", "cross_size"));
			font_size = Convert.ToInt32(parser.GetSetting("GRAPHICS", "font_size"));
			draw_center_line = Convert.ToInt32(parser.GetSetting("GRAPHICS", "draw_center_line"));
			html_cross_color = parser.GetSetting("GRAPHICS", "cross_color");
			html_text_color =  parser.GetSetting("GRAPHICS", "text_color");

            int filter_not_vis = Convert.ToInt32(parser.GetSetting("GRAPHICS", "filter_not_visible"));
            if (filter_not_vis == 1)
                FilterNotVisible = true;
            else
                FilterNotVisible = false;

            int archive_lenth_days = Convert.ToInt32(parser.GetSetting("TLE", "archive_length_days"));
            TLEWorker.SetArchiveLength(archive_lenth_days);

            AutoHorShiftMeteorM2 = Convert.ToInt32(parser.GetSetting("SHIFT", "meteor_m_2"));
            AutoHorShiftMeteorM2_2 = Convert.ToInt32(parser.GetSetting("SHIFT", "meteor_m_2_2"));
        }
		
		//загрузить в элемены формы значения
		void set_form_controlls()
		{
			numericUpDown1.Value = mark_size;
			if (draw_center_line == 0) chkShowCenterLine.Checked = false; else chkShowCenterLine.Checked = true;
		}
		
		//load time from LOG file
		void Button8Click(object sender, EventArgs e)
		{
			//cur_satellite_calc.load_time_table("D:/METEOR_proc/Meteor_table.log");
			string log_path = "";
			openFileDialog1.Filter = "LOG Files|*.log";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				log_path = openFileDialog1.FileName;
				CurSatelliteCalc.load_time_table(log_path);
				use_table_time = true;
				label14.Text = "Image time present";
				update_full_time();
   			}
			else
			{
				MessageBox.Show("Impossible to open file: " + log_path ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}		
		}
		
		void load_TLE_data()
		{
			string archive_path = Application.StartupPath + @"\Archive";
			string best_tle_path = TLEWorker.find_best_tle(archive_path,full_start_time);
			string best_tle_filename = Path.GetFileName(best_tle_path);
			cur_tle_path = best_tle_path;
			label8.Text = "TLE name: " + best_tle_filename;
			label9.Text = "TLE days difference: " + TLEWorker.cur_min_diff.ToString();
		}
		
		//load TLE from archive
		void Button9Click(object sender, EventArgs e)
		{
			load_TLE_data();
		}
		
		
		
		//auto process
		void Button3Click(object sender, EventArgs e)
		{
			start_date = CurImageWorker.fileCreatedDate;
			
            int path_lng = CurImageWorker.cur_image_path.Length;
            string name_whithout_ext = CurImageWorker.cur_image_path.Remove(path_lng - 4, 4);

            string stat_path1 = CurImageWorker.cur_image_path + ".stat";
            string stat_path2 = name_whithout_ext + ".stat";

            string stat_path = "";

            if (File.Exists(stat_path1))
                stat_path = stat_path1;
            else if (File.Exists(stat_path2))
                stat_path = stat_path2;
            else
			{
				MessageBox.Show("Can not open STAT file near image!","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
				return;
			}

			update_time_from_stat_file(stat_path);
			update_full_time();
			
			load_TLE_data();
			button1.PerformClick();
		}

        private void Met_code_button2_CheckedChanged(object sender, EventArgs e)
        {
            update_full_time();
        }

        private void ChkMeteorM2_2_CheckedChanged(object sender, EventArgs e)
        {
            update_full_time();
        }
    }
}
