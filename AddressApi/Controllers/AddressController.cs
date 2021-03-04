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

        private readonly ILogger<AddressController> _logger;
        private AddressProvider provider;

        public AddressController(ILogger<AddressController> logger)
        {
            _logger = logger;
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

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<List<Address>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();

            try
            {
                List<Address> list = await this.provider.Query(query).ConfigureAwait(false);

                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.StatusCode(500, "A request to the database could not be processed");
                throw;
            }
        }


        [Microsoft.AspNetCore.Mvc.HttpGet(AddressController.AttributeValueIdentifier)]
        public async Task<Address> Get([FromUri] string countryIdentifier)
        {
            //switch country return address extension for the country
            return new Address()
            {
                City = "London",
                Country = "England"
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult> Post([Microsoft.AspNetCore.Mvc.FromBody] Address newAddress)
        {
            return this.NoContent();
        }
    }
}
