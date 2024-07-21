using Godot;
using System;

public class pieceType
{
    public pieceType(int xSize, int ySize, tileSet[,] tiles, string name, Color color) 
    {
        dimensions = new Vector2I(xSize, ySize);
        tileSet = tiles;
        this.name = name;
 
    }
    public tileSet[,] tileSet { get; set; }
    public Vector2I dimensions { get; set; }
    public string name { get; set; }
    public rarity rarity { get; set; }
    public colorSet color { get; set; } //properly define and use colorsets

    public void addToBag(bag bag) //add new piece to player's bag //REWORK HEAVILY to use new split bag piece and board pieces!!!!!
    {
        tileType[,] tiles = new tileType[dimensions.X,dimensions.Y];
        
        for (int x = 0; x < dimensions.X; x++){
            for (int y = 0; y < dimensions.Y; y++){
                GD.Print(x + "," + y);
                if (tileSet[x, y] != null){ //get the tiletype from each tileset
                    tiles[x, y] = tileSet[x, y].getRandomType();
                }
            }}
        bagPiece piece = new bagPiece(tiles, dimensions, name, rarity, Colors.White); //create new piece
        //replace Colors.White with colorset.getrandomcolor later

        bag.pieces.Add(piece); //add new piece to player's bag
        GD.Print("piece " + piece.name + " added!");
    }
    public void addToStarterBag(bag bag)
    {
        bag.pieces = new System.Collections.Generic.List<bagPiece>();
        
    }
}
