using Godot;
using System;

public class basicTile : tileType
{
    public tile tile;
    public board board;

    public basicTile(tile tile = null, board board = null)
    {
        this.tile = tile;
        this.board = board;
    }

    public override tileType getNewInstance(board board, tile tile)
    {
        return new basicTile(tile, board);
    }
    public override string getAscii()
    {
        return "O";
    }
    public override bool shouldCollide()
    {
        return true;
    }
    public override bool checkMoveCollision(Vector2I boardPos, Vector2I checkPos)
    {
        return board.tiles[checkPos.X, checkPos.Y] != null && board.tiles[checkPos.X, checkPos.Y].checkPlacedCollision(); //rework this to call events and such later
    }
    public override bool checkFallingCollision(Vector2I boardPos)
    {
        return board.tiles[boardPos.X, boardPos.Y - 1] != null && board.tiles[boardPos.X, boardPos.Y-1].checkPlacedCollision(); //check if there is a piece below the tile

    }
    public override bool checkPlacedCollision()
    {
        return true;
    }
    public override void boardCollide(tile tile)
    {
    }
    public override void collideFalling(tile hit)
    {
    }

    public override void collideGround(tile tile)
    {
    }

    public override void destroy(tile tile)
    {
    }

    public override void place(tile tile)
    {
    }

    public override long score(tile tile)
    {
        return 1;
    }
    public override long getMultiplier(tile tile)
    {
        return 0;
    }

    public override void tick(tile tile)
    {
    }
}