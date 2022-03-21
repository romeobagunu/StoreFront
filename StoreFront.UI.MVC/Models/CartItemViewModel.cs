using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreFront.Data.EF;//Added for access to Product.
using System.ComponentModel.DataAnnotations;//Added for access to validation/metadata.

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        public Product Product { get; set; }

        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            Product = product;
        }
    }
}