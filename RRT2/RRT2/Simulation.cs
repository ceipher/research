using System;
using System.Collections.Generic;
using System.Linq;

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
			//Console.WriteLine (sample.toString());
		}
		double minValue = double.MaxValue;
		Node best = root;
		foreach(Node nn in graph)
		{
			
			Console.WriteLine(nn.toString());
			double val = nn.enemies.Sum();
			if (minValue > val)
			{
				minValue = val;
				best = nn;
			}
		}
		Console.WriteLine("RRT graph size: " + graph.Count);
		printNodePath(best, 0, new List<Node>());
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
			int randEnemy = 0;//rand.Next (enemies[j]);
			randEnemies[j] = randEnemy;
		}
		return new Node(randPlayers,randEnemies);
	}
	
	static private void connect(Node candidate, List<Node> graph, Node root)
	{
		
		List<Node> validNodes = new List<Node>();
		foreach (Node node in graph) 
		{
			if (isNodeSmaller(candidate, node))
			{
				validNodes.Add(node);
			}			
			foreach (Node child in node.getChildren())
			{
				if (isNodeSmaller(candidate, child))
				{
					validNodes.Add(child);
				}
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
		addNode (graph,closestNode);
	}
	
	/* distance function */
	static private double findDistance(Node fromNode, Node toNode) 
	{
		double distance = 0;
		/* methoed - manhattan distance */
//		for(int i = 0; i < fromNode.players.Length; i++)
//		{
//			distance += Math.Sqrt(fromNode.players[i] - toNode.players[i]);
//		}
//		for(int i = 0; i < fromNode.enemies.Length; i++)
//		{
//			distance += Math.Sqrt(fromNode.enemies[i] - toNode.enemies[i]);
//		}
		
		/* method - sum of enemy threat */
		int totalthreat = 0;
		int totalhealth = 0;
		foreach(int enemy in fromNode.enemies) 
		{
			totalhealth += enemy;
		}		
		for (int i = 0; i < fromNode.enemies.Length; i++)
		{
			if (fromNode.enemies[i] > 0)
			{
				totalthreat += enemies_att[i] * (totalhealth - fromNode.enemies[i]);
			}
		}
		distance = totalthreat;
		
		return distance;
	}
	
	static private bool isNodeSmaller(Node left, Node right)
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
				s += p+",";
			}
			s+="],e[";
			foreach (int e in enemies)
			{
				s += e+",";
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
		Console.WriteLine("Player team total HPs: "+totalPlayerHP);	
			
		Console.Write("Players: "+String.Join("", 
			new List<int>(players).ConvertAll(i => i.ToString()+",").ToArray()) + "   ");
		Console.Write("Enemies: "+String.Join("", 
			new List<int>(enemies).ConvertAll(i => i.ToString()+",").ToArray()) + "   ");
	}
	
	
}

