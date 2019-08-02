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
                int ColStart = V.ColumnAt(e.ClipRectangle.Left, e.ClipRectangle.Top);
                ColStart = ColStart == -1 ? 0 : ColStart;//boundry handling
                int RowStart = V.RowAt(e.ClipRectangle.Left, e.ClipRectangle.Top);
                RowStart = RowStart == -1 ? 0 : RowStart;
                int ColEnd = V.ColumnAt(e.ClipRectangle.Right, e.ClipRectangle.Bottom);
                ColEnd = ColEnd == -1 ? _Map.MapWidth : ColEnd;
                if (ColEnd == _Map.MapWidth) ColEnd--;
                int RowEnd = V.RowAt(e.ClipRectangle.Right, e.ClipRectangle.Bottom);
                RowEnd = RowEnd == -1 ? _Map.MapHeight : RowEnd;
                if (RowEnd == _Map.MapHeight) RowEnd--;
                //paint tiles
                for (int row = RowStart; row <= RowEnd; row++)
                {
                    for (int col = ColStart; col <= ColEnd; col++)
                    {
                        Point p = V.TileStart(col, row);
                        _Map.PaintTilesAt(e.Graphics, p.X, p.Y, col, row);
                    }
                }//end paint tiles
            }//else (if there is no map) do nothing
        }//end Onpiant

    }//end class
}//end namespace
