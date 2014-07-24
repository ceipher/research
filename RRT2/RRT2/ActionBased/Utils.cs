using System;
using System.Collections.Generic;
using System.Linq;

namespace RRT2
{
	
	public enum STRATEGY {RANDOM_TARGET, LOWEST_HP_TARGET, HIGHEST_ATTACK_TARGET, THREAT_TARGET}
	
	public class Utils
	{
		public Utils ()
		{
		}
		
		public static List<Character> generateDefaultPlayers()
		{
			List<Character> players = new List<Character>();
			players.Add (new Character(16,2,1));
			players.Add (new Character(16,2,1));
			players.Add (new Character(16,2,1));
			return players;
		}
		
		public static List<Character> generateDefaultEnemies()
		{
			List<Character> enemies = new List<Character>();
			enemies.Add (new Character(10,2,0));
			enemies.Add (new Character(10,2,0));
			enemies.Add (new Character(10,2,0));
			return enemies;
		}
	
		public static Action getPlayerStrategy(STRATEGY strategy, List<Character> enemies) 
		{
			Action action = new Action(ACTION_TYPE.NONE, -1);
			int target = -1;
			/* non-waste-damage mechanism */
			List<int> aliveEnemyPointers = new List<int>();
			for(int j = 0; j < enemies.Count; j++) 
			{
				if (enemies[j].health > 0) 
				{
					aliveEnemyPointers.Add(j);
				}
			}
			if (aliveEnemyPointers.Count == 0) return action;
			
			/** Strategy implementation **/
			switch(strategy)
			{
				case STRATEGY.RANDOM_TARGET:			
					Random rand = new Random();
					action = new Action(ACTION_TYPE.ATTACK, aliveEnemyPointers[rand.Next(aliveEnemyPointers.Count)]);
					break;		
						
				case STRATEGY.LOWEST_HP_TARGET:
					int minHP = int.MaxValue;
					foreach (int pp in aliveEnemyPointers)
					{
						if (enemies[pp].health < minHP)
						{
							target = pp;
							minHP = enemies[pp].health;
						}
					}	
					action = new Action(ACTION_TYPE.ATTACK, target);
					break;
				
				case STRATEGY.HIGHEST_ATTACK_TARGET:
					int maxAttack = int.MinValue;
					foreach (int pp in aliveEnemyPointers) 
					{
						if (enemies[pp].attack > maxAttack)
						{
							target = pp;
							maxAttack = enemies[pp].attack;
						}
					}
					action = new Action(ACTION_TYPE.ATTACK, target);
					break;
					
				case STRATEGY.THREAT_TARGET:
					int maxThreatValue = int.MinValue;
					int enemyTotalHealth = 0;
					foreach (Character enemy in enemies) 
					{
						enemyTotalHealth += enemy.health;
					}	
					foreach (int pp in aliveEnemyPointers)
					{
						int threatValue = enemies[pp].attack * (enemyTotalHealth - enemies[pp].health);
						if (threatValue > maxThreatValue)
						{
							target = pp;
							maxThreatValue = threatValue;
						}
					}
					action = new Action(ACTION_TYPE.ATTACK, target);
					break;
			}
				
			return action;
		}
		
		public static Action getEnemyStrategy(STRATEGY strategy, List<Character> players) 
		{
			Action action = new Action(ACTION_TYPE.NONE, -1);
			int target = -1;
			List<int> alivePlayerPointers = new List<int>();
			for(int j = 0; j < players.Count; j++) 
			{
				if (players[j].health > 0)
				{
					alivePlayerPointers.Add(j);
				}
			}
			if (alivePlayerPointers.Count == 0) return action;
			
			/** Strategy implementation **/
			switch (strategy)
			{
				case STRATEGY.LOWEST_HP_TARGET:	
					int minHP = int.MaxValue;
					foreach (int pp in alivePlayerPointers)
					{
						if (players[pp].health < minHP)
						{
							target = pp;
							minHP = players[pp].health;
						}
					}				
					action = new Action(ACTION_TYPE.ATTACK, target);
					break;
				
				case STRATEGY.RANDOM_TARGET:
					Random rand = new Random();
					action = new Action(ACTION_TYPE.ATTACK, alivePlayerPointers[rand.Next(alivePlayerPointers.Count)]);
					break;				
			}
	
			
			return action;
		}
		
		public static void addNode(List<RRTNode> graph, RRTNode n)
		{
			// remove duplicates
			bool exist = false;
			foreach(RRTNode node in graph)
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
		
		public static int getHealthSum(List<Character> listOfCharacters)
		{
			int sum = 0;
			foreach (Character c in listOfCharacters)
			{
				sum += c.health;
			}
			return sum;
		}
	}
	
}

