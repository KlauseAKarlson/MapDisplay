using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MapDisplay
{
    class Tile
    {
        private Bitmap _TileImage;
        private string _TileName;

        public Tile(Image i, string name)
        {
            //ensure we are using a copy of the image in memory rather than locking a file or stream
            _TileImage = new Bitmap(i);
            _TileName = name;
        }
        public string getName()
        {
            return _TileName;
        }
        public Image getImage()
        {
            return _TileImage;
        }
        public void draw(Graphics g, int x, int y)
        {
            g.DrawImage(_TileImage, x, y);
        }
        public override string ToString()
        {
            return _TileName;
        }
    }//end tile
}
