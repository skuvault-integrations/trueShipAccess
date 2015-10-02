using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace TrueShipAccessTests.Items
{
	[ TestFixture ]
	public class ItemsTests : TestBase
	{
		[ Test ]
		public void CanGetAllItems()
		{
			//------------ Arrange
			var service = this._factory.CreateCommonService( this.Config );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var items = service.GetItems( ctSource.Token );
			items.Wait();

			//------------ Assert
			items.Result.Should().HaveCount( 6 );
		}
	}
}