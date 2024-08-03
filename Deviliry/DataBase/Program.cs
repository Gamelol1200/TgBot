using DataBase;
using System.Runtime.Intrinsics.X86;

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureDeleted();
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
    Product product11 = new Product { Name = "Водичка", Price = 50 };
    Product product12 = new Product { Name = "Спрайт", Price = 80 };
    Product product13 = new Product { Name = "Фанта", Price = 80 };
    Product product14 = new Product { Name = "Американо", Price = 100 };
    Product product15 = new Product { Name = "Капучино", Price = 100 };
    Product product16 = new Product { Name = "Вишнёвый", Price = 80 };
    Product product17 = new Product { Name = "Апельсиновый", Price = 80 };
    Product product18 = new Product { Name = "Яблочный", Price = 100 };
    db.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10,
        product11, product12, product13, product14, product15, product16, product17, product18);
    db.SaveChanges();
}