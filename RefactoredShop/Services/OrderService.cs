using System;
using System.Linq;
using System.Collections.Generic;
using RefactoredShop.Domain;
using RefactoredShop.Interfaces;

namespace RefactoredShop.Services
{
    public class OrderService
    {
        private readonly IRepository<Client> _clientRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly INotificationService _notifier;

        public OrderService(
            IRepository<Client> clientRepo,
            IRepository<Product> productRepo,
            IRepository<Order> orderRepo,
            INotificationService notifier)
        {
            _clientRepo = clientRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _notifier = notifier;
        }

        public Order PlaceOrder(int clientId, List<(int productId, int quantity)> items, IDiscountStrategy discountStrategy)
        {
            var client = _clientRepo.GetById(clientId);
            if (client == null) throw new ArgumentException("Cliente não encontrado.");

            var order = new Order(_orderRepo.GetAll().Count() + 1, client);

            foreach (var item in items)
            {
                var product = _productRepo.GetById(item.productId);
                if (product == null) throw new ArgumentException($"Produto {item.productId} não existe.");

                product.DecreaseStock(item.quantity);
                order.AddItem(new OrderItem(product, item.quantity));
            }

            double rawTotal = order.Items.Sum(i => i.Total);
            double discount = discountStrategy.CalculateDiscount(rawTotal);
            order.ApplyDiscount(discount);

            _orderRepo.Add(order);
            _notifier.Notify(client.Email, "Pedido Criado", $"Pedido {order.Id} realizado. Total: {order.CalculateTotal():C2}");

            return order;
        }

        public void UpdateStatus(int orderId, OrderStatus newStatus)
        {
            var order = _orderRepo.GetById(orderId);
            if (order == null) return;

            order.SetStatus(newStatus);
            _notifier.Notify(order.Client.Email, "Status Atualizado", $"Novo status: {newStatus}");
        }
    }
}