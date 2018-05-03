namespace IoCWinForm
{
	using System;
	using System.Windows.Forms;
	using IoCExampleSet.IoCExampleSetSupport;

	public partial class mfIoCWinForm : Form
	{
		private readonly IIoCExampleClass _Exampleclass;

		public mfIoCWinForm(IIoCExampleClass aExampleclass)
		{
			if (aExampleclass == null)
				throw new ArgumentNullException(nameof(aExampleclass));
			_Exampleclass = aExampleclass;
			InitializeComponent();
		}

		private void btnMain_Click(object sender, EventArgs e)
		{
			label1.Text = _Exampleclass.DoTheStuff();
		}

	}
}