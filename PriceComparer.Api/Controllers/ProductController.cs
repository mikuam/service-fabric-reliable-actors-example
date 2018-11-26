using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using PriceComparer.Interfaces;

namespace PriceComparer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(string id)
        {
            try
            {
                var productActor = ActorProxy.Create<IProductActor>(new ActorId(id));
                var product = await productActor.GetState(CancellationToken.None);

                return new JsonResult(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost]
        public async Task Post([FromBody] Product product)
        {
            try
            {
                var productActor = ActorProxy.Create<IProductActor>(new ActorId(product.Id));
                await productActor.Reset(product, CancellationToken.None);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
