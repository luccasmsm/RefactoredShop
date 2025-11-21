using System;
using System.Collections.Generic;
using RefactoredShop.Domain;
using RefactoredShop.Infrastructure;
using RefactoredShop.Services;
using RefactoredShop.Strategies;

class Program
{
    static void Main(string[] args)
    {
        var clientRepo = new InMemoryRepository<Client>();
        var productRepo = new InMemoryRepository<Product>();
        var orderRepo = new InMemoryRepository<Order>();
        var notifier = new ConsoleNotificationService();

        var service = new OrderService(clientRepo, productRepo, orderRepo, notifier);

        clientRepo.Add(new Client(1, "Maria Silva", "maria@email.com"));
        productRepo.Add(new Product(1, "Caneta", 2.50, 100));
        productRepo.Add(new Product(2, "Mochila", 120.00, 5));

        try
        {
            Console.WriteLine("=== Criando Pedido 1 (Sem Desconto) ===");
            var items1 = new List<(int, int)> { (1, 10), (2, 1) };
            var order1 = service.PlaceOrder(1, items1, new NoDiscountStrategy());
            Console.WriteLine($"Total Pedido 1: {order1.CalculateTotal():C2}");

            Console.WriteLine("\n=== Criando Pedido 2 (Desconto 10%) ===");
            var items2 = new List<(int, int)> { (2, 1) };
            var strategy = DiscountFactory.Create(10, isPercentage: true);
            var order2 = service.PlaceOrder(1, items2, strategy);
            Console.WriteLine($"Total Pedido 2: {order2.CalculateTotal():C2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO: {ex.Message}");
        }
    }
}