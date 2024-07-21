using Godot;
using System;

[Serializable]
public class basicTile : tileType
{


    public override bool checkCollision(tile[,] board, Vector2I boardPos)
    {
        return board[boardPos.X, boardPos.Y - 1] != null; //check if there is a piece 1 tile below this one
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