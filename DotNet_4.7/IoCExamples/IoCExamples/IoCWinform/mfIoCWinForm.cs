namespace IoCWinform
{
	using System.Windows.Forms;
	using AWEFramework.IoC;
	using IoCExampleLibrary;

// ReSharper disable InconsistentNaming
	public partial class mfIoCWinForm: Form
// ReSharper restore InconsistentNaming
	{
		public mfIoCWinForm(IIoCExampleClass aIoCExampleClass)
		{
			InitializeComponent();
		}

#if DEBUG
		public mfIoCWinForm()
			: this(InstanceFactory.GetInstance<IoCExampleClass>())
		{
		}
#endif

	}
}
