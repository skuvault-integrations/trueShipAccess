namespace TrueShipAccess.Models
{
	public class TrueShipMoney
	{
		public decimal Amount { get; private set; }
		public string CurrencyCode { get; private set; }

		public TrueShipMoney( decimal amount, string currencyCode )
		{
			Amount = amount;
			CurrencyCode = currencyCode;
		}

		public static TrueShipMoney Zero()
		{
			return new TrueShipMoney( default( decimal ), string.Empty );
		}
	}

	public static class MoneyExtensions
	{
		public static TrueShipMoney ToTrueShipMoney( this string cost )
		{
			if( string.IsNullOrWhiteSpace( cost ) )
				return TrueShipMoney.Zero();
			var parts = cost.Split( ' ' );
			decimal amount;
			decimal.TryParse( parts[ 0 ], out amount );
			string currencyCode = "";
			if( parts.Length > 1 )
				currencyCode = parts[ 1 ];
			return new TrueShipMoney( amount, currencyCode );
		}
	}
}