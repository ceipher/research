using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class BFSearch
	{
		public static List<GameState> Build(GameState rootState)
		{
			List<GameState> graph = new List<GameState>();
			List<GameState> currentLevelStates = new List<GameState>();
			currentLevelStates.Add(rootState);
			int rounds = 0;
			while(currentLevelStates.Count != 0)
			{
				
				List<GameState> tempList = new List<GameState>();
				foreach(GameState state in currentLevelStates)
				{
					foreach(GameState child in state.GetAllChildren())
					{
						if (child.getGameState() == GAME_STATE.INPROCESS)
						{
							tempList.Add(child);
							graph.Add(child);
						}
					}
				}
				currentLevelStates = tempList;
				rounds++;
				Console.WriteLine("ROUNDS: "+rounds);
				Console.WriteLine("Level Size: "+currentLevelStates.Count);
			}
			return graph;
		}
		
	}
}

