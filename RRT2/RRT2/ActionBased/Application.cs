using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RRT2;

public static class Application
{

	static public void Main ()
	{		
		List<Character> players = Utils.generateDefaultPlayers();
		List<Character> enemies = Utils.generateDefaultEnemies();
		RRT.run (players, enemies);
	}
	
	
}
