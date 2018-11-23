using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using PriceComparer.Interfaces;

namespace PriceComparer
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class ProductActor : Actor, IProductActor
    {
        private const string StateName = nameof(ProductActor);

        public ProductActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task ResetAsync(Product product, CancellationToken cancellationToken)
        {
            await StateManager.AddOrUpdateStateAsync(StateName, product, (key, value) => value, cancellationToken);
        }

        public async Task UpdateSellerOffer(SellerOffer offer, CancellationToken cancellationToken)
        {
            var product = await StateManager.GetOrAddStateAsync(StateName, new Product(), cancellationToken);

            var existingMatchingOffer = product.Offers.FirstOrDefault(o => o.SellerId == offer.SellerId);
            if (existingMatchingOffer != null)
            {
                product.Offers.Remove(existingMatchingOffer);
            }

            product.Offers.Add(offer);
            product.Offers = product.Offers.OrderByDescending(o => o.SellerRating).ToList();

            await StateManager.SetStateAsync(StateName, product, cancellationToken);
        }

        public async Task UpdateSellerRating(string sellerId, decimal sellerRating, CancellationToken cancellationToken)
        {
            var product = await StateManager.GetOrAddStateAsync(StateName, new Product(), cancellationToken);

            var existingMatchingOffer = product.Offers.FirstOrDefault(o => o.SellerId == sellerId);
            if (existingMatchingOffer != null)
            {
                existingMatchingOffer.SellerRating = sellerRating;
                product.Offers = product.Offers.OrderByDescending(o => o.SellerRating).ToList();

                await StateManager.SetStateAsync(StateName, product, cancellationToken);
            }
        }
    }
}
