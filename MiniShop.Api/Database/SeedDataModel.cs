using MiniShop.Model;
using System.Collections.Generic;

namespace MiniShop.Api.Database
{
    public class SeedDataModel
    {
        public List<Shop> Shops { get; set; }
        public List<Item> Items { get; set; }
        public List<Categorie> Categories { get; set; }
    }
}
