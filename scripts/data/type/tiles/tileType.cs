using Godot;
using System;

[Serializable]
public abstract class tileType //add method parameters later
{
    //maybe rework these to take board as a paramter instead of board.tiles
    public abstract bool checkMoveCollision(board board, Vector2I boardPos, Vector2I checkPos); //returns whether or not the next move collides
    public abstract bool checkFallingCollision(board board, Vector2I boardPos); //returns whether or not the falling piece is colliding with something below it
    public abstract bool checkPlacedCollision(board board);
    public abstract void collideFalling(board board, tile tile); //runs when this falling tile collides with another tile
    public abstract void boardCollide(board board, tile tile); //runs when this falling tile collides with the bottom of the board
    public abstract void collideGround(board board, tile tile); //runs when this placed tile collides with a falling tile

    public abstract void place(board board, tile tile); //places the tile on the board properly
    public abstract void tick(board board, tile tile); //runs after every piece played
    public abstract long score(board board, tile tile, long score); //runs when this tile is scored
    public abstract void destroy(board board, tile tile); //runs when this tile is removed without scoring it
}
