using System.Collections.Generic;

namespace TrueShipAccess.Models
{
    public class CompanyResponse
    {
        public class Meta
        {
            public int Limit { get; set; }
            public string Next { get; set; }
            public int Offset { get; set; }
            public object Previous { get; set; }
            public int total_count { get; set; }
        }

        public class Company
        {
            public string company_id { get; set; }
            public string created_at { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string resource_uri { get; set; }
            public string updated_at { get; set; }
        }

        public class Companies : IEnumerable<Company>
        {
            public Meta Meta { get; set; }
            public List<Company> Objects { get; set; }

            public Companies()
            {
                this.Objects = new List<Company>();
            }

            public IEnumerator<Company> GetEnumerator()
            {
                return Objects.GetEnumerator();
            }
            
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}