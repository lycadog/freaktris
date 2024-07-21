using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class board : Node2D
{
	public tile[,] tiles;
	public List<tile> staleTiles = new List<tile>(); //delete each graphics update
	public Vector2I dimensions;
    Node2D nBoard;
	Node2D nTiles; // returns world/board/tiles path
    Polygon2D defaultTile;

    public board(Node2D board, Polygon2D templateTile, Vector2I dim)
    {
        nBoard = board;
        defaultTile = templateTile;
        dimensions = dim;
		nTiles = board.GetChild<Node2D>(0);
		initializeTiles();
    }


	public void freeTiles() //we need a reworked system to clear up old tiles
	{

	}

    public void initializeTiles()
	{
		tiles = new tile[dimensions.X,dimensions.Y];
		
		//load bullshit
		//level.loadStarterTiles whatever
    }

	public void updateGraphics()
	{
		foreach(tile staleTile in staleTiles) //remove tiles queued for deletion
		{
			staleTile.gfx.QueueFree();
			staleTile.specialGfx.QueueFree();
		}
		foreach(tile tile in tiles) //add sprites of new tiles
		{
			if (tile != null){
				if (tile.gfx == null){ //check every tile to see if it has a polygon assigned
					tile.initializeGfx(nTiles, defaultTile);
					//ADD SPECIAL GFX HERE LATER for unique tile type graphics
					
				}}
		}
	}
	public void renderPiecefall(bagPiece piece) //renders a currently falling piece
	{
		//figure out something for this later
	}
	
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
