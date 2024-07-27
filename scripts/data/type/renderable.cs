using Godot;
using System;

public class renderable
{
    public renderable(Vector2I pos, string text, bool temp)
    {
        this.pos = pos;
        this.text = text;
        temporary = temp;
    }
    public Vector2I pos {  get; set; }
    public string text { get; set; }
    public bool temporary {  get; set; } //if the tile should be removed next graphics tick or not
}
