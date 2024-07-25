using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

public partial class board : Node2D
{
	//need a complete graphics rework eventually
	//turn this class into graphics handler maybe
	//add better code to remove old tiles
	public tile[,] tiles;
//delete each graphics update
	public Vector2I dimensions;

	public RichTextLabel[,] asciiTiles;
	public List<RichTextLabel> staleTiles; //use to remove stale tiles 
	public List<tile> renderQueue;

    Node2D nBoard;
	Control asciiControl;
	Node2D nTiles; // returns world/board/tiles path
    Polygon2D defaultTile;

    public board(Node2D board, Control control, Vector2I dim)
    {
        nBoard = board;
		asciiControl = control;
        dimensions = dim;
		nTiles = board.GetChild<Node2D>(0);
		initializeTiles();
    }

    public void initializeTiles()
	{
		tiles = new tile[dimensions.X,dimensions.Y];
		asciiTiles = new RichTextLabel[dimensions.X,dimensions.Y];
		staleTiles = new List<RichTextLabel>();
		renderQueue = new List<tile>();
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

		foreach(tile tile in renderQueue) //render new tiles
		{
			renderTile(tile.boardPos, "O");
			if (!tile.isPlaced)
			{
				staleTiles.Add(asciiTiles[tile.boardPos.X,tile.boardPos.Y]); //if they are not placed on the board, remove them next render
			}
		}
		renderQueue.Clear();
	}

	public void renderTile(Vector2I pos, string text) //text is what should render, usually just "O"
	{
		RichTextLabel node = asciiTiles[pos.X,pos.Y];
		node.Text = text;
		GD.Print($"updated graphics at {pos.X}, {pos.Y}");
	}

	public void lowerRows(List<int> scoredRows) //lowers rows above the scored rows after scoring
	{
		int length = scoredRows.Count;
		if(length != 0)
		{
            for (int y = scoredRows.Max() + 1; y < dimensions.Y; y++)
            {
                for (int x = 0; x < dimensions.X; x++)
                {
					tile tile = tiles[x, y];
					if(tile != null)
					{
                        renderQueue.Add(tile);
                        staleTiles.Add(asciiTiles[x, y]);
						tiles[x, y] = null;

                        tiles[x, y - length] = tile;
						tile.boardPos = new Vector2I(tile.boardPos.X, tile.boardPos.Y - length);
                        
                    }   
                }
            }
        }
		
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
				asciiTiles[x, y] = node;
			}
		}
	}

	public void pieceMoveEvent(boardPiece piece)
	{
		foreach(tile tile in piece.tiles)
		{
			if(tile != null)
			{
                renderQueue.Add(tile);
            }
		}
	}

	
	//DEPRECATED METHOD
	/*public void updateGraphics() //this method is inefficient, we need a complete graphics rework eventually
	{
		foreach(tile staleTile in staleTiles) //remove tiles queued for deletion
		{
			staleTile.gfx.Free();
			//staleTile.specialGfx.Free();
		}
		staleTiles.Clear();
		foreach(tile tile in tiles) //add sprites of new tiles
		{
			if (tile != null){
				if (tile.gfx == null){ //check every tile to see if it has a polygon assigned
					tile.initializeGfx(nTiles, defaultTile);
					//ADD SPECIAL GFX HERE LATER for unique tile type graphics
					
				}}
		}
	}*/

	//DEPRECATED METHOD
	public Polygon2D addTile(int x, int y) //create a new polygon and set up its properties
	{
		Polygon2D newTile = new Polygon2D();
		AddChild(newTile);
		newTile.Reparent(nTiles);
		newTile.Position = defaultTile.Position; //should probably make this less jank
		newTile.Polygon = defaultTile.Polygon;
		newTile.Name = "tile"+x+","+y;
		newTile.MoveLocalX(x*28);
		newTile.MoveLocalY(y * -28);
		return newTile;
	}

	
}
