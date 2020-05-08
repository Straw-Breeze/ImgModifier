using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgModifier
{
    public partial class Form1 : Form
    {
        public static bool flag = false;
        public static int[,] formatHorizon = new int[3,2]{ { 1366, 768 }, { 448, 252 }, { 1080, 680 } };
        public static int[,] formatVertical = new int[2, 2] { { 960, 1440 }, { 448, 672 } };
        public Rectangle rectangle;
        public const int areaWidth = 440;
        public const int areaHeight = 440;
        public static bool canMove = false;
        public static int mouseX = 0;
        public static int mouseY = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            pictureBox1.Image = Image.FromFile(fileName);
            textBox1.Text = fileName;
            label3.Text = "默认分辨率：" + pictureBox1.Image.Width.ToString() + "x" + pictureBox1.Image.Height.ToString();
            flag = pictureBox1.Image.Width >= pictureBox1.Image.Height;
            label4.Text = "格式：" +  (flag ? "横板" : "竖版");
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("输入图片不能为空！");
                return;
            }
            string[] tmpPath = getImgCatalog(textBox1.Text);
            ImageFormat imageFormat = getImgFormat(tmpPath[1]);
            for (int i = 0; i < 2; i++)
            {
                int x = formatHorizon[i, 0];
                int y = formatHorizon[i, 1];
                if (!flag)
                {
                    x = formatVertical[i, 0];
                    y = formatVertical[i, 1];
                }
                Image newImg = new Bitmap(x, y);
                Graphics graphics = Graphics.FromImage(newImg);
                graphics.DrawImage(pictureBox1.Image, 0, 0, x, y);
                newImg.Save(tmpPath[0] + "_" + x.ToString() + "x" + y.ToString() + tmpPath[1], imageFormat);
                newImg.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("输入图片不能为空！");
                return;
            }
            string[] tmpPath = getImgCatalog(textBox1.Text);
            ImageFormat imageFormat = getImgFormat(tmpPath[1]);
            int x = formatHorizon[2, 0];
            int y = formatHorizon[2, 1];
            Image newImg = new Bitmap(x, y);
            Graphics graphics = Graphics.FromImage(newImg);
            graphics.DrawImage(pictureBox1.Image, 0, 0, x, y);
            newImg.Save(tmpPath[0] + "_" + x.ToString() + "x" + y.ToString() + tmpPath[1], imageFormat);
            newImg.Dispose();
        }

        private string[] getImgCatalog(string imgPath)
        {
            int L = imgPath.Length;
            for (int i = L - 1; i >= 0; i--)
            {
                if (imgPath[i] == '.')
                {
                    return new string[] { imgPath.Remove(i), imgPath.Substring(i) };
                }
            }
            return new string[] { "error", "error" };
        }

        private ImageFormat getImgFormat(string imgSuffix)
        {
            if (imgSuffix == ".jpeg" || imgSuffix == ".jpg")
            {
                return ImageFormat.Jpeg;
            }
            else if (imgSuffix == ".bmp")
            {
                return ImageFormat.Bmp;
            }
            return ImageFormat.Png;
        }

        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            pictureBox2.Image = Image.FromFile(fileName);
            textBox2.Text = fileName;
            label7.Text = "默认分辨率：" + pictureBox2.Image.Width.ToString() + "x" + pictureBox2.Image.Height.ToString();
            flag = pictureBox2.Image.Width >= pictureBox2.Image.Height;
            label5.Text = "格式：" + (flag ? "横板" : "竖版");
            Bitmap bitmap = new Bitmap(Image.FromFile(fileName));
            if (flag && bitmap.Width > areaWidth)
            {
                pictureBox2.Width = areaWidth;
                pictureBox2.Height = bitmap.Height * areaWidth / bitmap.Width;
            }
            if (!flag && bitmap.Height > areaHeight)
            {
                pictureBox2.Height = areaHeight;
                pictureBox2.Width = bitmap.Width * areaHeight / bitmap.Height;
            }
            if (bitmap.Width < pictureBox2.Width)
            {
                pictureBox2.Width = bitmap.Width;
            }
            if (bitmap.Height < pictureBox2.Height)
            {
                pictureBox2.Height = bitmap.Height;
            }
            rectangle = new Rectangle(0, 0, 0, 0);
            rectangle.Width = (int)(Math.Min(pictureBox2.Width, pictureBox2.Height) * 0.8);
            rectangle.Height = rectangle.Width;
            trackBar1.Value = (int)(trackBar1.Maximum * 0.8);
        }
        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (rectangle != null && pictureBox2.Image != null)
            {
                Color backcolor = Color.FromArgb(155, Color.Black);
                List<Rectangle> rects = new List<Rectangle>();
                rects.Add(new Rectangle(0, 0, pictureBox2.Width, rectangle.Y));
                rects.Add(new Rectangle(0, rectangle.Y, rectangle.X, rectangle.Height));
                rects.Add(new Rectangle(rectangle.Right, rectangle.Top, pictureBox2.Width - rectangle.Right, rectangle.Height));
                rects.Add(new Rectangle(0, rectangle.Bottom, pictureBox2.Width, pictureBox2.Height - rectangle.Bottom));
                e.Graphics.FillRectangles(new SolidBrush(backcolor), rects.ToArray());
                Pen pen = new Pen(new SolidBrush(Color.DeepSkyBlue), 3);
                // pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, rectangle);
            }
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            pictureBox2.Left = panel1.Width / 2 - pictureBox2.Width / 2;
            pictureBox2.Top = panel1.Height / 2 - pictureBox2.Height / 2;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = pictureBox2.PointToClient(Control.MousePosition);
            if (point.X < rectangle.X || point.X > rectangle.X + rectangle.Width || point.Y < rectangle.Y || point.Y > rectangle.Y + rectangle.Height)
            {
                canMove = false;
            }
            else
            {
                canMove = true;
                mouseX = point.X;
                mouseY = point.Y;
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (canMove)
            {
                Point point = pictureBox2.PointToClient(Control.MousePosition);
                rectangle.X += point.X - mouseX;
                rectangle.Y += point.Y - mouseY;
                mouseX = point.X;
                mouseY = point.Y;
                rectangle.X = rectangle.X < 0 ? 0 : rectangle.X;
                rectangle.Y = rectangle.Y < 0 ? 0 : rectangle.Y;
                rectangle.X = rectangle.X + rectangle.Width >= pictureBox2.Width ? pictureBox2.Width - rectangle.Width - 1 : rectangle.X;
                rectangle.Y = rectangle.Y + rectangle.Height >= pictureBox2.Height ? pictureBox2.Height - rectangle.Height - 1 : rectangle.Y;
                pictureBox2.Invalidate();
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            canMove = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("输入图片不能为空！");
                return;
            }
            int sx = pictureBox2.Image.Width * rectangle.X / pictureBox2.Width;
            int sy = pictureBox2.Image.Height * rectangle.Y / pictureBox2.Height;
            int w = pictureBox2.Image.Width * rectangle.Width / pictureBox2.Width;
            int h = pictureBox2.Image.Height * rectangle.Height / pictureBox2.Height;
            int W = 540, H = 540;
            string[] tmpPath = getImgCatalog(textBox2.Text);
            ImageFormat imageFormat = getImgFormat(tmpPath[1]);
            Image newImg = new Bitmap(W, H);
            Graphics graphics = Graphics.FromImage(newImg);
            graphics.DrawImage(pictureBox2.Image, new Rectangle(0, 0, W, H), new Rectangle(sx, sy, w, h), GraphicsUnit.Pixel);
            newImg.Save(tmpPath[0] + "_" + W.ToString() + "x" + H.ToString() + tmpPath[1], imageFormat);
            newImg.Dispose();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (rectangle == null || pictureBox2.Image == null)
            {
                return;
            }
            double rate = trackBar1.Value * 1.0 / trackBar1.Maximum;
            if (flag)
            {
                rectangle.Height = Math.Min((int)(pictureBox2.Height * rate), pictureBox2.Height - 1);
                rectangle.Width = rectangle.Height;
            }
            else
            {
                rectangle.Width = Math.Min((int)(pictureBox2.Width * rate), pictureBox2.Width - 1);
                rectangle.Height = rectangle.Width;
            }
            if (rectangle.X + rectangle.Width >= pictureBox2.Width)
            {
                rectangle.X = pictureBox2.Width - rectangle.Width - 1;
            }
            if (rectangle.Y + rectangle.Height >= pictureBox2.Height)
            {
                rectangle.Y = pictureBox2.Height - rectangle.Height - 1;
            }
            pictureBox2.Invalidate();
        }
    }
}