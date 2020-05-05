using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Netco.Logging;

namespace TrueShipAccessTests.Orders
{
	[ TestFixture ]
	public class OrdersTests : TestBase
	{
		[ Test ]
		public void GetOrders()
		{
			//------------ Arrange
			var service = this._factory.CreateCommonService( this.Config );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var orders = service.GetOrdersAsync( DateTime.UtcNow.AddDays(-90), DateTime.UtcNow, ctSource.Token, Mark.Blank() );
			orders.Wait();

			//------------ Assert
			orders.Should().NotBeNull();
			orders.Result.Should().NotBeEmpty();
		}

		[ Test ]
		public async Task GetUnshippedOrdersAsync()
		{
			var service = this._factory.CreateCommonService( this.Config );

			var orders = await service.GetUnshippedOrdersAsync( DateTime.UtcNow, CancellationToken.None, Mark.Blank() );

			orders.Should().NotBeEmpty();
		}
	}
}