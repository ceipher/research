using System;
using System.Collections.Generic;
using AG;

public static class Application
{
	static public void Main () 
	{
		HeatMap ();
	}

	static private void HeatMap ()
	{
		int minHealth = 8;
		int healthStep = 2;
		int minAttack = 1;
		int attackStep = 1;

		List<float[]> data = new List<float[]> ();
		
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 10; j++)	{
				int testHealth = minHealth+healthStep*i;
				int testAttack = minAttack+attackStep*j;
				Console.WriteLine("i:"+i+" j:"+j);
				List<Character> players = new List<Character>();
				players.Add (new Character(30,1,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
				players.Add (new Character(30,3,"P2"));
				players.Add (new Character(30,3,"P3"));
				List<Character> enemies = new List<Character>();
				enemies.Add (new Character(12,5,"E1"));
				enemies.Add (new Character(testHealth, testAttack, "E2"));

				int K = 10;
				int numOfSleeps = 0;

				for (int k = 0; k < K; k++) {
					GameState start = new GameState(players, enemies, 0);

					List<GameState> graph = RRT.Build (start);
					
					GameState best = null;
					double maxPlayerTeamHealth = double.MinValue;
					foreach (GameState s in graph)	{
						if (s.getGameState() == GAME_STATE.PLAYER_WIN){
							double playerTeamHealth = Utils.getHealthSum(s.players);
							if (playerTeamHealth > maxPlayerTeamHealth) {
								maxPlayerTeamHealth = playerTeamHealth;
								best = s;
							}
						}
					}
					if (best == null) {
						Console.WriteLine("ALGORITHM FAILS");
						k--;
					} else {
						GameState current = best;
						//Utils.printNodePath(best,0,new List<GameState>());
						while(current != null) {
							if (current.enemies[1].debuff == DEBUFF.SLEEPING) {
								numOfSleeps++;
								break;
							}
							current = current.parent;
						}							
					}
				}

				data.Add (new float[] {minHealth+healthStep*i, minAttack+attackStep*j, (float) numOfSleeps/K});
			}
		}
		foreach (float[] dataline in data) {
			Console.WriteLine(string.Join(" ", dataline));
		}
	}

	static public void Main2 ()
	{		
		List<Character> players = new List<Character>();
		players.Add (new Character(10,1,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
		players.Add (new Character(10,3,"P2"));
		players.Add (new Character(10,1,"P3"));
		List<Character> enemies = new List<Character>();
		enemies.Add (new Character(4,3,"E1"));
		enemies.Add (new Character(4,3,"E2"));
		enemies.Add (new Character(4,5,"E3"));
		
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
		Console.WriteLine("RRT graph size: " + graph.Count);
		Utils.printNodePath(best, 0, new List<GameState>());
		Console.WriteLine("Player team total Health: " + Utils.getHealthSum(best.players));
		Console.WriteLine("BEST: " + best.ToString());
		if (best.getGameState () == GAME_STATE.INPROCESS) 
			Console.WriteLine ("ALGORITHM FAIL");
		else 
			Console.WriteLine ("ALGORITHM SUCCESS");
		Utils.outputGraph(graph, graph[0]);
	}
	
}

