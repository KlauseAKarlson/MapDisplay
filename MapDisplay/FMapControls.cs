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
        public FMapControls()
        {
            InitializeComponent();
            DLoadMap.Filter = "Map (*.map)|*.map";
            DNewToken.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
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
            MapHolder.Controls.Add(_MapView);
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
            using(System.IO.Compression.ZipArchive save = 
                new System.IO.Compression.ZipArchive( DLoadMap.OpenFile() )  )
            {
                Map m = Map.loadMap(save, TokenPanel);
                SwapMap(m);
            }
        }//end load map file okay

        private void DNewToken_FileOk(object sender, CancelEventArgs e)
        {
            if (_Map == null) return;//do nothing if there is no map
            using(Bitmap i = new Bitmap(DNewToken.OpenFile()))//create token
            {
                Token t = _Map.tokenSet.CreateToken(i);
                _Map.tokenSet.PlaceInPanel(t);
            }
        }//end newtoken file okay


    }//end form
}//end namespace
