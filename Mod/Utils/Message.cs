using MessagePack;

namespace HollowKnightAI.Utils
{
	[MessagePackObject]
	public class Reset
	{
		[Key(0)]
		public string SceneName { get; set; }
		[Key(1)]
		public int speed { get; set; }
		[Key(2)]
		public int KnightHealth { get; set; }
	}

	[MessagePackObject]
	public class Observation
	{
		[Key(0)]
		public byte[] obs { get; set; }

		public Observation(byte[] obs)
		{
			this.obs = obs;
		}
	}

	[MessagePackObject]
	public class Step
	{
		// left/right/none
		[Key(0)]
		public int LRN { get; set; }
		[Key(1)]
		// up/down/none
		public int UDN { get; set; }
		[Key(2)]
		// cast/attack/none
		public int CAN { get; set; }
		[Key(3)]
		// dash/none
		public int DN { get; set; }
		[Key(4)]
		// jump/none
		public int JN { get; set; }
	}

	[MessagePackObject]
	public class Reward
	{
		[Key(0)]
		public float reward { get; set; }
		[Key(1)]
		public bool done { get; set; }
		[Key(2)]
		public string info { get; set; }

		public Reward(float reward, bool done, string info)
		{
			this.reward = reward;
			this.done = done;
			this.info = info;
		}
	}

}