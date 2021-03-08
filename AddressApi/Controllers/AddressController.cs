using AddressApi.Protocol;
using AddressApi.Schemas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AddressApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class AddressController : ControllerBase
    {
        internal const string AttributeValueIdentifier = "{identifier}";

        private readonly AddressContext context;
        private readonly ILogger<AddressController> logger;
        private AddressProvider provider;

        public AddressController(AddressContext context, ILogger<AddressController> logger)
        {
            this.context = context;
            this.logger = logger;
            this.provider = new AddressProvider(this.context, this.logger);
        }

        //[Microsoft.AspNetCore.Mvc.HttpGet]
        //public async Task<Address> Get()
        //{
        //    return new Address()
        //    {
        //        City = "Seattle",
        //        Country = "USA"
        //    };
        //} //for testing


        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<List<Address>> Post([Microsoft.AspNetCore.Mvc.FromBody] Address addressQuery)
        {

            try
            {
                List<Address> list = await this.provider.Query(addressQuery).ConfigureAwait(false);

                return list;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
