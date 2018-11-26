using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace PriceComparer.Interfaces
{
    public interface IProductActor : IActor
    {
        Task<Product> GetState(CancellationToken cancellationToken);

        Task Reset(Product product, CancellationToken cancellationToken);

        Task UpdateSellerOffer(SellerOffer offer, CancellationToken cancellationToken);

        Task UpdateSellerRating(string sellerId, decimal sellerRating, CancellationToken cancellationToken);
    }
}
