using System;
using System.Collections.Generic;
namespace AG
{
	public class StrategyForEnemy
	{
		public static Action NextAction(Character actionDealer, STRATEGY strategy, GameNode currentState) 
		{
			
			Action action = Action.NoAction(actionDealer);
			if (!actionDealer.isNormal ()) {
				return action;
			}
			Character target = null;
			List<int> alivePlayerPointers = new List<int>();
			for(int j = 0; j < currentState.players.Count; j++) 
			{
				if (currentState.players[j].health > 0)
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
					if (currentState.players[pp].health < minHP)
					{
						target = currentState.players[pp];
						minHP = currentState.players[pp].health;
					}
				}                
				action = new Action(ACTION_TYPE.ATTACK, actionDealer, target);
				break;
				
			case STRATEGY.RANDOM_ACTION:
				Random rand = new Random();
				action = new Action(ACTION_TYPE.ATTACK, actionDealer, 
				                    currentState.players[alivePlayerPointers[rand.Next(alivePlayerPointers.Count)]]);
				break;                
			}
			
			return action;
		}
	}
}

