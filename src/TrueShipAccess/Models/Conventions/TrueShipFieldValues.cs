namespace TrueShipAccess.Models.Conventions
{
	public class ExpandFieldValues
	{ 
		public string Value { get; private set; }

		public static readonly ExpandFieldValues BoxesItems = new ExpandFieldValues
		{
			Value = "boxes,items"
		};
	}

	public class ShippingStatusInFieldValues
	{
		public string Value { get; private set; }

		public static readonly ShippingStatusInFieldValues NotFulfilled = new ShippingStatusInFieldValues
		{
			Value = "!" + OrderShippingStatusExtensions.ShippingStatusFulfilled
		};
	}
}
