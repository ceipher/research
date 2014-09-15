using System;
using System.Collections.Generic;
using System.IO;
using AG;

public static class Application
{
	static public void Main () 
	{
		//Main2 ();
		SleepUseTest.GenerateHeapMap ();
	}

	static public void Main2 ()
	{		
		List<Character> players = new List<Character>();
		players.Add (new Character(12,8,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
		players.Add (new Character(12,8,"P2"));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(24,12,"E1"));
		enemies.Add (new Character(12,8,"E2"));


		GameNode startState = new GameNode(players, enemies, 1);

		List<GameNode> graph = RRT.Build (startState);
		//List<GameState> graph = Greedy.Build (startState);
		//List<GameState> graph = BFSearch.Build (startState);
		//List<GameNode> graph = MonteCarlo.Build (startState);
		
		
		//Choose best path
		GameNode best = Utils.GetBest (graph);
		best.markOptimal();
		Utils.printNodePath(best, 0, new List<GameNode>());
		if (best.getNodeState () == GAME_STATE.INPROCESS) 
			Console.WriteLine ("FAIL TO FIND SOLUTION");
		else 
			Console.WriteLine ("SOLUTION FOUND");
		Utils.printToHTML(graph, graph[0]);
	}
	
}

