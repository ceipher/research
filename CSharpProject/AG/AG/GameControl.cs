using System;
namespace AG
{
	public class GameControl
	{
		public GameControl ()
		{
		}

		public static void RoundBegin(GameState state){
			foreach(Character c in state.players)
			{
				if (c.debuffLeft > 0) c.debuffLeft--;
				if (c.debuffLeft == 0) c.debuff = DEBUFF.NONE;
			}
			foreach(Character c in state.enemies)
			{
				if (c.debuffLeft > 0) {
					c.debuffLeft--;
				}
				if (c.debuffLeft == 0) c.debuff = DEBUFF.NONE;
			}
		}
	}
}

