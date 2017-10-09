using UnityEngine;
using System.Collections;

public class MoveModel : Move
{
    public PieceModel piece;
    public int x;
    public int y;
    public Capture capture = null;
    public bool isCapture()
    {
        if(capture!=null)
            return true;
        return false;
    }

    public override string ToString()
    {
        return piece+ " to ["+x+" "+y+"]";
    }
}