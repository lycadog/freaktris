using Godot;
using System;

public class bagPiece //used for pieces held in the bag
{ //nest this in board piece maybe?
    public bagPiece(tileType[,] tiles, Vector2I dimensions, Vector2I origin, int tileCount, string name, rarity rarity, string color)
    {
        this.tiles = tiles;
        this.dimensions = dimensions;
        this.origin = origin;
        this.tileCount = tileCount;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
        weight = 1;
    }

    public boardPiece getBoardPiece(board board)
    {
        
        tile[] tiles = new tile[tileCount];
        boardPiece piece = new boardPiece(tiles, dimensions, origin, name, rarity, color, board);
        int index = 0;
        for (int x = 0; x < dimensions.X; x++){
            for(int y = 0; y < dimensions.Y; y++){ //process through each tile of the bagPiece and create a real tile for the boardPiece
                if (this.tiles[x, y] != null) //only process solid tiles!
                {
                    
                    tile tile = new tile(piece, new Vector2I(x - origin.X, y - origin.Y), board);
                    tile.type = this.tiles[x, y].getNewInstance(board, tile);
                    tiles[index] = tile;
                    index++;
                }}}
        return piece;

    }

    public tileType[,] tiles { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I origin { get; set; }
    public int tileCount {  get; set; }
    public float weight { get; set; } //used to decide chance of drawing this from the bag. may or may not remain a float, decrease this temporarily after drawing the piece maybe?
    public string name { get; set; }
    public string color { get; set; }
    public rarity rarity { get; set; }
}
