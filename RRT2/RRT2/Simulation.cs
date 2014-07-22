using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using RRT2;

public static class Simulation
{

	
	/* Constant */
	private const int RANDOM_TARGET = 1;
	private const int LOWEST_HP_TARGET = 2;
	private const int HIGHEST_ATTACK_TARGET = 3;
	private const int THREAT_TARGET = 4;
	
	/* Init Value */
	
	/* Good data */
	private static int[] init_players = {100,100,100};
	private static int[] init_players_att = {4,8,5};
	private static int[] init_enemies = {14,15,30};
	private static int[] init_enemies_att = {4,8,9};
	
	/* 2v2 */
//	private static int[] init_players = {20,20};
//	private static int[] init_players_att = {1,1};
//	private static int[] init_enemies = {5,3};
//	private static int[] init_enemies_att = {1,2};
	
	/* 2v6 */
//	private static int[] init_players = {80,70};
//	private static int[] init_players_att = {20,30};
//	private static int[] init_enemies = {45,45,45,29,29,29};
//	private static int[] init_enemies_att = {1,1,1,2,2,2};
	
	/* 5v5 */
	//	private static int[] init_players = {20,20,20,20,20};
	//	private static int[] init_players_att = {2,2,2,2,2};
	//	private static int[] init_enemies = {10,10,10,6,6};
	//	private static int[] init_enemies_att = {1,1,1,2,2};
	
	/* 7v6 */
//	private static int[] init_players = {80,45,45,45,29,29,29};
//	private static int[] init_players_att = {5,1,1,1,2,2,2};
//	private static int[] init_enemies = {45,45,45,29,29,29};
//	private static int[] init_enemies_att = {1,1,1,2,2,2};
	
	/* Game Variables */
	private static int[] players = new int[init_players.Length];
	private static int[] players_att = new int[init_players_att.Length];
	private static int[] enemies = new int[init_enemies.Length];
	private static int[] enemies_att = new int[init_enemies_att.Length];
	private static int turnCount = 0;	
	private static List<int[]> strategyTree = new List<int[]>();
	
	static public void Main ()
	{
		RRTNode n = new RRTNode();
		resetGame();
		runRRT();
		//runGreedyGame(1, LOWEST_HP_TARGET);
		
	}
	

	static public IEnumerable<IEnumerable<T>> IntCombinations<T>(this IEnumerable<T> elements, int k)
	{
	  return k == 0 ? new[] { new T[0] } :
	    elements.SelectMany((e, i) =>
	      elements.Skip(i + 1).IntCombinations(k - 1).Select(c => (new[] {e}).Concat(c)));
	}
	
	static private void resetGame()
	{
		Array.Copy(init_players, players, init_players.Length);
		Array.Copy(init_players_att, players_att, init_players_att.Length);
		Array.Copy(init_enemies, enemies, init_enemies.Length);
		Array.Copy(init_enemies_att, enemies_att, init_enemies_att.Length);		
		turnCount = 0;
	}
	
	/**
	 * Random AI
	 */
	static private void runGreedyGame(int playerStrategy, int enemyStrategy) {
		turnCount = 0;
		while(getGameState() == 0) 
		{
			turnCount++;
			
			for (int i = 0; i < players.Length; i++) 
			{
				if (players[i] > 0) 
				{
					int target = getPlayerStrategy(playerStrategy, enemies);
					if (target >= 0)
					{					
						enemies[target] -= players_att[i];
						if (enemies[target] < 0) enemies[target] = 0;
						//Console.WriteLine("Player " + (i+1) + " (" + players[i] + ") attacks enemy " + (target+1));
					}					
				}
				System.Threading.Thread.Sleep(2);
			}
			
			for (int i = 0; i < enemies.Length; i++)
			{
				if (enemies[i] > 0) {
					int target = getEnemyStrategy(enemyStrategy, players);
					if (target >= 0)
					{
						players[target] -= enemies_att[i];
						if (players[target] < 0) players[target] = 0;
						//Console.WriteLine("Enemy " + (i+1) + " (" + enemies[i] + ") attacks player " + (target+1));
					}					
				}
				System.Threading.Thread.Sleep(2);
			}	
		}
		printStatistics(playerStrategy);
	}
	
