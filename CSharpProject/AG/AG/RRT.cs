using System;
using System.Collections.Generic;
using System.IO;
using AG;

namespace AG 
{
	public class RRT
	{
		public static List<Character> players;
		public static List<Character> enemies;
		public static int K = Global.MAX_ITERATION;
		
		public static List<GameState> Build(GameState rootState)
		{
			players = rootState.players;
			enemies = rootState.enemies;
			
			List<GameState> graph = new List<GameState>();
			graph.Add(rootState);
			Random rand = new Random();
			for (int i = 0; i < K; i++)  //K iterations
			{
				GameState sample = sampleNode(rand);
				connect(sample, graph, graph[0]);
			}
			return graph;
		}
			
		
		private static GameState sampleNode(Random rand) 
		{
			List<Character> randPlayers = new List<Character>();
			List<Character> randEnemies = new List<Character>();
			
			for (int i = 0; i < players.Count; i++)
			{
				Character randPlayer = new Character(players[i].maxHealth, players[i].attack, "P"+(i+1));
				randPlayer.health = rand.Next (players[i].maxHealth);
				randPlayers.Add(randPlayer);
			}
			for (int j = 0; j < enemies.Count; j++)
			{
				Character randEnemy = new Character(enemies[j].maxHealth, enemies[j].attack, "E"+(j+1));
				randEnemy.health = rand.Next (enemies[j].maxHealth);
				randEnemies.Add(randEnemy);
			}
			return new GameState(randPlayers,randEnemies,0);
		}

		static private void connect(GameState candidate, List<GameState> graph, GameState root)
		{
			//Phase 1 - choose best node in graph
			GameState best = root;
			double min = findDistance(root, candidate);
			foreach (GameState node in graph)
			{	
				if (candidate.isNodeEqualSmaller(node))
				{
					double distance = findDistance(node, candidate);
					if (distance < min)
					{
						min = distance;
						best = node;
					}
				}
			}
			
			//Phase 2 - choose best node among selected node and its children
			List<GameState> currentPool = new List<GameState>();
			List<GameState> validNodes = new List<GameState>();
			currentPool.Add(best);
			if (best.exploredChildren.Count == 0) // check if the node's children get explored
			{
				foreach(GameState child in best.GetAllChildren())
				{
					currentPool.Add(child);
					best.exploredChildren.Add(child);
				}
			} else {
				foreach(GameState explored in best.exploredChildren)
				{
					currentPool.Add(explored);
				}
			}
			
			foreach (GameState node in currentPool)
			{
				if (true)//candidate.isNodeEqualSmaller(node))
				{
					validNodes.Add(node);
				}					
			}
					
			GameState closestNode = root;
			double minDistance = findDistance(root, candidate);
			
			foreach (GameState node in validNodes)
			{
				double distance = findDistance(node, candidate);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestNode = node;
				}
			}
			closestNode.nodeType = GameState.NODE_TYPE.IN_RRT;
			Utils.addNode(graph, closestNode);
		}
	
		private static double findDistance(GameState fromNode, GameState toNode) 
		{
			double distance = 0;
			/* method - euclidean distance */
			for(int i = 0; i < fromNode.players.Count; i++)
			{
				distance += Math.Sqrt(fromNode.players[i].health - toNode.players[i].health);
			}
			for(int i = 0; i < fromNode.enemies.Count; i++)
			{
				distance += Math.Sqrt(fromNode.enemies[i].health - toNode.enemies[i].health);
				// think of e[4,4] vs e[8,0] as 2+2 vs 2.XX
			}
			
			
			/* method - sum of enemy threat */
//			int totalthreat = 0;
//			for (int i = 0; i < fromNode.enemies.Count; i++)
//			{
//				if (fromNode.enemies[i].health > 0)
//				{
//					totalthreat += enemies[i].attack * (Utils.getHealthSum(fromNode.enemies) - fromNode.enemies[i].health);
//				}
//			}
//			distance = totalthreat;
			
			return distance;
		}
			
		

	}

}

