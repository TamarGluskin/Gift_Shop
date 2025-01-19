using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftShopManager.Models
{
    public class ProductItem
    {

        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string ImagePath { get; set; }
        public string Image { get; set; }


    }
}