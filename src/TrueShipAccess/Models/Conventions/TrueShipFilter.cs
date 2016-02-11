using System;

namespace TrueShipAccess.Models.Conventions
{
	public class TrueShipFilter
	{
		public readonly TrueShipField Field;
		public readonly TrueShipFilterRelation Relation;
		public readonly string Value;

		public TrueShipFilter( TrueShipField field, TrueShipFilterRelation relation, object value )
		{
			this.Field = field;
			this.Relation = relation;
			this.Value = value.ToString();
		}
	}

	internal class TrueShipFilterBuilder
	{
		private readonly TrueShipField Field;
		private TrueShipFilterRelation Relation;
		private string Value;

		public TrueShipFilterBuilder( TrueShipField field )
		{
			this.Field = field;
		}

		public TrueShipFilterBuilder GreaterThan( object value )
		{
			this.Relation = TrueShipFilterRelations.GreaterThan;
			this.Value = ( value is DateTime ) ? string.Format( "{0:s}", ( DateTime ) value ) : value.ToString();
			return this;
		}

		public TrueShipFilterBuilder LessThan( object value )
		{
			this.Relation = TrueShipFilterRelations.LessThan;
			this.Value = ( value is DateTime ) ? string.Format( "{0:s}", ( DateTime ) value ) : value.ToString();
			return this;
		}

		public static implicit operator TrueShipFilter( TrueShipFilterBuilder tb )
		{
			return new TrueShipFilter( tb.Field, tb.Relation, tb.Value );
		}
	}

	public class TrueShipFilterRelation
	{
		public readonly string RelationName;

		public TrueShipFilterRelation( string relationName )
		{
			this.RelationName = relationName;
		}
	}

	internal abstract class TrueShipFilterRelations
	{
		public static readonly TrueShipFilterRelation GreaterThan = new TrueShipFilterRelation( "__gte" );
		public static readonly TrueShipFilterRelation LessThan = new TrueShipFilterRelation( "__lte" );
	}
}
