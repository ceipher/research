using System;

namespace AG
{
	
	public enum ACTION_TYPE {ATTACK, POTION, NONE};
	
	public class Action
	{
		public ACTION_TYPE type;
		public Character source;
		public Character target;
		
		public Action (ACTION_TYPE actionType, Character actionSource, Character actionTarget)
		{
			this.type = actionType;
			this.source = actionSource;
			this.target = actionTarget;
		}
		
		public override string ToString ()
		{
			if (type == ACTION_TYPE.NONE) 
			{
				return source.name + " does NOTHING";
			} else {
				return source.name + " does "+ type.ToString() + " on " + target.name;
			}
		}
		
		
	}
	
}

