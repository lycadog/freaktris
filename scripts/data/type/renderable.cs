using Godot;
using System;

public class renderable
{
    public renderable(Vector2I pos, string text, int z, bool temp)
    {
        this.pos = pos;
        this.text = text;
        this.z = z;
        temporary = temp;
    }
    public Vector2I pos {  get; set; }
    public int z {  get; set; }
    public string text { get; set; }
    public bool temporary {  get; set; } //if the tile should be removed next graphics tick or not
}
