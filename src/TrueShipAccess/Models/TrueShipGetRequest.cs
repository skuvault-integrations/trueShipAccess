using System;
using System.Net;
using Netco.Extensions;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public class TrueShipGetRequestBase : AbstractTrueShipRequest
	{
		public TrueShipGetRequestBase()
		{ }

		public TrueShipGetRequestBase( TrueShipApiEndpoint endpoint, string organizationKey ) 
			: base( endpoint, organizationKey )
		{ }

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

		public TrueShipGetRequestBase SetExpandField( ExpandFieldValues fieldValue )
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

		internal TrueShipGetRequestBase SetShippingStatusInField( ShippingStatusInFieldValues fieldValue )
		{
			this.SetField( TrueShipFields.Expand, fieldValue.Value );
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
		public TrueshipGetOrdersRequest( string organizationKey )
			: base( TrueShipApiEndpoints.Orders, organizationKey ) { }
	}

	internal class RequestCreatorService
	{
		private readonly string Token;

		public RequestCreatorService( string token )
		{
			this.Token = token;
		}

		public TrueShipGetRequestBase CreateGetOrdersRequest( string organizationKey, DateTime dateTime )
		{
			return new TrueshipGetOrdersRequest( organizationKey )
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).GreaterThan( dateTime ) );
		}

		public TrueShipGetRequestBase CreateGetBoxesRequest( string organizationKey )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.Boxes, organizationKey )
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems );
		}

		public TrueShipGetRequestBase CreateGetBoxesRequest( string organizationKey, int? orderId )
		{
			return this.CreateGetBoxesRequest( organizationKey )
				.SetField( TrueShipFields.OrderId, orderId.ToString() );
		}

		public TrueShipGetRequestBase CreateGetCompanyRequest( string organizationKey )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.Company, organizationKey )
				.SetBearerToken( this.Token );
		}

		public TrueShipGetRequestBase CreateGetItemsRequest( string organizationKey )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.Items, organizationKey )
				.SetBearerToken( this.Token );
		}

		public TrueShipGetRequestBase CreateGetOrderRequest( string organizationKey, int orderId )
		{
			return new TrueshipGetOrdersRequest( organizationKey )
				.SetBearerToken( this.Token )
				.SetField( TrueShipFields.PrimaryId, orderId.ToString() )
				.SetExpandField( ExpandFieldValues.BoxesItems );
		}

		public TrueShipGetRequestBase CreateGetOrdersRequest( string organizationKey, DateTime dateFrom, DateTime dateTo )
		{
			return new TrueshipGetOrdersRequest( organizationKey )
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).LessThan( dateTo ) )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).GreaterThan( dateFrom ) );
		}

		public TrueShipGetRequestBase CreateGetRemainingOrdersRequest( string organizationKey, int companyId )
		{
			return new TrueShipGetRequestBase( TrueShipApiEndpoints.RemainingOrders, organizationKey )
				.SetBearerToken( this.Token )
				.SetField( TrueShipFields.Id, companyId.ToString() );
		}

		public TrueShipGetRequestBase CreateGetUnshippedOrdersRequest( string organizationKey, DateTime dateTo )
		{
			return new TrueshipGetOrdersRequest( organizationKey )
				.SetBearerToken( this.Token )
				.SetExpandField( ExpandFieldValues.BoxesItems )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.ShippingStatusIn ).In( ShippingStatusInFieldValues.NotFulfilled.Value ) )
				.SetFilter( new TrueShipFilterBuilder( TrueShipFields.UpdateAt ).LessThan( dateTo ) ); 
		}

		public TrueShipPatchRequest CreateUpdatePickLocationRequest( ItemLocationUpdateModel updateModel  )
		{
			return ( ( TrueShipPatchRequest ) new TrueShipPatchRequest( updateModel.GetEndPoint() )
				.SetBearerToken( this.Token ) )
				.SetBody( updateModel.Location );
		}

		public TrueShipOrganizationsRequest CreateGetOrganizationsRequest()
		{
			return ( TrueShipOrganizationsRequest )new TrueShipOrganizationsRequest()
				.SetBearerToken( this.Token ) ;
		}
	}
}
