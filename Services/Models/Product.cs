﻿using Microsoft.Azure.Cosmos.Table;

namespace Services.Models
{
    public class Product : TableEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; }
    }
}