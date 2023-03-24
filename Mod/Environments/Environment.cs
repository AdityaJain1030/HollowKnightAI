namespace HollowKnightAI.Environments
{
	abstract class Environment
	{
		public abstract void Setup();
		public abstract void Update();
		public abstract void Cleanup();
	}
}