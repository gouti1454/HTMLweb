using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GoutiClothing.Entities
{
    
    public class Cart
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public int TotalPrice
        {
            get
            {
                int totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }

                return totalprice;
            }
        }
    }
}
