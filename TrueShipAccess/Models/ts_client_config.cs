using System;

namespace TrueShipConfiguration
{
    public class tsConfiguration
    {
        public string BEARERTOKEN { get; set; }
        public DateTime LASTORDERSYNC { get; set; }
        public DateTime LASTLOCATIONSYNC { get; set; }
        public int COMPANYID { get; set; }
    }
}
