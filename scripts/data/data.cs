using Godot;
using System;

public static class data
{
    public static tileSet[,] tiles;
    static tileSet common = new(new tileType[] { new basicTile() }, new float[] { 100 }); //this contains all possible tiletypes for the Common tileset; rewrite this eventually to have weights as well


    static colorSet starter = new colorSet(new string[] { "2cf5eb", "19e68a", "f52cd3", "e66b19", "f5ee2f", "35f52f", "a52ff5", "f53c2f" });

    public static pieceType bLong = new(4, 1, tiles = new tileSet[,] { { common }, { common }, { common }, { common } }, new Vector2I(2, 0), 4, "Line", starter);
    public static pieceType bSquare = new(2, 2, tiles = new tileSet[,] { { common, common }, { common, common } }, new Vector2I(1, 1), 4, "Square", starter);
    public static pieceType bTBlock = new(3, 2, tiles = new tileSet[,] { { common, null }, { common, common }, { common, null } }, new Vector2I(1, 0), 4, "T Block", starter);
    public static pieceType bLBlockR = new(2, 3, tiles = new tileSet[,] { { common, common, common }, { common, null, null } }, new Vector2I(0, 1), 4, "Right L Block", starter);
    public static pieceType bLBlockL = new(2, 3, tiles = new tileSet[,] { { common, null, null }, { common, common, common } }, new Vector2I(0, 1), 4, "Left L Block", starter);
    public static pieceType bZBlockR = new(2, 3, tiles = new tileSet[,] { { null, common, common }, { common, common, null } }, new Vector2I(0, 1), 4, "Right Z Block", starter);
    public static pieceType bZBlockL = new(2, 3, tiles = new tileSet[,] { { common, common, null }, { null, common, common } }, new Vector2I(0, 1), 4, "Left Z Block", starter);

    public static pieceType bWedge = new pieceType(2, 2, new tileSet[,] { { common, common }, { common, null } }, new Vector2I(0, 0), 3, "Wedge", starter);
    public static pieceType bShortStick = new pieceType(3, 1, new tileSet[,] { { common }, { common }, { common } }, new Vector2I(1, 0), 3, "Short Line", starter);
    public static pieceType bBowl = new pieceType(3, 2, new tileSet[,] { { common, common }, { common, null }, { common, common } }, new Vector2I(1, 0), 5, "Bowl", starter);
    public static pieceType bCorner = new pieceType(3, 3, new tileSet[,] { { common, common, common }, { common, null, null }, { common, null, null } }, new Vector2I(0, 0), 5, "Corner", starter);
    public static pieceType bRectangle = new pieceType(3, 2, new tileSet[,] { { common, common }, { common, common }, { common, common } }, new Vector2I(1, 0), 6, "Rectangle", starter);
    public static pieceType bCrowbarL = new pieceType(2, 5, new tileSet[,] { { common, common, common, common, common }, { null, null, null, null, common } }, new Vector2I(0, 3), 6, "Left Crowbar", starter);
    public static pieceType bCrowbarR = new pieceType(2, 5, new tileSet[,] { { null, null, null, null, common }, { common, common, common, common, common } }, new Vector2I(0, 3), 6, "Right Crowbar", starter);
    public static pieceType bLongT = new pieceType(3, 3, new tileSet[,] { { null, null, common }, { common, common, common }, { null, null, common } }, new Vector2I(1, 1), 5, "Long T Block", starter);
    
    public static pieceType bDiamond = new pieceType(3, 3, new tileSet[,] { { null, common, null }, { common, common, common }, { null, common, null } }, new Vector2I(1, 1), 5, "Diamond", starter);

    public static pieceType bBrick = new(4, 3, tiles = new tileSet[,] { { common, common, common }, { common, common, common }, { common, common, common }, { common, common, common } }, new Vector2I(2, 2), 12, "Brick", starter);
}