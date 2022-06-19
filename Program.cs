using System;
using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    class Program
    {
        static void Main(string[] args)
        {
            //Выведите платёжные ссылки для трёх разных систем платежа: 
            //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
            //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
            //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

            Order order = new Order(1234, 1200);

            Console.WriteLine(new MD5Id().GetPayingLink(order));
            Console.WriteLine(new MD5IdAndAmount().GetPayingLink(order));
            Console.WriteLine(new SHA1IdAndAmountAndKey().GetPayingLink(order));
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    public interface IPaymentSystem
    {
        public string GetPayingLink(Order order);
    }

    public interface IReadOnlyPaymentSystems
    {
        public string HashMD5(string message);
        public string HashSHA1(string message);
    }

    class PaymentSystems : IReadOnlyPaymentSystems
    {
        public string HashMD5(string message)
        {
            using MD5 md5 = MD5.Create();

            byte[] input = Encoding.ASCII.GetBytes(message);
            byte[] hash = md5.ComputeHash(input);

            return GetHashString(hash);
        }

        public string HashSHA1(string message)
        {
            using SHA1 sha1 = SHA1.Create();

            byte[] input = Encoding.UTF8.GetBytes(message);
            byte[] hash = sha1.ComputeHash(input);

            return GetHashString(hash);
        }

        private string GetHashString(byte[] hash)
        {
            StringBuilder _stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                _stringBuilder.Append(hash[i].ToString("X2"));
            }

            return _stringBuilder.ToString();
        }
    }

    class MD5Id : IPaymentSystem
    {
        private readonly IReadOnlyPaymentSystems _paymentSystem = new PaymentSystems();

        public string GetPayingLink(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            return _paymentSystem.HashMD5(order.Id.ToString());
        }
    }

    class MD5IdAndAmount : IPaymentSystem
    {
        private readonly IReadOnlyPaymentSystems _paymentSystem = new PaymentSystems();

        public string GetPayingLink(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            string id = order.Id.ToString();
            string amount = order.Amount.ToString();

            return _paymentSystem.HashMD5(id + amount);
        }
    }

    class SHA1IdAndAmountAndKey : IPaymentSystem
    {
        private readonly IReadOnlyPaymentSystems _paymentSystem = new PaymentSystems();
        private readonly string _secretKey = "123";
        public string GetPayingLink(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            string id = order.Id.ToString();
            string amount = order.Amount.ToString();

            return _paymentSystem.HashSHA1(id + amount + _secretKey);
        }
    }
}
