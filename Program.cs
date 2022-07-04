using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderForm orderForm = new OrderForm();
            InitSystem initSystem = new InitSystem();
            string systemId = orderForm.ShowForm();
            PaymentSystem system = initSystem.GetSystem(systemId);
            system.Call();
            system.ShowResult();
        }
    }

    public class OrderForm
    {
        private readonly InitSystem _paymentSystems = new InitSystem();

        public string ShowForm()
        {
            Console.WriteLine($"Мы принимаем: ");
            _paymentSystems.ShowSystems();

            // симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }

    public class InitSystem
    {
        private readonly Dictionary<string, PaymentSystem> _paymentSystems = new Dictionary<string, PaymentSystem>()
        {
            { "QIWI", new QIWI() },
            { "WebMoney", new WebMoney() },
            { "Card", new Card() },
        };

        public void ShowSystems()
        {
            foreach (var system in _paymentSystems.Values)
            {
                Console.WriteLine($"{system.Name}");
            }
        }

        public PaymentSystem GetSystem(string systemId)
        {
            if (!_paymentSystems.ContainsKey(systemId))
            {
                throw new ArgumentNullException("Такой платежной системы нет!");
            }

            return _paymentSystems[systemId];
        }
    }

    internal abstract class PaymentSystem
    {
        public virtual string Name => this.GetType().Name;

        public abstract void Call();

        public void ShowResult()
        {
            Console.WriteLine($"Вы оплатили с помощью {Name}");
            Console.WriteLine($"Проверка платежа через {Name}...");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    internal class QIWI : PaymentSystem
    {
        public override void Call()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }
    }

    internal class WebMoney : PaymentSystem
    {
        public override void Call()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }
    }

    internal class Card : PaymentSystem
    {
        public override void Call()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }
    }
}