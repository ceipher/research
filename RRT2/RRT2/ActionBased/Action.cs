using System;

namespace RRT2
{
	
	public enum ACTION_TYPE {ATTACK, POTION, NONE};
	
	public class Action
	{
		public ACTION_TYPE type;
		public int target;
		
		public Action (ACTION_TYPE type, int target)
		{
			this.type = type;
			this.target = target;
		}
	}
	
}

