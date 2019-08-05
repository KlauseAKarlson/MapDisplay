using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MapDisplay
{
    class TokenSet
    {
        private Map _Map;
        private Panel _TokenPanel;
        private Dictionary<Button, Token> _PanelTokens;

        //internal drag and drop logic
        private bool _TokenGrabbed;
        public bool TokenGrabbed { get { return _TokenGrabbed; } }
        private Token _GrabbedToken;
        private Point _GrabbedLocation;
        public static Point TokenPanel = new Point(-1, -1);

        public ICollection<Token> getPanelTokens()
        {
            return _PanelTokens.Values;
        }
        public TokenSet(Map m, Panel p)
        {
            _Map = m;
            _TokenPanel = p;
            _PanelTokens = new Dictionary<Button, Token>();
        }
        public Token CreateToken(Image i)
        {
            return new Token(i, (4 * Math.Min(_Map.TileHeight, _Map.TileWidth) / 5));
        }

        public void GrabToken(Token t, Point location)
        {
            if (_TokenGrabbed)
                BounceToken();
            _TokenGrabbed = true;
            _GrabbedToken = t;
            _GrabbedLocation = location;
        }
        public void BounceToken()
        {
            //return a token to whence it came
            if (_TokenGrabbed)
            {
                Console.Out.WriteLine("Bounce");
                if (_GrabbedLocation == TokenPanel)
                {
                    PlaceInPanel(_GrabbedToken);
                }
                else
                {
                    _Map.setToken(_GrabbedToken, _GrabbedLocation.X, _GrabbedLocation.Y);
                }
                _TokenGrabbed = false;
            }
        }//end bounce token
        public Token DropToken()
        {

            _TokenGrabbed = false;
            return _GrabbedToken;

        }
        public void PlaceInPanel(Token t)
        {
            //create button for the token
            Button TButton = new Button();
            TButton.Image = t.getImage();
            TButton.Size = t.getImage().Size;
            TButton.MouseDown += new MouseEventHandler(Token_OnMouseDown);
            //add button
            _PanelTokens.Add(TButton, t);
            _TokenPanel.Controls.Add(TButton);
        }
        void Token_OnMouseDown(object sender, MouseEventArgs e)
        {
            Button source = (Button)sender;
            if (e.Button==MouseButtons.Left)
            {
                //grab token
                Token t = _PanelTokens[source];
                GrabToken(t, TokenSet.TokenPanel);
                //remove source button
                _PanelTokens.Remove(source);
                _TokenPanel.Controls.Remove(source);
            }
            else if(e.Button==MouseButtons.Right)
            {
                //delete this token
                _PanelTokens.Remove(source);
                _TokenPanel.Controls.Remove(source);
            }
        }//end event handler
    }//token set
}//end Map display
