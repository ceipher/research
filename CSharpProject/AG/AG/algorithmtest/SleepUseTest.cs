
using System;
using System.Collections.Generic;
using System.IO;
namespace AG
{
	public class SleepUseTest
	{
		static List<float[]> enemyXdata = new List<float[]> ();
		static List<float[]> enemyTeamdata = new List<float[]> ();
		static int H = 20;
		static int A = 8;
		static int minHealth = H/4;
		static int maxHealth = H*3;
		static int healthStepSize = H/4;
		static int minAttack = A/4;
		static int maxAttack = A*3;
		static int attackStepSize = A/4;

		static int HMP = 1;
		static int AMP = 1;
		static int HME = 1;
		static int AME = 1;

		public static void GenerateHeapMap()
		{
			for (int i = 0; minHealth+i*healthStepSize<=maxHealth; i++) {
				for (int j = 0; minAttack+j*attackStepSize<=maxAttack; j++)	{
					int testHealth = minHealth+healthStepSize*i;
					int testAttack = minAttack+attackStepSize*j;
					Console.WriteLine("i:"+i+" j:"+j);
					List<Character> players = new List<Character>();
					players.Add (new Character(H*HMP,A*AMP,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
					players.Add (new Character(H*HMP,A*AMP,"P2"));
					players.Add (new Character(H*HMP,A*AMP,"P3"));
					List<Character> enemies = new List<Character>();
					enemies.Add (new Character(testHealth,testAttack,"E1"));
					enemies.Add (new Character(H*HME,A*AME,"E2"));
					enemies.Add (new Character(H*HME,A*AME,"E3"));
					FillBFSearchData(i, j, players, enemies);
				}
			}
			
			
			//Write To Files
			string fileContent = "";
			fileContent += (""
			               	+ "set multiplot"
			                + "set size 0.5,1"
			                + "set origin 0.0,0.0"
							+ "\set title \"BFS - 3x3\""
			                + "\r\n" + "unset key" 
			                + "\r\n" + "set tic scale 0"
			                + "\r\n" + "set palette rgbformula -7,2,-7"
			                + "\r\n" + "set cbrange [-0.1:1]"
			                + "\r\n" + "set cblabel \"Probability of Using Sleep\""
			                + "\r\n" + "unset cbtics"
			                + "\r\n" + "set xlabel \"health\""
			                + "\r\n" + "set ylabel \"attack\""
			                + "\r\n" + "set xrange [" + (float)(minHealth-healthStepSize/2) + ":" + (float)(maxHealth+healthStepSize/2) + "]"
			                + "\r\n" + "set yrange [" + (float)(minAttack-attackStepSize/2) + ":" + (float)(maxAttack+attackStepSize/2) + "]"
			                + "\r\n" + "set xtics " + healthStepSize
			                + "\r\n" + "set ytics " + attackStepSize
			                + "\r\n" + "plot \'-\' using 1:2:3 with image");
			foreach (float[] dataline in enemyXdata) {
				fileContent += ("\r\n" + string.Join(" ", dataline));
			}
			fileContent += "\r\ne";
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\HeatMaps\\"+"H"+HMP+"A"+AMP+"_3x3.plt", FileMode.Create);
			StreamWriter file = new StreamWriter(fcreate);
			file.WriteLine(fileContent);
			Console.WriteLine (fileContent);
			file.Close();	
		}

		private static bool IsSleepUsedForEnemyX(GameNode node) 
		{
			GameNode current = node;
			while(current != null) {
				if (current.enemies[0].debuff == DEBUFF.SLEEPING) {
					return true;
				}
				current = current.parent;
			}		
			return false;
		}

		private static bool IsSleepUsedForEnemyTeam(GameNode node) 
		{
			GameNode current = node;
			while(current != null) {
				foreach (Character e in current.enemies)
				{
					if (e.debuff == DEBUFF.SLEEPING) {
						return true;
					}
				}
				current = current.parent;
			}		
			return false;
		}

		private static void FillRRTData(int i, int j, List<Character> players, List<Character> enemies)
		{					
			bool success = true;
			int K = 10;
			int numOfSleeps1 = 0;
			int[] failCount = new int[K];
			for (int k = 0; k < K; k++) {
				GameNode start = new GameNode(players, enemies, 0);				
				List<GameNode> graph = RRT.Build (start);				
				GameNode best = Utils.GetBest(graph);
				
				if (best.getNodeState() != GAME_STATE.PLAYER_WIN) {
					Console.WriteLine("ALGORITHM FAILS");
					failCount[k]++;
					if (failCount[k] > 9) {
						success = false;
						break;
					}
					k--;
				} else {
					if(IsSleepUsedForEnemyX(best))
					{
						numOfSleeps1++;
					}
					//Utils.printNodePath(best,0,new List<GameNode>());
				}
			}
			if (success) {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					(float) numOfSleeps1/K});
			} else {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.1f});
			}
			
		}

		private static void FillBFSearchData(int i, int j, List<Character> players, List<Character> enemies)
		{
			List<GameNode> bestSolutions = new List<GameNode> ();
			GameNode start = new GameNode(players, enemies, 0);				
			List<GameNode> graph = BFSearch.Build (start);				
			GameNode best = Utils.GetBest(graph);
			if (best.getNodeState() != GAME_STATE.PLAYER_WIN) {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.1f});
				enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.1f});
				return;
			}

			int sleepCountX = 0;
			int sleepCountTeam = 0;
			foreach (GameNode node in graph) {
				if (Utils.getHealthSum(node.players) == Utils.getHealthSum(best.players)
				    && node.getRounds() == best.getRounds()) {
					if (IsSleepUsedForEnemyX(node)) {
						sleepCountX++;
					}
					if (IsSleepUsedForEnemyTeam(node)) {
						sleepCountTeam++;
					}
					bestSolutions.Add(node);
				}
			}
			enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
				(float) sleepCountX/bestSolutions.Count});
			enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
				(float) sleepCountTeam/bestSolutions.Count});
		}
	}
}

