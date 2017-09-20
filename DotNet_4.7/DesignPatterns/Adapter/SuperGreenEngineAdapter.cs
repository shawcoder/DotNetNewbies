namespace Adapter
{
	using System;
	using Common;

	/// <summary>
	/// The adapter class contains only enough code to represent the foreign
	/// class appropriately.
	/// </summary>
	/// <remarks>
	/// The general formula to apply for creating adapters is this:
	///		1.	Extend the class you are adapting to (or implement it, if it's an 
	///				interface).
	///		2.	Specify the class you are adpating from in the constructor and store
	///				a reference to it in the adapter as a reference variable.
	///		2a.	Notice that this is a Bad Place for dependency injection UNLESS an
	///				instance of the "foreign" class is acceptable in it's default form.
	///				Usually, this isn't the case.
	///		3.	For each method in the foreign class you are extending (or interface
	///				you are implementing), override it to delegate to the corresponding
	///				method of the class you are adapting from (the "foreign" class).
	/// </remarks>
	public class SuperGreenEngineAdapter: AbstractEngine
	{
		private readonly SuperGreenEngine _SuperGreenEngine;

		public SuperGreenEngineAdapter(SuperGreenEngine aSuperGreenEngine)
			: base(aSuperGreenEngine.EngineSize, false)
		{
			if (aSuperGreenEngine == null)
				throw new ArgumentNullException(nameof(aSuperGreenEngine));
			_SuperGreenEngine = aSuperGreenEngine;

			// Other stuff that may be necessary. For now, nothing else need be done.			
		}

	}
}
