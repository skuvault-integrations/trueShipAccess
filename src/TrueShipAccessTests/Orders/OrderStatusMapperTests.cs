using FluentAssertions;
using NUnit.Framework;
using TrueShipAccess.Models;

namespace TrueShipAccessTests.Orders
{
	[ TestFixture ]
	public class OrderStatusMapperTests
	{
		[ Test ]
		public void ToBillingStatus()
		{
			var billingStatusStr = "pending";

			var billingStatus = billingStatusStr.ToBillingStatus();

			billingStatus.Should().Be( TrueShipOrderBillingStatusEnum.Pending );
		}

		[ Test ]
		public void ToShippingStatus()
		{
			var shippingStatusStr = OrderShippingStatusExtensions.ShippingStatusFulfilled;

			var shippingStatus = shippingStatusStr.ToShippingStatus();

			shippingStatus.Should().Be( TrueShipOrderShippingStatusEnum.Fulfilled );
		}
	}
}
