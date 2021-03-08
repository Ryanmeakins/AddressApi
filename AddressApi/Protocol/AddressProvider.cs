using AddressApi.Controllers;
using AddressApi.Schemas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AddressApi.Protocol
{
    public class AddressProvider
    {
        private readonly AddressContext context;
        private int defaultStartIndex = 1;
        private readonly ILogger<AddressController> logger;

        public AddressProvider(AddressContext context, ILogger<AddressController> log)
        {
            this.context = context;
            this.logger = log;
        }

        public async Task<List<Address>> Query(Address query)
        {

            IEnumerable<Address> addresses = this.context.addresses;


            if(query.Country != null)
            {
                addresses = addresses.Where(p => p.Country.Equals(query.Country ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.City != null)
            {
                addresses = addresses.Where(p => p.City.Equals(query.City ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.District != null)
            {
                addresses = addresses.Where(p => p.District.Equals(query.District ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.PostalCode != null)
            {
                addresses = addresses.Where(p => p.PostalCode.Equals(query.PostalCode ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.Unit != null)
            {
                addresses = addresses.Where(p => p.Unit.Equals(query.Unit ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.Province != null)
            {
                addresses = addresses.Where(p => p.Province.Equals(query.Province ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (query.Street != null)
            {
                addresses = addresses.Where(p => p.Street.Equals(query.Street ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }


            int total = addresses.Count();

            if(total > 20)
            {
                addresses = addresses.Take(20);
            }

            return (List<Address>)addresses;
        }
    }
}
