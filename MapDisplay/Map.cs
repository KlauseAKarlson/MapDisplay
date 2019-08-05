using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;

namespace MapDisplay
{

    partial class Map
    {
        private int _MapWidth, _MapHeight;
        private MapView _MapView = null;
        private TileSet _TileSet;
        private String _MapStyle;
        private List<Layer> _layers;
        private TokenSet _TokenSet;
        private List<Token> _TokenLoadBuffer=null;//used for loading saved maps
        public TokenSet tokenSet { get { return _TokenSet; } }
        private TokenLayer _Tokens;
        public static readonly string HexMap = "HexMap", SquareMap = "SquareMap";
        private string _SavePath;
        public string SavePath { get { return _SavePath; } }
        public int MapWidth { get { return _MapWidth; } }
        public int MapHeight { get { return _MapHeight; } }
        public int TileWidth { get { return _TileSet.Width; } }
        public int TileHeight { get { return _TileSet.Height; } }
        public Map(int width, int height, String style, TileSet t, System.Windows.Forms.Panel TokenPanel)
        {
            _MapWidth = width;
            _MapHeight = height;
            _MapStyle = style;
            _TileSet = t;
            //create layer list
            _layers = new List<Layer>();
            _layers.Add(new Layer(this));
            _Tokens = new TokenLayer(this);
            _TokenSet = new TokenSet(this, TokenPanel);
        }
        public static Map loadMap(string SavePath, System.Windows.Forms.Panel TokenPanel)
        {
            
            ZipArchive source = new ZipArchive(
                new FileStream(SavePath, FileMode.Open), 
                ZipArchiveMode.Read);
            //create a mpa from a map save file, which is a zip archive
            TileSet tset;//tile set for the map being loaded
            Map saveMap;//the map beign loaded
            int layers;//the number of layers in the map beign loaded
            List<string> tileNames = new List<string>();
            //tools for reading data from streams
            string currentLine;//stores current line for searching and creating substrings
            int start, end;//indexes for creating substrings
            //read map 
            ZipArchiveEntry description = source.GetEntry("save.txt");
            using(StreamReader reader=new StreamReader( description.Open() )  )
            {
                //read style from the first line
                string style = reader.ReadLine();
                //read map width and height from second line
                currentLine = reader.ReadLine();
                start = currentLine.IndexOf(":") + 1;
                end = currentLine.IndexOf(",");
                int mapWidth =int.Parse( currentLine.Substring(start, end - start) );
                start = end + 1;
                int mapHeight = int.Parse(currentLine.Substring(start));
                //read map layer count
                currentLine = reader.ReadLine();
                start = currentLine.IndexOf(":") + 1;
                layers = int.Parse(currentLine.Substring(start));//will be used to load laters
                //read tile width and height
                currentLine = reader.ReadLine();
                start = currentLine.IndexOf(":") + 1;
                end = currentLine.IndexOf(",");
                int tileWidth = int.Parse(currentLine.Substring(start, end - start));
                start = end + 1;
                int tileHeight = int.Parse(currentLine.Substring(start));

                tset = new TileSet(tileWidth, tileHeight);
                //read tile names
                currentLine = reader.ReadLine();
                start = 0;
                end = currentLine.IndexOf(",");
                while (end != -1)//index of returns -1 if there are no matching characters
                {
                    tileNames.Add(currentLine.Substring(start, end-start));
                    start = end + 1;
                    end = currentLine.IndexOf(",", start);
                }//end while loop, will leave one more tile in the list
                tileNames.Add(currentLine.Substring(start));
                //create map
                saveMap = new Map(mapWidth, mapHeight, style, tset, TokenPanel);
                saveMap._SavePath = SavePath;
            }
            //load tiles, tiles must be in the Tile set for the map layers to be loaded
            foreach (string name in tileNames)
            {
                ZipArchiveEntry imageEntry = source.GetEntry(name + ".png");
                using (Stream iStream = imageEntry.Open())
                {
                    System.Drawing.Bitmap tImage = new System.Drawing.Bitmap(iStream);
                    tset.createTile(tImage, name);//tile constructor will create local copy of tImage
                }
            }//end load tiles
            //load layers
            for (int layer=0; layer<layers; layer++)
            {
                String tileName;
                if (saveMap.getLayerCount() <= layer)
                    saveMap.addLayer();
                ZipArchiveEntry layerEntry = source.GetEntry("Layer" + layer + ".csv");
                using ( StreamReader reader = new StreamReader( layerEntry.Open() )  )
                {
                    int collumn = 0;
                    for (int row =0;row<saveMap._MapHeight;row++)
                    {
                        currentLine = reader.ReadLine();
                        //ready substring indexes
                        start = 0;
                        end = currentLine.IndexOf(",");
                        collumn = 0;
                        while (end != -1)//index of returns -1 if there are no matching characters
                        {
                            tileName = currentLine.Substring(start, end-start);
                            saveMap.setTile(tileName, collumn, row, layer);
                            collumn++;
                            start = end + 2;
                            end = currentLine.IndexOf(",", start);
                        }//at the end there will be one remaining tile
                        tileName = currentLine.Substring(start);
                        saveMap.setTile(tileName, collumn, row, layer);
                    }//end row loop
                }//end stream reader
            }//end load layer loop
            //load tokens
            ZipArchiveEntry TokenList = source.GetEntry("TokenList.txt");
            if (TokenList != null)//not all maps will have a tokenlist
            {
               using(StreamReader reader=new StreamReader(TokenList.Open()) )
                {
                    //create local variables for use in loop
                    saveMap._TokenLoadBuffer = new List<Token>();
                    string tokenName;
                    ZipArchiveEntry TokenEntry;
                    Token Tkn;
                    int col, row;
                    while(!reader.EndOfStream)
                    {
                        currentLine = reader.ReadLine();
                       
                        //determine if the token is in the tokenpanel or map
                        char location = currentLine.ElementAt(0);
                        if (location == 'P')//if the location is the panel
                        {
                            tokenName = currentLine;//no additional data is stored for this type of token
                            //retrieve image and create token
                            TokenEntry = source.GetEntry(tokenName+".png");
                            using (Bitmap TImage = new Bitmap(TokenEntry.Open()) )
                            {
                                //we assume the token image has already been processed to make it into the save
                                Tkn = saveMap.tokenSet.CreateToken(TImage);
                                //we need to wait to place tokens into the token panel until the GUI has been cleared of the old map
                                saveMap._TokenLoadBuffer.Add(Tkn);
                                //we will oad these tokens into the panel when the map view is created
                            }//end using TImage
                        }
                        else if (location == 'L')//if the token is in the map token layer
                        {
                            end = currentLine.IndexOf(",");
                            tokenName = currentLine.Substring(0, end);
                            //the token name is only the first part of the entry
                            //we also need to retireve the column and row where the token is located
                            start = end + 1;
                            end = currentLine.IndexOf(",", start);
                            col = int.Parse(currentLine.Substring(start, end - start));
                            row = int.Parse(currentLine.Substring(end + 1));
                            //retreive toekn image and create token
                            TokenEntry= source.GetEntry(tokenName + ".png");
                            using (Bitmap TImage = new Bitmap(TokenEntry.Open()))
                            {
                                //we assume the token image has already been processed to make it into the save
                                Tkn = saveMap.tokenSet.CreateToken(TImage);
                                //place toekn in panel
                                saveMap.setToken(Tkn, col, row);
                            }//end using TImage
                        }//else unknown, do nothing
                    }//end loop throguh token list
                }//end using tokenlist reader
            }//end if tokenlist exists

            source.Dispose();
            //return map
            return saveMap;
        }

