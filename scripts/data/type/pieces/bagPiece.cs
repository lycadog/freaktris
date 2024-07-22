using Godot;
using System;

public class bagPiece //used for pieces held in the bag
{ //nest this in board piece maybe?
    public bagPiece(tileType[,] tiles, Vector2I dimensions, string name, rarity rarity, Color color)
    {
        this.tiles = tiles;
        this.dimensions = dimensions;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
    }

    public boardPiece getBoardPiece()
    {
        
        tile[,] tiles = new tile[dimensions.X, dimensions.Y];
        boardPiece piece = new boardPiece(tiles, dimensions, name, rarity, color);
        for (int x = 0; x < dimensions.X; x++){
            for(int y = 0; y < dimensions.Y; y++){ //process through each tile of the bagPiece and create a real tile for the boardPiece
                if (this.tiles[x, y] != null) //only process solid tiles!
                {
                    tiles[x, y] = new tile(this.tiles[x, y], piece, new Vector2I(x, y));
                }}}
        return piece;

    }

    public tileType[,] tiles { get; set; }
    public Vector2I dimensions { get; set; }
    public string name { get; set; }
    public int cooldown { get; set; } //may or may not use
    public rarity rarity { get; set; }
    public float weight { get; set; } //used to decide chance of drawing this from the bag. may or may not remain a float, decrease this temporarily after drawing the piece maybe?
    public Color color { get; set; }
}
