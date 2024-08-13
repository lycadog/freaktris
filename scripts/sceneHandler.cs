using Godot;
using System;
using static main;

public partial class sceneHandler : Node
{

    [Signal]
    public delegate void EncounterEventHandler(int g);

    public void startEncounter(bag bag) //specify enemy encounter and area later
    { //load up board scene and attach it to root node
        var scene = GD.Load<PackedScene>("res://scenes/board.tscn");
        AddChild(scene.Instantiate());
        Node2D world = GetNode<Node2D>("boardScene/world");
        //ADD CODE to send a signal to Main containing the bag
    }

    public void startRun(starterBag bag)
    {
    }

    public override void _Ready()
    {
        startEncounter(new bag(data.classicBag));
    }
}
