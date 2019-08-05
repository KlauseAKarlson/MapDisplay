using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;

namespace MapDisplay
{

    class Token 
    {
        private Bitmap _Image;
        public int Width { get { return _Image.Width; } }
        public int Height { get { return _Image.Height; } }


   
        public Token(Image i)
        {
            _Image = new Bitmap(i);
        }

        public Token (Image i, int Diameter)
        {
            _Image = ProcessImage(i, Diameter);
        }

        public Bitmap ProcessImage(Image i, int diameter)
        {
            //creates circular to be used by a token
            //the image is resized into a circle with a transparent background
            Bitmap ISource = new Bitmap(i, diameter, diameter);//resized source image
            Bitmap IOut = new Bitmap(diameter, diameter);//image to be painted to
            Graphics g = Graphics.FromImage(IOut);
            g.Clear(Color.Transparent);//set bakcground to transparent
            g.FillEllipse(new TextureBrush(ISource), 0, 0, diameter -1, diameter-1);//draw source image
            g.DrawEllipse(Pens.Black, 0, 0, diameter-1, diameter-1);//draw border
            //dispose
            ISource.Dispose();
            g.Dispose();
            return IOut;
        }


        public Image getImage()
        {
            return _Image;
        }
        public void draw(Graphics g, int x, int y)
        {
            g.DrawImage(_Image, x, y);
        }
        public void DrawCentered(Graphics g, int x, int y)
        {
            g.DrawImage(_Image, x-_Image.Width/2, y-_Image.Height/2 );
        }


        internal void paint(Graphics g, int x, int y)
        {
            g.DrawImage(_Image, x, y);
        }


    }//end token
}//end namespace
