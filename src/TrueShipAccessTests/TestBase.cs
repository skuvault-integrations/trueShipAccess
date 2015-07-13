using System;
using System.Linq;
using LINQtoCSV;
using NUnit.Framework;
using TrueShipAccess;
using TrueShipAccess.Models;
using TrueShipAccessTests.Orders;

namespace TrueShipAccessTests
{
	public class TestBase
	{
		internal ITrueShipFactory _factory;
		internal TrueShipConfiguration Config { get; set; }
		internal TrueShipCredentials Credentials { get; set; }

		[SetUp]
		public void Init()
		{
			const string credentialsFilePath = @"..\..\Files\TrueShipCredentials.csv";

			var cc = new CsvContext();
			var testConfig =
				cc.Read<TestConfig>(credentialsFilePath, new CsvFileDescription { FirstLineHasColumnNames = true, SeparatorChar = ';' }).FirstOrDefault();

			if (testConfig != null)
			{
				this.Config = new TrueShipConfiguration(DateTime.MinValue, DateTime.MinValue);
				this.Credentials = new TrueShipCredentials(testConfig.CompanyId, testConfig.AccessToken);

				this._factory = new TrueShipFactory(this.Config);
			}
		}
	}
}