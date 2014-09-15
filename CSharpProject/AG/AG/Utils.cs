using System;
using System.Collections.Generic;
using System.IO;
using AG;

namespace AG
{
    
    public enum STRATEGY {RANDOM_ACTION, LOWEST_HP_TARGET, HIGHEST_ATTACK_TARGET, THREAT_TARGET}
    
    public class Utils
    {
		public static GameNode GetBest(List<GameNode> graph)
		{
			if (graph == null || graph.Count == 0)
				return null;

			double minNodeScore = double.MaxValue;
			GameNode best = graph[0];
			foreach(GameNode nn in graph)
			{			
				int val = Utils.getHealthSum(nn.enemies);
				if (minNodeScore > val) {
					// 1st - enemies' health
					minNodeScore = val;
					best = nn;
				} else if (minNodeScore == val)	{
					if (Utils.getHealthSum(nn.players) > Utils.getHealthSum(best.players)) {
						// 2nd - players' health
						best = nn;
					} else  if (Utils.getHealthSum(nn.players) == Utils.getHealthSum(best.players)) {
						if (nn.getRounds() < best.getRounds()) {
							// 3rd - # of rounds
							best = nn;
						} 
					}
				}
			}
			return best;
		}

        public static void DoPlayerAction(GameNode state, int playerIndex, List<GameNode> childrenList)
		{
			if (state.getNodeState () != GAME_STATE.INPROCESS) {
				childrenList.Add(state);
			}
			if (getHealthSum (state.players) == 0) {
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
	
        public static void addNode(List<GameNode> graph, GameNode n)
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
        
        public static int getHealthSum(List<Character> listOfCharacters)
        {
            int sum = 0;
            foreach (Character c in listOfCharacters)
            {
                sum += c.health;
            }
            return sum;
        }
		
		public static int getMaxHealthSum(List<Character> listOfCharacters)
        {
            int sum = 0;
            foreach (Character c in listOfCharacters)
            {
                sum += c.maxHealth;
            }
            return sum;
        }


        

		/******************** OUTPUT ********************/
        public static void printToHTML(List<GameNode> graph, GameNode root) 
        {
            foreach(GameNode n in graph)
            {
                if (n.parent != null)
                {
                    n.parent.rrtChildren.Add(n);
                }
            }
			string lines = outputGameState(root, 0);
			FileStream fcreate = File.Open("D:\\CSharpProject\\Visualization\\data.json", FileMode.Create);
            StreamWriter file = new StreamWriter(fcreate);
            file.WriteLine(lines);
            file.Close();
        }
        
        private static string outputGameState(GameNode node, int level)
        {
            string output = "\r\n" + indent(level) + "{\r\n";
            
            level++;
            output += indent(level) + "\"name\": \"" + node.ToString() + "\"";
            switch (node.nodeType)
            {
                case GameNode.NODE_TYPE.IN_EXPLORED:
                        
                    output += ",\r\n" + indent(level) + "\"type\": " + "0";
                    break;
                
                case GameNode.NODE_TYPE.IN_RRT:
                    output += ",\r\n" + indent(level) + "\"type\": " + "1";
                    break;    
                
                case GameNode.NODE_TYPE.IN_OPTIMAL:
                    output += ",\r\n" + indent(level) + "\"type\": " + "2";
                    break;
                
                default:
                    output += ",\r\n" + indent(level) + "\"type\": " + "0";
                    break;
                
            }
            
            // Children recursion
            if (node.rrtChildren.Count > 0 || node.exploredChildren.Count > 0)
            {
                output += ",\r\n" + indent(level) + "\"children\": " + "[";
                if (node.rrtChildren.Count > 0) 
                {            
                    foreach (GameNode child in node.rrtChildren)
                    {
                        output += "\t" + outputGameState(child, level) + ",";
                    }    
                }
                if (node.exploredChildren.Count > 0)
                {
                    foreach (GameNode child in node.exploredChildren)
                    {
                        if (child.nodeType == GameNode.NODE_TYPE.IN_EXPLORED) {
                            output += "\t" + outputGameState(child, level) + ",";
                        }
                    }
                }
                output = output.Substring(0, output.Length - 1);
                output += "\r\n" + indent(level) + "]";
            }
            output += "\r\n" + indent(level-1) + "}";
            return output;
        }
        
            
        private static string indent(int level)
        {
            string indent = "";
            for (int i = 0; i < level; i++) {
                indent += "\t";
            }
            return indent;
        }
        
        public static void printNodePath(GameNode n, int count, List<GameNode> path)
        {
            if (n.parent != null)
            {
                path.Add(n);
                printNodePath(n.parent, ++count, path);
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