using AddressApi.Schemas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public AddressController(ILogger<AddressController> logger)
        {
            _logger = logger;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<Address> Get()
        {
            return new Address()
            {
                City = "Seattle"
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpGet(AddressController.AttributeValueIdentifier)]
        public async Task<Address> Get([FromUri] string countryIdentifier)
        {
            //switch country return address extension for the country
            return new Address()
            {
                City = "Seattle"
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult> Post([Microsoft.AspNetCore.Mvc.FromBody] Address newAddress)
        {
            return this.NoContent();
        }
    }
}