	static private int getPlayerStrategy(int strategy, int[] enemies) 
	{
		int target = -1;
		/* non-waste-damage mechanism */
		List<int> aliveEnemyPointers = new List<int>();
		for(int j = 0; j < enemies.Length; j++) 
		{
			if (enemies[j] > 0) 
			{
				aliveEnemyPointers.Add(j);
			}
		}
		if (aliveEnemyPointers.Count == 0) return target;
		
		/** Strategy implementation **/
		switch(strategy)
		{
			case RANDOM_TARGET:			
				Random rand = new Random();
				target = aliveEnemyPointers[rand.Next(aliveEnemyPointers.Count)];
				break;		
					
			case LOWEST_HP_TARGET:
				int minHP = int.MaxValue;
				foreach (int pp in aliveEnemyPointers)
				{
					if (enemies[pp] < minHP)
					{
						target = pp;
						minHP = enemies[pp];
					}
				}				
				break;
			
			case HIGHEST_ATTACK_TARGET:
				int maxAttack = int.MinValue;
				foreach (int pp in aliveEnemyPointers) 
				{
					if (enemies_att[pp] > maxAttack)
					{
						target = pp;
						maxAttack = enemies_att[pp];
					}
				}
				break;
				
			case THREAT_TARGET:
				int maxThreatValue = int.MinValue;
				foreach (int pp in aliveEnemyPointers)
				{
					int threatValue = enemies_att[pp] * (getEnemiesTotalHealth() - enemies[pp]);
					if (threatValue > maxThreatValue)
					{
						target = pp;
						maxThreatValue = threatValue;
					}
				}
				break;
		}
			
		return target;
	}
	
	
	
	static private int getEnemyStrategy(int strategy, int[] playersHealthList) 
	{
		int target = -1;
		List<int> alivePlayerPointers = new List<int>();
		for(int j = 0; j < playersHealthList.Length; j++) 
		{
			if (playersHealthList[j] > 0)
			{
				alivePlayerPointers.Add(j);
			}
		}
		if (alivePlayerPointers.Count == 0) return target;
		
		/** Strategy implementation **/
		switch (strategy)
		{
			case LOWEST_HP_TARGET:	
				int minHP = int.MaxValue;
				foreach (int pp in alivePlayerPointers)
				{
					if (playersHealthList[pp] < minHP)
					{
						target = pp;
						minHP = players[pp];
					}
				}				
				break;
			
			case RANDOM_TARGET:
				Random rand = new Random();
				target = alivePlayerPointers[rand.Next(alivePlayerPointers.Count)];
				break;				
		}

		
		return target;
	}
	
	
	/*********   RRT   *********/
	
	static private void runRRT()
	{
		int K =2000;
		List<Node> graph = new List<Node>();
		Node root = new Node(players, enemies);
		graph.Add(root);
		Random rand = new Random();
		for (int i = 0; i < K; i++)  //K iterations
		{
			Node sample = sampleNode(rand);
			connect(sample, graph, root);
		}
		
		//Choose best path
		double minValue = double.MaxValue;
		Node best = root;
		foreach(Node nn in graph)
		{			
			double val = nn.enemies.Sum();
			if (minValue > val)
			{
				minValue = val;
				best = nn;
			} else if (minValue == val)
			{
				if (nn.players.Sum() > best.players.Sum())
				{
					best = nn;
				}
			}
			
			foreach(Node nnChild in nn.getChildren())
			{			
				double childval = nnChild.enemies.Sum();
				if (minValue > childval)
				{
					minValue = childval;
					best = nnChild;
				} else if (minValue == childval)
				{
					if (nnChild.players.Sum() > best.players.Sum())
					{
						best = nnChild;
					}
				}
			}
			
		}
		addNode(graph, best);
		markOptimal(best);
		Console.WriteLine("RRT graph size: " + graph.Count);
		printNodePath(best, 0, new List<Node>());
		Console.WriteLine("Player team total Health: " + best.players.Sum());
		outputGraph(graph, root);
	}
	
	static private void markOptimal(Node best)
	{
		best.nodeType = Node.NODE_TYPE.IN_OPTIMAL;
		if (best.parent != null)
		{
			markOptimal(best.parent);
		}
	}
	
	static private void outputGraph(List<Node> graph, Node root) 
	{
		foreach(Node n in graph)
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
		Console.Write("HERE");
		file.Close();
	}
	
