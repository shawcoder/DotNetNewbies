namespace IoCUnitTest
{
	using System;
	using Moq;
	using NUnit.Framework;
	using Ninject;
	using Ninject.MockingKernel.Moq;

	public interface IMockInstanceFactory
	{
		T GetActual<T>();
		Mock<T> GetMock<T>
			(MockBehavior aMockBehavior = MockBehavior.Strict) where T: class;
		void VerifyAll();
	}

	public class MockInstanceFactory: IMockInstanceFactory
	{
		private readonly MoqMockingKernel _Kernel;

		public const MockBehavior DefaultMockBehavior = MockBehavior.Strict;

		public MockInstanceFactory(MoqMockingKernel aKernel)
		{
			if (aKernel == null)
			{
				throw new ArgumentNullException("aKernel");
			}
			_Kernel = aKernel;
		}

		/// <summary>
		/// Return an instance of the specified class, do NOT create a mocked
		/// version that needs one or more calls to Setup.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetActual<T>()
		{
			return _Kernel.Get<T>();
		}

		/// <summary>
		/// Return an instance of a class that has been converted to a mock class.
		/// This variant allows calls to the Setup method such that the methods
		/// can be subonred into returning a desired result for testing purposes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="aMockBehavior"></param>
		/// <returns></returns>
		public Mock<T> GetMock<T>
			(MockBehavior aMockBehavior = DefaultMockBehavior) where T: class
		{
			Type vType = typeof(T);
			// If you're asking for a mock, it better be an inteface!
			Assert.That(vType.IsInterface, Is.True);
			if (aMockBehavior == MockBehavior.Strict)
			{
				return _Kernel.GetMock<T>();
			}
			// FRAGILE! VerifyAll() won't verify this mock!
			Mock<T> vInstance = new Mock<T>(aMockBehavior);
			_Kernel.Rebind<T>().ToConstant(vInstance.Object);
			return new Mock<T>(aMockBehavior);
		}

		public void VerifyAll()
		{
			_Kernel.MockRepository.VerifyAll();
		}

	}
}
