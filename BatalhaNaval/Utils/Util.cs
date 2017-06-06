using System;
using System.Drawing;

namespace Utils
{
    /// <summary>
    /// Math utilities class.
    /// </summary>
    public class Util
    {
        public static float Lerp(float start, float end, double value)
        {
            return Convert.ToSingle(start + value * (end - start));
        }

        public static int Lerp(int start, int end, double value)
        {
            return Convert.ToInt32(start + value * (end - start));
        }

        public static double Lerp(double start, double end, double value)
        {
            return start + value * (end - start);
        }

        public static Point Lerp(Point p1, Point p2, double value)
        {
            return new Point(Lerp(p1.X, p1.Y, value),
                             Lerp(p2.X, p2.X, value));
        }

        public static PointF Lerp(PointF p1, PointF p2, double value)
        {
            return new PointF(Lerp(p1.X, p1.Y, value),
                              Lerp(p2.X, p2.X, value));
        }

        public static int Range(float rangeStart, float rangeEnd, int intervals, float value)
        {
            if (rangeStart > rangeEnd)
                throw new ArgumentOutOfRangeException("Invalid range");
            if (value < rangeStart || value > rangeEnd)
                throw new ArgumentOutOfRangeException("Value out of given range limits");

            if (intervals < 1)
                throw new ArgumentException("There must be at least one interval in the specified range");

            return (int)Math.Floor((value - rangeStart) * intervals / (rangeEnd - rangeStart));
        }

        public static int Range(double rangeStart, double rangeEnd, int intervals, double value)
        {
            if (rangeStart > rangeEnd)
                throw new ArgumentOutOfRangeException("Invalid range");
            if (value < rangeStart || value > rangeEnd)
                throw new ArgumentOutOfRangeException("Value out of given range limits");

            if (intervals < 1)
                throw new ArgumentException("There must be at least one interval in the specified range");

            return (int)Math.Floor((value - rangeStart) * intervals / (rangeEnd - rangeStart));
        }

        public static int Range(int rangeStart, int rangeEnd, int intervals, int value)
        {
            if (rangeStart > rangeEnd)
                throw new ArgumentOutOfRangeException("Invalid range");
            if (value < rangeStart || value > rangeEnd)
                throw new ArgumentOutOfRangeException("Value out of given range limits");

            if (intervals < 1)
                throw new ArgumentException("There must be at least one interval in the specified range");

            return (value - rangeStart) * intervals / (rangeEnd - rangeStart);
        }

        public static bool PointInRectangle(Point p, Rectangle rect)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width &&
                   p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }

        public static Bitmap RotateImage(Bitmap bmp, float angle)
        {
            angle = angle / 180 * (float)Math.PI; // Aceita ângulo em radianos

            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                g.DrawImage(bmp, new Point(0, 0));
                //g.ResetTransform();
            }

            return rotatedImage;
        }
    }
}
