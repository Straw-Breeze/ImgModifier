using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.AllowDrop = true;
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
            string[] tmpPath = getImgCatalog(textBox1.Text);
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
                newImg.Save(tmpPath[0] + "_" + x.ToString() + "x" + y.ToString() + tmpPath[1]);
                newImg.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] tmpPath = getImgCatalog(textBox1.Text);
            int x = formatHorizon[2, 0];
            int y = formatHorizon[2, 1];
            Image newImg = new Bitmap(x, y);
            Graphics graphics = Graphics.FromImage(newImg);
            graphics.DrawImage(pictureBox1.Image, 0, 0, x, y);
            newImg.Save(tmpPath[0] + "_" + x.ToString() + "x" + y.ToString() + tmpPath[1]);
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
    }
}