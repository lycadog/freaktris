using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class board : Node2D
{
	//need a complete graphics rework eventually
	//turn this class into graphics handler maybe
	//add better code to remove old tiles
	public tile[,] tiles;
	public Vector2I dimensions;

	public RichTextLabel[,] asciiTiles;
	public List<RichTextLabel> staleTiles; //use to remove stale tiles 
	public List<renderable> renderQueue;

    Node2D nBoard;
	Control asciiControl;
	RichTextLabel pieceShadow;
	RichTextLabel score;
	RichTextLabel currentPreview;
    RichTextLabel nextPreview;

    public board(Vector2I dim, Node2D board, Control control, RichTextLabel shadow, RichTextLabel score, RichTextLabel preview, RichTextLabel nextPreview)
    {
        dimensions = dim;
        nBoard = board;
		asciiControl = control;
		pieceShadow = shadow;
		this.score = score;
		currentPreview = preview;
		this.nextPreview = nextPreview;
		initializeTiles();
    }

    public void initializeTiles()
	{
		tiles = new tile[dimensions.X,dimensions.Y];
		asciiTiles = new RichTextLabel[dimensions.X,dimensions.Y];
		staleTiles = new List<RichTextLabel>();
		renderQueue = new List<renderable>();
		initializeAsciiNodes();


		//load bullshit
		//level.loadStarterTiles whatever
    }

	public void updateAscii() //unrender old tiles and render new ones
	{
		foreach(RichTextLabel text in staleTiles) //remove old tiles
		{
			text.Text = " ";
		}
		staleTiles.Clear();

		foreach(renderable render in renderQueue) //render new tiles
		{
			renderTile(render.pos, render.text);
			if (render.temporary)
			{
				staleTiles.Add(asciiTiles[render.pos.X,render.pos.Y]); //if they are not placed on the board, remove them next render
			}
		}
		renderQueue.Clear();
	}

	public void renderTile(Vector2I pos, string text) //text is what should render, usually just "O"
	{
		//GD.Print("RENDERABLE POS: " + pos);
		RichTextLabel node = asciiTiles[pos.X,pos.Y];
		node.Text = text;
	}

	public void lowerRows(List<int> scoredRows) //lowers rows above the scored rows after scoring
	{
		int length = scoredRows.Count; //BUGGED !!!!! SEPERATE rows scoring at the same time breaks! consider lowering down one tile at a time!
		if(length != 0)
		{
            for (int y = scoredRows.Max() + 1; y < dimensions.Y; y++)
            {
                for (int x = 0; x < dimensions.X; x++)
                {
					tile tile = tiles[x, y];
					if(tile != null)
					{
						
                        staleTiles.Add(asciiTiles[x, y]);
						tiles[x, y] = null;

                        tiles[x, y - length] = tile;
						tile.boardPos = new Vector2I(tile.boardPos.X, tile.boardPos.Y - length);
                        tile.render(this);
                    }   
                }
            }
        }
		
	}

	public void updatePiecePreview(boardPiece currentPiece, boardPiece nextPiece)
	{
		currentPreview.Text = $"NOW PLAYING:\n{currentPiece.name}";
		nextPreview.Text = $"NEXT UP:\n{nextPiece.name}";
		
	}
	public void updateScore(long value)
	{
		score.Text = $"SCORE:\n{value}";
	}

	public void initializeAsciiNodes() //create and set all ascii nodes properly
	{
		
		for(int x = 0; x < dimensions.X; x++)
		{
			for(int y = 0; y < dimensions.Y; y++)
			{
				RichTextLabel node = new RichTextLabel();
				AddChild(node);
				node.Reparent(asciiControl);
				node.Position = new Vector2((x + 1) * 18, 616 - (y * 28));
				node.Size = new Vector2(18, 28);
				node.Text = " ";
				node.BbcodeEnabled = true;
				asciiTiles[x, y] = node;
			}
		}
	}
	
	public bool isPositionValid(Vector2I pos, bool shouldCollide) //checks if a tile is occupied or otherwise outside of the board
	{ //shouldCollide refers to colliding with other tiles

        //if the tile is outside the board dimensions return false (invalid)
        if (pos.X < 0) { return false; }
        if (pos.X >= dimensions.X) { return false; }
        if (pos.Y < 0) { return false; }
        if (pos.Y >= dimensions.Y) { return false; }
		GD.Print($"{pos.X}, {pos.Y} IS VALID: {tiles[pos.X,pos.Y] == null} =============");
		

        //if (tiles[pos.X, pos.Y] != null) { return false; }
		return tiles[pos.X, pos.Y] == null;
	}

}
