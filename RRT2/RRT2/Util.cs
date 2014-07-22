using System;
using System.Collections.Generic;
using System.Linq;

namespace RRT2
{
	public class Util
	{
		public Util ()
		{
		}
		
		public static List<Character> generateDefaultPlayers()
		{
			List<Character> players = new List<Character>();
			players.Add (new Character(100,4));
			players.Add (new Character(100,8));
			players.Add (new Character(100,5));
			return players;
		}
		
		public static List<Character> generateDefaultEnemies()
		{
			List<Character> enemies = new List<Character>();
			enemies.Add (new Character(14,4));
			enemies.Add (new Character(15,8));
			enemies.Add (new Character(30,9));
			return enemies;
		}
	}
}

