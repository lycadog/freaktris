using Godot;
using System;

[Serializable]
public abstract class tileType //add method parameters later
{
    //maybe rework this class so the tile class itself has all these methods, and just calls from the type
    //so for example you can call tile.checkCollision and, it'll check the default collision needed for all tiles, AND then call the tiletype collision

    public abstract bool checkMoveCollision(tile[,] board, Vector2I boardPos, Vector2I checkPos); //returns whether or not the next move collides
    public abstract bool checkFallingCollision(tile[,] board, Vector2I boardPos); //returns whether or not the falling piece is colliding with something below it
    public abstract void collideFalling(tile[,] board, tile tile); //runs when this falling tile collides with another tile
    public abstract void boardCollide(tile[,] board, tile tile); //runs when this falling tile collides with the bottom of the board
    public abstract void collideGround(tile[,] board, tile tile); //runs when this placed tile collides with a falling tile

    public abstract void place(tile[,] board, tile tile); //places the tile on the board properly
    public abstract void tick(tile[,] board, tile tile); //runs after every piece played
    public abstract long score(tile[,] board, tile tile, long score); //runs when this tile is scored
    public abstract void destroy(tile[,] board, tile tile); //runs when this tile is removed without scoring it
}
