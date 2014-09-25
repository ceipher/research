using System;
using System.Collections.Generic;
using System.IO;
using AG;

namespace AG
{
	public enum BEST {TOTAL_HEALTH, ALL_ALIVE, PRIMARY_PLAYER, ATTRITION}
    public enum STRATEGY {RANDOM_ACTION, LOWEST_HP_TARGET, HIGHEST_ATTACK_TARGET, THREAT_TARGET, SLEEP1}
    
    public class Utils
    {
		/**
		 *  Return null if no winning requested node exists
		 * 			
		 */
		public static GameNode GetBest(List<GameNode> graph, BEST bestType)
		{
			if (graph == null || graph.Count == 0)
				return null;
			GameNode best = graph [0];

			switch (bestType) {
				
			case BEST.ATTRITION:
			case BEST.TOTAL_HEALTH:
				int minNodeScore = int.MaxValue;
				foreach (GameNode node in graph) {			
					int val = Utils.GetHealthSum (node.enemies);
					if (minNodeScore > val) {
						// 1st - enemies' health
						minNodeScore = val;
						best = node;
					} else if (minNodeScore == val) {
						if (Utils.GetHealthSum (node.players) > Utils.GetHealthSum (best.players)) {
							// 2nd - players' health
							best = node;
						} else if (Utils.GetHealthSum (node.players) == Utils.GetHealthSum (best.players)) {
							if (node.getRounds () < best.getRounds ()) {
								// 3rd - # of rounds
								best = node;
							} 
						}
					}
				}
				if (best.getNodeState () != GAME_STATE.PLAYER_WIN) {
					best = null;
				}	
				break;
			case BEST.ALL_ALIVE:
				int maxPlayerTeamHealth = int.MinValue;	
				List<GameNode> allAliveNodes = new List<GameNode> ();	
				foreach (GameNode node in graph) {
					if (node.getNodeState() == GAME_STATE.PLAYER_WIN
					    && IsAllAlive(node.players)) {
						allAliveNodes.Add(node);
						if (GetHealthSum(node.players) > maxPlayerTeamHealth) {
							best = node;
							maxPlayerTeamHealth = GetHealthSum(node.players);
						}
					}
				}
				if (allAliveNodes.Count == 0) {
					best = null;
				}
				break;
			case BEST.PRIMARY_PLAYER:
				break;
			default:
				break;
			}
			return best;
		}

        public static void DoPlayerAction(GameNode state, int playerIndex, List<GameNode> childrenList)
		{
			if (state.getNodeState () != GAME_STATE.INPROCESS) {
				childrenList.Add(state);
			}
			if (GetHealthSum (state.players) == 0) {
				return;
			}
            if (playerIndex >= state.players.Count) {		
                childrenList.Add(state);
                return;
            }
			Character currentPlayer = state.players[playerIndex];
			if (currentPlayer.health <= 0) 
			{
				GameNode nextState = state.Copy();if (playerIndex == 0) RoundControl.RoundBegin(nextState);

				Action noAction =  Action.NoAction(nextState.players[playerIndex]);
				nextState.doAction(noAction);
				nextState.playersAction.Add(noAction);
				DoPlayerAction(nextState, playerIndex+1, childrenList);
                return;
            }
            for(int i = 0; i < state.enemies.Count; i++)
            {
				if ( state.enemies[i].health <= 0 ) continue;
				GameNode nextState = state.Copy(); if (playerIndex == 0) RoundControl.RoundBegin(nextState);

				Character source = nextState.players[playerIndex];
				Character target = nextState.enemies[i];     
				Action action = new Action(ACTION_TYPE.ATTACK, source, target);
                nextState.doAction(action);
				nextState.playersAction.Add(action);
                DoPlayerAction(nextState, playerIndex+1, childrenList);

				foreach(Magic magic in currentPlayer.magics) {
					if (magic.manaCost > currentPlayer.mana) continue;
					nextState = state.Copy (); if (playerIndex == 0) RoundControl.RoundBegin(nextState);
					source = nextState.players[playerIndex];
					target = nextState.enemies[i]; 
					action = new Action(ACTION_TYPE.MAGIC, magic, source, target);
					nextState.doAction(action);
					nextState.playersAction.Add(action);
					DoPlayerAction(nextState, playerIndex+1, childrenList);
				}
            }
			if (state.playersPotionLeft > 0) {
				GameNode nextState = state.Copy(); if (playerIndex == 0) RoundControl.RoundBegin(nextState);
				Character source = nextState.players[playerIndex];
				Character target = nextState.players[playerIndex];
				Action action = new Action(ACTION_TYPE.POTION, source, target);
				nextState.doAction(action);
				nextState.playersAction.Add(action);
				DoPlayerAction(nextState, playerIndex+1, childrenList);
			}
        }
	
