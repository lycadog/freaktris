using Godot;
using System;

public static class data
{
    public static tileSet[,] tiles;
    static tileSet common = new(new tileType[] { new basicTile() }, new float[] {100}); //this contains all possible tiletypes for the Common tileset; rewrite this eventually to have weights as well

    public static pieceType bLong = new(4, 1, tiles = new tileSet[,] { { common }, { common }, { common }, { common } },"Line", Colors.White);
    public static pieceType bSquare = new(2, 2, tiles = new tileSet[,] { { common, common }, { common, common } }, "Square", Colors.White);
    public static pieceType bTBlock = new(3, 2, tiles = new tileSet[,] { { common, common, common, }, { null, common, null } },"T Block", Colors.White);
    public static pieceType bLBlockR = new(2, 3, tiles = new tileSet[,] { { common, null }, { common, null }, { common, common } }, "Right L Block", Colors.White);
    public static pieceType bLBlockL = new(2, 3, tiles = new tileSet[,] { { null, common }, { null, common }, { common, common } }, "Left L Block", Colors.White);
    public static pieceType bZBlockR = new(2, 3, tiles = new tileSet[,] { { null, common }, { common, common }, { common, null } }, "Right Z Block", Colors.White);
    public static pieceType bZBlockL = new(2, 3, tiles = new tileSet[,] { { common, null }, { common, common }, { null, common } }, "Left Z Block", Colors.White);

}