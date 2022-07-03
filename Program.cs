    class Weapon
    {
        private const int _bulletsPerShoot = 1;

        private int _bullets;

        public bool CanShoot() => _bullets >= _bulletsPerShoot;

        public void Shoot() => _bullets -= _bulletsPerShoot;
    }