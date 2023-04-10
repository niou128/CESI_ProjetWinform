namespace projetImage
{
    public partial class MainForm : Form
    {
        private Image image1;
        private Image image2;

        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sébastien Bernard");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void uploadImg1Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers image (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image1 = Image.FromFile(openFileDialog.FileName);

                // Calculer la taille de l'image pour conserver son rapport hauteur/largeur
                float panelRatio = (float)splitContainer1.Panel1.Width / splitContainer1.Panel1.Height;
                float imageRatio = (float)image1.Width / image1.Height;
                int newWidth, newHeight;
                if (panelRatio > imageRatio)
                {
                    // Le panel est plus large que l'image, donc on doit réduire la hauteur de l'image pour qu'elle rentre dans le panel
                    newHeight = splitContainer1.Panel1.Height;
                    newWidth = (int)(newHeight * imageRatio);
                }
                else
                {
                    // Le panel est plus haut que l'image, donc on doit réduire la largeur de l'image pour qu'elle rentre dans le panel
                    newWidth = splitContainer1.Panel1.Width;
                    newHeight = (int)(newWidth / imageRatio);
                }

                // Redimensionner l'image à la taille calculée
                Image resizedImage = new Bitmap(image1, newWidth, newHeight);

                splitContainer1.Panel1.BackgroundImage = resizedImage;
            }
        }

        private void uploadImg2Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers image (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image2 = Image.FromFile(openFileDialog.FileName);

                // Calculer la taille de l'image pour conserver son rapport hauteur/largeur
                float panelRatio = (float)splitContainer1.Panel2.Width / splitContainer1.Panel2.Height;
                float imageRatio = (float)image2.Width / image2.Height;
                int newWidth, newHeight;
                if (panelRatio > imageRatio)
                {
                    // Le panel est plus large que l'image, donc on doit réduire la hauteur de l'image pour qu'elle rentre dans le panel
                    newHeight = splitContainer1.Panel2.Height;
                    newWidth = (int)(newHeight * imageRatio);
                }
                else
                {
                    // Le panel est plus haut que l'image, donc on doit réduire la largeur de l'image pour qu'elle rentre dans le panel
                    newWidth = splitContainer1.Panel2.Width;
                    newHeight = (int)(newWidth / imageRatio);
                }

                // Redimensionner l'image à la taille calculée
                Image resizedImage = new Bitmap(image2, newWidth, newHeight);

                splitContainer1.Panel2.BackgroundImage = resizedImage;
            }
        }

        private void CompareButton_Click(object sender, EventArgs e)
        {
            if (image1 != null && image2 != null)
            {
                if (CompareBitmaps(image1, image2))
                {
                    MessageBox.Show("Les images sont identiques.");
                }
                else
                {
                    MessageBox.Show("Les images sont différentes.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez charger deux images avant de comparer.");
            }
        }

        private bool CompareBitmaps(Image bmp1, Image bmp2)
        {
            // Determine the smallest size between the two images
            int width = Math.Min(bmp1.Width, bmp2.Width);
            int height = Math.Min(bmp1.Height, bmp2.Height);

            // Resize both images to the smallest size
            bmp1 = new Bitmap(bmp1, new Size(width, height));
            bmp2 = new Bitmap(bmp2, new Size(width, height));

            // Compare the images pixel by pixel with a tolerance of 5%
            var diff = 0;
            var total = width * height;
            var tolerance = 0.05; // 5%
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color1 = ((Bitmap)bmp1).GetPixel(x, y);
                    var color2 = ((Bitmap)bmp2).GetPixel(x, y);
                    var deltaR = Math.Abs(color1.R - color2.R);
                    var deltaG = Math.Abs(color1.G - color2.G);
                    var deltaB = Math.Abs(color1.B - color2.B);
                    var delta = Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB) / 441.67; // Divide by the maximum delta (sqrt(255^2 + 255^2 + 255^2)) to get a value between 0 and 1
                    if (delta > tolerance)
                    {
                        diff++;
                    }
                }
            }

            // Return true if the images are considered identical
            return diff / (double)total <= tolerance;
        }

    }
}