using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace WebkologScreenShot
{
    public partial class Form1 : Form
    {
        int i;
        public Bitmap croppedImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            pictureBox1.Visible = false;
            btnSave.Enabled = false;
        }

        private Bitmap Screenshot()
        {
            Bitmap Screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics GFX = Graphics.FromImage(Screenshot);
            GFX.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            return Screenshot;
        }

        private void createNewImg()
        {
            Bitmap orgImg = Screenshot();
            Bitmap traImg = ChangeOpacity(orgImg, 0.5f);
            frmScreenshot frm = new frmScreenshot();
            frm.Show();
            frm.pictureBox1.Image = traImg;
            frm.orgImg = orgImg;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 1)
            {
                createNewImg();
                i = 0;
                timer1.Enabled = false;
            }
        }

        public Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (croppedImage != null)
            {
                int ciW = croppedImage.Size.Width;
                int ciH = croppedImage.Size.Height;
                if (ciW > 0)
                {
                    pictureBox1.Visible = true;
                    pictureBox1.Width = ciW;
                    pictureBox1.Height = ciH;
                    pictureBox1.Image = croppedImage;
                    this.WindowState = FormWindowState.Maximized;
                    btnSave.Enabled = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ImageFormat format = ImageFormat.Jpeg;
            saveFileDialog1.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case ".gif":
                        format = ImageFormat.Gif;
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                pictureBox1.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Size = new Size(10, 10);
            pictureBox1.Visible = false;
            btnSave.Enabled = false;
            this.Size = new Size(111, 43);
            this.WindowState = FormWindowState.Normal;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 abf = new AboutBox1();
            abf.ShowDialog(this);
        }
    }
}
