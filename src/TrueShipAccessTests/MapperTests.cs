using FluentAssertions;
using NUnit.Framework;
using System;
using TrueShipAccess.Models;

namespace TrueShipAccessTests
{
	[ TestFixture ]
	public class MapperTests
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

		[ Test ]
		public void ToUtcDateTime_ReturnsCorrectValue()
		{
			var year = 2020;
			int month = 3;
			int day = 21;
			int hour = 12;
			int minute = 45;
			int second = 55;
			var dateTimeStr = $"{year}-{month}-{day}T{hour}:{minute}:{second}.568137Z";

			var utcDateTime = dateTimeStr.ToUtcDateTime();

			utcDateTime.Year.Should().Be( year );
			utcDateTime.Month.Should().Be( month );
			utcDateTime.Day.Should().Be( day );
			utcDateTime.Hour.Should().Be( hour );
			utcDateTime.Minute.Should().Be( minute );
			utcDateTime.Second.Should().Be( second );
		}		

		[ Test ]
		public void ToUtcDateTime_HandlesNull()
		{
			string dateTimeStr = null;

			var utcDateTime = dateTimeStr.ToUtcDateTime();

			utcDateTime.Should().Be( DateTime.MinValue );
		}
	}
}
