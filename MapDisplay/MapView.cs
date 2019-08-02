using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapDisplay
{
    abstract class  MapView : Control
    {
        protected Map _Map;
        protected int _PMapWidth, _PMapHeight;

        protected override Size DefaultSize { get { return new Size(_PMapWidth, _PMapHeight); } }
        #region abstract methods
        internal abstract void SetMap(Map m);
        public abstract Point TileStart(int col, int row);
        public abstract int ColumnAt(int x, int y);
        public int ColumnAt(Point p)
        {
            return ColumnAt(p.X, p.Y);
        }
        public abstract int RowAt(int x, int y);
        public int RowAt(Point p)
        {
            return RowAt(p.X, p.Y);
        }
        #endregion

        #region event methods

        protected override void OnPaint(PaintEventArgs e)
        {
              base.OnPaint(e);

            //get region to paint
            int ColStart = ColumnAt(e.ClipRectangle.Left, e.ClipRectangle.Top);
            ColStart = ColStart == -1 ? 0 : ColStart;//boundry handling
            int RowStart = RowAt(e.ClipRectangle.Left, e.ClipRectangle.Top);
            RowStart = RowStart == -1 ? 0 : RowStart;
            int ColEnd = ColumnAt(e.ClipRectangle.Right, e.ClipRectangle.Bottom);
            ColEnd = ColEnd == -1 ? _Map.MapWidth : ColEnd;
            if (ColEnd == _Map.MapWidth) ColEnd--;
            int RowEnd=RowAt(e.ClipRectangle.Right, e.ClipRectangle.Bottom);
            RowEnd = RowEnd == -1 ? _Map.MapHeight : RowEnd;
            if (RowEnd == _Map.MapHeight) RowEnd--;
            //paint tiles
            for (int row=RowStart;row<=RowEnd;row++)
            {
                for (int col=ColStart;col<=ColEnd;col++)
                {
                    Point p = TileStart(col, row);
                    _Map.PaintTilesAt(e.Graphics, p.X, p.Y, col, row);
                }
            }//end paint tiles
        }//end on paint

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int col = ColumnAt(e.Location);
            int row = RowAt(e.Location);
            if (col != -1 & row != -1)//boundry check
            {
                Token t = _Map.getToken(col, row);
                if (t != null)//if a token is present
                {
                    if (e.Button==MouseButtons.Left)
                        //left click grabs
                    { 
                        _Map.pullToken(col, row);
                        _Map.tokenSet.GrabToken(t, new Point(col, row));
                    }
                    else if(e.Button == MouseButtons.Right)
                    {
                        //right click sends back to panel
                        _Map.pullToken(col, row);
                        _Map.tokenSet.PlaceInPanel(t);
                    }
                }//end if token present
            }//else if out of boundry do nothing
            base.OnMouseDown(e);
        }//end on map down

        protected override void OnMouseUp(MouseEventArgs e)
        {
            int col = ColumnAt(e.Location);
            int row = RowAt(e.Location);
            if (_Map.tokenSet.TokenGrabbed)//if a token is grabbed
            {
                if(col < 0  | col >= _Map.MapWidth
                    || 
                    row < 0 | row >= _Map.MapHeight)//boundry check
                {
                    _Map.tokenSet.BounceToken();//if out of bounds bounce
                }
                else//if in bounds check if a token is already present
                {
                    if (_Map.getToken(col,row) == null)
                    {
                        Token t = _Map.tokenSet.DropToken();
                        _Map.setToken(t, col, row);
                    }
                    else
                    {
                        _Map.tokenSet.BounceToken();
                    }
                }//end if in bounds
            }//end if token grabbed
            base.OnMouseUp(e);
        }//end on mouse up
        #endregion
    }//end map view
}//end namespace
