﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public virtual OrderHeader CartHeader { get; set; }
    }
}
