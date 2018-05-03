namespace NuGetHandler.Status
{
	/// <summary>
	/// Basically, this class is used to determine how far along the program is
	/// and what it was able to do during start-up so that the Help system can
	/// display the correct information e.g. If a ProjectPath was not supplied,
	/// the status will be false and the corresponding message will be "No Project
	/// supplied", otherwise the projects information will be displayed e.g.
	/// project type, version, etc.
	/// </summary>
	public static class StatusIndicator
	{
		public static bool ProjectFileProcessed { get; set; }

	}
}
