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

		double fight_duration_s = 0.0;//duration of image in seconds
		int cur_image_x_center_pix;
		bool use_table_time = false;//true for log file
		int image_vert_shift_pix = 0;
		int image_horiz_shift_pix = 0;

        TLEWorkerClass.SatelliteCode meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2;
		int draw_center_line = 0;
		int mark_size = 20;
		int font_size = 8;
		string html_cross_color = "#000000";
		string html_text_color  = "#000000";
        int TimezoneHours = 3;//+3 - moscow

        // Horizontal shift used in auto mode
        int AutoHorShiftMeteorM2 = 30;
        int AutoHorShiftMeteorM2_2 = -4;
        int AutoHorShiftMeteorM2_3 = -4;

        // Do not show not visible points
        bool FilterNotVisible = true;

        //***************************************************************************

        public MainForm()
		{
			InitializeComponent();
			CurImageWorker = new ImageWorkerClass();
			
			TLEWorker = new TLEWorkerClass();
			KMLWorker = new KMLWorkerClass();
            CurSatelliteCalc = new SatellitePosCalcClass();

            full_start_time = new DateTime(1,1,1,0,0,0,0);
			start_time = new DateTime(1,1,1,0,0,0,0);
			start_date = new DateTime(2023,01,01,23,00,0,0);

            LoadConfigFromIniFile();
            CurTimeProc = new TimeProcClass(TimezoneHours);

            string archive_path = Application.StartupPath + @"\Archive";
			TLEWorker.ScanArchiveAndRename(archive_path);
            TLEWorker.CleanArchive(archive_path);

            txtCurrTimeManual.Text = "11:15:16.572\r\n00:13:38.708";

            string path = Application.StartupPath + @"\points1.kml";

            if (File.Exists(path) == false)
            {
                MessageBox.Show("File 'points1.kml' doesn't exist! \r\n Place it near App exe file.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            KMLWorker.Load_KML(path);
         	SetFormControlls();
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
                LoadImage(image_path);
            }

            image_vert_shift_pix = Convert.ToInt32(txtVertShift.Text);
            image_horiz_shift_pix = Convert.ToInt32(txtHorizShift.Text);

            UpdateProgramParam();
            UpdateFullTime();

            if (cur_tle_path.Length < 6)
            {
                MessageBox.Show("Bad TLE path.", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                return;
            }

            Tle cur_tle = TLEWorker.Load_TLE(cur_tle_path);
            if (cur_tle == null)
            {
                MessageBox.Show("Problem with loading TLE info.", 
                    "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
                return;
            }
            else
            {
                CurSatelliteCalc.Load_TLE(cur_tle);
            }

            if (CurImageWorker.cur_image_height <= 0)
                return;

            Invoke((MethodInvoker)delegate ()
            {
                lblProcState.Text = "START";
            });
            
            Application.DoEvents();

            cur_image_x_center_pix = CurImageWorker.cur_image_width / 2 - 1;

            CurSatelliteCalc.CalculateSatellitePositions(
               full_start_time,
               fight_duration_s,
               CurImageWorker.cur_image_height,
               use_table_time);

            if (CurSatelliteCalc.NtoS_fight == false)
                rotate_image = true;

            // Real image load
            CurImageWorker.LoadImage(CurImageWorker.cur_image_path, rotate_image);

            if (chkShowCenterLine.Checked)
                CurImageWorker.ImageDrawCenterLine(1);

            //DRAWING CYCLE!

            int prev_percent = 0;
            //Check every point from KML file
            for (i = 0; i < KMLWorker.geo_points_cnt; i++)
            {
                int percent = i * 100 / KMLWorker.geo_points_cnt;

                latitude = KMLWorker.geo_points[i].Latitude;
                longitude = KMLWorker.geo_points[i].Longitude;
                point_name = KMLWorker.geo_points[i].Name;

                DrawMarkAtPos(latitude, longitude, point_name);

                if (percent != prev_percent)//if percent value has changed
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        lblProcState.Text = $"WORK: {percent} %";
                    });
                }
                prev_percent = percent;
            }

            CurImageWorker.SaveImage();

            Invoke((MethodInvoker)delegate ()
            {
                lblProcState.Text = "DONE";
            });
        }

        /// <summary>
        /// Draw geo point on image, will not drawn if it is not wisible
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="name"></param>
        void DrawMarkAtPos(double latitude, double longitude, string name)
		{
			int best_line = 0;
			int x_pose_pix = 0;
			int y_pose_pix = 0;
			int x_shift = 0;
			
			best_line = CurSatelliteCalc.FindBestLine2(latitude, longitude);
			x_shift = CalculateX_ShiftPos(best_line, latitude, longitude);
			
			//int y_shift = Convert.ToInt32(Math.Sin(0.0698)*x_shift);
			int y_shift = Convert.ToInt32(Math.Sin(0.07)*x_shift);
			
			if (CurSatelliteCalc.NtoS_fight == false)//flight "up"
			{
				//y_shift = y_shift;
				x_shift = x_shift * (-1);
			}
			
			if (CurImageWorker.image_type == 1)
			{
				x_shift = Convert.ToInt32(Math.Tan(Math.PI/2 * (double)x_shift / 1102)*711);
			}
			x_pose_pix = x_shift + cur_image_x_center_pix;
			y_pose_pix = best_line+y_shift+image_vert_shift_pix;
			
			if (CurSatelliteCalc.NtoS_fight == false)//flight "up"
			{
				x_pose_pix = CurImageWorker.cur_image_width - x_pose_pix;
				y_pose_pix = CurImageWorker.cur_image_height - y_pose_pix;
			}

            // Revove marks that are outsides image
            if (FilterNotVisible)
            {
                if (x_pose_pix < -mark_size)
                    return;

                if (x_pose_pix > (CurImageWorker.cur_image_width + mark_size / 2))
                    return;
            }

			CurImageWorker.ImageDrawCross(x_pose_pix, y_pose_pix, mark_size, 2, html_cross_color);

			//center cross
			if (draw_center_line == 1)
                CurImageWorker.ImageDrawCross(cur_image_x_center_pix, best_line+image_vert_shift_pix, mark_size/2, 1, html_cross_color);

			CurImageWorker.ImageDrawText(x_pose_pix+(mark_size/2)+3,y_pose_pix-6,name,html_text_color,font_size);
		}

        void LoadImage(string path)
        {
            //cur_image_worker.load_image(path);
            CurImageWorker.PreLoadImage(path);
            lblImageWidth.Text = "Image width: " + CurImageWorker.cur_image_width.ToString();
            lblImageHeight.Text = "Image height: " + CurImageWorker.cur_image_height.ToString();

            lblImageDateMain.Text = "Image date: " + CurImageWorker.file_created_date.ToShortDateString();
            lblImageType.Text = "Image type: " + CurImageWorker.image_type_string;
            lblImageName.Text = "Image name: " + CurImageWorker.cur_image_name;
        }

        int CalculateX_ShiftPos(int line, double latitude, double longitude)
        {
            Sat_geo_pos sat_pose = CurSatelliteCalc.satellite_positions[line];
            double dist = CurSatelliteCalc.Find_2pointsDistance(sat_pose.Latitude, sat_pose.Longitude, latitude, longitude);//km

            double angle_rad = CalculateArcAngleRad(dist, sat_pose.Altitude);
            double pixel_offset_d = angle_rad / 0.001225;//0.0012 - angular resolution of METEOR M2 //THIS IS WRONG, but working???

            //этот код не особо проверен!!!!!!!!

            if ((longitude > 0) && (sat_pose.Longitude > 0))//полностью западное полушарие
            {
                if (longitude < sat_pose.Longitude)
                {
                    pixel_offset_d = pixel_offset_d * (-1);
                }
                pixel_offset_d = pixel_offset_d + image_horiz_shift_pix;//shift
            }
            else if ((longitude > 0) && (sat_pose.Longitude < 0))
            {
                if (longitude > sat_pose.Longitude)
                {
                    pixel_offset_d = pixel_offset_d * (-1);
                }
                pixel_offset_d = pixel_offset_d + image_horiz_shift_pix;//shift
            }
            else if ((longitude < 0) && (sat_pose.Longitude < 0))//полностью восточное полушарие
            {
                if (longitude < sat_pose.Longitude)
                {
                    pixel_offset_d = pixel_offset_d * (-1);
                }
                pixel_offset_d = pixel_offset_d + image_horiz_shift_pix;//shift
            }
            else pixel_offset_d = 100000;//не будет отображено

            return Convert.ToInt32(pixel_offset_d);
        }

        /// <summary>
        /// Angular distance of "dist" at "height" on Earth
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="height"></param>
        /// <returns>Angle in rad</returns>
        double CalculateArcAngleRad(double dist, double height)
        {
            double earth_rad = 6378.1;
            double beta_angle = dist / earth_rad;//radians
            double l1 = Math.Cos(beta_angle) * earth_rad;
            double h = Math.Sin(beta_angle) * earth_rad;
            double l2 = earth_rad + height - l1;
            double alpha_angle = Math.Atan(h / l2);

            return alpha_angle;
        }

        //Update utility parametrs
        void UpdateProgramParam()
        {
            if (chkMeteorM2_2.Checked)
                meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2_2;
            else if (chkMeteorM2_0.Checked)
                meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2;
            else if (chkMeteorM2_3.Checked)
                meteor_code = TLEWorkerClass.SatelliteCode.METEOR_M2_3;

            if (chkAutoHorizShift.Checked)//auto horizontal correction
            {
                int horizShiftValue = 0;//shift in pixels
                if (meteor_code == TLEWorkerClass.SatelliteCode.METEOR_M2)
                    horizShiftValue = AutoHorShiftMeteorM2;
                else if (meteor_code == TLEWorkerClass.SatelliteCode.METEOR_M2_2)
                    horizShiftValue = AutoHorShiftMeteorM2_2;
                else if (meteor_code == TLEWorkerClass.SatelliteCode.METEOR_M2_3)
                    horizShiftValue = AutoHorShiftMeteorM2_3;

                if (CurSatelliteCalc.NtoS_fight)
                    image_horiz_shift_pix = -horizShiftValue;
                else
                    image_horiz_shift_pix = horizShiftValue;
            }

            TLEWorker.SetMeteorCode(meteor_code);

            if (chkShowCenterLine.Checked)
                draw_center_line = 1;
            else
                draw_center_line = 0;

            mark_size = Convert.ToInt32(nudMarkSize.Value);//ширина креста
        }


        void UpdateFullTime()
        {
            bool is_utc_time = false;

            if (use_table_time)
            {
                full_start_time = CurTimeProc.CreateFull_UTC(
                    start_date, CurSatelliteCalc.table_start_time, is_utc_time);
                fight_duration_s = CurSatelliteCalc.table_fight_duration;
            }
            else
            {
                full_start_time = CurTimeProc.CreateFull_UTC(start_date, start_time, is_utc_time);
                fight_duration_s = CurTimeProc.flight_duration;
            }
            lblUTC_FullDateTime.Text = "Full UTC Date and Time: " + full_start_time.ToString();
            lblFlightDuration.Text = "Flight duration: " + fight_duration_s.ToString() + " s";
        }

        void UpdateTimeFromStatFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            string stat_str = sr.ReadToEnd();
            int result = CurTimeProc.FillTimeFromString(stat_str);
            if (result != 1)
            {
                MessageBox.Show("Wrong time/duration data", "ERROR!", 0, System.Windows.Forms.MessageBoxIcon.Stop);
            }
            start_time = CurTimeProc.start_time;
            sr.Close();
            use_table_time = false;
        }

        void LoadConfigFromIniFile()
        {
            IniParser parser = new IniParser(Application.StartupPath + @"\config.ini");
            mark_size = Convert.ToInt32(parser.GetSetting("GRAPHICS", "cross_size"));
            font_size = Convert.ToInt32(parser.GetSetting("GRAPHICS", "font_size"));
            draw_center_line = Convert.ToInt32(parser.GetSetting("GRAPHICS", "draw_center_line"));
            html_cross_color = parser.GetSetting("GRAPHICS", "cross_color");
            html_text_color = parser.GetSetting("GRAPHICS", "text_color");
            TimezoneHours = Convert.ToInt32(parser.GetSetting("TIME", "timezone"));

            int filter_not_vis = Convert.ToInt32(parser.GetSetting("GRAPHICS", "filter_not_visible"));
            if (filter_not_vis == 1)
                FilterNotVisible = true;
            else
                FilterNotVisible = false;

            int archive_lenth_days = Convert.ToInt32(parser.GetSetting("TLE", "archive_length_days"));
            TLEWorker.SetArchiveLength(archive_lenth_days);

            AutoHorShiftMeteorM2 = Convert.ToInt32(parser.GetSetting("SHIFT", "meteor_m_2"));
            AutoHorShiftMeteorM2_2 = Convert.ToInt32(parser.GetSetting("SHIFT", "meteor_m_2_2"));
            AutoHorShiftMeteorM2_3 = Convert.ToInt32(parser.GetSetting("SHIFT", "meteor_m_2_3"));
        }

        /// <summary>
        /// Load values to some form controls
        /// </summary>
        void SetFormControlls()
        {
            nudMarkSize.Value = mark_size;
            if (draw_center_line == 0)
                chkShowCenterLine.Checked = false;
            else
                chkShowCenterLine.Checked = true;
        }

        void LoadTLE_Data()
        {
            string archive_path = Application.StartupPath + @"\Archive";
            string best_tle_path = TLEWorker.FindBest_TLE(archive_path, full_start_time);
            string best_tle_filename = Path.GetFileName(best_tle_path);
            cur_tle_path = best_tle_path;
            lblTLE_Name.Text = "TLE name: " + best_tle_filename;
            lblTLE_DiffTime.Text = "TLE days difference: " + TLEWorker.cur_min_diff.ToString();
        }

        //#################################################################################

        //process image
        void btnStartProcessClick(object sender, EventArgs e)
        {
            //DrawMarkersTask();
            Thread drawThread = new Thread(new ThreadStart(DrawMarkersTask));
            drawThread.Start();
        }

        //load image
        void btnOpenImageClick(object sender, EventArgs e)
		{
			//image_path = "D:/METEOR_proc/MeteorEc.jpg";
			openFileDialog1.Filter = "Image Files (*.BMP;*.JPG)|*.BMP;*.JPG;";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				image_path = openFileDialog1.FileName;
				LoadImage(image_path);
				btnTakeFromImage.Enabled = true;
				btnStartProcess.Enabled = true;
				btnAutoProcess.Enabled = true;
   			}
			else
			{
				MessageBox.Show("Impossible to open file." ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
		}
		
		//take date from calendar
		void btnTakeFromCalendarClick(object sender, EventArgs e)
		{
			start_date = monthCalendar1.SelectionStart.Date;
			UpdateFullTime();
			lblImageDate.Text = "Image date present";
		}
		//take date from image
		void btnTakeFromImageClick(object sender, EventArgs e)
		{
			start_date = CurImageWorker.file_created_date;
			UpdateFullTime();
			lblImageDate.Text = "Image date present";
		}
		
		//load time from MANUAL input
		void btnManualTimeInputClick(object sender, EventArgs e)
		{
			int result = CurTimeProc.FillTimeFromString(txtCurrTimeManual.Text);
			if (result != 1)
			{
				MessageBox.Show(
                    "Wrong time/duration data","ERROR!",0,
                    System.Windows.Forms.MessageBoxIcon.Stop);
			}
			start_time = CurTimeProc.start_time;
			use_table_time = false;
			UpdateFullTime();
		}
		
		//load time from STAT file
		void btnTakeFromStatFileClick(object sender, EventArgs e)
		{
			string stat_path = "";
			openFileDialog1.Filter = "STAT Files|*.stat";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				stat_path = openFileDialog1.FileName;
				UpdateTimeFromStatFile(stat_path);
				lblImageTimeInfo.Text = "Image time present";
				UpdateFullTime();
   			}
			else
			{
				MessageBox.Show("Impossible to open file: " + stat_path ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}
		}
		

		//load time from LOG file
		void btnTakeFromLogFileClick(object sender, EventArgs e)
		{
			//cur_satellite_calc.load_time_table("D:/METEOR_proc/Meteor_table.log");
			string log_path = "";
			openFileDialog1.Filter = "LOG Files|*.log";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
				log_path = openFileDialog1.FileName;
				CurSatelliteCalc.LoadTimeTable(log_path);
				use_table_time = true;
				lblImageTimeInfo.Text = "Image time present";
				UpdateFullTime();
   			}
			else
			{
				MessageBox.Show("Impossible to open file: " + log_path ,"ERROR!",0,System.Windows.Forms.MessageBoxIcon.Stop);
			}		
		}
		

		//load TLE from archive
		void btnFindBestTLEClick(object sender, EventArgs e)
		{
			LoadTLE_Data();
		}
		
		//auto process
		void btnAutoProcessClick(object sender, EventArgs e)
		{
			start_date = CurImageWorker.file_created_date;
			
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

			UpdateTimeFromStatFile(stat_path);
			UpdateFullTime();
			
			LoadTLE_Data();
			btnStartProcess.PerformClick();
		}

        private void chkMeteorM2_0_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFullTime();
        }

        private void ChkMeteorM2_2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFullTime();
        }

        private void chkMeteorM2_3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFullTime();
        }
    }
}
