using FluentAssertions;
using NUnit.Framework;
using TrueShipAccess.Models;

namespace TrueShipAccessTests
{
	[ TestFixture ]
	public class MoneyMapperTests
	{
		[ Test ]
		public void ToTrueShipMoney_ReturnsCorrectValue()
		{
			var amount = 10.20m;
			var currencyCode = "USD";
			var moneyFromApi = $"{amount} {currencyCode}";

			var money = moneyFromApi.ToTrueShipMoney();

			money.Amount.Should().Be( amount );
			money.CurrencyCode.Should().Be( currencyCode );
		}

		[ Test ]
		public void ToTrueShipMoney_HandlesNull()
		{ 
			string moneyFromApi = null;	

			var money = moneyFromApi.ToTrueShipMoney();

			money.Amount.Should().Be( default( decimal ) );
			money.CurrencyCode.Should().Be( string.Empty );
		}
	}
}
