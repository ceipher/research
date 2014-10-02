using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class SleepUseOrNotTest_Coverage
	{
		static int playerNum = 1;
		static int enemyNum = 1;
		
		static BEST bestType = BEST.ATTRITION;
		static int minHealth = 50;
		static int maxHealth = 100;
		static int healthStep = 10;
		static int minAttack = 25;
		static int maxAttack = 75;
		static int attackStep = 10;
		static int winCount = 0;
		static double winCoverage = 0;

		static List<float[]> data = new List<float[]> ();
		public static void Run() {
			double totalCase = 0;

			for (int playerH = minHealth; playerH<=maxHealth; playerH+=healthStep) {
				for (int playerA = minAttack; playerA<=maxAttack; playerA+=attackStep) {
				for (int player2A = minAttack; player2A<=maxAttack; player2A+=attackStep) {
					for (int enemyH = minHealth; enemyH<=maxHealth; enemyH+=healthStep) {
						for (int enemyA = minAttack; enemyA<=maxAttack; enemyA+=attackStep) {
						for (int enemy2A = minAttack; enemy2A<=maxAttack; enemy2A+=attackStep) {

							totalCase += 1;

							List<Character> players = new List<Character>();
							players.Add (new Character(playerH,playerA,"P1", new List<Magic> (new Magic[] {Magic.Sleep})));
							//players.Add (new Character(playerH,playerA,"P1"));
							players.Add (new Character(playerH,playerA,"P2"));
							List<Character> enemies = new List<Character>();
							enemies.Add (new Character(enemyH,enemyA,"E1"));
							enemies.Add (new Character(enemyH,enemyA,"E2"));

							List<GameNode> bestSolutions = new List<GameNode> ();
							GameNode start = new GameNode(players, enemies, 0);		
							List<GameNode> graph = BFSearch.Build (start);				
							GameNode best = Utils.GetBest(graph, bestType);
							int SleepCount = 0;
							foreach (GameNode node in graph) {
								if (Utils.IsWinAsGoodAs(best,node,bestType)) {
									bestSolutions.Add(node);

									GameNode current = node;
									bool used = false;
									while(used == false && current != null) {
										foreach (Character e in current.enemies)
										{
											if (e.debuff == DEBUFF.SLEEPING) {
												SleepCount++;
												used = true;
												break;
											}
										}
										current = current.parent;
									}		
								}
							}
							float[] thisData;
							if (bestSolutions.Count == 0) {
								thisData = new float[] {-1f,
								playerH,playerA,enemyH,enemyA};
								data.Add(thisData);
								//Console.WriteLine(playerH+","+playerA+","+enemyH+","+enemyA+","+thisData[0]);
							} else {
								winCount++;
								thisData = new float[] {(float)SleepCount/bestSolutions.Count,
								playerH,playerA,enemyH,enemyA};
								data.Add(thisData);
								//if (thisData[0]==1) Console.WriteLine(bestSolutions.Count+":"+playerH+","+playerA+","+enemyH+","+enemyA+","+thisData[0]);
							}
							//Console.WriteLine(playerH+","+playerA+","+enemyH+","+enemyA+","+thisData[0]);
									if (totalCase % 1000 == 0)Console.WriteLine(totalCase);
						}
						}
					}
				}
				}
			}
			winCoverage = (double) winCount / totalCase;
			Console.WriteLine (totalCase);
			Console.WriteLine (winCoverage);
		}
	}
}

