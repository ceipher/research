using System;
using System.Collections.Generic;
using System.Linq;

namespace RRT2
{
	public class RRTNode
	{
		
		public int K2 = 20; // K2 = number of sampled child node
		public RRTNode parent;		
		public List<Character> players;
		public List<Action> players_action = new List<Action>();
		public List<Character> enemies;
		public List<Action> enemies_action = new List<Action>();
		
		public List<RRTNode> rrtChildren = new List<RRTNode>();
		public List<RRTNode> exploredChildren = new List<RRTNode>();
		public enum NODE_TYPE {IN_EXPLORED, IN_RRT, IN_OPTIMAL};
		public NODE_TYPE nodeType = NODE_TYPE.IN_EXPLORED;
		
		public RRTNode(List<Character> pplayers, List<Character> penemies) 
		{
			players = new List<Character>();
			foreach(Character player in pplayers)
			{
				players.Add(player.Copy());
			}
			enemies = new List<Character>();
			foreach(Character enemy in penemies)
			{
				enemies.Add(enemy.Copy());
			}
		}
		
		public List<RRTNode> getChildren()
		{
			Random rand = new Random();
			List<RRTNode> children = new List<RRTNode>();
			for (int i = 0; i < K2; i++) 
			{
				RRTNode newChild = new RRTNode(players, enemies);
				newChild.parent = this;
				for (int playerIndex = 0; playerIndex < newChild.players.Count; playerIndex++)
				{
					Action playerAction;
					Character player = newChild.players[playerIndex];
					if (player.health > 0)
					{			
					
						if (player.health <= (player.maxHealth - Global.POTION_HEAL) && player.potionLeft > 0 && (new Random()).Next (100)>50) {
							// Use Potion
							playerAction = new Action(ACTION_TYPE.POTION, playerIndex);
							player.potionLeft--;
							player.health += Global.POTION_HEAL;
							if (player.health > player.maxHealth) player.health = player.maxHealth;
						} else {
							// Attack
							playerAction = Utils.getPlayerStrategy(STRATEGY.RANDOM_TARGET, newChild.enemies);
							
							System.Threading.Thread.Sleep(1);
							if (playerAction.type != ACTION_TYPE.NONE) 
							{						
								newChild.enemies[playerAction.target].health -= player.attack;
								if (newChild.enemies[playerAction.target].health < 0) 
								{
									newChild.enemies[playerAction.target].health = 0;
								}	
							}
						}
					} else {
						playerAction = new Action(ACTION_TYPE.NONE, -1);
					} 
					
					newChild.players_action.Add(playerAction);
				}
				for (int enemyIndex = 0; enemyIndex < newChild.enemies.Count; enemyIndex++)
				{
					if (newChild.enemies[enemyIndex].health >0)
					{
						Action enemyAction = Utils.getEnemyStrategy(STRATEGY.LOWEST_HP_TARGET, newChild.players);
						if (enemyAction.target < 0) 
						{
							newChild.enemies_action.Add(new Action(ACTION_TYPE.NONE, -1));	
							break;
						}
						newChild.players[enemyAction.target].health -= enemies[enemyIndex].attack;
						if (newChild.players[enemyAction.target].health < 0) 
						{
							newChild.players[enemyAction.target].health = 0;
						}
						newChild.enemies_action.Add(enemyAction);
					}
				}
				Utils.addNode(children, newChild);
				
			}
			
			return children;
		}
		
		public bool isEqual(RRTNode other)
		{
			bool result = true;
			for (int i = 0; i < players.Count; i++) 
			{
				result &= players[i].health == other.players[i].health;
			}
			for (int i = 0; i < enemies.Count; i++)
			{
				result &= enemies[i].health == other.enemies[i].health;
			}
			return result;
		}
		
		public bool isNodeEqualSmaller(RRTNode target)
		{
			bool result = true;
			for (int i = 0; i < target.players.Count; i++)
			{
				if (this.players[i].health > target.players[i].health)
				{
					result = false;
				}
			}
			for (int i = 0; i < target.enemies.Count; i++)
			{
				if (this.enemies[i].health > target.enemies[i].health)
				{
					result = false;
				}
			}
			return result;
		}
		
		public int getRounds()
		{
			RRTNode temp = this;
			int count = 0;
			while(temp.parent!=null)
			{
				count++;
				temp=temp.parent;
			}
			return count;
		}
		
		
		public String toString() 
		{
			String s = "";
			s+="p[";
			foreach (Character p in players)
			{
				s += p.health+"-";
			}
			s+="]-e[";
			foreach (Character e in enemies)
			{
				s += e.health+"-";
			}
			s+="]";
			return s;
		}
				
	}
}