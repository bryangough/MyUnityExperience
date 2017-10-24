using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardModel 
{
	public SquareModel[] currentMap;
	Team turn = Team.none;


	public BoardModel(Team turn, SquareModel[] currentMap)
	{
		this.turn = turn;
		this.currentMap = currentMap;
	}
	public bool doMove(SquareModel square)
	{
		if( turn == Team.none)
			return false;
		if( square.team != Team.none)
			return false;
		square.team = turn;
		return true;
	}
	public BoardModel doMoveClone(SquareModel square)
	{
		if( turn == Team.none || square.team != Team.none)
		{
			return this;
		}
		SquareModel[] copy = new SquareModel[currentMap.Length];
    	System.Array.Copy(currentMap, 0, copy, 0, currentMap.Length);
		for(var x=0;x<copy.Length;x++)
		{
			if(copy[x].x == square.x && copy[x].y==square.y)
			{
				copy[x].team = turn;
			}
		}
		BoardModel model = new BoardModel(this.turn, copy);
		return model;
	}
	public SquareModel[] getMoves()
	{
		List<SquareModel> returnList = new List<SquareModel>();
		for(int x=0; x<currentMap.Length; x++)
		{
			if(currentMap[x].team == Team.none)
			{
				returnList.Add(currentMap[x]);
			}
		}
		return returnList.ToArray();
	}
	public Team currentTeam()
	{
		return turn;
	}
	public Team changeTurn()
	{
		if(turn == Team.blue)
		{
			turn = Team.red;
		}
		else
		{
			turn = Team.blue;
		}
		return turn;
	}
	public bool isWin()
	{
		return false;
	}
}
