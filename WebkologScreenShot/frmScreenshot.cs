using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebkologScreenShot
{
    public partial class frmScreenshot : Form
    {
        public frmScreenshot()
        {
            InitializeComponent();
        }

        public Bitmap orgImg;
        Graphics g;
        bool startDraw;
        int msX, msY, mfX, mfY, recW, recH, recX, recY;
        Rectangle rec;

        private void frmScreenshot_Load(object sender, EventArgs e)
        {
            this.Text = "Screen";
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void drawCropArea()
        {
            pictureBox1.Refresh();
            Pen p = new Pen(Color.Red, 4f);
            if (msX < mfX)
            {
                recW = mfX - msX;
                recX = msX;
            }
            else
            {
                recW = msX - mfX;
                recX = mfX;
            }
            if (msY < mfY)
            {
                recH = mfY - msY;
                recY = msY;
            }
            else
            {
                recH = msY - mfY;
                recY = mfY;
            }
            rec = new Rectangle(recX, recY, recW, recH);
            g.DrawRectangle(p, rec);
        }

        private Bitmap cropImg(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics gr = Graphics.FromImage(nb);
            gr.DrawImage(b, -r.X, -r.Y);
            return nb;
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void frmScreenshot_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            ((Form1)Application.OpenForms["Form1"]).Visible = true;
        }

        private void frmScreenshot_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g = pictureBox1.CreateGraphics();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            msX = e.X;
            msY = e.Y;
            mfX = e.X;
            mfY = e.Y;
            startDraw = true;
            drawCropArea();
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (startDraw)
            {
                mfX = e.X;
                mfY = e.Y;
                drawCropArea();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            startDraw = false;
            ((Form1)Application.OpenForms["Form1"]).croppedImage = cropImg(orgImg, rec);
            this.Close();
        }
    }
}
