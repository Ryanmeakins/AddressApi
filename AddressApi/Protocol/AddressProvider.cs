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
        //private string[] alwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;
        private readonly AddressContext context;
        private int defaultStartIndex = 1;
        private readonly ILogger<AddressController> logger;

        public AddressProvider(AddressContext context, ILogger<AddressController> log)
        {
            this.context = context;
            this.logger = log;
        }

        public async Task<List<Address>> Query(string query)
        {

            IEnumerable<Address> addresses = new List<Address>();


            return (List<Address>)addresses;
        }
    }
}
