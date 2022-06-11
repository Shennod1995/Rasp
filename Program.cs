using System;

namespace Weapon
{
    class Weapon
    {
        public int Damage { get; private set; }
        public int Bullets { get; private set; }

        private bool HaveBullets => Bullets > 0;

        public Weapon(int damage, int bullets)
        {
            if (Bullets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bullets));
            }

            if (Damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            Damage = damage;
            Bullets = bullets;
        }

        public void Fire(Player player)
        {
            if (HaveBullets == false)
            {
                throw new InvalidOperationException();
            }

            Bullets -= 1;
            player.TakeDamage(Damage);
        }
    }

    class Player
    {
        public int Health { get; private set; }

        public Player(int health)
        {
            if(health < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(health));
            }

            Health = health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            TryDie();
        }

        private void TryDie()
        {
            if(Health <= 0)
            {
                Health = 0;
                Console.WriteLine("Игрок умер");
            }
        }
    }

    class Bot
    {
        private readonly Weapon _weapon;

        public Bot(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void OnSeePlayer(Player player)
        {
            _weapon.Fire(player);
        }
    }
}