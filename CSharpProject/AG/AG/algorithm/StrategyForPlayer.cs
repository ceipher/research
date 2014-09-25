using System;
using System.Collections.Generic;
namespace AG
{
	public class StrategyForPlayer
	{
		public static Action NextAction(Character actionDealer, STRATEGY strategy, GameNode currentState) 
		{
			
			Action action = Action.NoAction (actionDealer);
			if (actionDealer.health <= 0) return action;
			
			Character target = null;
			/* non-waste-damage mechanism */
			List<int> aliveEnemyPointers = new List<int>();
			for(int j = 0; j < currentState.enemies.Count; j++) 
			{
				if (currentState.enemies[j].health > 0) 
				{
					aliveEnemyPointers.Add(j);
				}
			}
			if (aliveEnemyPointers.Count == 0) return action;
			
			/** Strategy implementation **/
			List<Action> actionsAvailable = currentState.getAvailableActions(actionDealer);
			switch(strategy)
			{
			case STRATEGY.RANDOM_ACTION:            
				Random rand = new Random();
				action = actionsAvailable[rand.Next(actionsAvailable.Count)];
				break;        
				
			case STRATEGY.LOWEST_HP_TARGET:
				int minHP = int.MaxValue;
				foreach (int pp in aliveEnemyPointers)
				{
					if (currentState.enemies[pp].health < minHP)
					{
						target = currentState.enemies[pp];
						minHP = currentState.enemies[pp].health;
					}
				}    
				action = new Action(ACTION_TYPE.ATTACK, actionDealer, target);
				break;

			case STRATEGY.HIGHEST_ATTACK_TARGET:
				int maxAttack = int.MinValue;
				foreach (int pp in aliveEnemyPointers) 
				{
					if (currentState.enemies[pp].attack > maxAttack)
					{
						target = currentState.enemies[pp];
						maxAttack = currentState.enemies[pp].attack;
					}
				}
				action = new Action(ACTION_TYPE.ATTACK, actionDealer, target);
				break;
				
			case STRATEGY.THREAT_TARGET:
				int maxThreatValue = int.MinValue;
				foreach (int pp in aliveEnemyPointers)
				{
					int threatValue = currentState.enemies[pp].attack * (Utils.GetHealthSum(currentState.enemies) - currentState.enemies[pp].health);
					if (threatValue > maxThreatValue)
					{
						target = currentState.enemies[pp];
						maxThreatValue = threatValue;
					}
				}
				action = new Action(ACTION_TYPE.ATTACK, actionDealer, target);
				break;
			case STRATEGY.SLEEP1:
				bool usingSleep = false;
				foreach(Action a in actionsAvailable)
				{
					if (a.type == ACTION_TYPE.MAGIC) {
						action  = a;
						usingSleep = true;
						break;
					}
				}
				if (!usingSleep) {
					goto case STRATEGY.THREAT_TARGET;
				}
				break;
			}
			
			return action;
		}

	}
}

