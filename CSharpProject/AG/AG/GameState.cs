using System;
using System.Collections.Generic;

namespace AG
{
	public enum GAME_STATE {PLAYER_WIN, ENEMY_WIN, INPROCESS};
	public class GameState
	{
		public enum NODE_TYPE {IN_EXPLORED, IN_RRT, IN_OPTIMAL};
		
		public GameState parent;		
		public List<Character> players;
		public List<Action> playersAction = new List<Action>();
		public int playersPotionLeft;
		public List<Character> enemies;
		public List<Action> enemiesAction = new List<Action>();
		
		public int N = 0;//MonteCarlo
		public float Q = 0;//MonteCarlo
		public bool isExpanded = false;//MonteCarlo
		public List<GameState> exploredChildren = new List<GameState>();//MonteCarlo & RRT
		public NODE_TYPE nodeType = NODE_TYPE.IN_EXPLORED;//RRT
		public List<GameState> rrtChildren = new List<GameState>();//RRT Display
		
		public GameState(List<Character> pplayers, List<Character> penemies, int playersPotions) 
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

		public List<GameState> GetAllChildren()
		{
			List<GameState> allChildren = new List<GameState>();
			if (this.getGameState() != GAME_STATE.INPROCESS) return allChildren;
			
			List<GameState> playerTurnChildren = new List<GameState>();
			Utils.DoPlayerAction(this,0,playerTurnChildren);// Player turn
			foreach(GameState s in playerTurnChildren)// Enmey turn
			{
				foreach(Character e in s.enemies)
				{
					Action enemyAction;
					enemyAction = Utils.getEnemyStrategy(e, STRATEGY.LOWEST_HP_TARGET, s);
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
							if (magic.manaCost <= c.mana){
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
				s += e.health;
				if (e.debuff != DEBUFF.NONE)
				{
					s+= "(" + e.debuff.ToString()+ ")";
				}
				s+= "-";
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
		
		
		public GAME_STATE getGameState()
		{
			if (Utils.getHealthSum(players) > 0 && Utils.getHealthSum(enemies) == 0)
			{
				return GAME_STATE.PLAYER_WIN;
			} else if (Utils.getHealthSum(players) == 0 && Utils.getHealthSum(enemies) > 0)
			{
				return GAME_STATE.ENEMY_WIN;
			} else {
				return GAME_STATE.INPROCESS;
			}
		}
	}
}