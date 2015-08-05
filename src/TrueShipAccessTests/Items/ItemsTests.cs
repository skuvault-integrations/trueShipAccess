using FluentAssertions;
using NUnit.Framework;

namespace TrueShipAccessTests.Items
{
	[TestFixture]
	public class ItemsTests : TestBase
	{
		[Test]
		public void CanGetAllItems()
		{
			//------------ Arrange
			var service = this._factory.CreateService(this.Credentials);

			//------------ Act
			var items = service.GetItems();
			items.Wait();

			//------------ Assert
			items.Result.Should().HaveCount(6);

		}
	}
}