using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Breakout
{
    public static class TextureGen
    {

        public static Bitmap CreateBitmapImage(string sImageText, int fontSize)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            if (intWidth > 0 && intHeight > 0)
            {
                objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            }
            else
            {
                objBmpImage = new Bitmap(objBmpImage, new Size(1, 1));
            }

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.Transparent);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(255, 255, 255)), 0, 0);
            objGraphics.Flush();
            return (objBmpImage);
        }

        public static byte[] GetBytes(Bitmap bitmap)
        {
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // calculate the byte size: for PixelFormat.Format32bppArgb (standard for GDI bitmaps) it's the hight * stride
            int bufferSize = data.Height * data.Stride; // stride already incorporates 4 bytes per pixel

            // create buffer
            byte[] bytes = new byte[bufferSize];

            // copy bitmap data into buffer
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // unlock the bitmap data
            bitmap.UnlockBits(data);

            return bytes;
        }
        
    }
}
