using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDisplay
{
    partial class Map
    {
        //subclasses of mapview are implimented as private classes to protect the constructor from dangerous usage
        private class SquareMapView : MapView
        {
            internal override void SetMap(Map m)
            {
                _Map = m;
                _PMapWidth = m.getWidth() * m.getTileWidth();
                _PMapHeight = m.getHeight() * m.getTileHeight();
                this.Width = _PMapWidth;
                this.Height = _PMapHeight;
            }

            public override Point TileStart(int col, int row)
            {
                Point p = new Point();
                p.X = col * _Map.getTileWidth();
                p.Y = row * _Map.getTileHeight();
                return p;
            }
            public override int ColumnAt(int x, int y)
            {
                return x / _Map.getTileWidth();
            }

            public override int RowAt(int x, int y)
            {
                return y / _Map.getTileHeight();
            }
        }//end square map
    }//end map
}//end namespace
