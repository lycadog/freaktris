using Godot;
using System;

public class colorSet
{
    public colorSet(string[] colors)
    {
        this.colors = colors;
    }

    public string[] colors;
    public string getRandomColor()
    {
        return colors[GD.RandRange(0, colors.Length-1)];
    }
}
