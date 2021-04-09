using Microsoft.Xna.Framework;

namespace Arcane
{
    class Collision
    {
        private Player player;
        private Enemy enemy;

        public Collision(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public bool Update()
        {
            if (player.Bounds.Intersects(enemy.Bounds))
            {   
                return true;
            }
            return false;
        }
    }
}
