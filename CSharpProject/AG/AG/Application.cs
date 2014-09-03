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
		players.Add (new Character(18,5,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
		players.Add (new Character(18,3,"P2"));
		players.Add (new Character(18,4,"P3"));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(10,4,"E1"));
		enemies.Add (new Character(10,3,"E2"));
		enemies.Add (new Character(13,1,"E3"));
		
		GameState startState = new GameState(players, enemies, 0);
		
		List<GameState> graph = RRT.Build (startState);
		//List<GameState> graph = Greedy.Build (startState);
		//List<GameState> graph = BFSearch.Build (startState);
		//List<GameState> graph = MonteCarlo.Build (startState);
		
		
		//Choose best path
		double minValue = double.MaxValue;
		GameState best = graph[0];
		foreach(GameState nn in graph)
		{			
			double val = Utils.getHealthSum(nn.enemies);
			if (minValue > val) {// 1st - enemies' health
				minValue = val;
				best = nn;
			} else if (minValue == val)	{
				if (Utils.getHealthSum(nn.players) > Utils.getHealthSum(best.players)) {// 2nd - players' health
					best = nn;
				} else  if (Utils.getHealthSum(nn.players) == Utils.getHealthSum(best.players)) {
					if (nn.getRounds() < best.getRounds()) {// 3rd - # of rounds
						best = nn;
					} 
				}
			}
		}
		best.markOptimal();
		Utils.printNodePath(best, 0, new List<GameState>());
		if (best.getGameState () == GAME_STATE.INPROCESS) 
			Console.WriteLine ("FAIL TO FIND SOLUTION");
		else 
			Console.WriteLine ("SOLUTION FOUND");
		Utils.printToHTML(graph, graph[0]);
	}
	
}

