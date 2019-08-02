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
    public partial class FMirrorWindow : Form
    {
        private FMapControls _Parent;
        private MapMirror _Mirror;
        
        internal void SetMap(Map m)
        {
            _Mirror.SetMap(m);
            Invalidate();
        }
        public void InvalidateMirror(Rectangle rc)
        {
            _Mirror.Invalidate(rc);
        }
        public FMirrorWindow(FMapControls parent): base()
        {
            InitializeComponent();
            _Parent = parent;
            _Mirror = new MapMirror();
            Controls.Add(_Mirror);
        }

        private void FMirrorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
