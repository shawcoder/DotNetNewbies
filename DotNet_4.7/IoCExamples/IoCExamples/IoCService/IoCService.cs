namespace IoCService
{
	using System;
	using System.ServiceProcess;
	using System.Timers;
	using IoCExampleLibrary;

	public partial class IoCService: ServiceBase
	{
		private readonly IoCExampleClass _IoCExampleClass;
		private readonly Timer _Timer;

		public IoCService(IoCExampleClass aIoCExampleClass)
		{
			if (aIoCExampleClass == null)
			{
				throw new ArgumentNullException("aIoCExampleClass");
			}
			_IoCExampleClass = aIoCExampleClass;
			InitializeComponent();
			_Timer = new Timer(){Interval = 5000};
			_Timer.Elapsed += Timer_Elapsed;
			CanPauseAndContinue = true;
		}

		public void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void OnStart(string[] args)
		{
			_Timer.Enabled = true;
			base.OnStart(args);
		}

		protected override void OnPause()
		{
			_Timer.Enabled = false;
			base.OnPause();
		}

		protected override void OnContinue()
		{
			_Timer.Enabled = true;
			base.OnContinue();
		}

		protected override void OnStop()
		{
			_Timer.Enabled = false;
		}

	}
}
