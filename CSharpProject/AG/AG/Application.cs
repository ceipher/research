using System;
using System.Collections.Generic;
using System.IO;
using AG;

public static class Application
{
	static public void Main () 
	{
		//SimpleTest ();
		//SleepTargetChoosingTest.Run ();
		//WinRateTest.Run ();
		SleepUseOrNotTest_Coverage.Run ();
	}

	static public void SimpleTest ()
	{		
		List<Character> players = new List<Character>();
		players.Add (new Character(12,10,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
		//players.Add (new Character(12,6,"P2"));
		//players.Add (new Character(12,6,"P3"));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(12,6,"E1"));
		//enemies.Add (new Character(13,6,"E2"));
		//enemies.Add (new Character(13,6,"E3"));


		GameNode startState = new GameNode(players, enemies, 0);

		//List<GameNode> graph = RRT.Build (startState);
		//List<GameState> graph = Greedy.Build (startState);
		List<GameNode> graph = BFSearch.Build (startState);
		//List<GameNode> graph = MonteCarlo.Build (startState);
		
		
		//Choose best path
		GameNode best = Utils.GetBest (graph, BEST.ATTRITION);
		List<GameNode> bestSolutions = new List<GameNode> ();
		int sleepCount = 0;
		foreach (GameNode node in graph) {
			if (Utils.IsWinAsGoodAs(best, node, BEST.ATTRITION)) {
				bestSolutions.Add(node);
				node.markOptimal();
				Utils.PrintNodePath(node, 0, new List<GameNode>());
				bool used = false;
				GameNode current = node;
				while(current != null) {
					foreach (Character e in current.enemies)
					{
						if (e.debuff == DEBUFF.SLEEPING) {
							used = true;
							break;
						}
					}
					current = current.parent;
				}		
				if (used){
					sleepCount++;
				}
			}
		}
		Console.WriteLine ("Sleep "+sleepCount+"/"+bestSolutions.Count);
		if (best.getNodeState () != GAME_STATE.PLAYER_WIN) 
			Console.WriteLine ("FAIL TO FIND SOLUTION");
		else 
			Console.WriteLine ("SOLUTION FOUND");
		Utils.PrintToHTML(graph, graph[0]);
	}
	
}

