using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveGameData
{
    public List<float> playerOnePos = new List<float>();
    public List<float> playerOneRot = new List<float>();
    public List<float> playerTwoPos = new List<float>();
    public List<float> playerTwoRot = new List<float>();

    public float p1X { set; get; }
    public float p1Y { set; get; }
    public float p2X { set; get; }
    public float p2Y { set; get; }

    public SaveGameData(List<float> playerOnePos, List<float> playerOneRot, List<float> playerTwoPos, List<float> playerTwoRot)
    {
        this.playerOnePos = playerOnePos;
        this.playerTwoPos = playerTwoPos;

        this.p1X = playerOnePos[0];
        this.p1Y = playerOnePos[1];
        this.p2X = playerTwoPos[0];
        this.p2Y = playerTwoPos[1];
    }
}
