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
            String path = "C:/Users/drago/Desktop/test.jpg";
            Image newImg = new Bitmap(768, 1366);
            Graphics graphics = Graphics.FromImage(newImg);
            graphics.DrawImage(pictureBox1.Image, 0, 0, 768, 1366);
            newImg.Save(path);
            newImg.Dispose();
        }
    }
}