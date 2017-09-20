namespace GenericsDemo
{
	/// <summary>
	/// In order to store a list of "stuff", you need to do something like the
	/// following (pretending that all the .NET goodness doesn't exist, of course)
	/// </summary>
	public class TheHardWay
	{
		private object[] _Objects;

		public TheHardWay()
		{
			_Objects = new object[0];	
		}

		public void Add(object aObject)
		{
			object[] vNewStorage = new object[_Objects.Length + 1];
			for (int vLcv = 0; vLcv < _Objects.Length; vLcv++)
			{
				vNewStorage[vLcv] = _Objects[vLcv];
			}
			vNewStorage[vNewStorage.Length - 1] = aObject;
			_Objects = vNewStorage;
		}

		public object GetByIndex(int aIndex)
		{
			if ((aIndex < _Objects.Length) && (aIndex >= 0))
			{
				return _Objects[aIndex];
			}
			return null;
		}

		public int HowMany => _Objects.Length;
	}
}