	static private string outputNode(Node node, int level)
	{
		string output = "\r\n" + indent(level) + "{\r\n";
		
		level++;
		output += indent(level) + "\"name\": \"" + node.toString() + "\"";
		switch (node.nodeType)
		{
			case Node.NODE_TYPE.IN_EXPLORED:
					
				output += ",\r\n" + indent(level) + "\"type\": " + "0";
				break;
			
			case Node.NODE_TYPE.IN_RRT:
				output += ",\r\n" + indent(level) + "\"type\": " + "1";
				break;	
			
			case Node.NODE_TYPE.IN_OPTIMAL:
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
				foreach (Node child in node.rrtChildren)
				{
					output += "\t" + outputNode(child, level) + ",";
				}	
			}
			if (node.exploredChildren.Count > 0)
			{
				foreach (Node child in node.exploredChildren)
				{
					if (child.nodeType == Node.NODE_TYPE.IN_EXPLORED) {
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
	
	static private string indent(int level)
	{
		string indent = "";
		for (int i = 0; i < level; i++) {
			indent += "\t";
		}
		return indent;
	}
	
	static private void printNodePath(Node n, int count, List<Node> path)
	{
		if (n.parent != null)
		{
			path.Add(n);
			printNodePath(n.parent, ++count, path);
		} else {
			path.Reverse();
			
			Console.WriteLine("Best Path:");
			Console.WriteLine(n.toString());
			foreach(Node nn in path)
			{
				Console.WriteLine("↓ " + nn.toString());
			}			
			Console.WriteLine("Number of rounds: " + count);
		}
	}
	
	static private Node sampleNode(Random rand) 
	{
		int[] randPlayers = new int[players.Length];
		int[] randEnemies = new int[enemies.Length];
		
		for (int i = 0; i < players.Length; i++)
		{
			int randPlayer = rand.Next (players[i]);
			randPlayers[i] = randPlayer;
		}
		for (int j = 0; j < enemies.Length; j++)
		{
			int randEnemy = rand.Next (enemies[j]);
			randEnemies[j] = randEnemy;
		}
		return new Node(randPlayers,randEnemies);
	}
	
	static private void connect(Node candidate, List<Node> graph, Node root)
	{
		//Phase 1 - choose best node in graph
		Node best = root;
		double min = findDistance(root, candidate);
		foreach (Node node in graph)
		{	
			if (isNodeEqualSmaller(candidate, node))
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
		List<Node> currentPool = new List<Node>();
		List<Node> validNodes = new List<Node>();
		currentPool.Add(best);
		if (best.exploredChildren.Count == 0) // check if the node's children get explored
		{
			foreach(Node child in best.getChildren())
			{
				currentPool.Add(child);
				best.exploredChildren.Add(child);
			}
		} else {
			foreach(Node explored in best.exploredChildren)
			{
				currentPool.Add(explored);
			}
		}
		
		foreach (Node node in currentPool)
		{
			if (isNodeEqualSmaller(candidate, node))
			{
				validNodes.Add(node);
			}					
		}
				
		Node closestNode = root;
		double minDistance = findDistance(root, candidate);
		
		foreach (Node node in validNodes)
		{
			double distance = findDistance(node, candidate);
			if (distance < minDistance)
			{
				minDistance = distance;
				closestNode = node;
			}
		}
		closestNode.nodeType = Node.NODE_TYPE.IN_RRT;
		addNode (graph,closestNode);
	}
	
	/* distance function */
	static private double findDistance(Node fromNode, Node toNode) 
	{
		double distance = 0;
		/* method - euclidean distance */
		for(int i = 0; i < fromNode.players.Length; i++)
		{
			distance += Math.Sqrt(fromNode.players[i] - toNode.players[i]);
		}
		for(int i = 0; i < fromNode.enemies.Length; i++)
		{
			distance += Math.Sqrt(fromNode.enemies[i] - toNode.enemies[i]);
		}
		
		/* method - sum of enemy threat */
//		int totalthreat = 0;
//		int totalhealth = 0;
//		foreach(int enemy in fromNode.enemies) 
//		{
//			totalhealth += enemy;
//		}		
//		for (int i = 0; i < fromNode.enemies.Length; i++)
//		{
//			if (fromNode.enemies[i] > 0)
//			{
//				totalthreat += enemies_att[i] * (totalhealth - fromNode.enemies[i]);
//			}
//		}
//		distance = totalthreat;
		
		return distance;
	}
	
	static private bool isNodeEqualSmaller(Node left, Node right)
	{
		bool result = true;
		for (int i = 0; i < right.players.Length; i++)
		{
			if (left.players[i] > right.players[i])
			{
				result = false;
			}
		}
		for (int i = 0; i < right.enemies.Length; i++)
		{
			if (left.enemies[i] > right.enemies[i])
			{
				result = false;
			}
		}
		return result;
	}
		
	static private void addNode(List<Node> graph, Node n)
	{
		// remove duplicates
		bool exist = false;
		foreach(Node node in graph)
		{
			if (node.isEqual(n))
			{
				exist = true;
			}
		}
		if (!exist)
		{
			graph.Add(n);
		}		
	}
	
	class Node
	{
		public int K2 = 20; // K2 = number of sampled child node
		public Node parent;		
		public int[] players;
		public List<int> players_target = new List<int>();
		public int[] enemies;
		public List<int> enemies_target = new List<int>();
		
		public List<Node> rrtChildren = new List<Node>();
		public List<Node> exploredChildren = new List<Node>();
		public enum NODE_TYPE {IN_EXPLORED, IN_RRT, IN_OPTIMAL};
		public NODE_TYPE nodeType = NODE_TYPE.IN_EXPLORED;
		
		public Node(int[] pplayers, int[] penemies) 
		{
			players = new int[pplayers.Length];
			enemies = new int[penemies.Length];
			Array.Copy(pplayers,players,pplayers.Length);
			Array.Copy(penemies,enemies,penemies.Length);
		}
		
		public List<Node> getChildren()
		{
			Random rand = new Random();
			List<Node> children = new List<Node>();
			for (int i = 0; i < K2; i++) 
			{
				int[] p = new int[init_players.Length];
				int[] e = new int[init_enemies.Length];
				Array.Copy(players,p,players.Length);
				Array.Copy(enemies,e,enemies.Length);
				Node newNode = new Node(p,e);
				newNode.parent = this;
				for (int pi = 0; pi < newNode.players.Length; pi++)
				{
					if (newNode.players[pi] > 0)
					{						
						int playerTarget = getPlayerStrategy(RANDOM_TARGET, newNode.enemies);
						System.Threading.Thread.Sleep(1);
						if (playerTarget < 0) continue;
						newNode.enemies[playerTarget] -= players_att[pi];
						if (newNode.enemies[playerTarget] < 0) newNode.enemies[playerTarget] = 0;
						newNode.players_target.Add(playerTarget);
					}
				}
				for (int ei = 0; ei < newNode.enemies.Length; ei++)
				{
					if (newNode.enemies[ei] >0)
					{
						int enemyTarget = getEnemyStrategy(LOWEST_HP_TARGET, newNode.players);
						if (enemyTarget < 0) continue;
						newNode.players[enemyTarget] -= enemies_att[ei];
						if (newNode.players[enemyTarget] < 0) newNode.players[enemyTarget] = 0;
						newNode.enemies_target.Add(enemyTarget);
					}
				}
				addNode(children, newNode);
				
			}
			
			return children;
		}
		
		public bool isEqual(Node s)
		{
			bool result = true;
			for (int i = 0; i < players.Length; i++) 
			{
				result &= players[i] == s.players[i];
			}
			for (int i = 0; i < enemies.Length; i++)
			{
				result &= enemies[i] == s.enemies[i];
			}
			return result;
		}
		
		public String toString() 
		{
			String s = "";
			s+="p[";
			foreach (int p in players)
			{
				s += p+"-";
			}
			s+="]-e[";
			foreach (int e in enemies)
			{
				s += e+"-";
			}
			s+="]";
			return s;
		}
				
	}
	
	static private int getGameState()
	{
		bool playerLose = true;
		foreach(int player_hp in players)
		{
			playerLose &= (player_hp <= 0);
		}
		if (playerLose) return 2;
		bool enemyLose = true;
		foreach (int enemy_hp in enemies) 
		{
			enemyLose &= (enemy_hp <= 0);
		}
		if (enemyLose) return 1;
		return 0;
	}
	
	static private int getPlayersTotalHealth() 
	{
		int health = 0;
		foreach (int hp in players)
		{
			health += hp;
		}
		return health;
	}
		
	static private int getEnemiesTotalHealth() 
	{
		int health = 0;
		foreach (int hp in enemies)
		{
			health += hp;
		}
		return health;
	}
	
	static private void printStatistics(int playerStrategy) {
		int win = getGameState();
		switch (win)
		{
			case 1:	Console.WriteLine("Players win "); break;
			case 2:	Console.WriteLine("Enemies win "); break;
			default: break;
		} 
		
		Console.WriteLine(turnCount+" turns");		
		
		
		int totalPlayerHP = 0;
		foreach (int hp in players)
		{
			totalPlayerHP += hp;	
		}
		Console.WriteLine("Player team total Health: "+totalPlayerHP);	
			
		Console.Write("Players: "+String.Join("", 
			new List<int>(players).ConvertAll(i => i.ToString()+",").ToArray()) + "   ");
		Console.Write("Enemies: "+String.Join("", 
			new List<int>(enemies).ConvertAll(i => i.ToString()+",").ToArray()) + "   ");
	}
	
	
}

