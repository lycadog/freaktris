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

	public RichTextLabel[,] asciiZ0; //Z layers 0-2 where 1 draws over 0, 2 draws over 1
    public RichTextLabel[,] asciiZ1;
	public RichTextLabel[,] asciiZ2;

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
		asciiZ0 = new RichTextLabel[dimensions.X,dimensions.Y];
        asciiZ1 = new RichTextLabel[dimensions.X, dimensions.Y];
        asciiZ2 = new RichTextLabel[dimensions.X, dimensions.Y];
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
			renderTile(render);
			
		}
		renderQueue.Clear();
	}

	public void renderTile(renderable render) //text is what should render, usually just "O"
	{
		RichTextLabel node;
        switch (render.z)
        {
            case 0:
				node = asciiZ0[render.pos.X, render.pos.Y];
                break;
			case 1:
                node = asciiZ1[render.pos.X, render.pos.Y];
                break;
			case 2:
                node = asciiZ2[render.pos.X, render.pos.Y];
                break;
			default:
				goto End;
		}
        node.Text = render.text;
		if (render.temporary)
		{
			staleTiles.Add(node);
		}
    End: {}
	}

	public void lowerRows(List<int> scoredRows) //lowers rows above the scored rows after scoring
	{
		int length = scoredRows.Count; 
		List<tile> movedTiles = new List<tile>();
		int[] rows = new int[length];

		for(int l = 0; l < length; l++) //sort scoredRows by descending
		{
			rows[l] = scoredRows.Max();
			scoredRows.Remove(scoredRows.Max());
		}

		GD.Print(length);
        for (int i = 0; i < length; i++)
		{
		
			GD.Print("FAT BITCHES!!!!!!!!!!11");
			for (int y = rows[i] + 1; y < dimensions.Y; y++)
			{
				for (int x = 0; x < dimensions.X; x++)
				{
					tile tile = tiles[x, y];
					if (tile != null)
					{
                        GD.Print($"MOVING PIECE from {tile.boardPos} to [{tile.boardPos.X}, {tile.boardPos.Y - 1}]");


						staleTiles.Add(asciiZ1[x, y]);
                        tiles[x, y - 1] = tile;
                        tile.boardPos = new Vector2I(tile.boardPos.X, tile.boardPos.Y - 1);
                        tiles[x, y] = null;
						
							
						movedTiles.Add(tile);
					}
				}
			}
		}
		foreach(tile tile in movedTiles)
		{
			tile.render(this);
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
                RichTextLabel node0 = createAsciiNode(x, y, 0);
                RichTextLabel node1 = createAsciiNode(x, y, 1);
                RichTextLabel node2 = createAsciiNode(x, y, 2);

                asciiZ0[x, y] = node0;
                asciiZ1[x, y] = node1;
                asciiZ2[x, y] = node2;
            }
		}
	}
	
	public RichTextLabel createAsciiNode(int x, int y, int z)
	{
        RichTextLabel node = new RichTextLabel();
        AddChild(node);
        node.Reparent(asciiControl);
        node.Position = new Vector2((x + 1) * 18, 616 - (y * 28));
        node.Size = new Vector2(18, 28);
        node.Text = " ";
        node.BbcodeEnabled = true;
		node.ZIndex = z;
		return node;
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
