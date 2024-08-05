using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class board : Node2D
{
	//rework the board and tick graphics renderers to be less messy and wasteful, reuse code don't repeat it
	
	public tile[,] tiles;
	public Vector2I dimensions;

	public RichTextLabel[,] asciiBackground; //Z layers 0-2 where 1 draws over 0, 2 draws over 1
    public RichTextLabel[,] asciiForeground;
	public RichTextLabel[,] asciiAnimations; // this is updated on tick, not on board update

	public List<animatable> animatables = new List<animatable>();
	public List<animatable> staleAnimatables = new List<animatable>();

	public List<renderable> tickRenderQueue = new List<renderable>(); //used for rendering every tick
    public List<RichTextLabel> tickStaleTiles = new List<RichTextLabel>(); //use to remove stale graphics every tick


    public List<RichTextLabel> boardStaleTiles = new List<RichTextLabel>(); //use to remove stale board tiles every board update
    public List<renderable> boardRenderQueue = new List<renderable>(); //used for rendering every board update

    Node2D nBoard;
	Control asciiControl;
	RichTextLabel pieceShadow;
	RichTextLabel score;
	RichTextLabel currentPreview;
    RichTextLabel nextPreview;
	RichTextLabel heldPiece;
	RichTextLabel levelUI;
	Label levelTimesUI;

    public board(Vector2I dim, Node2D board, Control control, RichTextLabel shadow, RichTextLabel score, RichTextLabel preview, RichTextLabel nextPreview, RichTextLabel heldPiece, RichTextLabel levelUI, Label levelTimesUI)
    {
        dimensions = dim;
        nBoard = board;
		asciiControl = control;
		pieceShadow = shadow;
		this.score = score;
		currentPreview = preview;
		this.nextPreview = nextPreview;
		this.heldPiece = heldPiece;
		this.levelUI = levelUI;
		this.levelTimesUI = levelTimesUI;
		initializeTiles();
    }

    public void initializeTiles()
	{
		tiles = new tile[dimensions.X,dimensions.Y];
		asciiBackground = new RichTextLabel[dimensions.X,dimensions.Y];
        asciiForeground = new RichTextLabel[dimensions.X, dimensions.Y];
        asciiAnimations = new RichTextLabel[dimensions.X, dimensions.Y];

		initializeAsciiNodes();


		//load bullshit
		//level.loadStarterTiles whatever
    }

	public void updateGraphics() //RUNS ON TICK
	{
        foreach (RichTextLabel text in tickStaleTiles) //remove old tiles
        {
            text.Text = " ";
        }
        tickStaleTiles.Clear();


        foreach (renderable render in tickRenderQueue) //render new tiles
        {
            renderTile(render);
        }
        tickRenderQueue.Clear();
    }
    public void markTickStale(Vector2I pos, int z) //mark a position to clear graphics
    {
        tickStaleTiles.Add(getAsciiNode(pos, z));
    }

    public void updateBoard() //RUNS ON BOARD UPDATE
    { //unrender old tiles and render new ones
        foreach (RichTextLabel text in boardStaleTiles) //remove old tiles
        {
			text.Text = " ";
		}
		boardStaleTiles.Clear();


		foreach(renderable render in boardRenderQueue) //render new tiles
		{
			renderTile(render);
		}
		boardRenderQueue.Clear();
	}
    public void markBoardStale(Vector2I pos, int z) //mark a position to clear graphics
    {
        boardStaleTiles.Add(getAsciiNode(pos, z));
    }

    public void animationTick(double delta) //run from main
    {
        foreach (animatable animate in animatables)
        {
            animate.tick(delta);
        }
		foreach (animatable stale in staleAnimatables)
		{
			animatables.Remove(stale);
		}
    }

    public void renderTile(renderable render) //renders a renderable object to the ascii board
	{
		RichTextLabel node = getAsciiNode(render.pos, render.z);
        node.Text = render.text;
		if (render.temporary)
		{
			boardStaleTiles.Add(node);
		}
	}

	

	public RichTextLabel getAsciiNode(Vector2I pos, int z)
	{
        RichTextLabel node;
        switch (z)
        {
            case 0:
                node = asciiBackground[pos.X, pos.Y];
                break;
            case 1:
                node = asciiForeground[pos.X, pos.Y];
                break;
            case 2:
                node = asciiAnimations[pos.X, pos.Y];
                break;
            default:
				return null;
        }
		return node;
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
		
			for (int y = rows[i] + 1; y < dimensions.Y; y++)
			{
				for (int x = 0; x < dimensions.X; x++)
				{
					tile tile = tiles[x, y];
					if (tile != null)
					{
                        //GD.Print($"MOVING PIECE from {tile.boardPos} to [{tile.boardPos.X}, {tile.boardPos.Y - 1}]");


						boardStaleTiles.Add(asciiForeground[x, y]);
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

	public void updateLevelUI(int level, double times)
	{
		levelUI.Text = level.ToString();
		levelTimesUI.Text = times.ToString();
	}

	public void updateHeldUI(boardPiece piece)
	{
		if(piece != null)
		{
            heldPiece.Text = $"HOLD:\n{piece.name}";
        }
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

                asciiBackground[x, y] = node0;
                asciiForeground[x, y] = node1;
                asciiAnimations[x, y] = node2;
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

	public void clearGraphics()
	{
		foreach(RichTextLabel text in asciiBackground)
		{
			text.Clear();
		}
        foreach (RichTextLabel text in asciiForeground)
        {
            text.Clear();
        }
        foreach (RichTextLabel text in asciiAnimations)
        {
            text.Clear();
        }
    }

	public void resetUI()
	{
		score.Text = "SCORE:";
		currentPreview.Text = "NOW PLAYING:";
		nextPreview.Text = "NEXT UP:";
		heldPiece.Text = "HOLD:";
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
