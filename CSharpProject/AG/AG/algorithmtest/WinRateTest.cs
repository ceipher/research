using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class WinRateTest
	{		
		static string testAlgo = "Greedy";
		static List<float[]> winRateData = new List<float[]> ();
		static BEST bestType = BEST.ATTRITION;
		static int samples = 100;
		static int H = 12;
		static int A = 8;
		static int minHealth = H/4;
		static int maxHealth = H*3;
		static int healthStepSize = H/4;
		static int minAttack = A/4;
		static int maxAttack = A*3;
		static int attackStepSize = A/4;		
		public WinRateTest ()
		{
		}

		public static void run() {
			for (int i = 0; minHealth+i*healthStepSize<=maxHealth; i++) {
				for (int j = 0; minAttack+j*attackStepSize<=maxAttack; j++)	{
					int testHealth = minHealth+healthStepSize*i;
					int testAttack = minAttack+attackStepSize*j;
					Console.WriteLine("i:"+i+" j:"+j);
					List<Character> players = new List<Character>();
					players.Add (new Character(H,A,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
					players.Add (new Character(H,A,"P2"));
					//players.Add (new Character(H*HMP,A*AMP,"P3"));
					List<Character> enemies = new List<Character>();
					enemies.Add (new Character(testHealth,testAttack,"E1"));
					enemies.Add (new Character(H,A,"E2"));
					//enemies.Add (new Character(H*HME,A*AME,"E3"));

					List<GameNode> bestSolutions = new List<GameNode> ();
					GameNode start = new GameNode(players, enemies, 0);				

					float winRate;
					if(testAlgo.Equals("RRT")){
						winRate = GetWinRateRRT(start);
					} else {
						winRate = GetWinRateGreedy(start);
					}
					winRateData.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
						winRate});
				}
			}
			drawHeapMaps ();
		}

		private static float GetWinRateRRT(GameNode startNode){
			List<GameNode> graph = BFSearch.Build (startNode);				
			GameNode best = Utils.GetBest(graph, bestType);
			if (best == null)
				return 0;
			else 
				return 1;
		}

		private static float GetWinRateGreedy(GameNode startNode) {
			int winCount = 0;
			for (int i = 0; i < samples; i++) {
				GameNode currentStart = startNode.Copy ();
				List<GameNode> graph = Greedy.Build (currentStart);		
				GameNode best = Utils.GetBest(graph, bestType);
				if (best != null)
					winCount++;
			}
			return (float) winCount / samples;
		}

		private static void drawHeapMaps() {
			//Write To Files
			string fileContent = "";
			fileContent += (""
			                + "\r\n" + "set title \"H="+H+" A="+A+"\\nWin Rate Using " + testAlgo
			                + " ("+bestType.ToString()+")\""
			                + "\r\n" + "unset key" 
			                + "\r\n" + "set tic scale 0"
			                + "\r\n" + "set palette rgbformula -7,2,-7"
			                + "\r\n" + "set cbrange [0:1]"
			                + "\r\n" + "set cblabel \"Probablity of Sleep on Enemy X\""
			                + "\r\n" + "unset cbtics"
			                + "\r\n" + "set xlabel \"health\""
			                + "\r\n" + "set ylabel \"attack\""
			                + "\r\n" + "set xrange [" + (float)(minHealth-healthStepSize/2) + ":" + (float)(maxHealth+healthStepSize/2) + "]"
			                + "\r\n" + "set yrange [" + (float)(minAttack-attackStepSize/2) + ":" + (float)(maxAttack+attackStepSize/2) + "]"
			                + "\r\n" + "set xtics " + healthStepSize
			                + "\r\n" + "set ytics " + attackStepSize
			                + "\r\n" + "plot \'-\' using 1:2:3 with image");
			foreach (float[] dataline in winRateData) {
				fileContent += ("\r\n" + string.Join(" ", dataline));
			}
			fileContent += "\r\ne";
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\HeatMaps\\"+"H"+H
			                               +"A"+A+"_"+testAlgo+"_"+bestType.ToString()+".plt", FileMode.Create);
			StreamWriter file = new StreamWriter(fcreate);
			file.WriteLine(fileContent);
			Console.WriteLine (fileContent);
			file.Close();
		}
	}
}

