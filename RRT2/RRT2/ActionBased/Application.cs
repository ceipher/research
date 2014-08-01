using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RRT2;

public static class Application
{
	static public void Main2 (){
		List<Character> players = new List<Character>();
		players.Add (new Character(3,1));
		players.Add (new Character(3,1));
		players.Add (new Character(3,1));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(2,1));
		enemies.Add (new Character(3,1));
		enemies.Add (new Character(3,1));
		GameState startState = new GameState(players, enemies, 1);
		List<GameState> children = new List<GameState>();
		Utils.DoPlayerAction(startState,0,children);
		foreach(GameState s in children)
		{
			Console.WriteLine(s.ToString());	
		}
		Console.WriteLine(children.Count);	
		
	}
	static public void Main ()
	{		
		List<Character> players = new List<Character>();
		players.Add (new Character(10,2));
		players.Add (new Character(8,5));
		players.Add (new Character(7,1));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(10,2));
		enemies.Add (new Character(10,2));
		enemies.Add (new Character(10,2));
		
		GameState startState = new GameState(players, enemies, 1);
		

		List<GameState> graph = RRT.Build (startState);
		foreach (GameState s in graph)
		{
			Console.WriteLine(s.ToString());
		}
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
					if (nn.getRounds() < best.getRounds()) {// 2nd - # of rounds
						best = nn;
					} else if (nn.getRounds() == best.getRounds()) {
						if (Utils.getHealthSum(nn.players) > Utils.getHealthSum(best.players)) {// 3rd - players' health
							best = nn;
						}
					}
				}
				
				foreach(GameState nnChild in nn.GetAllChildren())
				{			
					double childval = Utils.getHealthSum(nnChild.enemies);
					if (minValue > childval)
					{
						minValue = childval;
						best = nnChild;
					} else if (minValue == childval) {
						if (nnChild.getRounds() < best.getRounds()) {// 2nd - # of rounds
							best = nnChild;
						} else if (nnChild.getRounds() == best.getRounds()) {
							if (Utils.getHealthSum(nnChild.players) > Utils.getHealthSum(best.players)) {// 3rd - players' health
								best = nnChild;
							}
						}
					}
				}
				
			}
			Utils.addNode(graph, best);
			best.markOptimal();
			Console.WriteLine("RRT graph size: " + graph.Count);
			Utils.printNodePath(best, 0, new List<GameState>());
			Console.WriteLine("Player team total Health: " + Utils.getHealthSum(best.players));
			Console.WriteLine("BEST: " + best.ToString());
			Utils.outputGraph(graph, graph[0]);
		
	}
			
}
