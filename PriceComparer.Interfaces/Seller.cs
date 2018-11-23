using System.Collections.Generic;

namespace PriceComparer.Interfaces
{
    public class Seller
    {
        public Seller()
        {
            Offers = new List<Offer>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int MarksCount { get; set; }

        public decimal MarksSum { get; set; }

        public decimal Rating => MarksSum / MarksCount;

        public List<Offer> Offers { get; set; }
    }
}
