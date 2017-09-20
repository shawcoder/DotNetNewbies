namespace AbstractFactory
{
	public class CarBody: IBody
	{
		public virtual string BodyParts => "Body shell parts for a car";

		/* The above is the same as this:
		public virtual strihg BodyParts
		{
			get
			{
				return "Body shell parts for a car".
			}	
		}
		*/
	}
}
