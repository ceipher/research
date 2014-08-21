using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class Greedy
	{		 
		public static List<GameState> Build(GameState rootState)
		{
			List<GameState> graph = new List<GameState>();
			GameState current = rootState;
			graph.Add(current);
			if (current.getGameState() != GAME_STATE.INPROCESS) return graph;
			while(true)
			{
				GameState newState = current.Copy();
				foreach(Character p in newState.players)
				{
					Action pAction = Utils.getPlayerStrategy(p, STRATEGY.RANDOM_ACTION, newState);
					newState.doAction(pAction);
					newState.playersAction.Add(pAction);
				}
				foreach(Character e in newState.enemies)
				{
					Action eAction = Utils.getEnemyStrategy(e, STRATEGY.LOWEST_HP_TARGET, newState);
					newState.doAction(eAction);
					newState.enemiesAction.Add(eAction);
				}
				newState.parent = current;
				graph.Add(newState);
				
				if (newState.getGameState() != GAME_STATE.INPROCESS) 
				{
					break;
				} else {
					current = newState;
				}					
			}
			return graph;
		}
	}
}

