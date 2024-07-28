using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public class boardPiece
{ //add methods to move and rotate the piece during piecefall
    public boardPiece(tile[] tiles, Vector2I dimensions, Vector2I origin, string name, rarity rarity, string color, board board)
    {
        this.tiles = tiles;
        this.dimensions = dimensions;
        this.origin = origin;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
        this.board = board;
        rotDimensions = dimensions;
        isPlaced = false;
    }
    public board board;
    public tile[] tiles { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I rotDimensions { get; set; } //piece dimensions but rotated properly after rotation
    public Vector2I pos { get; set; } //this position might be desynced from the piece's tile's positions due to 0 index array shenanigans, look into later //what
    public Vector2I dropPos {  get; set; } 
    public Vector2I origin {  get; set; }
    public bool isPlaced {  get; set; }
    public string name { get; set; }
    public string color { get; set; }
    public rarity rarity { get; set; }

    public void updateGraphics()
    {
        if (!isPlaced) { renderDropShadow(); }
        foreach (tile tile in tiles)
        {
            GD.Print(tile);
                tile.render(board);
        }
        
        
        board.updateAscii();
    }

    public void rotatePiece(int direction) //returns true if the rotation is valid
    {
        foreach(tile tile in tiles)
        {
                tile.rotate(direction);
                tile.render(board);
        }
        Vector2I swap = new Vector2I(rotDimensions.X, rotDimensions.Y);
        rotDimensions = swap; //switch rot dimensions since rotDimensions is rotate-adjusted dimensions
        renderDropShadow();
        
        board.updateAscii();
    }
    
    public bool isRotationValid(int direction)
    {
        GD.Print("CHECKING ROTATION");
        foreach(tile tile in tiles)
        {           
            Vector2I rotateBoardPos = tile.getRotatePos(direction) + pos;
            if (!board.isPositionValid(rotateBoardPos, tile.type.shouldCollide()))
            {
                return false;
            }

        }
        return true;
    }

    public void playPiece() //runs when a piece is dropped
    {
        pos = new Vector2I(5, 22-(dimensions.Y-origin.Y+1)); //change to proper position later
        updateTilePosition();
        updateGraphics();
    }

    public void moveFallingPiece(int xOffset, int yOffset) //x and y offset for which direction to move
    {
        Vector2I offset = new Vector2I(xOffset, yOffset);
        pos += offset;
        updateTilePosition();
        updateGraphics();
    }

    public void placePiece()
    {
        foreach (tile tile in tiles) //place every tile in the piece
        {
            tile.place();
            tile.isPlaced = true;
            tile.render(board);
        }
        isPlaced = true;
        board.updateAscii();
    }

    public bool isMoveValid(int xOffset = 0, int yOffset = 0) //returns whether or not a move is valid
    {
        foreach(tile tile in tiles)
        {            
            if (tile.checkMoveCollision(xOffset, yOffset))    
            {
                return false;
            }
        }
        return true;
    }

    public bool shouldPlace() //checks if a piece should be placed or not
    {
        foreach(tile tile in tiles)
        {
            //process through every solid tile and check the collision
            
            if (tile.checkFallingCollision() == false)    
            {
                continue; //keep checking collision if the tile is not colliding, if one tile collides then the else will return true
            }
                
            else { return true; }
        }
        return false; //if no tiles collide then return false
    }

    public bool fallPiece() //checks collision and lowers piece accordingly, returns if piece should be placed
    {
        if (shouldPlace())
        {
            return true;
        }
        moveFallingPiece(0, -1);
        return false;
    }

    public void renderDropShadow() //renders the shadow below pieces and the piece preview
    {
        int y = 0;
        dropPos = pos;
        while(true)
        {
            if (!isMoveValid(0, -y-1)) //i have no idea why but it always goes too low and reaches out of bounds without the -1. do NOT remove it
            {
                dropPos = new Vector2I(pos.X, pos.Y - y);
                break;
            }
            else { y += 1; }
            
        }
        foreach(tile tile in tiles)
        {
            Vector2I previewPos = new Vector2I(tile.boardPos.X, tile.boardPos.Y - y);
            renderable render = new renderable(previewPos, "▒", true);
            board.renderQueue.Add(render);
            
        }
    }

    public void updateTilePosition()
    {
        foreach(tile tile in tiles)
        {
            tile.updatePos();
        }
    }
}
