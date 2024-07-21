using Godot;
using System;

public class tile
{
    public tile(tileType type, boardPiece piece, Vector2I localPos)
    {
        this.type = type;
        this.piece = piece;
        this.localPos = localPos; //we need a function for newly created tiles to initialize their local position! perhaps when new pieces are added to bag? //i think i did this already
    }

    public Node2D initializeGfx(Node2D nTiles, Polygon2D defaultTile) //create a square polygon for the new tile
    {
        Polygon2D newTile = new Polygon2D();
        nTiles.AddChild(newTile);
        newTile.Position = defaultTile.Position;
        newTile.Polygon = defaultTile.Polygon;
        newTile.Name = "tile" + boardPos.X + "," + boardPos.Y;
        //newTile.Color = piece.color;
        newTile.MoveLocalX(boardPos.X * 28);
        newTile.MoveLocalY(boardPos.Y * -28);
        gfx = newTile;
        return newTile;
    }

    public tileType type { get; set; } //rework how tiletype methods work maybe by adding them here
    public boardPiece piece { get; set; }
    public Vector2I boardPos { get; set; } //used for placed tiles on the board
    public Vector2I localPos { get; set; } //used for local position relative to the origin of the piece the tile belongs to
    public Node2D gfx { get; set; }
    public Node2D specialGfx { get; set; }

    public bool checkCollision(board board) //returns whether or not the tile is currently colliding with something (ie about to be placed)
    {
        GD.Print($"checkCollision event at {boardPos.X}, {boardPos.Y}");

        if (boardPos.Y == 0) //check if the tile is at the bottom of the board
        {
            boardCollideFalling(board);
            return true;
        }
        return type.checkCollision(board.tiles, boardPos);

    }
    public void collideFalling(board board) //runs when this falling tile collides with another tile ***WORRY ABOUT THESE 2 METHODS LATER***
    {
        GD.Print($"collideFalling event at {boardPos.X}, {boardPos.Y}");

        type.collideFalling(board.tiles, this);
    }
    public void boardCollideFalling(board board) //runs when this falling tile collides with the bottom of the board 
    {
        GD.Print($"boardCollideFalling event at {boardPos.X}, {boardPos.Y}");

        type.boardCollide(board.tiles, this);
    }
    public void collideGround(board board) //runs when this placed tile collides with a falling tile ***WORRY ABOUT THESE 2 METHODS LATER***
    {
        GD.Print($"collideGround event at {boardPos.X}, {boardPos.Y}");

        type.collideGround(board.tiles, this);
    }

    public void place(board board) //places the tile on the board properly **** SOMETHING STRANGE happening here, the first tile is placed properly but the rest lag (are 1 tile up briefly), may result in issues
    {
        GD.Print($"place event at {boardPos.X}, {boardPos.Y}");

        boardPos = piece.pos + localPos; //get proper tile position

        board.tiles[boardPos.X, boardPos.Y] = this; //place tile on the board
        type.place(board.tiles, this);
    }
    public void tick(board board) //runs after every turn
    {
        GD.Print($"tick event at {boardPos.X}, {boardPos.Y}");

        type.tick(board.tiles, this);
    }

    public long score(board board, long score) //runs when this tile is scored
    {
        GD.Print($"score event at {boardPos.X}, {boardPos.Y}");

        long tileScore = type.score(board.tiles, this, score);
        remove(board);
        return tileScore;
    }

    public void destroy(board board) //runs when this tile is removed without scoring it
    {
        type.destroy(board.tiles, this);
        remove(board);
    }


    public void updatePos() //updates tile board position
    {
        boardPos = piece.pos + localPos;
    }
    public void remove(board board) //used to remove a tile
    {
        board.tiles[boardPos.X, boardPos.Y] = null;
        board.staleTiles.Add(this);
    }
}