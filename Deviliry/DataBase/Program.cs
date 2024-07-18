using DataBase;
using System.Runtime.Intrinsics.X86;

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureCreated();
    Product product1 = new Product { Name = "Картошечка", Price = 200 };
    Product product2 = new Product { Name = "Четыре сыра", Price = 600 };
    Product product3 = new Product { Name = "Пеперони", Price = 700 };
    Product product4 = new Product { Name = "Маргарита", Price = 500 };
    Product product5 = new Product { Name = "Суши", Price = 700 };
    Product product6 = new Product { Name = "Шаурма", Price = 300 };
    Product product7 = new Product { Name = "Бургер", Price = 200 };
    Product product8 = new Product { Name = "Котлетки", Price = 200 };
    Product product9 = new Product { Name = "Салатик", Price = 200 };
    Product product10 = new Product { Name = "Шоколадка", Price = 100 };
    db.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10);
    db.SaveChanges();
}