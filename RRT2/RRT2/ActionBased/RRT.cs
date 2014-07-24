using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RRT2;

namespace RRT2
{
	public class RRT
	{
		public static List<Character> players;
		public static List<Character> enemies;
		
		public static void run(List<Character> pPlayers, List<Character> pEnemies)
		{
			int K = Global.RRT_ITERATION;
			players = pPlayers;
			enemies = pEnemies;
			List<RRTNode> graph = new List<RRTNode>();
			RRTNode root = new RRTNode(players, enemies);
			graph.Add(root);
			Random rand = new Random();
			for (int i = 0; i < K; i++)  //K iterations
			{
				RRTNode sample = sampleNode(rand);
				connect(sample, graph, root);
			}
			
			//Choose best path
			double minValue = double.MaxValue;
			RRTNode best = root;
			foreach(RRTNode nn in graph)
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
				
				foreach(RRTNode nnChild in nn.getChildren())
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
			markOptimal(best);
			Console.WriteLine("RRT graph size: " + graph.Count);
			printNodePath(best, 0, new List<RRTNode>());
			Console.WriteLine("Player team total Health: " + Utils.getHealthSum(best.players));
			Console.WriteLine("BEST: " + best.toString());
			outputGraph(graph, root);
		}	
		
		static private void markOptimal(RRTNode best)
		{
			best.nodeType = RRTNode.NODE_TYPE.IN_OPTIMAL;
			if (best.parent != null)
			{
				markOptimal(best.parent);
			}
		}		
		
		static private void printNodePath(RRTNode n, int count, List<RRTNode> path)
		{
			if (n.parent != null)
			{
				path.Add(n);
				printNodePath(n.parent, ++count, path);
			} else {
				path.Reverse();
				
				Console.WriteLine("Best Path:");
				Console.WriteLine(n.toString());
				foreach(RRTNode nn in path)
				{
					Console.WriteLine("↓ " + nn.toString());
				}			
				Console.WriteLine("Number of rounds: " + count);
			}
		}
		
		private static void outputGraph(List<RRTNode> graph, RRTNode root) 
		{
			foreach(RRTNode n in graph)
			{
				if (n.parent != null)
				{
					n.parent.rrtChildren.Add(n);
				}
			}
			string lines = outputNode(root, 0);
			FileStream fcreate = File.Open("C:\\Users\\lenovo\\Desktop\\Research\\Project\\research\\Visualization\\data.json", FileMode.Create);
			StreamWriter file = new StreamWriter(fcreate);
			file.WriteLine(lines);
			file.Close();
		}
		
		private static string outputNode(RRTNode node, int level)
		{
			string output = "\r\n" + indent(level) + "{\r\n";
			
			level++;
			output += indent(level) + "\"name\": \"" + node.toString() + "\"";
			switch (node.nodeType)
			{
				case RRTNode.NODE_TYPE.IN_EXPLORED:
						
					output += ",\r\n" + indent(level) + "\"type\": " + "0";
					break;
				
				case RRTNode.NODE_TYPE.IN_RRT:
					output += ",\r\n" + indent(level) + "\"type\": " + "1";
					break;	
				
				case RRTNode.NODE_TYPE.IN_OPTIMAL:
					output += ",\r\n" + indent(level) + "\"type\": " + "2";
					break;
				
				default:
					output += ",\r\n" + indent(level) + "\"type\": " + "0";
					break;
				
			}
			
			// Children recursion
			if (node.rrtChildren.Count > 0 || node.exploredChildren.Count > 0)
			{
				output += ",\r\n" + indent(level) + "\"children\": " + "[";
				if (node.rrtChildren.Count > 0) 
				{			
					foreach (RRTNode child in node.rrtChildren)
					{
						output += "\t" + outputNode(child, level) + ",";
					}	
				}
				if (node.exploredChildren.Count > 0)
				{
					foreach (RRTNode child in node.exploredChildren)
					{
						if (child.nodeType == RRTNode.NODE_TYPE.IN_EXPLORED) {
							output += "\t" + outputNode(child, level) + ",";
						}
					}
				}
				output = output.Substring(0, output.Length - 1);
				output += "\r\n" + indent(level) + "]";
			}
			output += "\r\n" + indent(level-1) + "}";
			return output;
		}
		
			
		private static string indent(int level)
		{
			string indent = "";
			for (int i = 0; i < level; i++) {
				indent += "\t";
			}
			return indent;
		}
		
		private static RRTNode sampleNode(Random rand) 
		{
			List<Character> randPlayers = new List<Character>();
			List<Character> randEnemies = new List<Character>();
			
			for (int i = 0; i < players.Count; i++)
			{
				Character randPlayer = new Character(players[i].maxHealth, players[i].attack, players[i].potionLeft);
				randPlayer.health = rand.Next (players[i].maxHealth);
				randPlayers.Add(randPlayer);
			}
			for (int j = 0; j < enemies.Count; j++)
			{
				Character randEnemy = new Character(enemies[j].maxHealth, enemies[j].attack, enemies[j].potionLeft);
				randEnemy.health = rand.Next (enemies[j].maxHealth);
				randEnemies.Add(randEnemy);
			}
			return new RRTNode(randPlayers,randEnemies);
		}
		
		static private void connect(RRTNode candidate, List<RRTNode> graph, RRTNode root)
		{
			//Phase 1 - choose best node in graph
			RRTNode best = root;
			double min = findDistance(root, candidate);
			foreach (RRTNode node in graph)
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
			List<RRTNode> currentPool = new List<RRTNode>();
			List<RRTNode> validNodes = new List<RRTNode>();
			currentPool.Add(best);
			if (best.exploredChildren.Count == 0) // check if the node's children get explored
			{
				foreach(RRTNode child in best.getChildren())
				{
					currentPool.Add(child);
					best.exploredChildren.Add(child);
				}
			} else {
				foreach(RRTNode explored in best.exploredChildren)
				{
					currentPool.Add(explored);
				}
			}
			
			foreach (RRTNode node in currentPool)
			{
				if (true)//candidate.isNodeEqualSmaller(node))
				{
					validNodes.Add(node);
				}					
			}
					
			RRTNode closestNode = root;
			double minDistance = findDistance(root, candidate);
			
			foreach (RRTNode node in validNodes)
			{
				double distance = findDistance(node, candidate);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestNode = node;
				}
			}
			closestNode.nodeType = RRTNode.NODE_TYPE.IN_RRT;
			Utils.addNode(graph, closestNode);
		}
	
		private static double findDistance(RRTNode fromNode, RRTNode toNode) 
		{
			double distance = 0;
			/* method - euclidean distance */
//			for(int i = 0; i < fromNode.players.Count; i++)
//			{
//				distance += Math.Sqrt(fromNode.players[i].health - toNode.players[i].health);
//			}
//			for(int i = 0; i < fromNode.enemies.Count; i++)
//			{
//				distance += Math.Sqrt(fromNode.enemies[i].health - toNode.enemies[i].health);
//				// think of e[4,4] vs e[8,0] as 2+2 vs 2.XX
//			}
			
			
			/* method - sum of enemy threat */
			int totalthreat = 0;
			for (int i = 0; i < fromNode.enemies.Count; i++)
			{
				if (fromNode.enemies[i].health > 0)
				{
					totalthreat += enemies[i].attack * (Utils.getHealthSum(fromNode.enemies) - fromNode.enemies[i].health);
				}
			}
			distance = totalthreat;
			
			return distance;
		}
			
		

	}

}

