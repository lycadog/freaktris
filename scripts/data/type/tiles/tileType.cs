using Godot;
using System;

public abstract class tileType //add method parameters later
{
    //maybe rework these to take board as a paramter instead of board.tiles
    public abstract bool shouldCollide();
    public abstract string getAscii();
    public abstract tileType getNewInstance(board board, tile tile);

    public abstract bool checkMoveCollision(Vector2I boardPos, Vector2I checkPos); //returns whether or not the next move collides
    public abstract bool checkFallingCollision(Vector2I boardPos); //returns whether or not the falling piece is colliding with something below it
    public abstract bool checkPlacedCollision();
    public abstract void collideFalling(tile tile); //runs when this falling tile collides with another tile
    public abstract void boardCollide(tile tile); //runs when this falling tile collides with the bottom of the board
    public abstract void collideGround(tile tile); //runs when this placed tile collides with a falling tile
    public abstract void place(tile tile); //places the tile on the board properly
    public abstract void tick(tile tile); //runs after every piece played
    public abstract long score(tile tile); //runs when this tile is scored
    public abstract long getMultiplier(tile tile); //this is ADDITIVE, meaning a value of 1 will add 1 to the multplier, making it 2x - 0 means no multiplier
    public abstract void destroy(tile tile); //runs when this tile is removed without scoring it
}
