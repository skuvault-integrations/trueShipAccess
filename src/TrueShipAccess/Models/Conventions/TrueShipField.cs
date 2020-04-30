namespace TrueShipAccess.Models.Conventions
{
	public class TrueShipField
	{
		public readonly string FieldName;

		public TrueShipField( string fieldName )
		{
			this.FieldName = fieldName;
		}
	}

	abstract class TrueShipFields
	{
		public static readonly TrueShipField OrderId = new TrueShipField( "order" );
		public static readonly TrueShipField UpdateAt = new TrueShipField( "updated_at" );
		public static readonly TrueShipField PrimaryId = new TrueShipField( "primary_id" );
		public static readonly TrueShipField Id = new TrueShipField( "id" );
		public static readonly TrueShipField Format = new TrueShipField( "format" );
		public static readonly TrueShipField Limit = new TrueShipField( "limit" );
		public static readonly TrueShipField Offset = new TrueShipField( "offset" );
		public static readonly TrueShipField Token = new TrueShipField( "bearer_token" );
		public static readonly TrueShipField Expand = new TrueShipField( "expand" );
		public static readonly TrueShipField StatusShipped = new TrueShipField( "status_shipped" );
	} 
}
