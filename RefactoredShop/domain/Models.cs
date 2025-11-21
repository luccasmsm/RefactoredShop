using System;
using System.Collections.Generic;

namespace RefactoredShop.Domain
{
    public class Client
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Client(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }

    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Stock { get; private set; }

        public Product(int id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void DecreaseStock(int quantity)
        {
            if (Stock < quantity)
                throw new InvalidOperationException($"Estoque insuficiente para o produto {Name}.");
            Stock -= quantity;
        }
    }

    public class OrderItem
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public double UnitPrice { get; private set; }

        public double Total => Quantity * UnitPrice;

        public OrderItem(Product product, int quantity)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            UnitPrice = product.Price;
            Quantity = quantity;
        }
    }

    public enum OrderStatus { New, Processing, Shipped, Delivered }

    public class Order
    {
        public int Id { get; set; }
        public Client Client { get; private set; }
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public double DiscountAmount { get; private set; }

        public Order(int id, Client client)
        {
            Id = id;
            Client = client;
            Status = OrderStatus.New;
            CreatedAt = DateTime.Now;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        public void ApplyDiscount(double discount)
        {
            DiscountAmount = discount;
        }

        public double CalculateTotal()
        {
            double subtotal = 0;
            foreach (var item in Items) subtotal += item.Total;
            return Math.Max(0, subtotal - DiscountAmount);
        }
    }
}