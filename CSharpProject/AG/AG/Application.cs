using System;
using System.Collections.Generic;
using System.IO;
using AG;

public static class Application
{
	static public void Main () 
	{
		//SimpleTest ();
		//SleepUseTest.GenerateHeapMap ();
		WinRateTest.run ();
	}

	static public void SimpleTest ()
	{		
		List<Character> players = new List<Character>();
		players.Add (new Character(12,8,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
		players.Add (new Character(12,8,"P2"));
		players.Add (new Character(12,8,"P3"));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(18,12,"E1"));
		enemies.Add (new Character(12,8,"E2"));
		enemies.Add (new Character(12,8,"E3"));


		GameNode startState = new GameNode(players, enemies, 0);

		//List<GameNode> graph = RRT.Build (startState);
		//List<GameState> graph = Greedy.Build (startState);
		List<GameNode> graph = BFSearch.Build (startState);
		//List<GameNode> graph = MonteCarlo.Build (startState);
		
		
		//Choose best path
		GameNode best = Utils.GetBest (graph, BEST.ATTRITION);
		List<GameNode> bestSolutions = new List<GameNode> ();
		foreach (GameNode node in graph) {
			if (Utils.IsWinAsGoodAs(best, node, BEST.ATTRITION)) {
				bestSolutions.Add(node);
				node.markOptimal();
				Utils.PrintNodePath(node, 0, new List<GameNode>());
			}
		}

		if (best.getNodeState () == GAME_STATE.INPROCESS) 
			Console.WriteLine ("FAIL TO FIND SOLUTION");
		else 
			Console.WriteLine ("SOLUTION FOUND");
		Utils.PrintToHTML(graph, graph[0]);
	}
	
}

