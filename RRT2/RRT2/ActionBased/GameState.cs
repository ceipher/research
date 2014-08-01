using System;
using System.Collections.Generic;
using System.Linq;

namespace RRT2
{
	public class GameState
	{
		
		public GameState parent;		
		public List<Character> players;
		public List<Action> playersAction = new List<Action>();
		public int playersPotionLeft;
		public List<Character> enemies;
		public List<Action> enemiesAction = new List<Action>();
		
		public List<GameState> rrtChildren = new List<GameState>();
		public List<GameState> exploredChildren = new List<GameState>();
		public enum NODE_TYPE {IN_EXPLORED, IN_RRT, IN_OPTIMAL};
		public NODE_TYPE nodeType = NODE_TYPE.IN_EXPLORED;
		
		public GameState(List<Character> pplayers, List<Character> penemies, int playersPotions) 
		{
			players = new List<Character>();
			foreach(Character player in pplayers)
			{
				players.Add(player.Copy());
			}
			playersPotionLeft = playersPotions;
			
			enemies = new List<Character>();
			foreach(Character enemy in penemies)
			{
				enemies.Add(enemy.Copy());
			}
		}
		
		public GameState Copy() 
		{
			List<Character> copyPlayers = new List<Character>();	
			List<Character> copyEnemies = new List<Character>();
			List<Action> copyPlayerActions = new List<Action>();	
			List<Action> copyEnemieActions = new List<Action>();
			foreach(Character p in players)
			{
				copyPlayers.Add (p.Copy ());
			}
			foreach(Character e in enemies) 
			{
				copyEnemies.Add (e.Copy ());
			}
			foreach(Action a in playersAction)
			{
				copyPlayerActions.Add(a);
			}
			foreach(Action a in enemiesAction)
			{
				copyEnemieActions.Add(a);
			}
			GameState newState = new GameState(copyPlayers, copyEnemies, playersPotionLeft);
			newState.playersAction = copyPlayerActions;
			newState.enemiesAction = copyEnemieActions;
			return newState;
		}
		
		public List<GameState> GetAllChildren()
		{
			List<GameState> allChildren = new List<GameState>();
			List<GameState> playerTurnChildren = new List<GameState>();
			Utils.DoPlayerAction(this,0,playerTurnChildren);// Player turn
			foreach(GameState s in playerTurnChildren)// Enmey turn
			{
				foreach(Character e in s.enemies)
				{
					Action enemyAction;
					if (e.health > 0)
					{
						enemyAction = Utils.getEnemyStrategy(e, STRATEGY.LOWEST_HP_TARGET, s.players);
						e.castAction(enemyAction);
					} else {
						enemyAction = new Action(ACTION_TYPE.NONE, e, e);
					}
					s.enemiesAction.Add(enemyAction);
				}
				s.parent = this;
				allChildren.Add(s);
			}
			return allChildren;
		}
		
		public List<GameState> GetSampleChildren()
		{
			Random rand = new Random();
			List<GameState> children = new List<GameState>();
			for (int i = 0; i < Global.CHILDREN_SAMPLING; i++) 
			{
				GameState newChild = new GameState(players, enemies, playersPotionLeft);
				newChild.parent = this;
				for (int playerIndex = 0; playerIndex < newChild.players.Count; playerIndex++)
				{
					Action playerAction;
					Character player = newChild.players[playerIndex];
					if (player.health > 0)
					{		
						if (newChild.playersPotionLeft > 0 && (new Random()).Next (100)>50) {
							// Use Potion
							playerAction = new Action(ACTION_TYPE.POTION, player, player);
							newChild.playersPotionLeft--;
							player.castAction(playerAction);
						} else {
							// Attack or DoNothing
							playerAction = Utils.getPlayerStrategy(player, STRATEGY.RANDOM_TARGET, newChild.enemies);
							
							System.Threading.Thread.Sleep(1);
							player.castAction(playerAction);
						}
					} else {
						playerAction = new Action(ACTION_TYPE.NONE, player, player);
					} 
					newChild.playersAction.Add(playerAction);
				}
				for (int enemyIndex = 0; enemyIndex < newChild.enemies.Count; enemyIndex++)
				{
					Action enemyAction;
					if (newChild.enemies[enemyIndex].health >0)
					{
						enemyAction = Utils.getEnemyStrategy(newChild.enemies[enemyIndex], STRATEGY.LOWEST_HP_TARGET, newChild.players);
						enemies[enemyIndex].castAction(enemyAction);
						
					} else {
						enemyAction = new Action(ACTION_TYPE.NONE, enemies[enemyIndex],  enemies[enemyIndex]);
					}
					newChild.enemiesAction.Add(enemyAction);
				}
				Utils.addNode(children, newChild);
				
			}
			
			return children;
		}
		
		public bool isEqual(GameState other)
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
		
		public bool isNodeEqualSmaller(GameState target)
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
			GameState temp = this;
			int count = 0;
			while(temp.parent!=null)
			{
				count++;
				temp=temp.parent;
			}
			return count;
		}
		
		public void markOptimal()
		{
			this.nodeType = GameState.NODE_TYPE.IN_OPTIMAL;
			if (this.parent != null)
			{
				this.parent.markOptimal();
			}
		}
		
		public override String ToString() 
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
			s+="-potions["+playersPotionLeft+"]";
			return s;
		}
				
		public String printActions()
		{
			String output = "";
			for (int i=0; i<getRounds(); i++)
			{
				for (int j=0; j<players.Count; j++)
				{
					output += playersAction[j+(i*players.Count)].ToString()+"\n";
				}
				for (int j=0; j<enemies.Count; j++)
				{
					output += enemiesAction[j+(i*enemies.Count)].ToString()+"\n";
				}
				output += "Round End"+"\n";
			}
			return output;
		}
	}
}