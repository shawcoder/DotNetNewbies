namespace SpyStoreDal.SpyStore.DAL.EF
{
	using System;
	using Microsoft.EntityFrameworkCore.Storage;

	public class CustomExecutionStrategy : ExecutionStrategy
	{
		public CustomExecutionStrategy(ExecutionStrategyContext aContext)
			: base(aContext, DefaultMaxRetryCount, DefaultMaxDelay)
		{

		}


		public CustomExecutionStrategy
		(
			ExecutionStrategyContext aContext
			, int aMaxRetryCount
			, TimeSpan aMaxRetryDelay
		) : base(aContext, aMaxRetryCount, aMaxRetryDelay)
		{
		}

		protected override bool ShouldRetryOn(Exception exception) { return true; }
	}
}
