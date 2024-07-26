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
    public int rotation {  get; set; } //varies from 0-3, since rotation can only be done in 90 degree increments
    public string name { get; set; }
    public rarity rarity { get; set; }
    public Color color { get; set; }


    public void updateGraphics(board board)
    {
        foreach(tile tile in tiles)
        {
            if(tile!= null)
            {
                board.renderQueue.Add(tile);
            }
        }
        board.updateAscii();
    }

    public void rotatePiece(board board, int direction) //DIRECTION SHOULD ONLY EVER BE -1 OR 1
    {
        foreach (tile tile in tiles)
        {
            if(tile != null)
            {
                tile.rotate(direction);
                rotation += direction;
                tile.updatePos();
                updateGraphics(board);
            }
        }
        
    }

    public void playPiece(board board) //runs when a piece is dropped
    {
        pos = new Vector2I(5, 18); //change to proper position later
        updateTilePosition();
        updateGraphics(board);
    }

    public void moveFallingPiece(board board,int xOffset, int yOffset) //x and y offset for which direction to move
    {
        Vector2I offset = new Vector2I(xOffset, yOffset);
        pos += offset;
        updateTilePosition();
        updateGraphics(board);
    }

    public void placePiece(board board)
    {
        foreach (tile tile in tiles) //place every tile in the piece
        {
            if(tile != null)
            {
                tile.place(board);
                tile.isPlaced = true;
                updateGraphics(board);
            }
           
        }
        //update graphics stuff
    }

    public bool isMoveValid(board board, int xOffset = 0, int yOffset = 0) //returns whether or not a move is valid
    {
        foreach(tile tile in tiles)
        {
            if(tile != null)
            {
                if (tile.checkMoveCollision(board, xOffset, yOffset))
                {
                    return false;
                }
            }
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

    public bool fallPiece(board board) //checks collision and lowers piece accordingly, returns if piece should be placed
    {
        if (shouldPlace(board))
        {
            return true;
        }
        moveFallingPiece(board, 0, -1);
        return false;
    }
    public void updateTilePosition()
    {
        foreach(tile tile in tiles)
        {
            if(tile != null )
            {
                tile.updatePos();
            }
        }
    }
}
