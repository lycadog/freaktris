using Godot;
using System;

[Serializable]
public class basicTile : tileType
{

    public override bool checkMoveCollision(board board, Vector2I boardPos, Vector2I checkPos)
    {
        return board.tiles[checkPos.X, checkPos.Y] != null && board.tiles[checkPos.X, checkPos.Y].checkPlacedCollision(board); //rework this to call events and such later
    }
    public override bool checkFallingCollision(board board, Vector2I boardPos)
    {
        return board.tiles[boardPos.X, boardPos.Y - 1] != null && board.tiles[boardPos.X, boardPos.Y-1].checkPlacedCollision(board); //check if there is a piece below the tile

    }
    public override bool checkPlacedCollision(board board)
    {
        return true;
    }
    public override void boardCollide(board board, tile tile)
    {
    }
    public override void collideFalling(board board, tile hit)
    {
    }

    public override void collideGround(board board, tile tile)
    {
    }

    public override void destroy(board board, tile tile)
    {
    }

    public override void place(board board, tile tile)
    {
    }

    public override long score(board board, tile tile, long rowScore)
    {
        return rowScore + 1;
    }

    public override void tick(board board, tile tile)
    {
    }


}