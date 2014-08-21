using System;
using System.Collections.Generic;
namespace AG
{
	public class MonteCarlo
	{ 
		
		private static  float Cp = 1 / (float) Math.Sqrt (2);
		
		public static List<GameState> Build(GameState rootState)
		{
			List<GameState> graph = new List<GameState>();
			GameState current = rootState;
			graph.Add(current);
			
			int i = 0;
			GameState v0 = rootState;
			while(i < Global.MAX_ITERATION)
			{
				GameState vl = TreePolicy(v0);
				if (vl == null) break;
				float delta = DefaultPolicy(vl);
				Backup(vl,delta);
				i++;
			}
			GameState ve = v0;
			while(ve.getGameState() == GAME_STATE.INPROCESS)
			{
				ve = BestChild(ve,0);
				if (ve == null)
				{
					break;
				}
				graph.Add(ve);
			}
			return graph;
		}
		
		private static GameState TreePolicy(GameState v)
		{
			while(v!= null && v.getGameState() == GAME_STATE.INPROCESS)
			{
				if (v.exploredChildren.Count ==0){
					v.exploredChildren = v.GetAllChildren();
				}
				if (!isFullyExpanded(v))
				{
					return Expand(v);
				} else {
					v = BestChild(v, Cp);
				}
			}
			return v;
		}
		
		private static GameState Expand(GameState v)
		{
			foreach(GameState s in v.exploredChildren)
			{
				if (!s.isExpanded) 
				{
					s.isExpanded = true;
					
					return s;
				}
			}
			return null;
		}
		
		private static bool isFullyExpanded(GameState v)
		{
			if (v.getGameState() != GAME_STATE.INPROCESS && v.exploredChildren.Count == 0)
			{
				// Terminal State
				return true;
			}
			foreach(GameState s in v.exploredChildren)
			{
				if (s.isExpanded == false)
				{
					return false;
				}
			}
			return true;
		}
		
		private static GameState BestChild(GameState v, float c)
		{
			GameState best = null;
			float currentValue = float.MinValue;
			foreach(GameState vprime in v.exploredChildren)
			{
				float mcValue = (vprime.Q / vprime.N)
					+ c * (float) Math.Sqrt(2 * Math.Log(v.N) / vprime.N);
				if (mcValue > currentValue)
				{
					currentValue = mcValue;
					best = vprime;
				}
			}
			return best;
		}
		
		private static float DefaultPolicy(GameState v)
		{
			GameState current = v;
			while(current.getGameState() == GAME_STATE.INPROCESS)
			{
				GameState newState = current.Copy();
				foreach(Character p in newState.players)
				{
					Action pAction = Utils.getPlayerStrategy(p, STRATEGY.RANDOM_ACTION, newState);
					newState.doAction(pAction);
				}
				foreach(Character e in newState.enemies)
				{
					Action eAction = Utils.getEnemyStrategy(e, STRATEGY.LOWEST_HP_TARGET, newState);
					newState.doAction(eAction);
				}
				current = newState;
			}
			
			return Utils.getHealthSum(current.players) / Utils.getMaxHealthSum(current.players);
		}
		
		private static void Backup(GameState v, float delta)
		{
			while (v != null)
			{
				v.N = v.N+1;
				v.Q = v.Q+delta;
				v = v.parent;
			}
		}
	}
}
