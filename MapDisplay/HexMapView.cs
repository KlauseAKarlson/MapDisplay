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
        private class HexMapView : MapView
        {
            internal override void SetMap(Map m)
            {
                _Map = m;
                _PMapWidth = (int)((0.5 + m.getWidth()) * m.getTileWidth());
                _PMapHeight = (int)((m.getHeight() * 0.75 + 0.25) * m.getTileHeight());
                this.Width = _PMapWidth;
                this.Height = _PMapHeight;
            }


            public override Point TileStart(int col, int row)
            {
                Point p = new Point();
                p.X = col * _Map.getTileWidth() + ((row % 2 == 0) ? 0 : _Map.getTileWidth() / 2);
                p.Y = (int)(_Map.getTileHeight() * 0.75 * row);
                return p;
            }
            private bool InHex(int x, int y)
            {
                //returns true if the coordinates are inside the hexagon bounded by the width and height of the tile
                int tileHeight = _Map.getTileHeight();
                int tileWidth = _Map.getTileWidth();
                double slope = (tileHeight / 2.0) / (tileWidth);
                bool inside;
                //check in acordance to sides
                if (x < (tileWidth / 2 + 1))
                {
                    inside =
                            (y < (0.75 * tileHeight + slope * x)) &&
                            (y > (0.25 * tileHeight - slope * x));
                }
                else
                {
                    int x2 = x - tileWidth / 2;
                    inside =
                            (y <= (tileHeight - slope * x2)) &&
                            (y >= (0 + slope * x2));
                }
                return inside;
            }
            public override int ColumnAt(int x, int y)
            {
                // provides the collumn of the tile occupying the map contianing the pixel with the provided coordinates
                //returns -1 if not in a tile, hexes outside of map are garrenteed to be shown
                //first check if this will be easy or hard
                int tWidth = _Map.TileWidth;
                int tHeight = _Map.TileHeight;
                int col = -1;
                //Create a rectangle representing the top of an even rowed hex to the top of the one bellow it. Anything outside the hex will be ofset by half a hex
                int x2 = x % (tWidth);
                int y2 = y % (int)(tHeight * 1.5);
                if (InHex(x2, y2))
                {
                    col = x / tWidth;
                }
                else
                {
                    col = (x - tWidth / 2) / tWidth;
                }

                if (col < 0 || col >= _Map.getWidth())//return -1 if not in map
                    col = -1;
                return col;
            }//end collumn at

            public override int RowAt(int x, int y)
            {
                //provides the collumn of the tile occupying the map contianing the pixel with the provided coordinates
                //returns -1 if not in a tile
                int tWidth = _Map.TileWidth;
                int tHeight = _Map.TileHeight;
                //Create a rectangle representing the top of an even rowed hex to the top of the one bellow it. Anything outside the hex will be eitehr above or bellow
                int x2 = x % (tWidth);
                int row = 2 * (int)(y / (tHeight * 1.5));
                int y2 = y % (int)(tHeight * 1.5);
                if (InHex(x2, y2))
                {
                    //do nothing
                }
                else
                {
                    if (y2 > tHeight / 2)
                    {
                        row++;
                    }
                    else
                    {
                        row--;
                    }
                }//end if in hex

                if (row < 0 || row >= _Map.getHeight())//return -1 if not in map
                    row = -1;
                return row;
            }//end row at


        }//end hex map
    }
}
