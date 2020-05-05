using FluentAssertions;
using NUnit.Framework;
using TrueShipAccess.Models;

namespace TrueShipAccessTests
{
	[ TestFixture ]
	public class OrderStatusMapperTests
	{
		[ Test ]
		public void ToBillingStatus()
		{
			var billingStatusStr = "pending";

			var billingStatus = billingStatusStr.ToBillingStatus();

			billingStatus.Should().Be( OrderBillingStatusEnum.Pending );
		}

		[ Test ]
		public void ToShippingStatus()
		{
			var shippingStatusStr = "fulfilled";

			var shippingStatus = shippingStatusStr.ToShippingStatus();

			shippingStatus.Should().Be( OrderShippingStatusEnum.Fulfilled );
		}
	}
}