        public static void AddNode(List<GameNode> graph, GameNode n)
        {
            // remove duplicates
            bool exist = false;
            foreach(GameNode node in graph)
            {
                if (node.isEqual(n))
                {
                    exist = true;
                }
            }
            if (!exist)
            {
                graph.Add(n);
            }        
        }
        
		public static bool IsAllAlive(List<Character> characterList) 
		{
			foreach (Character c in characterList) {
				if (c.health <= 0) {
					return false;
				}
			}
			return true;
		}

		/**
		 *	Assume from and to are winning node
		 */
		public static bool IsWinAsGoodAs(GameNode nodeA, GameNode nodeB, BEST comparator) 
		{
			if (nodeA.getNodeState () != GAME_STATE.PLAYER_WIN ||
				nodeB.getNodeState () != GAME_STATE.PLAYER_WIN) {
				return false;
			}

			bool result = false;
			switch (comparator) {
			case BEST.TOTAL_HEALTH:
				if (GetHealthSum(nodeA.players) == GetHealthSum(nodeB.players)
				    && nodeA.getRounds() == nodeB.getRounds()){
					result = true;
				}
				break;
			case BEST.ALL_ALIVE:
				if (IsAllAlive(nodeA.players) && IsAllAlive(nodeB.players)
				    && GetHealthSum(nodeA.players) == GetHealthSum(nodeB.players)
				    && nodeA.getRounds() == nodeB.getRounds()) {
					result = true;
				}
				break;
			case BEST.ATTRITION:
				result = true;
				break;
			case BEST.PRIMARY_PLAYER:
				break;
			default:
				break;
			}

			return result;
		}

        public static int GetHealthSum(List<Character> characterList)
        {
            int sum = 0;
            foreach (Character c in characterList)
            {
                sum += c.health;
            }
            return sum;
        }
		
		public static int GetMaxHealthSum(List<Character> characterList)
        {
            int sum = 0;
            foreach (Character c in characterList)
            {
                sum += c.maxHealth;
            }
            return sum;
        }


        

		/******************** OUTPUT ********************/
        public static void PrintToHTML(List<GameNode> graph, GameNode root) 
        {
            foreach(GameNode n in graph)
            {
                if (n.parent != null)
                {
                    n.parent.rrtChildren.Add(n);
                }
            }
			string lines = OutputGameState(root, 0);
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\data.json", FileMode.Create);
            StreamWriter file = new StreamWriter(fcreate);
            file.WriteLine(lines);
            file.Close();
        }
        
        private static string OutputGameState(GameNode node, int level)
        {
            string output = "\r\n" + Indent(level) + "{\r\n";
            
            level++;
            output += Indent(level) + "\"name\": \"" + node.ToString() + "\"";
            switch (node.nodeType)
            {
                case GameNode.NODE_TYPE.IN_EXPLORED:
                        
                    output += ",\r\n" + Indent(level) + "\"type\": " + "0";
                    break;
                
                case GameNode.NODE_TYPE.IN_RRT:
                    output += ",\r\n" + Indent(level) + "\"type\": " + "1";
                    break;    
                
                case GameNode.NODE_TYPE.IN_OPTIMAL:
                    output += ",\r\n" + Indent(level) + "\"type\": " + "2";
                    break;
                
                default:
                    output += ",\r\n" + Indent(level) + "\"type\": " + "0";
                    break;
                
            }
            
            // Children recursion
            if (node.rrtChildren.Count > 0 || node.exploredChildren.Count > 0)
            {
                output += ",\r\n" + Indent(level) + "\"children\": " + "[";
                if (node.rrtChildren.Count > 0) 
                {            
                    foreach (GameNode child in node.rrtChildren)
                    {
                        output += "\t" + OutputGameState(child, level) + ",";
                    }    
                }
                if (node.exploredChildren.Count > 0)
                {
                    foreach (GameNode child in node.exploredChildren)
                    {
                        if (child.nodeType == GameNode.NODE_TYPE.IN_EXPLORED) {
                            output += "\t" + OutputGameState(child, level) + ",";
                        }
                    }
                }
                output = output.Substring(0, output.Length - 1);
                output += "\r\n" + Indent(level) + "]";
            }
            output += "\r\n" + Indent(level-1) + "}";
            return output;
        }
        
            
        private static string Indent(int level)
        {
            string indent = "";
            for (int i = 0; i < level; i++) {
                indent += "\t";
            }
            return indent;
        }
        
        public static void PrintNodePath(GameNode n, int count, List<GameNode> path)
        {
            if (n.parent != null)
            {
                path.Add(n);
                PrintNodePath(n.parent, ++count, path);
            } else {
                path.Reverse();
                
                Console.WriteLine("Best Path:");
                Console.WriteLine(n.ToString());
                foreach(GameNode nn in path)
                {
                    Console.WriteLine("↓ " + nn.ToString());
                }            
                Console.WriteLine("Number of rounds: " + count);
            }
        }


		
    }
    
}