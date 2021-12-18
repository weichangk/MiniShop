using System;
using System.Collections.Generic;
using System.Text;

namespace MiniShop.Model.Code
{
    public class InitializationData
    {
        public List<Shop> Shop { get; set; }
        public List<User> User { get; set; }
        public List<Categorie> Categorie { get; set; }
        public List<Item> Item { get; set; }

        public static InitializationData Initialization { get; set; } = new InitializationData();
    }
}
