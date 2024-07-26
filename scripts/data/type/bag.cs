using Godot;
using System;
using System.Collections.Generic;

public class bag
{
    //add a proper constructor to use a new starter set type to add pieces!
    public bag()
    {
        pieces = new List<bagPiece>();
        data.bLong.addToBag(this);
        data.bLBlockL.addToBag(this);
        data.bLBlockR.addToBag(this);
        data.bZBlockL.addToBag(this);
        data.bZBlockR.addToBag(this);
        data.bSquare.addToBag(this);
        data.bTBlock.addToBag(this);
        name = "bag";
        GD.Print("bag made!");
    }

    public List<bagPiece> pieces;
    public tileSet starterSet;



    public string name { get; set; }
    public boardPiece getPiece()
    {
        int index = GD.RandRange(0, pieces.Count-1);
        return pieces[index].getBoardPiece();
    }
}
