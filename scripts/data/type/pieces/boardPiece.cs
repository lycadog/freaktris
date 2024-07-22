using Godot;
using System;

public class boardPiece
{ //add methods to move and rotate the piece during piecefall
    public boardPiece(tile[,] tiles, Vector2I dimensions, string name, rarity rarity, Color color)
    {
        this.tiles = tiles;
        this.dimensions = dimensions;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
    }
    public tile[,] tiles { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I pos { get; set; } //this position might be desynced from the piece's tile's positions due to 0 index array shenanigans, look into later
    public string name { get; set; }
    public rarity rarity { get; set; }
    public Color color { get; set; }

    public void playPiece(board board) //runs when a piece is dropped
    {
        pos = new Vector2I(5, 20); //change to proper position later
        foreach (tile tile in tiles)
        {
            tile.updatePos();
            //add graphics stuff here
        }
    }

    public void placePiece(board board)
    {
        foreach (tile tile in tiles) //place every tile in the piece
        {
            tile.place(board);
        }
        //update graphics stuff
    }

    public bool isMoveValid(board board, int xOffset = 0, int yOffset = 0) //returns whether or not a move is valid
    {
        foreach(tile tile in tiles)
        {
            tile.checkMoveCollision(board, xOffset, yOffset);
        }


        return true;
    }
    public bool shouldPlace(board board) //checks if a piece should be placed or not
    {
        foreach(tile tile in tiles)
        {
            if(tile != null)
            { //process through every solid tile and check the collision
                if (tile.checkFallingCollision(board) == false)
                {
                    continue; //keep checking collision if the tile is not colliding, if one tile collides then the else will return true
                }
                else { return true; }
            }
        }
        return false; //if no tiles collide then return false
    }

    public void fallPiece(board board) //lowers the piece
    {
        pos = new Vector2I(pos.X, pos.Y - 1); //lower the piece
        foreach (tile tile in tiles) //update every tile's position
        {
            tile.updatePos();
        }
        //update graphics to lower with the piece!
        //maybe use the board renderer to render them while falling similar to a normal placed tile
    }
}
