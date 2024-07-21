using Godot;
using System;

public class boardPiece
{
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
    public string name { get; set; }
    public rarity rarity { get; set; }
    public Color color { get; set; }
}
