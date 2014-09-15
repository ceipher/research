using System;
using System.Collections.Generic;
namespace AG
{
	public class MonteCarlo
	{ 
		
		private static  float Cp =  1 / (float) Math.Sqrt(2) ;
		
		public static List<GameNode> Build(GameNode rootState)
		{
			List<GameNode> graph = new List<GameNode>();
			GameNode current = rootState;
			graph.Add(current);
			
			int i = 0;
			GameNode v0 = rootState;
			while(i < Global.MAX_ITERATION)
			{
				GameNode vl = TreePolicy(v0);
				if (vl == null) break;
				float delta = DefaultPolicy(vl);
				Backup(vl,delta);
				i++;
			}
			GameNode ve = v0;
			while(ve.getNodeState() == GAME_STATE.INPROCESS)
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
		
		private static GameNode TreePolicy(GameNode v)
		{
			while(v!= null && v.getNodeState() == GAME_STATE.INPROCESS)
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
		
		private static GameNode Expand(GameNode v)
		{
			foreach(GameNode s in v.exploredChildren)
			{
				if (!s.isExpanded) 
				{
					s.isExpanded = true;
					
					return s;
				}
			}
			return null;
		}
		
		private static bool isFullyExpanded(GameNode v)
		{
			if (v.getNodeState() != GAME_STATE.INPROCESS && v.exploredChildren.Count == 0)
			{
				// Terminal State
				return true;
			}
			foreach(GameNode s in v.exploredChildren)
			{
				if (s.isExpanded == false)
				{
					return false;
				}
			}
			return true;
		}
		
		private static GameNode BestChild(GameNode v, float c)
		{
			GameNode best = null;
			float currentValue = float.MinValue;
			foreach(GameNode vprime in v.exploredChildren)
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
		
		private static float DefaultPolicy(GameNode v)
		{
			GameNode current = v;
			while(current.getNodeState() == GAME_STATE.INPROCESS)
			{
				GameNode newState = current.Copy();
				foreach(Character p in newState.players)
				{
					Action pAction = StrategyForPlayer.NextAction(p, STRATEGY.LOWEST_HP_TARGET, newState);
					newState.doAction(pAction);
				}
				foreach(Character e in newState.enemies)
				{
					Action eAction = StrategyForEnemy.NextAction(e, STRATEGY.LOWEST_HP_TARGET, newState);
					newState.doAction(eAction);
				}
				current = newState;
			}
			
			return Utils.getHealthSum(current.players) / Utils.getMaxHealthSum(current.players);
		}
		
		private static void Backup(GameNode v, float delta)
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
