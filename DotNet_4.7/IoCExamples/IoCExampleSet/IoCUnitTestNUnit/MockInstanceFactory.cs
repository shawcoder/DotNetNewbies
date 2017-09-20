namespace IoCUnitTestNUnit
{
	using System;
	using FluentAssertions;
	using Moq;
	using Ninject;
	using Ninject.MockingKernel.Moq;

	public interface IMockInstanceFactory
	{
		/// <summary>
		/// Return an instance of the specified class, do NOT create a mocked
		/// version that needs one or more calls to Setup.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetActual<T>();

		/// <summary>
		/// Return an instance of a class that has been converted to a mock class.
		/// This variant allows calls to the Setup method such that the methods
		/// can be subonred into returning a desired result for testing purposes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="aMockBehavior"></param>
		/// <returns></returns>
		Mock<T> GetMock<T>
			(MockBehavior aMockBehavior = MockInstanceFactory.DEFAULT_MOCK_BEHAVIOR)
				where T : class;

		void VerifyAll();
	}

	public class MockInstanceFactory: IMockInstanceFactory
	{
		private readonly MoqMockingKernel _Kernel;

		public const MockBehavior DEFAULT_MOCK_BEHAVIOR = MockBehavior.Strict;

		public MockInstanceFactory(MoqMockingKernel aKernel)
		{
			if (aKernel == null)
			{
				throw new ArgumentNullException(nameof(aKernel));
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
			(MockBehavior aMockBehavior = DEFAULT_MOCK_BEHAVIOR) where T : class
		{
			Type vType = typeof(T);
			// If you're asking for a mock, it better be an inteface!
			vType.IsInterface.Should().Be(true);
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
