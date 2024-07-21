using Godot;
using System;

public class tileSet
{
    public tileSet(tileType[] types, float[] chance)
    {
        this.types = types;
        this.chance = chance;
    }

    public tileType[] types { get; set; }
    public float[] chance {  get; set; } //chance of getting the tiletype with the same index in types array, WRITE LARGEST TO SMALLEST
    public tileType getRandomType() //rework this later to use real weighted chance!!!!!
    {
        float rand = GD.Randf(); //rework this entire function later
        int index = 0;

        for (int i = 0; i < types.Length; i++) { //if random number is lower than the tile chance, it will succeed
            if (rand < chance[i])
            {
                index = i;
            }
        }
        return types[index];
    }
    public tileType getStarterType(pieceType piece, bag bag)
    {
        return null;
    }
}
