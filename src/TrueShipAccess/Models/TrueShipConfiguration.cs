using System;

namespace TrueShipAccess.Models
{
    public class TrueShipConfiguration
    {
        public string BEARERTOKEN { get; set; }
        public DateTime LASTORDERSYNC { get; set; }
        public DateTime LASTLOCATIONSYNC { get; set; }
        public int COMPANYID { get; set; }
    }
}
