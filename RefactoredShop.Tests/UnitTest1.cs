using Xunit;
using Moq; 
using System.Collections.Generic;
using RefactoredShop.Domain;
using RefactoredShop.Services;
using RefactoredShop.Interfaces;
using RefactoredShop.Strategies;
using System;

namespace RefactoredShop.Tests
{
    public class OrderServiceTests
    {
        
        private readonly Mock<IRepository<Client>> _mockClientRepo = new();
        private readonly Mock<IRepository<Product>> _mockProductRepo = new();
        private readonly Mock<IRepository<Order>> _mockOrderRepo = new();
        private readonly Mock<INotificationService> _mockNotifier = new();

        [Fact]
        public void PlaceOrder_DeveCalcularTotal_E_BaixarEstoque()
        {
          
            var service = new OrderService(
                _mockClientRepo.Object,
                _mockProductRepo.Object,
                _mockOrderRepo.Object,
                _mockNotifier.Object
            );

         
            var cliente = new Client(1, "Luccas", "lucas@teste.com");
            var produto = new Product(1, "Mouse Gamer", 100.00, 10); 

            
            _mockClientRepo.Setup(repo => repo.GetById(1)).Returns(cliente);
            _mockProductRepo.Setup(repo => repo.GetById(1)).Returns(produto);
            _mockOrderRepo.Setup(repo => repo.GetAll()).Returns(new List<Order>());

           
            var itensCompra = new List<(int, int)> { (1, 2) }; 
            var pedido = service.PlaceOrder(1, itensCompra, new NoDiscountStrategy());

            
            Assert.Equal(200.00, pedido.CalculateTotal()); 
            Assert.Equal(8, produto.Stock); 
        }

        [Fact]
        public void PlaceOrder_DeveDarErro_SeEstoqueInsuficiente()
        {
           
            var service = new OrderService(_mockClientRepo.Object, _mockProductRepo.Object, _mockOrderRepo.Object, _mockNotifier.Object);

            var produto = new Product(1, "Mouse", 100.00, 1); 
            _mockProductRepo.Setup(r => r.GetById(1)).Returns(produto);
            _mockClientRepo.Setup(r => r.GetById(1)).Returns(new Client(1, "L", "e"));

            Assert.Throws<InvalidOperationException>(() =>
                service.PlaceOrder(1, new List<(int, int)> { (1, 5) }, new NoDiscountStrategy())
            );
        }
    }
}