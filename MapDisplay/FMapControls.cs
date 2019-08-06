using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapDisplay
{
    public partial class FMapControls : Form
    {
        private Map _Map=null;
        private MapView _MapView=null;
        private FMirrorWindow _MirrorWindow=null;
        public FMapControls()
        {
            InitializeComponent();
            DLoadMap.Filter = "Map (*.map)|*.map";
            DSaveMap.Filter = "Map (*.map)|*.map";
            DNewToken.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            _MirrorWindow = new FMirrorWindow(this);
            _MirrorWindow.Hide();
        }

        private void BLoadMap_Click(object sender, EventArgs e)
        {
            DLoadMap.ShowDialog();
        }

        private void SwapMap(Map m)
        {
            if (_Map != null)
            {
                _Map = null;
                _MapView = null;
                MapHolder.Controls.Clear();
                TokenPanel.Controls.Clear();
            }
            _Map = m;
            _MapView = _Map.GetMapView();
            _MirrorWindow.SetMap(_Map);
            _MapView.Paint += new PaintEventHandler(InvalidateOnPaint);
            MapHolder.Controls.Add(_MapView);

        }

        private void InvalidateOnPaint(object sender, PaintEventArgs e)
        {
            //invalidate the mirror when the map view is painted
            _MirrorWindow.InvalidateMirror(e.ClipRectangle); 
        }

        private void BNewToken_Click(object sender, EventArgs e)
        {
            if (_Map == null) return;//do nothing if there is no map
            DNewToken.ShowDialog();
        }


        #region drag and drop
        private void TokenPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Map == null) return;//do nothing if there is no map
            if (_Map.tokenSet.TokenGrabbed)
            {
                Token t = _Map.tokenSet.DropToken();
                _Map.tokenSet.PlaceInPanel(t);
            }
        }

        private void FMapControls_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Map == null) return;//do nothing if there is no map
            _Map.tokenSet.BounceToken();
        }

        private void BTrash_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Map.tokenSet.TokenGrabbed)
            {
                _Map.tokenSet.DropToken();
            }
        }
        private void TokenPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void TokenPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (_Map == null) return;//do nothing if there is no map
            if (e.Data.GetDataPresent(DataFormats.Bitmap))//create token from images
            {
                Bitmap image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                Token t = _Map.tokenSet.CreateToken(image);
                _Map.tokenSet.PlaceInPanel(t);
            }
        }
        #endregion
        private void DLoadMap_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                String saveFile = DLoadMap.FileName;
                Map m = Map.loadMap(saveFile, TokenPanel);
                SwapMap(m);
            }catch (System.IO.IOException EX)
            {
                MessageBox.Show(EX.ToString(), "En error has occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }//end load map file okay


        private void BShowMirror_Click(object sender, EventArgs e)
        {
            _MirrorWindow.Show();
        }

        private void BSaveMap_Click(object sender, EventArgs e)
        {
            if (_Map != null)
            {
                DSaveMap.FileName = _Map.SavePath;
                DSaveMap.ShowDialog();
            }
        }

        private void DNewToken_FileOk_1(object sender, CancelEventArgs e)
        {
            using (Bitmap image = new Bitmap(DNewToken.FileName))
            {
                Token t = _Map.tokenSet.CreateToken(image);
                _Map.tokenSet.PlaceInPanel(t);
            }
        }

        private void DSaveMap_FileOk(object sender, CancelEventArgs e)
        {
            _Map.SaveMap(DSaveMap.FileName);
        }

        private void MapHolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Map != null)
            {
                _Map.tokenSet.BounceToken();
            }

        }
    }//end form
}//end namespace
