using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ClayBotScreenshotHelper
{
    public partial class PictureForm : Form
    {
        private Bitmap originalBitmap;
        private string savePath;
        private bool isDragging = false;
        private Rectangle selectedRectangle;
        private Point initialPoint;

        public PictureForm(Bitmap image, string savePath)
        {
            InitializeComponent();

            originalBitmap = image;
            this.savePath = savePath;
            pictureBox.Image = originalBitmap;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDragging) return;

            isDragging = true;

            initialPoint = new Point(e.X, e.Y);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            calculateAndDisplay(new Point(e.X, e.Y));
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            isDragging = false;

            calculateAndDisplay(new Point(e.X, e.Y));
        }

        private void calculateAndDisplay(Point newPoint)
        {
            selectedRectangle = new Rectangle(
                new Point(
                    Math.Min(initialPoint.X, newPoint.X),
                    Math.Min(initialPoint.Y, newPoint.Y)),
                new Size(
                    Math.Abs(initialPoint.X - newPoint.X),
                    Math.Abs(initialPoint.Y - newPoint.Y)));

            Bitmap newBitmap = new Bitmap(originalBitmap);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.DrawRectangle(Pens.Red, selectedRectangle);
            }

            pictureBox.Image = newBitmap;

            rectangleInfoTextBox.Text = new RectangleConverter().ConvertToString(selectedRectangle);
        }

        private void cropAndSaveButton_Click(object sender, EventArgs e)
        {
            if (isDragging) return;

            selectedRectangle = (Rectangle)new RectangleConverter().ConvertFromString(rectangleInfoTextBox.Text);

            using (Bitmap bitmap = new Bitmap(selectedRectangle.Width, selectedRectangle.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(originalBitmap, new Rectangle(0, 0, selectedRectangle.Width, selectedRectangle.Height), selectedRectangle, GraphicsUnit.Pixel);
                bitmap.Save(savePath, ImageFormat.Png);
            }
        }
    }
}
