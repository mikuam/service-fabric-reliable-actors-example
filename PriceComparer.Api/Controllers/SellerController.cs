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
    public class SellerController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(string id)
        {
            try
            {
                var sellerActor = ActorProxy.Create<ISellerActor>(new ActorId(id));
                var seller = await sellerActor.GetState(CancellationToken.None);

                return new JsonResult(seller);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task Post([FromBody] Seller seller)
        {
            try
            {
                var sellerActor = ActorProxy.Create<ISellerActor>(new ActorId(seller.Id));
                await sellerActor.AddSeller(seller, CancellationToken.None);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("{id}/offer")]
        public async Task AddOffer(string id, [FromBody] Offer offer)
        {
            try
            {
                var sellerActor = ActorProxy.Create<ISellerActor>(new ActorId(id));
                await sellerActor.AddOffer(offer, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("{id}/mark/{mark}")]
        public async Task AddMark(string id, decimal mark)
        {
            try
            {
                var sellerActor = ActorProxy.Create<ISellerActor>(new ActorId(id));
                await sellerActor.Mark(mark, CancellationToken.None);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
