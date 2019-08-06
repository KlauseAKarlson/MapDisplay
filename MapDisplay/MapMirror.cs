using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MapDisplay
{
    class MapMirror : Control
    {
        private Map _Map=null;
        private int _PMapWidth, _PMapHeight;
        public Map View
        {
            set { SetMap(value); }
        }

        public void SetMap(Map m)
        {
            _Map = m;
            MapView v = m.GetMapView();
            //resize
            _PMapWidth = v.Width;
            _PMapHeight = v.Height;
            this.Width = _PMapWidth;
            this.Height = _PMapHeight;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //get region to paint
            if (_Map != null)//null pointer control
            {
                MapView V = _Map.GetMapView();
                //paint tiles
                for (int row = 0; row < _Map.MapHeight; row++)
                {
                    for (int col = 0; col < _Map.MapWidth; col++)
                    {
                        Point p = V.TileStart(col, row);
                        _Map.PaintTilesAt(e.Graphics, p.X, p.Y, col, row);
                    }
                }//end paint tiles
            }//else (if there is no map) do nothing
        }//end Onpiant

    }//end class
}//end namespace
