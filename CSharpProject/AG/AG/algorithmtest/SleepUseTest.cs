using System;
using System.Collections.Generic;
using System.IO;
namespace AG
{
	public class SleepUseTest
	{
		static List<float[]> enemyXdata = new List<float[]> ();
		static List<float[]> enemyTeamdata = new List<float[]> ();
		static int H = 12;
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
		static BEST bestType = BEST.ATTRITION;

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
					//players.Add (new Character(H*HMP,A*AMP,"P3"));
					List<Character> enemies = new List<Character>();
					enemies.Add (new Character(testHealth,testAttack,"E1"));
					enemies.Add (new Character(H*HME,A*AME,"E2"));
					//enemies.Add (new Character(H*HME,A*AME,"E3"));
					FillBFSearchData(i, j, players, enemies);
				}
			}
			
			
			//Write To Files
			string fileContent = "";
			fileContent += (""
			       	+ "\r\n" + "set multiplot"
			       	+ "\r\n" + "set size 0.5,0.8"
			      	+ "\r\n" + "set origin 0.0,0.0"
			      	+ "\r\n" + "set title \"H="+H+" A="+A+"\\nSleep Uses On Enemy X ("+bestType.ToString()+")\""
			       	+ "\r\n" + "unset key" 
			       	+ "\r\n" + "set tic scale 0"
	                + "\r\n" + "set palette rgbformula -7,2,-7"
	                + "\r\n" + "set cbrange [-0.2:1]"
	                + "\r\n" + "set cblabel \"Probablity of Sleep on Enemy X\""
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
			fileContent += ("\r\nset origin 0.5,0.0 "
			        + "\r\n" + "set title \"H="+H+" A="+A+"\\nSleep Uses On Enemy Team ("+bestType.ToString()+")\""
			       	+ "\r\n" + "set cblabel \"Probablity of Sleep on Enemy Team\""
			        + "\r\n" + "plot \'-\' using 1:2:3 with image");
			foreach (float[] dataline in enemyTeamdata) {
				fileContent += ("\r\n" + string.Join(" ", dataline));
			}
			fileContent += "\r\ne";
			fileContent += "\r\nunset multiplot";
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\HeatMaps\\"+"H"+H
			                               +"A"+A+"_"+bestType.ToString()+".plt", FileMode.Create);
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
			int sleepCountX = 0;
			int sleepCountTeam = 0;
			int[] failCount = new int[K];
			for (int k = 0; k < K; k++) {
				GameNode start = new GameNode(players, enemies, 0);				
				List<GameNode> graph = RRT.Build (start);				
				GameNode best = Utils.GetBest(graph, bestType);
				
				if (best == null) {
					Console.WriteLine("ALGORITHM FAILS TO FIND SOLUTION");
					failCount[k]++;
					if (failCount[k] > 9) {
						success = false;
						break;
					}
					k--;
				} else {
					if(IsSleepUsedForEnemyX(best))
					{
						sleepCountX++;
					}
					if(IsSleepUsedForEnemyTeam(best))
					{
						sleepCountTeam++;
					}
					//Utils.printNodePath(best,0,new List<GameNode>());
				}
			}
			if (success) {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					(float) sleepCountX/K});
				enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					(float) sleepCountTeam/K});
			} else {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.2f});
				enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.2f});
			}
			
		}

		private static void FillBFSearchData(int i, int j, List<Character> players, List<Character> enemies)
		{
			List<GameNode> bestSolutions = new List<GameNode> ();
			GameNode start = new GameNode(players, enemies, 0);				
			List<GameNode> graph = BFSearch.Build (start);				
			GameNode best = Utils.GetBest(graph, bestType);
			if (best == null) {
				enemyXdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.2f});
				enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
					-.2f});
				return;
			}

			int sleepCountX = 0;
			int sleepCountTeam = 0;
			foreach (GameNode node in graph) {
				if (Utils.IsWinAsGoodAs(best,node,bestType)) {
					if (IsSleepUsedForEnemyX(node)) {
						sleepCountX++;
					}
					if (IsSleepUsedForEnemyTeam(node)) {
						sleepCountTeam++;
					}
					bestSolutions.Add(node);
				}
			}
			enemyXdata.Add (new float[] {
				(float) healthStepSize*i+minHealth,
				(float) attackStepSize*j+minAttack,
				(float) sleepCountX/bestSolutions.Count});
			enemyTeamdata.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
				(float) sleepCountTeam/bestSolutions.Count});
		}
	}
}

