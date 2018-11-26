using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using PriceComparer.Interfaces;

namespace PriceComparer
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class SellerActor : Actor, ISellerActor
    {
        private const string StateName = nameof(SellerActor);

        public SellerActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task AddSeller(Seller seller, CancellationToken cancellationToken)
        {
            await StateManager.AddOrUpdateStateAsync(StateName, seller, (key, value) => value, cancellationToken);
        }

        public async Task AddOffer(Offer offer, CancellationToken cancellationToken)
        {
            var seller = await StateManager.GetOrAddStateAsync(StateName, new Seller(), cancellationToken);

            var existingOffer = seller.Offers.FirstOrDefault(o => o.ProductId == offer.ProductId);
            if (existingOffer != null)
            {
                seller.Offers.Remove(existingOffer);
            }

            seller.Offers.Add(offer);
            var sellerOffer = new SellerOffer
            {
                ProductId = offer.ProductId,
                Price = offer.Price,
                SellerId = seller.Id,
                SellerRating = seller.Rating,
                SellerName = seller.Name
            };

            var productActor = ActorProxy.Create<IProductActor>(new ActorId(offer.ProductId));
            await productActor.UpdateSellerOffer(sellerOffer, cancellationToken);

            await StateManager.SetStateAsync(StateName, seller, cancellationToken);
        }

        public async Task Mark(decimal value, CancellationToken cancellationToken)
        {
            var seller = await StateManager.GetOrAddStateAsync(StateName, new Seller(), cancellationToken);
            seller.MarksCount += 1;
            seller.MarksSum += value;

            await StateManager.SetStateAsync(StateName, seller, cancellationToken);

            foreach (var offer in seller.Offers)
            {
                var productActor = ActorProxy.Create<IProductActor>(new ActorId(offer.ProductId));
                await productActor.UpdateSellerRating(seller.Id, seller.Rating, cancellationToken);
            }
        }
    }
}
