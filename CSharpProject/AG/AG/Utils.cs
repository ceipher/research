using System;
using System.Collections.Generic;
using System.IO;
using AG;

namespace AG
{
    
    public enum STRATEGY {RANDOM_ACTION, LOWEST_HP_TARGET, HIGHEST_ATTACK_TARGET, THREAT_TARGET}
    
    public class Utils
    {
        public static void DoPlayerAction(GameState state, int playerIndex, List<GameState> childrenList)
		{
			if (state.getGameState () != GAME_STATE.INPROCESS) {
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
				GameState nextState = state.Copy();if (playerIndex == 0) GameControl.RoundBegin(nextState);

				Action noAction =  Action.NoAction(nextState.players[playerIndex]);
				nextState.doAction(noAction);
				nextState.playersAction.Add(noAction);
				DoPlayerAction(nextState, playerIndex+1, childrenList);
                return;
            }
            for(int i = 0; i < state.enemies.Count; i++)
            {
				if ( state.enemies[i].health <= 0 ) continue;
				GameState nextState = state.Copy(); if (playerIndex == 0) GameControl.RoundBegin(nextState);

				Character source = nextState.players[playerIndex];
				Character target = nextState.enemies[i];     
				Action action = new Action(ACTION_TYPE.ATTACK, source, target);
                nextState.doAction(action);
				nextState.playersAction.Add(action);
                DoPlayerAction(nextState, playerIndex+1, childrenList);

				foreach(Magic magic in currentPlayer.magics) {
					if (magic.manaCost > currentPlayer.mana) continue;
					nextState = state.Copy (); if (playerIndex == 0) GameControl.RoundBegin(nextState);
					source = nextState.players[playerIndex];
					target = nextState.enemies[i]; 
					action = new Action(ACTION_TYPE.MAGIC, magic, source, target);
					nextState.doAction(action);
					nextState.playersAction.Add(action);
					DoPlayerAction(nextState, playerIndex+1, childrenList);
				}
            }
			if (state.playersPotionLeft > 0) {
				GameState nextState = state.Copy(); if (playerIndex == 0) GameControl.RoundBegin(nextState);
				Character source = nextState.players[playerIndex];
				Character target = nextState.players[playerIndex];
				Action action = new Action(ACTION_TYPE.POTION, source, target);
				nextState.doAction(action);
				nextState.playersAction.Add(action);
				DoPlayerAction(nextState, playerIndex+1, childrenList);
			}


        }

		public static Action getPlayerStrategy(Character actionDealer, STRATEGY strategy, GameState currentState) 
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
            switch(strategy)
            {
                case STRATEGY.RANDOM_ACTION:            
                    Random rand = new Random();
					List<Action> actionsAvailable = currentState.getAvailableActions(actionDealer);
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
                        int threatValue = currentState.enemies[pp].attack * (getHealthSum(currentState.enemies) - currentState.enemies[pp].health);
                        if (threatValue > maxThreatValue)
                        {
                            target = currentState.enemies[pp];
                            maxThreatValue = threatValue;
                        }
                    }
                    action = new Action(ACTION_TYPE.ATTACK, actionDealer, target);
                    break;
            }
                
            return action;
        }
        
        public static Action getEnemyStrategy(Character actionDealer, STRATEGY strategy, GameState currentState) 
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
        
        public static void addNode(List<GameState> graph, GameState n)
        {
            // remove duplicates
            bool exist = false;
            foreach(GameState node in graph)
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
        public static void outputGraph(List<GameState> graph, GameState root) 
        {
            foreach(GameState n in graph)
            {
                if (n.parent != null)
                {
                    n.parent.rrtChildren.Add(n);
                }
            }
			string lines = outputGameState(root, 0);
			FileStream fcreate = File.Open("D:\\CSharpProject\\AG\\AG\\Visualization\\data.json", FileMode.Create);
            StreamWriter file = new StreamWriter(fcreate);
            file.WriteLine(lines);
            file.Close();
        }
        
        private static string outputGameState(GameState node, int level)
        {
            string output = "\r\n" + indent(level) + "{\r\n";
            
            level++;
            output += indent(level) + "\"name\": \"" + node.ToString() + "\"";
            switch (node.nodeType)
            {
                case GameState.NODE_TYPE.IN_EXPLORED:
                        
                    output += ",\r\n" + indent(level) + "\"type\": " + "0";
                    break;
                
                case GameState.NODE_TYPE.IN_RRT:
                    output += ",\r\n" + indent(level) + "\"type\": " + "1";
                    break;    
                
                case GameState.NODE_TYPE.IN_OPTIMAL:
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
                    foreach (GameState child in node.rrtChildren)
                    {
                        output += "\t" + outputGameState(child, level) + ",";
                    }    
                }
                if (node.exploredChildren.Count > 0)
                {
                    foreach (GameState child in node.exploredChildren)
                    {
                        if (child.nodeType == GameState.NODE_TYPE.IN_EXPLORED) {
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
        
        public static void printNodePath(GameState n, int count, List<GameState> path)
        {
            if (n.parent != null)
            {
                path.Add(n);
                printNodePath(n.parent, ++count, path);
            } else {
                path.Reverse();
                
                Console.WriteLine("Best Path:");
                Console.WriteLine(n.ToString());
                foreach(GameState nn in path)
                {
                    Console.WriteLine("↓ " + nn.ToString());
                }            
                Console.WriteLine("Number of rounds: " + count);
            }
        }
		
    }
    
}