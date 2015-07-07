using System.Collections.Generic;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// Retrieve a list of companies.
	/// https://www.readycloud.com/static/api-doc/apireference.html#company-resource
	/// </summary>
    public class CompanyResource
    {
        public class Company
        {
            public string company_id { get; set; }
            public string created_at { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string resource_uri { get; set; }
            public string updated_at { get; set; }
        }

        public class Response : IEnumerable<Company>
        {
            public Meta Meta { get; set; }
            public List<Company> Objects { get; set; }

            public Response()
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