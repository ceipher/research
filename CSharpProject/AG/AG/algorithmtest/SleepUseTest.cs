
using System;
using System.Collections.Generic;
using System.IO;
namespace AG
{
	public class SleepUseTest
	{



		public static void GenerateHeapMap()
		{
			int minHealth = 6;
			int maxHealth = 20;
			int healthStepSize = 1;
			int minAttack = 1;
			int maxAttack = 18;
			int attackStepSize = 1;
			
			List<float[]> data = new List<float[]> ();
			
			for (int i = 0; minHealth+i*healthStepSize<=maxHealth; i++) {
				for (int j = 0; minAttack+j*attackStepSize<=maxAttack; j++)	{
					int testHealth = minHealth+healthStepSize*i;
					int testAttack = minAttack+attackStepSize*j;
					Console.WriteLine("i:"+i+" j:"+j);
					List<Character> players = new List<Character>();
					players.Add (new Character(80,1,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
					players.Add (new Character(80,3,"P2"));
					players.Add (new Character(80,3,"P3"));
					List<Character> enemies = new List<Character>();
					enemies.Add (new Character(testHealth,testAttack,"E2"));
					enemies.Add (new Character(13,6,"E1"));
					
					int K = 20;
					int numOfSleeps1 = 0;
					int numOfSleeps2 = 0;
					
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
								} else if (playerTeamHealth == maxPlayerTeamHealth) {
									if (s.getRounds () < best.getRounds()){
										best = s;
									}
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
								if (current.enemies[0].debuff == DEBUFF.SLEEPING) {
									numOfSleeps1++;
									break;
								}
								if (current.enemies[1].debuff == DEBUFF.SLEEPING) {
									numOfSleeps2++;
									break;
								}
								current = current.parent;
							}							
						}
					}
					
					data.Add (new float[] {minHealth+healthStepSize*i, minAttack+attackStepSize*j,
						(float) numOfSleeps1/K, (float) numOfSleeps2/K});
				}
			}
			
			
			//Write To Files
			string fileContent = "";
			fileContent += ("set title \"RRT - 3 players vs. 2 enemies\""
			                + "\r\n" + "unset key" 
			                + "\r\n" + "set tic scale 0"
			                + "\r\n" + "set palette rgbformula -7,2,-7"
			                + "\r\n" + "set cbrange [0:1]"
			                + "\r\n" + "set cblabel \"Probability of Using Sleep\""
			                + "\r\n" + "unset cbtics"
			                + "\r\n" + "set xlabel \"health\""
			                + "\r\n" + "set ylabel \"attack\""
			                + "\r\n" + "set xrange [" + (float)(minHealth-0.5) + ":" + (float)(maxHealth+0.5) + "]"
			                + "\r\n" + "set yrange [" + (float)(minAttack-0.5) + ":" + (float)(maxAttack+0.5) + "]"
			                + "\r\n" + "set xtics " + healthStepSize
			                + "\r\n" + "set ytics " + attackStepSize
			                + "\r\n" + "plot \'-\' using 1:2:3 with image");
			foreach (float[] dataline in data) {
				fileContent += ("\r\n" + string.Join(" ", dataline));
			}
			fileContent += "\r\ne";
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\HeatMaps\\a.plt", FileMode.Create);
			StreamWriter file = new StreamWriter(fcreate);
			file.WriteLine(fileContent);
			Console.WriteLine (fileContent);
			file.Close();	
		}
	}
}

