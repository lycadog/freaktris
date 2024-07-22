using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class main : Node2D
{ //add an events system eventually!!!!

	public static gameState state;

    //run constants
    public bag bag;

	//board variables
	public int level = 1;
	public int rowsCleared = 0;
	public int enemyCooldown;

	public long turnScore = 0;
	public long totalScore = 0;
	public long scoreRequired = 1000; //this is enemy health, rework later to initialize along with custom enemy class health

	public double piecefallTimer = 0;
	public double inputCooldownTimer = 1;
	public boardPiece currentPiece;
	public boardPiece heldPiece;
	public boardPiece nextPiece; 
	//split off these variables or "stats" as they could be called into their own class maybe?
	//stuff like the board dimensions and bag and such, so they could be freely altered by items and other things easily

	public List<int> updatedRows = new List<int>();
	public List<int> scorableRows = new List<int>();

	//misc
    public board board;
    Node2D nBoard;
    Polygon2D nDefaultTile;

    public void runInit() //runs on run start, initializes important variables
    {
		state = new gameState();
		state = gameState.roundStart;
		bag = new bag();
    }

    public void coreGameLoop(double deltaTime)
    {
			switch (state)
		{
            // ========== ROUND START ==========
            case gameState.roundStart: //run once when entering a battle (round)!

                boardStart();
				state = gameState.turnStart;
                break;

            // ========== TURN START ==========
            case gameState.turnStart: //run once at the start of a turn

                currentPiece = nextPiece;
				nextPiece = bag.getPiece();
				GD.Print(currentPiece.name);
				state = gameState.pieceWait;
                break;

            // ========== PIECE WAIT ==========
            case gameState.pieceWait: //run until input

                if (Input.IsActionJustPressed("pieceDrop")) //change this to a configurable 5 second timer that can be ended early with an input eventually
                {   //initiate piecefall
                    currentPiece.playPiece(board);
                    state = gameState.midTurn;
                }
				break;

            // ========== MID TURN ==========
            case gameState.midTurn: //run continuously during a turn, while a piece is falling (ie piecefall!)

                //add input code to move falling piece

                piecefallTimer += deltaTime; //increment timer
				inputCooldownTimer += deltaTime;

                //rework the input later to be a bit more natural (like with pressing vs holding move keys
                if (inputCooldownTimer >= 0.2)
                {
					parseInput();
				}

                if (piecefallTimer >= 1.0)
					{
                        if (currentPiece.fallPiece(board))//check for collision
                        {
                            GD.Print("piece placed!");
                            state = gameState.endTurn;
                        }
                        
						GD.Print(currentPiece.pos.X + ", " + currentPiece.pos.Y);
						piecefallTimer = 0;
                    }

                break;

				// ========== END TURN ==========
			case gameState.endTurn:

				currentPiece.placePiece(board);
				board.updateGraphics();
                GD.Print("piece placed at " + currentPiece.pos);

				
                //need to add a bunch of extra lines here to call all of the tile event methods (tile.collided, tile.score, etc)

                foreach (tile tile in currentPiece.tiles) //when a piece is placed, add each row it intersects to updatedRows
				{
					if(tile != null) { updatedRows.Add(tile.boardPos.Y); }
				}
				updatedRows = updatedRows.Distinct().ToList(); //remove duplicate rows

				foreach(int row in updatedRows)	{ //checks each row that was just updated, if any are scorable they are scored and stuff
					if (isRowScoreable(row)){ //process through each scored row and set them to be scored
						scorableRows.Add(row);
					}
				}

				scorableRows.Sort();
				for(int x = 0; x < board.dimensions.X; x++) //process top to bottom, left to right through every scorable line
				{
					foreach(int y in scorableRows)
					{
						turnScore = board.tiles[x, y].score(board, turnScore);
					}
				}
				totalScore += turnScore; //REWORK SCORE calculations later ****
				updatedRows.Clear();
				scorableRows.Clear();

				foreach(tile tile in board.tiles) { //tick every tile
					if(tile != null)
					{tile.tick(board);}}

				if(totalScore >= scoreRequired) //if the player has enough score to beat the encounter, end the encounter
				{
					state = gameState.endRound; break;
				}

				//run enemy behavior like lowering enemy cooldown and playing enemy lines
				
                //CHECK FOR SCORABLE LINES AND SUCH
                //place the piece, check and clear scorable lines
                //add score, calculate level, count down enemy cooldown, play enemy lines,
                state = gameState.turnStart;
                break;

			default:
				break;
		}

    }

	//a lot of the functions below need to properly call the tileType function to do things rather than doing them on their own
	//otherwise we will not get our intended custom tileType behavior

    public void boardStart()
	{
        nBoard = GetNode<Node2D>("board");
        nDefaultTile = GetNode<Polygon2D>("board/tiles/dTile");
        board = new board(nBoard, nDefaultTile, new Vector2I(10,22));
        heldPiece = null;
		nextPiece = bag.getPiece();
	}

	public void parseInput() //runs during piecefall, checks input to move piece, determines move validity and executes move
	{
		bool isMoveValid = false;
		if (Input.IsActionPressed("boardLeft"))
		{
			if (currentPiece.isMoveValid(board, -1))
			{
				currentPiece.moveFallingPiece(-1, 0);
				isMoveValid = true;
			} 

		}
		else if (Input.IsActionPressed("boardRight"))
		{
            if (currentPiece.isMoveValid(board, 1))
            {
                currentPiece.moveFallingPiece(1, 0);
                isMoveValid = true;
            }
        }
		else if (Input.IsActionJustPressed("boardSlam"))
		{
			bool isSlamming = true;
			while (isSlamming)
			{
				if (currentPiece.fallPiece(board))
				{
					isSlamming = false;
					state = gameState.endTurn;
					break;
				}
            }
		}

		if (isMoveValid)
		{
            piecefallTimer = 0;
            inputCooldownTimer = 0;
        }
    }
	
	public bool isRowScoreable(int y)
	{
		for(int x = 0; x < board.dimensions.X; x++)
		{
			if (board.tiles[x, y] == null) return false; //if any tile is empty, return false
			else { continue; } //if the tile isn't empty, keep looping
		}
		return true; //if no tiles return empty, this will run and return true
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		runInit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		coreGameLoop(delta);
	}

    public enum gameState
    {
        roundStart, //run once when round starts
		turnStart, //once at the start of the turn
		pieceWait, //checking for input before dropping piece
		midTurn, //handling piece falling and collision
		endTurn, //placing piece and checking lines for score
		endRound, //finish up round and transition to the world map
		world,
		room
    }
}
