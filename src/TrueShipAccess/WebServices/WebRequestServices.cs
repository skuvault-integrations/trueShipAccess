using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using NuGet;
using ServiceStack;
using ServiceStack.Text;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;
using HttpClient = System.Net.Http.HttpClient;

namespace TrueShipAccess.WebServices
{
	public sealed class WebRequestServices : IWebRequestServices
	{
		private readonly TrueShipConfiguration _config;
		private readonly TrueShipLogger _logservice = new TrueShipLogger();
		private readonly HttpClient _client = new HttpClient();

		public WebRequestServices( TrueShipConfiguration config )
		{
			Condition.Requires( config, "config" ).IsNotNull();
			this._config = config;
		}

		public HttpRequestMessage CreateUpdateOrderItemPickLocationRequest( KeyValuePair< string, PickLocation > oneorderitem )
		{
			var reSerializedOrder = JsonSerializer.SerializeToString( oneorderitem.Value );

			var putApi = new Uri( string.Format( "{0}/{1}?bearer_token={2}",
				this._config.ServiceBaseUri,
				oneorderitem.Key,
				this._config.Credentials.AccessToken ) );

			var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), putApi )
			{
				Content = new StringContent( reSerializedOrder, Encoding.UTF8, "application/json" )
			};

			return request;
		}

		private string GetRawResponse( Stream stream )
		{
			if ( stream == null ) return "";
			var responseString = stream.ReadToEnd();
//			stream.Seek( 0, SeekOrigin.Begin );
//			return "Raw response content: {0}".FormatWith( responseString );
			return responseString;
		}

		public async Task< HttpResponseMessage > SubmitPatch( TrueShipPatchRequestBase request, CancellationToken ct, string logPrefix )
		{
			HttpResponseMessage response = null;
			var httpRequest = request.ToHttpRequest();
			this._logservice.LogTrace( logPrefix, "Submitting PATCH request to {0} with body: {1}".FormatWith( httpRequest.RequestUri, request.GetSerializedBody() ) );

			await ActionPolicies.SubmitAsync.Do( async () =>
			{
				response = await this._client.SendAsync( httpRequest, ct );
			} );
			return response;
		}  

		public async Task< T > SubmitGet< T >( TrueShipGetRequestBase requestModel, CancellationToken ct, string logPrefix ) where T : class
		{
			var request = requestModel.ToHttpRequest();

			this._logservice.LogTrace( logPrefix, "Submitting GET request: {0}".FormatWith( request.RequestUri ) );

			HttpWebResponse response = null;
			await ActionPolicies.GetAsync.Do( async () =>
			{
				response = await GetWrappedAsyncResponse( request, ct );
			} );
			var stream = response.GetResponseStream();
			this._logservice.LogTrace( logPrefix, "Got response with status {0}. Raw response stream: {1}".FormatWith( response.StatusCode, "N/A" ) );
			return JsonSerializer.DeserializeFromStream< T >( stream );
		}

		public static Uri MakeAbsoluteUri( string path, string query )
		{
			return new Uri( string.Format( "{0}?{1}", path, query ) );
		}

		public async Task< T > SubmitGet< T >( Uri absoluteUri, CancellationToken ct, string logPrefix ) where T : class
		{
			var request = this.CreateHttpWebRequest( absoluteUri );

			var response = await GetWrappedAsyncResponse( request, ct );
			var stream = response.GetResponseStream();

			return JsonSerializer.DeserializeFromStream< T >( stream );
		}

		public T SubmitGetBlocking< T >( Uri uri, string logPrefix ) where T : class
		{
			var request = this.CreateHttpWebRequest( uri );
			this._logservice.LogTrace( logPrefix, "Submitting GET request: {0}".FormatWith( request.RequestUri ) );

			HttpWebResponse response = null;
			ActionPolicies.Get.Do( () =>
			{
				response = ( HttpWebResponse ) request.GetResponse();
			} );

			var stream = response.GetResponseStream();
//			var rawResponseString = this.GetRawResponse( stream );
			this._logservice.LogTrace( logPrefix, "Got response with status {0}. Raw response stream: {1}".FormatWith( response.StatusCode, "N/A" ) );
			return JsonSerializer.DeserializeFromStream<T>( stream );
		}

		public T SubmitGetBlocking< T >( TrueShipGetRequestBase trueShipRequest, string logPrefix ) where T : class
		{
			var request = trueShipRequest.ToHttpRequest();
			this._logservice.LogTrace( logPrefix, "Submitting GET request: {0}".FormatWith( request.RequestUri ) );

			HttpWebResponse response = null;
			ActionPolicies.Get.Do( () =>
			{
				response = ( HttpWebResponse )request.GetResponse();
			} );

			var stream = response.GetResponseStream();
			this._logservice.LogTrace( logPrefix, "Got response with status {0}. {1}".FormatWith( response.StatusCode, "N/A" ) );
			return JsonSerializer.DeserializeFromStream< T >( stream );
		}

		private static async Task< HttpWebResponse > GetWrappedAsyncResponse( HttpWebRequest request, CancellationToken ct )
		{
			using( ct.Register( request.Abort ) )
			{
				try
				{
					var response = await request.GetResponseAsync();
					ct.ThrowIfCancellationRequested();

					return ( HttpWebResponse )response;
				}
				catch( WebException ex )
				{
					if( ct.IsCancellationRequested )
						throw new OperationCanceledException( ex.Message, ex, ct );

					throw;
				}
			}
		}

		private HttpWebRequest CreateHttpWebRequest( Uri absoluteUri )
		{
			var request = ( HttpWebRequest ) WebRequest.Create( absoluteUri );
			request.Method = WebRequestMethods.Http.Get;
			request.ContentType = "application/json";

			return request;
		}
	}
}