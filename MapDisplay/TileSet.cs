using System;
using System.Collections.Generic;


namespace MapDisplay
{
    class TileSet
    {
        protected Dictionary<String, Tile> _Tiles;
        protected int _TileWidth, _TileHeight;
        public int Width { get { return _TileWidth; } }
        public int Height { get { return _TileHeight; } }
        public TileSet(int width, int height)
        {
            _TileWidth = width;
            _TileHeight = height;
            _Tiles = new Dictionary<string, Tile>();
        }
        public int getTileWidth()
        {
            return _TileWidth;
        }
        public int getTileHeight()
        {
            return _TileHeight;
        }
        public void createTile(System.Drawing.Image i, string name)
        {
            Tile t = new Tile(i, name);
            _Tiles.Add(name, t);
        }
        public void add(Tile t)
        {
            _Tiles[t.getName()] = t;//unlike Dictionary.add, this allows tiles to be overwritten
        }
        public Tile get(string name)
        {
            return _Tiles[name];
        }
        public ICollection<string> getTileNames()
        {
            return _Tiles.Keys;
        }
        public bool ContainsTile(string name)
        {
            return _Tiles.ContainsKey(name);
        }
    }//end class
}//end namespace