        public void SaveMap(String savePath)
        {
            //if we are reusing the source save, only update tokens, otherwise save the whole map
            using (ZipArchive Save = new ZipArchive(
                new FileStream(savePath, FileMode.OpenOrCreate),
                ZipArchiveMode.Update) )
            {
                int TokenCounter = 0;
                
                ZipArchiveEntry TokenList = Save.GetEntry("TokenList.txt");
                if(TokenList!=null) TokenList.Delete();
                TokenList = Save.CreateEntry("TokenList.txt");
                using (StreamWriter writer = new StreamWriter(TokenList.Open()) )
                {
                    string name;
                    //save tokens in token panel
                    foreach (Token t in _TokenSet.getPanelTokens())
                    {
                        name = "P" + TokenCounter;
                        TokenCounter++;

                        writer.WriteLine(name);
                        SaveToken(t, Save, name);
                    }
                    //save tokens in tokenlayer
                    for (int row=0; row<_MapHeight; row++)
                    {
                        for(int col=0; col<_MapWidth; col++)
                        {
                            Token t = getToken(col, row);
                            if(t !=null)
                            {
                                name = "L" + TokenCounter;
                                TokenCounter++;

                                writer.WriteLine($"{name},{col},{row}");
                                SaveToken(t, Save, name);
                            }//end if token present
                        }
                    }
                }//end write tokenlist
                if (_SavePath != savePath)//if this is not the soruce save, create or overwrite the file
                {
                    //if this is not the original map, save the rest of the map
                    //create description and save tile images
                    ZipArchiveEntry description = Save.GetEntry("save.txt");
                    if (description == null) description.Delete();//replace not append
                    description = Save.CreateEntry("save.txt");
                    using (StreamWriter saveWriter = new StreamWriter(description.Open()))
                    {
                        String mapType = this._MapStyle;
                        //write type
                        saveWriter.WriteLine(mapType);
                        //write width and height
                        saveWriter.WriteLine("Map Size:" +
                                this.getWidth() + "," +
                                this.getHeight());
                        //write layers
                        saveWriter.WriteLine("layers:" + this._layers.Count);
                        //write tile size
                        saveWriter.WriteLine("tileSize:" +
                                this.getTileSet().Width + "," +
                                this.getTileSet().Height);
                        List<string> TileNames = new List<string>(_TileSet.getTileNames());
                        for (int i = 0; i<TileNames.Count; i++)
                        {
                            string name = TileNames[i];
                            saveWriter.Write(name);
                            if (i < TileNames.Count-1)
                            {
                                saveWriter.Write(",");
                            }
                            //save tile as well
                            ZipArchiveEntry e = Save.GetEntry(name + ".png");
                            if (e == null) e = Save.CreateEntry(name + ".png");

                            _TileSet.get(name).getImage().Save(
                                e.Open(),
                                System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }//end write description
                    //write layers
                    for (int layer =0;layer<_layers.Count;layer++)
                    {
                        //open or create layer file
                        ZipArchiveEntry layerEntry = Save.GetEntry($"Layer{layer}.csv");
                        if (layerEntry == null) layerEntry.Delete();//replace not append
                        layerEntry = Save.CreateEntry($"Layer{layer}.csv");
                        //create writer for layer file
                        using(StreamWriter saveWriter=new StreamWriter(layerEntry.Open()))
                        {
                            //create local variables
                            Layer currentLayer = _layers[layer];
                            string tileText;
                            Tile currentTile;
                            //write file
                            for (int y = 0; y < _MapHeight; y++)
                            {
                                //write rows naturally
                                for (int x = 0; x < _MapWidth; x++)
                                {
                                    currentTile = currentLayer.Tiles[x,y];
                                    if (currentTile == null)//take care of uninitialized empty tiles
                                    {
                                        tileText = "Empty";
                                    }
                                    else
                                    {
                                        tileText = currentTile.getName();
                                    }
                                    //logic for handling seperators
                                    if (x < _MapWidth - 1)
                                    {
                                        tileText += ", ";
                                    }
                                    else
                                    {
                                        tileText += "\n";
                                    }
                                    saveWriter.Write(tileText);
                                }//end X (width) loop
                            }//end y (height) loop
                        }//end layer save writer
                    }//end layer loop
                }//end save entire map
            }//End ziparchive save
        }//end save map
        private void SaveToken(Token t, ZipArchive a, string name)
        {
            ZipArchiveEntry e=a.GetEntry(name+".png");
            if (e == null)
                e = a.CreateEntry(name + ".png");
            t.getImage().Save(e.Open(), System.Drawing.Imaging.ImageFormat.Png);
        }
        public void addLayer()
        {
            _layers.Add(new Layer(this));
        }
        public int getWidth()
        {
            return _MapWidth;
        }
        public int getHeight()
        {
            return _MapHeight;
        }
        public int getTileWidth()
        {
            return _TileSet.getTileWidth();
        }
        public int getTileHeight()
        {
            return _TileSet.getTileHeight();
        }
        public int getLayerCount()
        {
            return _layers.Count;
        }
        public TileSet getTileSet()
        {
            return _TileSet;
        }
        public string getStyle()
        {
            return _MapStyle;
        }
        public MapView GetMapView()
        {
            //to prevent dangerous creation of Map views, 
            //instances of MapView are implimented as private classes
            if (_MapView == null)//create map view if not present
            {
                if (_MapStyle == Map.SquareMap)
                {
                    _MapView = new SquareMapView();
                }
                else if (_MapStyle == Map.HexMap)
                {
                    _MapView = new HexMapView();
                }
                else
                {
                    throw new Exception("Unkown style:" + _MapStyle);
                }
                _MapView.SetMap(this);
                //if the map had tokens saved, we can now place any tokens that need to go into the token panel
                if (_TokenLoadBuffer != null)
                {
                    foreach (Token t in _TokenLoadBuffer)
                    {
                        tokenSet.PlaceInPanel(t);
                    }
                    _TokenLoadBuffer = null;
                }
            }//end create mapview if needed
            return _MapView;
        }//end get map view
        public void setTile(string tileName, int x, int y, int l)
        {
            if (_TileSet.ContainsTile(tileName))
            {
                setTile(_TileSet.get(tileName), x, y, l);
                InvalidateTile(x, y);
            }
        }
        public void setToken(Token t, int x, int y)
        {
            if (x >= 0 & x < _MapWidth &&
                y >= 0 & y < _MapHeight)
            {
                _Tokens[x, y] = t;
                InvalidateToken(x, y, t);
            }
        }//end set token
        public void setTile(Tile t, int x, int y, int l)
        {
            //places tile t in layer l at position x,y
            //check bounds
            if(x>=0 & x<_MapWidth &&
                y>=0 & y<_MapHeight &&
                l>=0 & l<_layers.Count)
            {
                _layers[l].Tiles[x, y] = t;
                InvalidateTile(x, y);
            }
        }//end set tile
        private void InvalidateTile(int col, int row)
        {
            if (_MapView!=null)
            {
                Size s = new Size(TileWidth, TileHeight);
                Rectangle region = new Rectangle(_MapView.TileStart(col, row), s);
                _MapView.Invalidate(region);
            }
        }//invalidate tile
        private void InvalidateToken(int col, int row, Token t)
        {
            if (_MapView!=null)
            {
                Point tileStart = _MapView.TileStart(col, row);
                Rectangle region = new Rectangle();
                region.X = tileStart.X + (TileWidth - t.Width) / 2;
                region.Y = tileStart.Y + (TileHeight - t.Height) / 2;
                region.Width = t.Width;
                region.Height = t.Height;
                _MapView.Invalidate(region);
            }
        }
        public Tile[] GetTiles(int Col, int Row)
        {
            //returns an array of all tiles from the lowest layer to highest at specified col and row
            if (Col >= 0 & Col < _MapWidth &&
                Row >= 0 & Row < _MapHeight)
            {
                Tile[] stack = new Tile[_layers.Count];
                for (int l=0;l<_layers.Count;l++)
                {
                    stack[l] = _layers[l].Tiles[Col, Row];
                }
                return stack;
            }
            else
            {
                return null;
            }
        }
        public Tile getTile(int Col, int Row, int l)
        {
            //returns tile in layer l at position x,y
            //check bounds
            if (Col >= 0 & Col < _MapWidth &&
                Row >= 0 & Row < _MapHeight &&
                l >= 0 & l < _layers.Count)
            {
                return _layers[l].Tiles[Col, Row];
            }
            else
            {

                return null;
            }//end else
        }//end get tile
        public Token getToken(int Col, int Row)
        {
            if (Col >= 0 & Col < _MapWidth &&
                 Row >= 0 & Row < _MapHeight)
            {
                return _Tokens[Col, Row];
            }
            else
            {
                return null;
            }
        }
        public Token pullToken(int Col, int Row)
        {
            //retrieves and removes a token
            if (Col >= 0 & Col < _MapWidth &&
                Row >= 0 & Row < _MapHeight)
            {
                Token t = _Tokens.Pull(Col, Row);
                if (t!=null)
                {
                    InvalidateToken(Col, Row, t);
                }
                return t;
            }
            else
            {
                return null;
            }
        }//end pull token
        public void PaintTilesAt(System.Drawing.Graphics G, int x, int y, int Col, int Row)
        {
            //draws all tiles at Col,Row in Graphics G at point x,y
            Tile[] tiles = GetTiles(Col, Row);
            foreach (Tile t in tiles)
            {
                t.draw(G, x, y);
            }//end for each tile
            //Draw token
            _Tokens.Paint(G, x, y, Col, Row);
        }//end paint tiles at

        private class Layer
        {
            public Tile[,] Tiles;
            internal Layer(Map m)
            {
                Tiles = new Tile[m._MapWidth, m._MapHeight];
            }
        }//end Layer
        private class TokenLayer
        {
            private Token[,] _Tokens;
            Map _map;
            public Token this[int x, int y]
            {
                get { return _Tokens[x, y]; }
                set { _Tokens[x, y] = value; }
            }
            internal TokenLayer(Map m)
            {
                _map = m;
                _Tokens = new Token[m._MapWidth, m._MapHeight];
            }
            internal Token Pull(int x, int y)
            {
                Token t = _Tokens[x, y];
                _Tokens[x, y] = null;
                return t;
            }
            internal void Paint(Graphics g, int x, int y, int Col, int Row)
            {
                Token t = _Tokens[Col, Row];
                if(t!=null)//if null do nothing
                {
                    //ensure that the token is centured over the tile
                    int xt = x + (_map.TileWidth - t.Width) / 2;
                    int yt = y + (_map.TileHeight - t.Height) / 2;
                    t.paint(g, xt, yt);
                }//end if
            }//end paint
        }//end token layer


    }//end Map
}//end namespace
