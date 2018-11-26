using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace PriceComparer.Interfaces
{
    public interface ISellerActor : IActor
    {
        Task AddOffer(Offer offer, CancellationToken cancellationToken);

        Task<Seller> GetState(CancellationToken cancellationToken);

        Task AddSeller(Seller seller, CancellationToken cancellationToken);

        Task Mark(decimal value, CancellationToken cancellationToken);
    }
}