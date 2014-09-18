using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class BFSearch
	{
		public static List<GameNode> Build(GameNode rootState)
		{
			List<GameNode> graph = new List<GameNode>();
			List<GameNode> currentLevelStates = new List<GameNode>();
			currentLevelStates.Add(rootState);
			int rounds = 0;
			while(currentLevelStates.Count != 0)
			{
				
				List<GameNode> tempList = new List<GameNode>();
				foreach(GameNode state in currentLevelStates)
				{
					foreach(GameNode child in state.GetAllChildren())
					{
						tempList.Add(child);
						graph.Add(child);
					}
				}
				currentLevelStates = tempList;
				rounds++;
				//Console.WriteLine("ROUNDS: "+rounds);
				//Console.WriteLine("Level Size: "+currentLevelStates.Count);
			}
			return graph;
		}		
	}
}

