using System;
using CuttingEdge.Conditions;

namespace TrueShipAccess.Models
{
    public class TrueShipConfiguration
    {
        public TrueShipConfiguration(string bearerToken, DateTime lastOrderSync, DateTime lastLocationSync, int companyId)
        {
            Condition.Requires(bearerToken, "bearertoken").IsNotNullOrWhiteSpace();

            BearerToken = bearerToken;
            LastOrderSync = lastOrderSync;
            LastLocationSync = lastLocationSync;
            CompanyId = companyId;
        }

        public string BearerToken { get; private set; }
        public DateTime LastOrderSync { get; set; }
        public DateTime LastLocationSync { get; set; }
        public int CompanyId { get; private set; }
    }
}
