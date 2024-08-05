using Godot;
using System.Collections.Generic;

public class animatable
{
    public List<string> sequence = new List<string>(); //the list of characters to display in order
    public int frame; //indicates how far in the sequence the animation is
    public bool loops;
    public Vector2I pos;

    public double time;
    public double hangTime; //time needed before progressing to the next frame

    public board board;

    public animatable(List<string> sequence, bool loops, Vector2I pos, double hangTime, board board)
    {
        this.sequence = sequence;
        this.loops = loops;
        this.pos = pos;
        this.hangTime = hangTime;
        this.board = board;

        frame = 0;
        time = 0;
        advanceSequence();
    }

    public void tick(double deltaTime)
    {
        
        time += deltaTime;
        if(time > hangTime) //if enough time has passed, advance the sequence
        {
            advanceSequence();
            time = 0;
        }
    }

    public void advanceSequence()
    {
        if (frame >= sequence.Count) //if the sequence is over: loop if the animation loops, otherwise end the animation
        {
            if (loops) { frame = 0; } else { endSequence(); return; }
        }
        
        renderable render = new renderable(pos, sequence[frame], 2, false); //create a render object to add to the render queue
        board.tickRenderQueue.Add(render);

        frame++; //increment frame
    }

    public void endSequence()
    {
        board.markTickStale(pos, 2);
        board.staleAnimatables.Add(this);
    }
}
