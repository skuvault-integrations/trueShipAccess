using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LINQtoCSV;
using NUnit.Framework;
using TrueShipAccess;
using TrueShipAccess.Models;

namespace TrueShipAccessTests
{
	public class TestBase
	{
		internal ITrueShipFactory _factory;
		protected ITrueShipCommonService _commonService;
		internal TrueShipConfiguration Config { get; set; }
		public string OrganizationKey { get; private set; }
		internal TrueShipCredentials Credentials { get; set; }

		[ SetUp ]
		public void Init()
		{
			string basePath = new Uri( Path.GetDirectoryName( Assembly.GetExecutingAssembly().CodeBase ) ).LocalPath;
			const string credentialsFilePath = @"\..\..\credentials.csv";

			var cc = new CsvContext();
			var testConfig =
				cc.Read< TestConfig >( basePath + credentialsFilePath, new CsvFileDescription { FirstLineHasColumnNames = true, SeparatorChar = ',' } ).FirstOrDefault();

			if( testConfig != null )
			{
				this.Credentials = new TrueShipCredentials( testConfig.AccessToken );
				this.Config = new TrueShipConfiguration( this.Credentials );
				this.OrganizationKey = testConfig.OrganizationKey;

				this._factory = new TrueShipFactory( "" );
				this._commonService = this._factory.CreateCommonService( this.Config );
			}
		}
	}

	internal class TestConfig
	{
		public string AccessToken { get; set; }
		public string OrganizationKey { get; set; }
	}
}