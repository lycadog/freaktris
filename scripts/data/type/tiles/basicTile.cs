using Godot;
using System;

[Serializable]
public class basicTile : tileType
{

    public override bool checkMoveCollision(tile[,] board, Vector2I boardPos, Vector2I checkPos)
    {
        return board[checkPos.X, checkPos.Y] != null; //rework this to call events and such later
    }
    public override bool checkFallingCollision(tile[,] board, Vector2I boardPos)
    {
        return board[boardPos.X, boardPos.Y - 1] != null; //check if there is a piece below the tile

    }
    public override void boardCollide(tile[,] board, tile tile)
    {
    }
    public override void collideFalling(tile[,] board, tile hit)
    {
    }

    public override void collideGround(tile[,] board, tile tile)
    {
    }

    public override void destroy(tile[,] board, tile tile)
    {
    }

    public override void place(tile[,] board, tile tile)
    {
    }

    public override long score(tile[,] board, tile tile, long rowScore)
    {
        return rowScore + 1;
    }

    public override void tick(tile[,] board, tile tile)
    {
    }
}