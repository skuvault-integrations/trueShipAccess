namespace TrueShipAccess.Models.Conventions
{
	public class ExpandFieldValue
	{ 
		public readonly string Value;

		public ExpandFieldValue( string value )
		{
			this.Value = value;
		}
	}

	public static class ExpandFieldValues
	{
		public static readonly ExpandFieldValue BoxesItems = new ExpandFieldValue( "boxes,items"  );
	}
}
