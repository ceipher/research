using System;
using System.Collections.Generic;
using System.IO;

namespace AG
{
	public class Greedy
	{		 
		public static List<GameNode> Build(GameNode rootState)
		{
			List<GameNode> graph = new List<GameNode>();
			GameNode current = rootState;
			graph.Add(current);
			if (current.getNodeState() != GAME_STATE.INPROCESS) return graph;
			while(true)
			{
				GameNode newState = current.Copy();
				RoundControl.RoundBegin(newState);
				foreach(Character p in newState.players)
				{
					Action pAction = StrategyForPlayer.NextAction(p, STRATEGY.RANDOM_ACTION, newState);
					newState.doAction(pAction);
					newState.playersAction.Add(pAction);
				}
				foreach(Character e in newState.enemies)
				{
					Action eAction = StrategyForEnemy.NextAction(e, STRATEGY.LOWEST_HP_TARGET, newState);
					newState.doAction(eAction);
					newState.enemiesAction.Add(eAction);
				}
				newState.parent = current;
				graph.Add(newState);
				
				if (newState.getNodeState() != GAME_STATE.INPROCESS) 
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

