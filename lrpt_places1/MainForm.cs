using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zeptomoby.OrbitTools;


namespace lrpt_places1
{
	public partial class MainForm : Form
	{
		Satellite_pos_calc cur_satellite_calc;
		image_worker cur_image_worker;
		Time_proc cur_time_proc;
		TLE_worker cur_tle_worker;
		KML_worker cur_kml_worker;
		
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
		
		int meteor_code = 2;
		int draw_center_line = 0;
		int mark_size = 20;
		int font_size = 8;
		string html_cross_color = "#000000";
		string html_text_color  = "#000000";
		
		public MainForm()
		{
			InitializeComponent();
			cur_image_worker = new image_worker();
			cur_time_proc = new Time_proc();
			cur_tle_worker = new TLE_worker();
			cur_kml_worker = new KML_worker();
			
			full_start_time = new DateTime(1,1,1,0,0,0,0);
			start_time = new DateTime(1,1,1,0,0,0,0);
			start_date = new DateTime(2015,02,14,11,15,0,0);
			
			string archive_path = Application.StartupPath + @"\Archive";
			cur_tle_worker.scan_archive_and_rename(archive_path);
			  	
         	textBox1.Text = "11:15:16.572\r\n00:13:38.708";
         	
         	cur_satellite_calc = new Satellite_pos_calc();

            string path = Application.StartupPath + @"\points1.kml";

            if (File.Exists(path) == false)
            {
                MessageBox.Show("File 'points1.kml' doesn't exist! \r\n Place it near App exe file.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            cur_kml_worker.load_KML(path);
         	
         	load_config_from_ini_file();
         	set_form_controlls();

		}
		
		//process image
		void Button1Click(object sender, EventArgs e)
		{
			int i;
			double latitude;
			double longitude;
			string point_name;
			bool rotate_image = false;
			
			
			if (cur_image_worker.image_loaded == false)
			{
				load_image(image_path);
			}
			
			image_vert_shift = Convert.ToInt32(textBox2.Text);
			image_horizontal_shift = Convert.ToInt32(textBox3.Text);
			
			
			update_program_param();

			if (cur_tle_path.Length > 5)
			{
				Tle cur_tle = cur_tle_worker.load_tle(cur_tle_path);
				if (cur_tle == null)
				{
					MessageBox.Show("Problem with loading TLE info.","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
					return;
				}
				else
				{
					cur_satellite_calc.load_tle(cur_tle);
				}
			}
			else 
			{
				MessageBox.Show("Bad TLE path.","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
				return;
			}
			

         	if ((cur_image_worker.cur_image_height > 0))
         	{
         		
         		label1.Text = "START";
         		Application.DoEvents();
			
         		cur_image_x_center = cur_image_worker.cur_image_width / 2 - 1;

         		cur_satellite_calc.calculate_satellite_positions(full_start_time,fight_duration,cur_image_worker.cur_image_height,use_table_time);
         		
         		
         		if (checkBox1.Checked)//auto horizontal correction
         		{
         			if (cur_satellite_calc.NtoS_fight) {image_horizontal_shift = -30;}
         			else 
         			{
         				image_horizontal_shift = 30;
         			}
         		}

         		
         		if (cur_satellite_calc.NtoS_fight == false) rotate_image = true;
         		
         		cur_image_worker.load_image(cur_image_worker.cur_image_path,rotate_image);//истинная загрузка изображения
         		
         		if (checkBox2.Checked) cur_image_worker.image_draw_center_line(1);
         		
         		for (i=0;i<cur_kml_worker.geo_points_cnt;i++)
         		{
         			latitude = cur_kml_worker.geo_points[i].Latitude;
         			longitude = cur_kml_worker.geo_points[i].Longitude;
         			point_name = cur_kml_worker.geo_points[i].Name;
         			
         			
         			//if (point_name == "Мекорьюк")
         			if (point_name == "Москва")
         			{
         				//point_name = "test";
         			}
         			draw_pos(latitude, longitude,point_name);
         		}
         		
			    cur_image_worker.save_image();
			    label1.Text = "DONE";
         	}
		}
		
		//draw geo point on image
		void draw_pos(double latitude, double longitude, string name)
		{
			int best_line = 0;
			int x_pose = 0;
			int y_pose = 0;
			int x_shift = 0;
			
			
			best_line = cur_satellite_calc.find_best_line2(latitude,longitude);
			x_shift = calculate_x_shift_pos(best_line, latitude,longitude);
			
			//int y_shift = Convert.ToInt32(Math.Sin(0.0698)*x_shift);
			int y_shift = Convert.ToInt32(Math.Sin(0.07)*x_shift);
			
			if (cur_satellite_calc.NtoS_fight == false)//flight "up"
			{
				//y_shift = y_shift * (-1);
				y_shift = y_shift;
				x_shift = x_shift * (-1);
			}
			
			if (cur_image_worker.image_type == 1)
			{
				x_shift = Convert.ToInt32(Math.Tan(Math.PI/2 * (double)x_shift / 1102)*711);
			}
			x_pose = x_shift + cur_image_x_center;
			y_pose = best_line+y_shift+image_vert_shift;
			
			if (cur_satellite_calc.NtoS_fight == false)//flight "up"
			{
				x_pose = cur_image_worker.cur_image_width - x_pose;
				y_pose = cur_image_worker.cur_image_height - y_pose;
			}
			cur_image_worker.image_draw_cross(x_pose,y_pose,mark_size,2,html_cross_color);
			//center cross
			if (draw_center_line == 1) cur_image_worker.image_draw_cross(cur_image_x_center,best_line+image_vert_shift,mark_size/2,1,html_cross_color);
			cur_image_worker.image_draw_text(x_pose+(mark_size/2)+3,y_pose-6,name,html_text_color,font_size);
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
			cur_image_worker.pre_load_image(path);
			label2.Text = "Image width: " + cur_image_worker.cur_image_width.ToString();
			label3.Text = "Image height: " + cur_image_worker.cur_image_height.ToString();
		
			
			label5.Text = "Image date: " + cur_image_worker.fileCreatedDate.ToShortDateString();
			label13.Text = "Image type: " + cur_image_worker.image_type_string;
			label15.Text = "Image name: " + cur_image_worker.cur_image_name;
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
			start_date = cur_image_worker.fileCreatedDate;
			update_full_time();
			label4.Text = "Image date present";
		}
		
		int calculate_x_shift_pos(int line, double latitude, double longitude)
		{
			Sat_geo_pos sat_pose = cur_satellite_calc.image_positions[line];
			double dist = cur_satellite_calc.find_2points_distance(sat_pose.Latitude,sat_pose.Longitude, latitude, longitude);//km

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
			if (met_code_button22.Checked)
                meteor_code = 21;
			else if (met_code_button2.Checked)
                meteor_code = 2;
			cur_tle_worker.set_meteor_code(meteor_code);
			
			if (checkBox2.Checked)
                draw_center_line = 1;
            else
                draw_center_line = 0;
			
			mark_size = Convert.ToInt32(numericUpDown1.Value);//ширина креста
		}
		
		

		
		void update_full_time()
		{
			if (use_table_time)
			{
				full_start_time = cur_time_proc.create_full_utc(start_date,cur_satellite_calc.table_start_time);
				fight_duration = cur_satellite_calc.table_fight_duration;
			}
			else
			{
				full_start_time = cur_time_proc.create_full_utc(start_date,start_time);
				fight_duration = cur_time_proc.flight_duration;
			}
			label6.Text = "Full UTC Date and Time: " + full_start_time.ToString();
			label7.Text = "Flight duration: " + fight_duration.ToString() +" s";
		}
		
		//load time from MANUAL input
		void Button6Click(object sender, EventArgs e)
		{
			int result = cur_time_proc.fill_time_from_string(textBox1.Text);
			if (result != 1)
			{
				MessageBox.Show("Wrong time/duration data","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
			start_time = cur_time_proc.start_time;
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
			int result = cur_time_proc.fill_time_from_string(stat_str);
			if (result != 1)
			{
				MessageBox.Show("Wrong time/duration data","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
			start_time = cur_time_proc.start_time;
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
		}
		
		//загрузить в элемены формы значения
		void set_form_controlls()
		{
			numericUpDown1.Value = mark_size;
			if (draw_center_line == 0) checkBox2.Checked = false; else checkBox2.Checked = true;
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
				cur_satellite_calc.load_time_table(log_path);
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
			string best_tle_path = cur_tle_worker.find_best_tle(archive_path,full_start_time);
			string best_tle_filename = Path.GetFileName(best_tle_path);
			cur_tle_path = best_tle_path;
			label8.Text = "TLE name: " + best_tle_filename;
			label9.Text = "TLE days difference: " + cur_tle_worker.cur_min_diff.ToString();
		}
		
		//load TLE from archive
		void Button9Click(object sender, EventArgs e)
		{
			load_TLE_data();
		}
		
		
		
		//auto process
		void Button3Click(object sender, EventArgs e)
		{
			start_date = cur_image_worker.fileCreatedDate;
			string stat_path = cur_image_worker.cur_image_path + ".stat";
			
			if (File.Exists(stat_path) == false)
			{
				MessageBox.Show("Can not open STAT file near image!","ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
				return;
			}
			update_time_from_stat_file(stat_path);
			update_full_time();
			
			load_TLE_data();
			button1.PerformClick();
		}
		
				
	}
}
