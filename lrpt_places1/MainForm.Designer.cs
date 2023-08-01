/*
 * Создано в SharpDevelop.
 * Пользователь: s35398
 * Дата: 18.02.2015
 * Время: 15:33
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
namespace lrpt_places1
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnStartProcess;
		private System.Windows.Forms.Label lblProcState;
		private System.Windows.Forms.Button btnOpenImage;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label lblImageWidth;
		private System.Windows.Forms.Label lblImageHeight;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.MonthCalendar monthCalendar1;
		private System.Windows.Forms.Button btnTakeFromImage;
		private System.Windows.Forms.Button btnTakeFromCalendar;
		private System.Windows.Forms.Label lblUTC_FullDateTime;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnStartProcess = new System.Windows.Forms.Button();
            this.lblProcState = new System.Windows.Forms.Label();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblImageWidth = new System.Windows.Forms.Label();
            this.lblImageHeight = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblImageDate = new System.Windows.Forms.Label();
            this.btnTakeFromImage = new System.Windows.Forms.Button();
            this.btnTakeFromCalendar = new System.Windows.Forms.Button();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.lblUTC_FullDateTime = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblImageName = new System.Windows.Forms.Label();
            this.lblImageType = new System.Windows.Forms.Label();
            this.lblImageDateMain = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblImageTimeInfo = new System.Windows.Forms.Label();
            this.btnTakeFromLogFile = new System.Windows.Forms.Button();
            this.btnTakeFromStatFile = new System.Windows.Forms.Button();
            this.btnManualTimeInput = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMeteorM2_0 = new System.Windows.Forms.RadioButton();
            this.chkMeteorM2_2 = new System.Windows.Forms.RadioButton();
            this.lblTLE_DiffTime = new System.Windows.Forms.Label();
            this.lblTLE_Name = new System.Windows.Forms.Label();
            this.btnFindBestTLE = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chkAutoHorizShift = new System.Windows.Forms.CheckBox();
            this.txtHorizShift = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtVertShift = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblFlightDuration = new System.Windows.Forms.Label();
            this.chkShowCenterLine = new System.Windows.Forms.CheckBox();
            this.nudMarkSize = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAutoProcess = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarkSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartProcess
            // 
            this.btnStartProcess.Enabled = false;
            this.btnStartProcess.Location = new System.Drawing.Point(14, 330);
            this.btnStartProcess.Name = "btnStartProcess";
            this.btnStartProcess.Size = new System.Drawing.Size(133, 36);
            this.btnStartProcess.TabIndex = 0;
            this.btnStartProcess.Text = "START PROCESS";
            this.btnStartProcess.UseVisualStyleBackColor = true;
            this.btnStartProcess.Click += new System.EventHandler(this.btnStartProcessClick);
            // 
            // lblProcState
            // 
            this.lblProcState.Location = new System.Drawing.Point(331, 330);
            this.lblProcState.Name = "lblProcState";
            this.lblProcState.Size = new System.Drawing.Size(82, 23);
            this.lblProcState.TabIndex = 1;
            this.lblProcState.Text = "Wait";
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Location = new System.Drawing.Point(30, 25);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(74, 37);
            this.btnOpenImage.TabIndex = 2;
            this.btnOpenImage.Text = "Open image";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImageClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblImageWidth
            // 
            this.lblImageWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageWidth.Location = new System.Drawing.Point(18, 94);
            this.lblImageWidth.Name = "lblImageWidth";
            this.lblImageWidth.Size = new System.Drawing.Size(176, 23);
            this.lblImageWidth.TabIndex = 3;
            this.lblImageWidth.Text = "Image width: 0";
            // 
            // lblImageHeight
            // 
            this.lblImageHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageHeight.Location = new System.Drawing.Point(18, 117);
            this.lblImageHeight.Name = "lblImageHeight";
            this.lblImageHeight.Size = new System.Drawing.Size(209, 23);
            this.lblImageHeight.TabIndex = 4;
            this.lblImageHeight.Text = "Image height: 0";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.lblImageDate);
            this.groupBox3.Controls.Add(this.btnTakeFromImage);
            this.groupBox3.Controls.Add(this.btnTakeFromCalendar);
            this.groupBox3.Controls.Add(this.monthCalendar1);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 213);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Date";
            // 
            // lblImageDate
            // 
            this.lblImageDate.Location = new System.Drawing.Point(12, 190);
            this.lblImageDate.Name = "lblImageDate";
            this.lblImageDate.Size = new System.Drawing.Size(171, 23);
            this.lblImageDate.TabIndex = 11;
            this.lblImageDate.Text = "NO image date";
            // 
            // btnTakeFromImage
            // 
            this.btnTakeFromImage.Enabled = false;
            this.btnTakeFromImage.Location = new System.Drawing.Point(188, 111);
            this.btnTakeFromImage.Name = "btnTakeFromImage";
            this.btnTakeFromImage.Size = new System.Drawing.Size(75, 58);
            this.btnTakeFromImage.TabIndex = 10;
            this.btnTakeFromImage.Text = "Take from image";
            this.btnTakeFromImage.UseVisualStyleBackColor = true;
            this.btnTakeFromImage.Click += new System.EventHandler(this.btnTakeFromImageClick);
            // 
            // btnTakeFromCalendar
            // 
            this.btnTakeFromCalendar.Location = new System.Drawing.Point(188, 19);
            this.btnTakeFromCalendar.Name = "btnTakeFromCalendar";
            this.btnTakeFromCalendar.Size = new System.Drawing.Size(75, 58);
            this.btnTakeFromCalendar.TabIndex = 9;
            this.btnTakeFromCalendar.Text = "Take from calendar";
            this.btnTakeFromCalendar.UseVisualStyleBackColor = true;
            this.btnTakeFromCalendar.Click += new System.EventHandler(this.btnTakeFromCalendarClick);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(12, 19);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            // 
            // lblUTC_FullDateTime
            // 
            this.lblUTC_FullDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUTC_FullDateTime.Location = new System.Drawing.Point(10, 267);
            this.lblUTC_FullDateTime.Name = "lblUTC_FullDateTime";
            this.lblUTC_FullDateTime.Size = new System.Drawing.Size(528, 23);
            this.lblUTC_FullDateTime.TabIndex = 8;
            this.lblUTC_FullDateTime.Text = "Full UTC Date and Time:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(569, 253);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblImageName);
            this.tabPage1.Controls.Add(this.lblImageType);
            this.tabPage1.Controls.Add(this.lblImageDateMain);
            this.tabPage1.Controls.Add(this.lblImageHeight);
            this.tabPage1.Controls.Add(this.btnOpenImage);
            this.tabPage1.Controls.Add(this.lblImageWidth);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(561, 227);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblImageName
            // 
            this.lblImageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageName.Location = new System.Drawing.Point(18, 140);
            this.lblImageName.Name = "lblImageName";
            this.lblImageName.Size = new System.Drawing.Size(512, 23);
            this.lblImageName.TabIndex = 8;
            this.lblImageName.Text = "Image name: none";
            // 
            // lblImageType
            // 
            this.lblImageType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageType.Location = new System.Drawing.Point(18, 186);
            this.lblImageType.Name = "lblImageType";
            this.lblImageType.Size = new System.Drawing.Size(340, 23);
            this.lblImageType.TabIndex = 7;
            this.lblImageType.Text = "Image type: ";
            // 
            // lblImageDateMain
            // 
            this.lblImageDateMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageDateMain.Location = new System.Drawing.Point(18, 163);
            this.lblImageDateMain.Name = "lblImageDateMain";
            this.lblImageDateMain.Size = new System.Drawing.Size(340, 23);
            this.lblImageDateMain.TabIndex = 6;
            this.lblImageDateMain.Text = "Image date: ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(561, 227);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Date and Time";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblImageTimeInfo);
            this.groupBox1.Controls.Add(this.btnTakeFromLogFile);
            this.groupBox1.Controls.Add(this.btnTakeFromStatFile);
            this.groupBox1.Controls.Add(this.btnManualTimeInput);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(301, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 213);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time";
            // 
            // lblImageTimeInfo
            // 
            this.lblImageTimeInfo.Location = new System.Drawing.Point(14, 174);
            this.lblImageTimeInfo.Name = "lblImageTimeInfo";
            this.lblImageTimeInfo.Size = new System.Drawing.Size(159, 36);
            this.lblImageTimeInfo.TabIndex = 16;
            this.lblImageTimeInfo.Text = "NO image time";
            // 
            // btnTakeFromLogFile
            // 
            this.btnTakeFromLogFile.Location = new System.Drawing.Point(152, 111);
            this.btnTakeFromLogFile.Name = "btnTakeFromLogFile";
            this.btnTakeFromLogFile.Size = new System.Drawing.Size(75, 58);
            this.btnTakeFromLogFile.TabIndex = 13;
            this.btnTakeFromLogFile.Text = "Take from LOG file";
            this.btnTakeFromLogFile.UseVisualStyleBackColor = true;
            this.btnTakeFromLogFile.Click += new System.EventHandler(this.btnTakeFromLogFileClick);
            // 
            // btnTakeFromStatFile
            // 
            this.btnTakeFromStatFile.Location = new System.Drawing.Point(17, 111);
            this.btnTakeFromStatFile.Name = "btnTakeFromStatFile";
            this.btnTakeFromStatFile.Size = new System.Drawing.Size(87, 58);
            this.btnTakeFromStatFile.TabIndex = 12;
            this.btnTakeFromStatFile.Text = "Take from STAT file";
            this.btnTakeFromStatFile.UseVisualStyleBackColor = true;
            this.btnTakeFromStatFile.Click += new System.EventHandler(this.btnTakeFromStatFileClick);
            // 
            // btnManualTimeInput
            // 
            this.btnManualTimeInput.Location = new System.Drawing.Point(152, 29);
            this.btnManualTimeInput.Name = "btnManualTimeInput";
            this.btnManualTimeInput.Size = new System.Drawing.Size(75, 58);
            this.btnManualTimeInput.TabIndex = 11;
            this.btnManualTimeInput.Text = "Manual input";
            this.btnManualTimeInput.UseVisualStyleBackColor = true;
            this.btnManualTimeInput.Click += new System.EventHandler(this.btnManualTimeInputClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(115, 55);
            this.textBox1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.lblTLE_DiffTime);
            this.tabPage3.Controls.Add(this.lblTLE_Name);
            this.tabPage3.Controls.Add(this.btnFindBestTLE);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(561, 227);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "TLE";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkMeteorM2_0);
            this.groupBox2.Controls.Add(this.chkMeteorM2_2);
            this.groupBox2.Location = new System.Drawing.Point(385, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(131, 96);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Satellite";
            // 
            // chkMeteorM2_0
            // 
            this.chkMeteorM2_0.Checked = true;
            this.chkMeteorM2_0.Location = new System.Drawing.Point(16, 24);
            this.chkMeteorM2_0.Name = "chkMeteorM2_0";
            this.chkMeteorM2_0.Size = new System.Drawing.Size(104, 24);
            this.chkMeteorM2_0.TabIndex = 1;
            this.chkMeteorM2_0.TabStop = true;
            this.chkMeteorM2_0.Text = "METEOR-M 2";
            this.chkMeteorM2_0.UseVisualStyleBackColor = true;
            this.chkMeteorM2_0.CheckedChanged += new System.EventHandler(this.chkMeteorM2_0_CheckedChanged);
            // 
            // chkMeteorM2_2
            // 
            this.chkMeteorM2_2.Location = new System.Drawing.Point(16, 54);
            this.chkMeteorM2_2.Name = "chkMeteorM2_2";
            this.chkMeteorM2_2.Size = new System.Drawing.Size(104, 24);
            this.chkMeteorM2_2.TabIndex = 0;
            this.chkMeteorM2_2.Text = "METEOR-M2.2";
            this.chkMeteorM2_2.UseVisualStyleBackColor = true;
            this.chkMeteorM2_2.CheckedChanged += new System.EventHandler(this.ChkMeteorM2_2_CheckedChanged);
            // 
            // lblTLE_DiffTime
            // 
            this.lblTLE_DiffTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTLE_DiffTime.Location = new System.Drawing.Point(18, 158);
            this.lblTLE_DiffTime.Name = "lblTLE_DiffTime";
            this.lblTLE_DiffTime.Size = new System.Drawing.Size(331, 23);
            this.lblTLE_DiffTime.TabIndex = 14;
            this.lblTLE_DiffTime.Text = "TLE days difference: NO";
            // 
            // lblTLE_Name
            // 
            this.lblTLE_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTLE_Name.Location = new System.Drawing.Point(18, 181);
            this.lblTLE_Name.Name = "lblTLE_Name";
            this.lblTLE_Name.Size = new System.Drawing.Size(331, 23);
            this.lblTLE_Name.TabIndex = 13;
            this.lblTLE_Name.Text = "TLE name: NO";
            // 
            // btnFindBestTLE
            // 
            this.btnFindBestTLE.Location = new System.Drawing.Point(22, 31);
            this.btnFindBestTLE.Name = "btnFindBestTLE";
            this.btnFindBestTLE.Size = new System.Drawing.Size(87, 51);
            this.btnFindBestTLE.TabIndex = 0;
            this.btnFindBestTLE.Text = "Find best TLE in archive";
            this.btnFindBestTLE.UseVisualStyleBackColor = true;
            this.btnFindBestTLE.Click += new System.EventHandler(this.btnFindBestTLEClick);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chkAutoHorizShift);
            this.tabPage4.Controls.Add(this.txtHorizShift);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.txtVertShift);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(561, 227);
            this.tabPage4.TabIndex = 5;
            this.tabPage4.Text = "Correction";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chkAutoHorizShift
            // 
            this.chkAutoHorizShift.Checked = true;
            this.chkAutoHorizShift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoHorizShift.Location = new System.Drawing.Point(184, 52);
            this.chkAutoHorizShift.Name = "chkAutoHorizShift";
            this.chkAutoHorizShift.Size = new System.Drawing.Size(104, 24);
            this.chkAutoHorizShift.TabIndex = 4;
            this.chkAutoHorizShift.Text = "Auto";
            this.chkAutoHorizShift.UseVisualStyleBackColor = true;
            // 
            // txtHorizShift
            // 
            this.txtHorizShift.Location = new System.Drawing.Point(106, 54);
            this.txtHorizShift.Name = "txtHorizShift";
            this.txtHorizShift.Size = new System.Drawing.Size(62, 20);
            this.txtHorizShift.TabIndex = 3;
            this.txtHorizShift.Text = "0";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(17, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 23);
            this.label11.TabIndex = 2;
            this.label11.Text = "Horizontal shift:";
            // 
            // txtVertShift
            // 
            this.txtVertShift.Location = new System.Drawing.Point(106, 22);
            this.txtVertShift.Name = "txtVertShift";
            this.txtVertShift.Size = new System.Drawing.Size(62, 20);
            this.txtVertShift.TabIndex = 1;
            this.txtVertShift.Text = "0";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(17, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 23);
            this.label10.TabIndex = 0;
            this.label10.Text = "Vertical shift:";
            // 
            // lblFlightDuration
            // 
            this.lblFlightDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFlightDuration.Location = new System.Drawing.Point(10, 290);
            this.lblFlightDuration.Name = "lblFlightDuration";
            this.lblFlightDuration.Size = new System.Drawing.Size(331, 23);
            this.lblFlightDuration.TabIndex = 12;
            this.lblFlightDuration.Text = "Flight duration: 0 s";
            // 
            // chkShowCenterLine
            // 
            this.chkShowCenterLine.Location = new System.Drawing.Point(444, 342);
            this.chkShowCenterLine.Name = "chkShowCenterLine";
            this.chkShowCenterLine.Size = new System.Drawing.Size(121, 24);
            this.chkShowCenterLine.TabIndex = 13;
            this.chkShowCenterLine.Text = "Show center line";
            this.chkShowCenterLine.UseVisualStyleBackColor = true;
            // 
            // nudMarkSize
            // 
            this.nudMarkSize.Location = new System.Drawing.Point(522, 316);
            this.nudMarkSize.Name = "nudMarkSize";
            this.nudMarkSize.Size = new System.Drawing.Size(43, 20);
            this.nudMarkSize.TabIndex = 14;
            this.nudMarkSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(455, 318);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 23);
            this.label12.TabIndex = 15;
            this.label12.Text = "Mark size:";
            // 
            // btnAutoProcess
            // 
            this.btnAutoProcess.Enabled = false;
            this.btnAutoProcess.Location = new System.Drawing.Point(165, 330);
            this.btnAutoProcess.Name = "btnAutoProcess";
            this.btnAutoProcess.Size = new System.Drawing.Size(133, 36);
            this.btnAutoProcess.TabIndex = 16;
            this.btnAutoProcess.Text = "AUTO PROCESS";
            this.btnAutoProcess.UseVisualStyleBackColor = true;
            this.btnAutoProcess.Click += new System.EventHandler(this.btnAutoProcessClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 378);
            this.Controls.Add(this.btnAutoProcess);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.nudMarkSize);
            this.Controls.Add(this.chkShowCenterLine);
            this.Controls.Add(this.lblFlightDuration);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblUTC_FullDateTime);
            this.Controls.Add(this.lblProcState);
            this.Controls.Add(this.btnStartProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "LRPT_places v1.10";
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarkSize)).EndInit();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label lblImageType;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown nudMarkSize;
		private System.Windows.Forms.CheckBox chkShowCenterLine;
		private System.Windows.Forms.CheckBox chkAutoHorizShift;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtHorizShift;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtVertShift;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label lblTLE_DiffTime;
		private System.Windows.Forms.Label lblTLE_Name;
		private System.Windows.Forms.Button btnFindBestTLE;
		private System.Windows.Forms.Label lblFlightDuration;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnManualTimeInput;
		private System.Windows.Forms.Button btnTakeFromStatFile;
		private System.Windows.Forms.Button btnTakeFromLogFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblImageDateMain;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton chkMeteorM2_0;
		private System.Windows.Forms.RadioButton chkMeteorM2_2;
		private System.Windows.Forms.Label lblImageDate;
		private System.Windows.Forms.Label lblImageTimeInfo;
		private System.Windows.Forms.Button btnAutoProcess;
		private System.Windows.Forms.Label lblImageName;
	}
}
