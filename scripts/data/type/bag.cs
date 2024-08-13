using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class bag
{
    //add a proper constructor to use a new starter set type to add pieces!
    public bag(starterBag bag)
    {
        pieces = new List<bagPiece>();

        GD.Print(bag.name);

        /*foreach(pieceType piece in sBag.pieces)
        {
            GD.Print(piece.name);
            piece.addToBag(this);
        }*/


        name = bag.name;
        GD.Print($"bag {name} made");
    }

    public List<bagPiece> pieces;
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

    /*data.bLong.addToBag(this);
	data.bTBlock.addToBag(this);
    data.bSquare.addToBag(this);
    data.bLBlockL.addToBag(this);
    data.bLBlockR.addToBag(this);
    data.bZBlockL.addToBag(this);
    data.bZBlockR.addToBag(this);*/

    /*data.bLong.addToBag(this);
    data.bStick.addToBag(this);
    data.bWedge.addToBag(this);
    data.bWedge.addToBag(this);
    data.bLongT.addToBag(this);
    data.bCorner.addToBag(this);
    data.bBowl.addToBag(this);
    data.bRectangle.addToBag(this);*/

    /*data.bLong.addToBag(this);
    data.bCaret.addToBag(this);
    data.bHatchetL.addToBag(this);
    data.bHatchetR.addToBag(this);
    data.bDipole.addToBag(this);
    data.bSlash.addToBag(this);
    data.bStump.addToBag(this);
    data.bWedge.addToBag(this);
    data.bTwig.addToBag(this);
    data.bTwig.addToBag(this);
    data.bNub.addToBag(this);*/
}
