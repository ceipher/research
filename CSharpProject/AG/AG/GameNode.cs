using System;
using System.Collections.Generic;

namespace AG
{
	public enum GAME_STATE {PLAYER_WIN, ENEMY_WIN, INPROCESS};
	public class GameNode
	{
		public enum NODE_TYPE {IN_EXPLORED, IN_RRT, IN_OPTIMAL};
		
		public GameNode parent;		
		public List<Character> players;
		public List<Action> playersAction = new List<Action>();
		public int playersPotionLeft;
		public List<Character> enemies;
		public List<Action> enemiesAction = new List<Action>();
		
		public int N = 0;//MonteCarlo
		public float Q = 0;//MonteCarlo
		public bool isExpanded = false;//MonteCarlo
		public List<GameNode> exploredChildren = new List<GameNode>();//MonteCarlo & RRT
		public NODE_TYPE nodeType = NODE_TYPE.IN_EXPLORED;//RRT
		public List<GameNode> rrtChildren = new List<GameNode>();//RRT Display
		
		public GameNode(List<Character> pplayers, List<Character> penemies, int playersPotions) 
		{
			players = new List<Character>();
			foreach(Character player in pplayers)
			{
				Character pc = player.Copy ();
				players.Add(pc);
			}
			playersPotionLeft = playersPotions;
			
			enemies = new List<Character>();
			foreach(Character enemy in penemies)
			{
				Character ec = enemy.Copy();
				enemies.Add(ec);
			}
		}

		public List<GameNode> GetAllChildren()
		{
			List<GameNode> allChildren = new List<GameNode>();
			if (this.getNodeState() != GAME_STATE.INPROCESS) return allChildren;
			
			List<GameNode> playerTurnChildren = new List<GameNode>();
			Utils.DoPlayerAction(this,0,playerTurnChildren);// Player turn
			foreach(GameNode s in playerTurnChildren)// Enmey turn
			{
				foreach(Character e in s.enemies)
				{
					Action enemyAction;
					enemyAction = StrategyForEnemy.NextAction(e, STRATEGY.LOWEST_HP_TARGET, s);
					s.doAction(enemyAction);
					s.enemiesAction.Add(enemyAction);
				}
				s.parent = this;
				allChildren.Add(s);
			}
			return allChildren;
		}

		public List<Action> getAvailableActions (Character c)
		{
			List<Action> actions = new List<Action> ();
			if (!c.isNormal()) { 
				return actions;
			}
			foreach (Character e in this.enemies) {
				if (e.health > 0) {
					actions.Add(new Action(ACTION_TYPE.ATTACK, c, e));

					if (c.magics.Count > 0)
					{
						foreach(Magic magic in c.magics)
						{
							if (magic.manaCost <= c.mana
							    && e.debuff != magic.debuff){
								actions.Add(new Action(ACTION_TYPE.MAGIC, magic, c, e));
							}
						}						
					}
				}
			}
			if (this.playersPotionLeft > 0) {
				actions.Add(new Action(ACTION_TYPE.POTION, c, c));
			}
			return actions;
		}

		
		public void doAction(Action action)
		{
			Character target = action.target;
			Character source = action.source;
			switch(action.type)
			{
			case ACTION_TYPE.NONE:
				break;
				
			case ACTION_TYPE.POTION:					
				target.health += Global.POTION_HEAL;
				if (target.health > target.maxHealth) target.health = target.maxHealth;
				this.playersPotionLeft--;
				break;
				
			case ACTION_TYPE.ATTACK:
				target.health -= source.attack;
				if (target.health < 0) target.health = 0;
				break;
			
			case ACTION_TYPE.MAGIC:
				target.health -= action.magic.damage;
				target.debuff =action.magic.debuff;
				target.debuffLeft = action.magic.effectiveRounds;
				source.mana -= action.magic.manaCost;
				if (source.mana < 0) source.mana = 0;
				break;
			}
		}

		public GameNode Copy() 
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
			GameNode newState = new GameNode(copyPlayers, copyEnemies, playersPotionLeft);
			newState.playersAction = copyPlayerActions;
			newState.enemiesAction = copyEnemieActions;
			return newState;
		}

		public bool isEqual(GameNode other)
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
		
		public bool isNodeEqualSmaller(GameNode target)
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
			GameNode temp = this;
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
			this.nodeType = GameNode.NODE_TYPE.IN_OPTIMAL;
			if (this.parent != null)
			{
				this.parent.markOptimal();
			}
		}
		
		public override String ToString() 
		{
			String s = "";
			s += "p[";
			foreach (Character p in players)
			{
				s += p.health+"-";
			}
			s += "]-e[";
			foreach (Character e in enemies)
			{
				s += e.health;
				if (e.debuff != DEBUFF.NONE)
				{
					s+= "(" + e.debuff.ToString()+ ")";
				}
				s+= "-";
			}
			s += "]";
			s += "-potions["+playersPotionLeft+"]";
			s += " PlayerTeam: " + Utils.GetHealthSum (players)
				+ " EnemyTeam: " + Utils.GetHealthSum (enemies);
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
		
		
		public GAME_STATE getNodeState()
		{
			if (Utils.GetHealthSum(players) > 0 && Utils.GetHealthSum(enemies) == 0)
			{
				return GAME_STATE.PLAYER_WIN;
			} else if (Utils.GetHealthSum(players) == 0 && Utils.GetHealthSum(enemies) > 0)
			{
				return GAME_STATE.ENEMY_WIN;
			} else {
				return GAME_STATE.INPROCESS;
			}
		}

		public float getReward()
		{
			return Utils.GetHealthSum(this.players);
		}
	}
}