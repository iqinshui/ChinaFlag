using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ChinaFlag2
{
    class ChinaFlag : INationFlag
    {
        #region -  INationFlag Members  -

        public float WidthProportion { get { return 3F; } }
        public float HeightProportion { get { return 2F; } }

        SizeF size = SizeF.Empty;

        public SizeF Size
        {
            get { return size; }
            set { if (size != value) size = value; }
        }

        public void AdjustFlagSizeF(int w)
        {
            int width = w + w % (int)WidthProportion;
            int height = (int)(HeightProportion * width / WidthProportion);

            Size = new SizeF(width, height);
        }

        public Image GetFlagImage()
        {
            Bitmap flag = new Bitmap((int)size.Width, (int)size.Height);
            float piece = size.Width / 30;

            int bigStarRadius = (int)(piece * 3);
            int smlStarRadius = (int)piece;

            using (Graphics g = Graphics.FromImage(flag))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillRectangle(Brushes.Red, 0, 0, size.Width, size.Height);

                g.DrawImage(GetStarImage(bigStarRadius), piece * 2, piece * 2);
                g.DrawImage(GetStarImage(smlStarRadius, -(int)(Math.Atan(3.0 / 5.0) * 180.0 / Math.PI) - 18), piece * 9, piece);
                g.DrawImage(GetStarImage(smlStarRadius, -(int)(Math.Atan(1.0 / 7.0) * 180.0 / Math.PI) - 18), piece * 11, piece * 3);
                g.DrawImage(GetStarImage(smlStarRadius, 0), piece * 11, piece * 6);
                g.DrawImage(GetStarImage(smlStarRadius, (int)(Math.Atan(4.0 / 5.0) * 180.0 / Math.PI) - 18), piece * 9, piece * 8);
            }

            return flag;
        }

        #endregion

        #region -  Private Members  -

        public Image GetStarImage(int radius)
        {
            Bitmap bigStar = new Bitmap(radius * 2, radius * 2);

            using (Graphics g = Graphics.FromImage(bigStar))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Point pc = new Point(radius, radius);

                PointF p0 = new PointF(pc.X, pc.Y - radius);
                PointF p1 = new PointF((float)(pc.X + radius * Math.Sin(72 * Math.PI / 180)), (float)(pc.Y - radius * Math.Cos(72 * Math.PI / 180)));
                PointF p2 = new PointF((float)(pc.X + radius * Math.Sin(36 * Math.PI / 180)), (float)(pc.Y + radius * Math.Cos(36 * Math.PI / 180)));
                PointF p3 = new PointF((float)(pc.X - radius * Math.Sin(36 * Math.PI / 180)), (float)(pc.Y + radius * Math.Cos(36 * Math.PI / 180)));
                PointF p4 = new PointF((float)(pc.X - radius * Math.Sin(72 * Math.PI / 180)), (float)(pc.Y - radius * Math.Cos(72 * Math.PI / 180)));

                using (GraphicsPath path = new GraphicsPath(FillMode.Winding))
                {
                    path.AddLine(p0, p2);
                    path.AddLine(p2, p4);
                    path.AddLine(p4, p1);
                    path.AddLine(p1, p3);
                    path.AddLine(p3, p0);
                    path.CloseFigure();
                    g.FillPath(Brushes.Yellow, path);
                }
            }
            return bigStar;
        }

        public Image GetStarImage(int radius, int angle)
        {
            return RotateImage(GetStarImage(radius), angle);
        }
        public Image RotateImage(Image b, int angle)
        {
            angle = angle % 360;
            // 弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            // 原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            // 目标位图
            Bitmap dsImage = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(dsImage))
            {
                g.InterpolationMode = InterpolationMode.Bilinear;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // 计算偏移量
                Point Offset = new Point((W - w) / 2, (H - h) / 2);
                // 构造图像显示区域：让图像的中心与窗口的中心点一致
                Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
                Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                g.TranslateTransform(center.X, center.Y);
                g.RotateTransform(angle - 360);
                // 恢复图像在水平和垂直方向的平移
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawImage(b, rect);
                // 重至绘图的所有变换
                g.ResetTransform();
                g.Save();

            }
            return dsImage.Clone(new Rectangle((W - w) / 2, (H - h) / 2, w, h), PixelFormat.DontCare);
        }

        #endregion

        #region -  Constructor  -

        public ChinaFlag(int w)
        {
            AdjustFlagSizeF(w);
        }

        #endregion
    }
}