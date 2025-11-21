using System.Collections.Generic;
using RefactoredShop.Domain;

namespace RefactoredShop.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        void Add(T entity);
        IEnumerable<T> GetAll();
    }

    public interface INotificationService
    {
        void Notify(string to, string subject, string message);
    }

    public interface IDiscountStrategy
    {
        double CalculateDiscount(double orderTotal);
    }
}