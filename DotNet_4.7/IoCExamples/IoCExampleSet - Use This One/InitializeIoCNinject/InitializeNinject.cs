// ReSharper disable PossibleNullReferenceException
namespace IoCExampleSet.InitializeIoCNinject
{
	using System;
	using System.IO;
	using System.Reflection;
	using IoC;
	using Ninject;
	using Ninject.Extensions.Conventions;

	public static class InitializeNinject
	{
		public static IKernel NewKernel()
		{
			return new StandardKernel();
		}

		public static void RegisterServices(IKernel aKernel)
		{
			string vDirPath =
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			if (vDirPath != null)
			{
				string vPath = new Uri(vDirPath).LocalPath.Normalize();
				aKernel.Bind
					(
					 aScanResult =>
					 aScanResult
						 .FromAssembliesInPath(vPath)
						 .SelectAllClasses()
						 .BindDefaultInterface()
					);
			}
		}

		public static void RegisterServices(IKernel aKernel, Type aType)
		{
			string vDirPath =
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			if (vDirPath != null)
			{
				string vPath = new Uri(vDirPath).LocalPath.Normalize();
				string vApplicationNamespace =
					aType.FullName.Split
						(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
				string vThisNamespace =
					typeof(InitializeNinject).FullName.Split
						(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
				aKernel.Bind
					(
						aScanResult =>
							aScanResult
								.FromAssembliesInPath(vPath)
								.Select
									(
										aClass =>
											aClass.IsClass
												&& !aClass.IsAbstract
												&&
													(
														aClass.FullName.StartsWith(vApplicationNamespace)
															|| aClass.FullName.StartsWith(vThisNamespace)
													)
									).BindDefaultInterface()
					);
			}
		}

		public static void StartUp()
		{
			StartUpWithExternalKernel(NewKernel());
		}

		public static void StartUp(Type aType)
		{
			StartUpWithExternalKernel(NewKernel(), aType);
		}

		public static IKernel StartupAndReturnKernel(Type aType)
		{
			IKernel vKernel = NewKernel();
			StartUpWithExternalKernel(vKernel, aType);
			return vKernel;
		}

		public static void StartUpWithExternalKernel(IKernel aKernel)
		{
			RegisterServices(aKernel);
			InstanceFactory.InstanceGenerator =
				(aKernel as IServiceProvider).GetService;
		}

		public static void StartUpWithExternalKernel(IKernel aKernel, Type aType)
		{
			RegisterServices(aKernel, aType);
			InstanceFactory.InstanceGenerator =
				(aKernel as IServiceProvider).GetService;
		}

	}
}
