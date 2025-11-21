using System;
using System.Collections.Generic;
using System.Linq;
using RefactoredShop.Domain;
using RefactoredShop.Interfaces;

namespace RefactoredShop.Infrastructure
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        protected readonly List<T> _data = new List<T>();

        public void Add(T entity) => _data.Add(entity);
        public IEnumerable<T> GetAll() => _data;

        public T GetById(int id)
        {
            // Reflexão simples apenas para este exemplo genérico
            return _data.FirstOrDefault(x => (int)x.GetType().GetProperty("Id").GetValue(x) == id);
        }
    }

    public class ConsoleNotificationService : INotificationService
    {
        public void Notify(string to, string subject, string message)
        {
            Console.WriteLine($"[EMAIL] Para: {to} | Assunto: {subject} | Msg: {message}");
        }
    }
}