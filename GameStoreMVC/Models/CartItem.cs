﻿namespace GameStoreMVC.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string GameName { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }
    }
}