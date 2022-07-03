using System;

namespace CleanCode_ExampleTask1
{
    class Player
    {
        public string Name { get; private set; }
        public int Age { get; private set; }

        public void Move()
        {
            //Do move
        }

        public void Attack()
        {
            //attack
        }
    }

    class Weapon
    {
        public float WeaponCooldown { get; private set; }
        public int WeaponDamage { get; private set; }

        private bool IsReloading()
        {
            throw new NotImplementedException();
        }
    }

    class Movement
    {
        public float MovementDirectionX { get; private set; }
        public float MovementDirectionY { get; private set; }
        public float MovementSpeed { get; private set; }
    }
}