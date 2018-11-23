using System.Collections.Generic;

namespace PriceComparer.Interfaces
{
    public class Product
    {
        public Product()
        {
            Offers = new List<SellerOffer>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<SellerOffer> Offers { get; set; }
    }
}
