namespace IoCExampleSet.IoC
{
	using System;

	public static class InstanceFactory
	{
		public static Func<Type, object> InstanceGenerator { get; set; }

		public static object GetInstance(Type aType)
		{
			if (InstanceGenerator == null)
			{
				throw new NullReferenceException
					("InstanceFactory (ServiceLocator) has not been initialized");
			}
			return InstanceGenerator(aType);
		}

		public static T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

	}
}
