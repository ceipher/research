using System;

namespace RRT2
{
	
	public enum ACTION_TYPE {ATTACK, POTION, NONE};
	
	public class Action
	{
		public ACTION_TYPE type;
		public Character target;
		
		public Action (ACTION_TYPE actionType, Character actionTarget)
		{
			this.type = actionType;
			this.target = actionTarget;
		}
	}
	
}

