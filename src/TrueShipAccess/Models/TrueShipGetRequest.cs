using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Netco.Extensions;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public class TrueShipGetRequestBase : AbstractTrueShipRequest
	{
		public TrueShipGetRequestBase( TrueShipApiEndpoint endpoint )
		{
			this.Endpoint = endpoint;
			this.UrlParams[ TrueShipFields.Format.FieldName ] = TrueShipConventions.DefaultFormat;
		}

		public TrueShipGetRequestBase SetLimit( int limit )
		{
			this.SetField( TrueShipFields.Limit, limit.ToString() );
			return this;
		}

		public TrueShipGetRequestBase SetOffset( int offset )
		{
			this.SetField( TrueShipFields.Offset, offset.ToString() );
			return this;
		}

		public TrueShipGetRequestBase SetBearerToken( string token )
		{
			this.SetField( TrueShipFields.Token, token );
			return this;
		}

		public TrueShipGetRequestBase SetExpandField( ExpandFieldValue fieldValue )
		{
			this.SetField( TrueShipFields.Expand, fieldValue.Value );
			return this;
		}

		public new TrueShipGetRequestBase SetField( TrueShipField field, string value )
		{
			base.SetField( field, value );
			return this;
		}

		public TrueShipGetRequestBase SetFilter( params TrueShipFilter[] filters )
		{
			filters.ForEach( filter =>
			{
				var key = "{0}{1}".FormatWith( filter.Field.FieldName, filter.Relation.RelationName );
				this.UrlParams[ key ] = filter.Value;
			} );
			return this;
		}

		public HttpWebRequest ToHttpRequest()
		{
			var requestUri = this.GetRequestUri();
			var request = ( HttpWebRequest )WebRequest.Create( requestUri );
			request.Method = WebRequestMethods.Http.Get;
			request.ContentType = "application/json";
//			request.Headers.Add( HttpRequestHeader.AcceptEncoding, "gzip" );
			return request;
		}

	}

	internal class TrueshipGetOrdersRequest : TrueShipGetRequestBase
	{
		public TrueshipGetOrdersRequest()
			: base( TrueShipApiEndpoints.Orders ) { }
	}

	internal class TrueshipGetBoxesRequest : TrueShipGetRequestBase
	{
		public TrueshipGetBoxesRequest()
			: base( TrueShipApiEndpoints.Boxes ) { }
	}

	internal class TrueshipGetCompanyRequest : TrueShipGetRequestBase
	{
		public TrueshipGetCompanyRequest()
			: base( TrueShipApiEndpoints.Company ) { }
	}

	internal class TrueshipGetItemsRequest : TrueShipGetRequestBase
	{
		public TrueshipGetItemsRequest()
			: base( TrueShipApiEndpoints.Items ) { }
	}

	internal class RequestCreatorService
	{
		private readonly string Token;

		public RequestCreatorService( string token )
		{
			this.Token = token;
		}

		public TrueShipGetRequestBase CreateGetOrdersRequest( DateTime dateTime )
		{
			return new TrueshipGetOrdersRequest()
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).GreaterThan( dateTime ) );
		}

		public TrueShipGetRequestBase CreateGetBoxesRequest()
		{
			return new TrueshipGetBoxesRequest()
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems );
		}

		public TrueShipGetRequestBase CreateGetBoxesRequest( int? orderId )
		{
			return this.CreateGetBoxesRequest()
				.SetField( TrueShipFields.OrderId, orderId.ToString() );
		}

		public TrueShipGetRequestBase CreateGetCompanyRequest()
		{
			return new TrueshipGetCompanyRequest()
				.SetBearerToken( this.Token );
		}

		public TrueShipGetRequestBase CreateGetItemsRequest()
		{
			return new TrueshipGetItemsRequest()
				.SetBearerToken( this.Token );
		}

		public TrueShipGetRequestBase CreateGetOrderRequest( int orderId )
		{
			return new TrueshipGetOrdersRequest()
				.SetBearerToken( this.Token )
				.SetField( TrueShipFields.PrimaryId, orderId.ToString() )
				.SetExpandField( ExpandFieldValues.BoxesItems );
		}

		public TrueShipGetRequestBase CreateGetOrdersRequest( DateTime dateFrom, DateTime dateTo )
		{
			return new TrueshipGetOrdersRequest()
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).LessThan( dateTo ) )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).GreaterThan( dateFrom ) );
		}

		public TrueShipGetRequestBase CreateGetRemainingOrdersRequest( int companyId )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.RemainingOrders )
				.SetBearerToken( this.Token )
				.SetField( TrueShipFields.Id, companyId.ToString() );
		}

		public TrueShipGetRequestBase CreateGetUnshippedOrdersRequest( DateTime dateTo )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.Orders )
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetField( TrueShipFields.StatusShipped, "false" )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).LessThan( dateTo ) ); 
		}

		public TrueShipPatchRequestBase CreateUpdatePickLocationRequest( ItemLocationUpdateModel updateModel  )
		{

			return new TrueShipPatchRequestBase( updateModel.GetEndPoint() )
				.SetBearerToken( this.Token )
				.SetBody( updateModel.Location );
		}
	}
}
