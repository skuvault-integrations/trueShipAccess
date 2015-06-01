using System.Collections.Generic;

namespace TrueShipAccess.Models
{
    public class CompaniesResource
    {
        public class Meta
        {
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total_count { get; set; }
        }

        public class Object
        {
            public string company_id { get; set; }
            public string created_at { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string resource_uri { get; set; }
            public string updated_at { get; set; }
        }

        public class Companies : IEnumerable<Object>
        {
            public Meta meta { get; set; }
            public List<Object> objects { get; set; }

            public Companies()
            {
                this.objects = new List<Object>();
            }

            public IEnumerator<Object> GetEnumerator()
            {
                return objects.GetEnumerator();
            }
            
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

    }
}