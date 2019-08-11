using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;



namespace lrpt_places1
{
   /// <summary>
   /// Working with BMP image
   /// </summary>
	public class ImageWorkerClass
	{
		Bitmap cur_image;
		Graphics cur_graphics;
		public int cur_image_height;
		public int cur_image_width;
		public string cur_image_path;
		public DateTime fileCreatedDate;
		public bool image_loaded = false;
		
		public int image_type = 0;//0- raw, 1 - processor, smoothmet
		public string image_type_string = "";
		public string cur_image_name = "none";

        public ImageWorkerClass()
		{
			cur_image_height = 0;
			cur_image_width = 0;
		}
		
		public void pre_load_image(string path)
		{
			Bitmap tempBitmap = (Bitmap)Image.FromFile(path);
			cur_image_height = tempBitmap.Height;
			cur_image_width = tempBitmap.Width;
			fileCreatedDate = File.GetLastWriteTime(path);
			cur_image_path = path;
			
			cur_image_name = Path.GetFileName(cur_image_path);
			
			if (cur_image_width < 2000)
			{
				image_type = 0;
				image_type_string = "Raw from LRPT decoder";
			}
			else if ((cur_image_width > 2900) && (cur_image_width < 3000))
			{
				image_type = 1;
				image_type_string = "LRPT Image Processor";
			}
			else if ((cur_image_width > 2600) && (cur_image_width < 2800))
			{
				image_type = 0;
				image_type_string = "Smoothmet (UNSUPPORTED)";
			}
			else
			{
				image_type = 0;
				image_type_string = "UNKNOWN (UNSUPPORTED)";
			}
		}
		
		
		/// <summary>
		/// Load BMP image to memory
		/// </summary>
		public void load_image(string path, bool rotate)
		{
			if (cur_image != null) {cur_image.Dispose();}
			if (cur_graphics != null) {cur_graphics.Dispose();}
			
			Bitmap originalBmp = (Bitmap)Image.FromFile(path);
			Bitmap tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);
			
			cur_image = tempBitmap;
			cur_image_height = cur_image.Height;
			cur_image_width = cur_image.Width;
			cur_image_path = path;
			
			cur_graphics = Graphics.FromImage(tempBitmap);
			if (rotate)
			{	
				cur_graphics.TranslateTransform(cur_image_width, cur_image_height);
				cur_graphics.RotateTransform(180);
			}

			cur_graphics.DrawImage(originalBmp, 0, 0);
			
			if (rotate)
			{	
				cur_graphics.TranslateTransform(cur_image_width, cur_image_height);
				cur_graphics.RotateTransform(-180);
			}
			
			System.Diagnostics.Debug.WriteLine("Image opened: " + path + "\n");
			System.Diagnostics.Debug.WriteLine("Image height: {0}\n", cur_image_height);
			System.Diagnostics.Debug.WriteLine("Image width: {0}\n", cur_image_width);
			
			fileCreatedDate = File.GetLastWriteTime(path);
			System.Diagnostics.Debug.WriteLine("File loaded: " + fileCreatedDate);
			image_loaded = true;
		}
		
		/// <summary>
		/// Draw a text on image
		/// </summary>
		public void image_draw_text(int x, int y, string text, string html_color, int font_size)
		{

			StringFormat drawFormat = new StringFormat();
			System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(html_color);
			
			cur_graphics.DrawString(text, new Font("Arial", font_size), new SolidBrush(col), x, y);
		}
		
		/// <summary>
		/// Draw a cross at current image
		/// </summary>
		public void image_draw_cross(int center_x, int center_y, int cross_width,int pen_width,string html_color)
		{
			System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(html_color);
			Pen l_Pen = new Pen(new SolidBrush(col));
			l_Pen.Width = pen_width;
			cur_graphics.DrawLine(l_Pen,(center_x-cross_width/2),(center_y-cross_width/2),(center_x+cross_width/2),(center_y+cross_width/2));
			cur_graphics.DrawLine(l_Pen,(center_x+cross_width/2),(center_y-cross_width/2),(center_x-cross_width/2),(center_y+cross_width/2));
		}
		
		public void image_draw_center_line(int pen_width)
		{
			Pen cur_pen = new Pen(Brushes.LightYellow);
			cur_pen.Width = pen_width;
			cur_graphics.DrawLine(cur_pen,(cur_image_width/2),0,(cur_image_width/2),cur_image_height);
		}
		
		/*
		public void rotate_image()
		{
			cur_graphics.TranslateTransform(cur_image_width, cur_image_height);
        	cur_graphics.RotateTransform(180);
		}
		*/
		
		/// <summary>
		/// Save processed BMP image to disk
		/// </summary>
		public void save_image()
		{
			string save_path;
			string im_type = cur_image_path.Substring(cur_image_path.Length - 3,3);
			save_path = cur_image_path.Remove(cur_image_path.Length - 4,4);
			//save_path = save_path +"_result.png";
			save_path = save_path +"_result." + im_type;
			//cur_image.Save(save_path);
			
			if (im_type == "jpg")
			{				
				ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
				System.Drawing.Imaging.Encoder myEncoder =  System.Drawing.Imaging.Encoder.Quality;
				EncoderParameters myEncoderParameters = new EncoderParameters(1);
				EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				
				cur_image.Save(save_path, jpgEncoder, myEncoderParameters);
			}
			else
			{
				//cur_image.Save(save_path);
				cur_image.Save(save_path,ImageFormat.Bmp);
			}

			System.Diagnostics.Debug.WriteLine("Image saved: " + save_path + "\n");
			
			cur_graphics.Dispose();
			cur_image.Dispose();
			image_loaded = false;
		}
		
		
		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
				if (codec.FormatID == format.Guid)
					return codec;
			return null;
		}
		
		
	}
}
