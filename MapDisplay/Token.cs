using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;

namespace MapDisplay
{
    [Serializable]
    class Token : ISerializable
    {
        private Bitmap _Image;
        private string _Name;
        public int Width { get { return _Image.Width; } }
        public int Height { get { return _Image.Height; } }
        public string Name { get { return _Name; } }

        public Token(SerializationInfo info, StreamingContext context)
        {
            _Image = (Bitmap) info.GetValue("Image", typeof(Bitmap));
            _Name = (string)info.GetValue("Name", typeof(string));
        }
        public Token(Image i, string name)
        {
            _Image = new Bitmap(i);
            _Name = name;
        }

        public Token (Image i, String Name, int Diameter)
        {
            _Image = ProcessImage(i, Diameter);
            _Name = Name;
        }

        public Bitmap ProcessImage(Image i, int diameter)
        {
            //creates circular to be used by a token
            //the image is resized into a circle with a transparent background
            Bitmap ISource = new Bitmap(i, diameter, diameter);//resized soruce image
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

        public string getName()
        {
            return _Name;
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
        public override string ToString()
        {
            return _Name;
        }

        internal void paint(Graphics g, int x, int y)
        {
            g.DrawImage(_Image, x, y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Image", _Image, typeof(Bitmap));
            info.AddValue("Name", _Name, typeof(string));
        }
    }//end token
}//end namespace
