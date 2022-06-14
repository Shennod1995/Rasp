using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    class Store
    {
        public static void Main()
        {
            Product iPhone12 = new Product("IPhone 12");
            Product iPhone11 = new Product("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Add(iPhone12, 10);
            warehouse.Add(iPhone11, 1);

            warehouse.ShowCollections();

            Cart cart = shop.CreateCart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.ShowCollections();

            Console.WriteLine(cart.CreateOrder().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Product
    {
        public readonly string Name;

        public Product(string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }
    }

    public class Nomenclature : IReadOnlyNomenclature
    {
        public Product Product { get; private set; }
        public int Count { get; private set; }

        public Nomenclature(Product product, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Product = product;
            Count = count;
        }

        public void Merge(Nomenclature newNomenclature)
        {
            if (newNomenclature.Product != Product)
            {
                throw new InvalidOperationException();
            }

            Count += newNomenclature.Count;
        }

        public void Subtract(Nomenclature newNomenclature)
        {
            if (newNomenclature.Product != Product)
            {
                throw new InvalidOperationException();
            }

            Count -= newNomenclature.Count;
        }
    }

    public interface IReadOnlyNomenclature
    {
        public Product Product { get; }
        public int Count { get; }
    }

    public class Warehouse
    {
        private readonly List<Nomenclature> _storage;

        public Warehouse()
        {
            _storage = new List<Nomenclature>();
        }

        public IReadOnlyList<IReadOnlyNomenclature> Nomenclatures => _storage;

        public void Add(Product product, int count)
        {
            var newNomenclature = new Nomenclature(product, count);
            int nomenclatureIndex = _storage.FindIndex(nomenclature => nomenclature.Product == product);

            if (nomenclatureIndex == -1)
            {
                _storage.Add(newNomenclature);
            }
            else
            {
                _storage[nomenclatureIndex].Merge(newNomenclature);
            }
        }

        public bool Contains(Product product, int count)
        {
            int nomenclatureIndex = _storage.FindIndex(nomenclature => nomenclature.Product == product);
            return _storage[nomenclatureIndex].Count > count;
        }

        public void Remove(Product product, int count)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            int nomenclatureIndex = _storage.FindIndex(nomenclature => nomenclature.Product == product);

            if (_storage[nomenclatureIndex].Count < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var changedNomenclature = new Nomenclature(product, count);
            _storage[nomenclatureIndex].Subtract(changedNomenclature);
        }

        public void ShowCollections()
        {
            foreach (var product in _storage)
            {
                Console.WriteLine($"{product.Product.Name} - {product.Count}");
            }
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException();
        }

        public Cart CreateCart()
        {
            return new Cart(_warehouse);
        }
    }

    public class Cart
    {
        private readonly Warehouse _storage;
        private readonly List<Nomenclature> _nomenclaturesInCarts;

        public Cart(Warehouse storage)
        {
            _storage = storage;
            _nomenclaturesInCarts = new List<Nomenclature>();
        }

        public void Add(Product product, int count)
        {
            if (_storage.Contains(product, count) == false)
            {
                throw new InvalidOperationException("Не достаточно товаров на складе");
            }

            _nomenclaturesInCarts.Add(new Nomenclature(product, count));
        }

        public void ShowCollections()
        {
            foreach (var product in _nomenclaturesInCarts)
            {
                Console.WriteLine($"{product.Product.Name} - {product.Count}");
            }
        }

        public Order CreateOrder()
        {
            for (int i = 0; i < _nomenclaturesInCarts.Count; i++)
            {
                _storage.Remove(_nomenclaturesInCarts[i].Product, _nomenclaturesInCarts[i].Count);
            }
            _nomenclaturesInCarts.Clear();
            return new Order();
        }
    }

    public class Order
    {
        public string Paylink { get; }

        public Order()
        {
            Paylink = "82872374623984628734692";
        }
    }
}