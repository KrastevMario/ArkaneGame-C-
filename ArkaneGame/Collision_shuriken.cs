using Microsoft.Xna.Framework;

namespace Arcane
{
    class Collision_shuriken
    {
        private Shuriken shuriken;
        private Enemy enemy;

        public Collision_shuriken(Shuriken shuriken, Enemy enemy)
        {
            this.shuriken = shuriken;
            this.enemy = enemy;
        }

        public bool Update()
        {
            if (shuriken.Bounds.Intersects(enemy.Bounds))
            {
                return true;
            }
            return false;
        }
    }
}
