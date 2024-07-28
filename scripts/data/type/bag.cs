using Godot;
using System;
using System.Collections.Generic;

public class bag
{
    //add a proper constructor to use a new starter set type to add pieces!
    public bag()
    {
        pieces = new List<bagPiece>();
        /*data.bLong.addToBag(this);
        data.bSquare.addToBag(this);
        data.bLBlockL.addToBag(this);
        data.bLBlockR.addToBag(this);
        data.bZBlockL.addToBag(this);
        data.bZBlockR.addToBag(this);*/
        data.bLong.addToBag(this);
        data.bShortStick.addToBag(this);
        data.bWedge.addToBag(this);
        data.bWedge.addToBag(this);
        data.bLongT.addToBag(this);
        data.bCorner.addToBag(this);
        data.bBowl.addToBag(this);
        data.bRectangle.addToBag(this);
        name = "bag";
        GD.Print("bag made!");
    }

    public List<bagPiece> pieces;
    public tileSet starterSet;



    public string name { get; set; }
    public boardPiece getPiece(board board)
    {
        foreach(bagPiece pieceW in pieces)
        {
            if(pieceW.weight != 1)
            {
                pieceW.weight += 0.2f;
            }
        }

        int index = 0;
        bagPiece piece;
        for(int i = 0; i < 20; i++)
        {
            float rand = GD.Randf();
            index = GD.RandRange(0, pieces.Count - 1);
            piece = pieces[index];
            if (rand < piece.weight)
            {
                piece.weight -= .4f;
                break;
            }
        }
        
        return pieces[index].getBoardPiece(board);
    }
}
