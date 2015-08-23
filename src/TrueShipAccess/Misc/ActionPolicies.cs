using System;
using System.Threading.Tasks;
using Netco.ActionPolicyServices;
using Netco.Utils;

namespace TrueShipAccess.Misc
{
	public static class ActionPolicies
	{
		private static readonly ActionPolicyAsync _trueShipGetAsyncPolicy = ActionPolicyAsync.Handle< Exception >()
			.RetryAsync( 10, async ( ex, i ) =>
			{
				TrueShipLogger.Log().Trace( ex, "Retrying TrueShip API get call for the {0} time", i );
				await Task.Delay( TimeSpan.FromSeconds( 0.5 + i ) ).ConfigureAwait( false );
			} );

		private static readonly ActionPolicyAsync _trueShipGetAsyncPolicyShort = ActionPolicyAsync.Handle< Exception >()
			.RetryAsync( 3, async ( ex, i ) =>
			{
				TrueShipLogger.Log().Trace( ex, "Retrying TrueShip API get call for the {0} time", i );
				await Task.Delay( TimeSpan.FromSeconds( 0.5 + i ) ).ConfigureAwait( false );
			} );

		private static readonly ActionPolicy _trueShipGetPolicy = ActionPolicy.Handle< Exception >().Retry( 10, ( ex, i ) =>
		{
			TrueShipLogger.Log().Trace( ex, "Retrying TrueShip API get call for the {0} time", i );
			SystemUtil.Sleep( TimeSpan.FromSeconds( 0.5 + i ) );
		} );

		private static readonly ActionPolicyAsync _trueShipSumbitAsyncPolicy = ActionPolicyAsync.Handle< Exception >()
			.RetryAsync( 10, async ( ex, i ) =>
			{
				TrueShipLogger.Log().Trace( ex, "Retrying TrueShip API submit call for the {0} time", i );
				await Task.Delay( TimeSpan.FromSeconds( 0.5 + i ) ).ConfigureAwait( false );
			} );

		private static readonly ActionPolicy _trueShipSumbitPolicy = ActionPolicy.Handle< Exception >().Retry( 10, ( ex, i ) =>
		{
			TrueShipLogger.Log().Trace( ex, "Retrying TrueShip API submit call for the {0} time", i );
			SystemUtil.Sleep( TimeSpan.FromSeconds( 0.5 + i ) );
		} );

		public static ActionPolicy Get
		{
			get { return _trueShipGetPolicy; }
		}

		public static ActionPolicyAsync GetAsync
		{
			get { return _trueShipGetAsyncPolicy; }
		}

		public static ActionPolicyAsync GetAsyncShort
		{
			get { return _trueShipGetAsyncPolicyShort; }
		}

		public static ActionPolicy Submit
		{
			get { return _trueShipSumbitPolicy; }
		}

		public static ActionPolicyAsync SubmitAsync
		{
			get { return _trueShipSumbitAsyncPolicy; }
		}
	}
}