using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcane
{
    class Enemy
    {
        public enum  Direction : int
        {
            RIGHT = 1,
            LEFT = -1
        };

        private Texture2D texture;
        private Texture2D texture_reversed;
        private Vector2 position;
        private int speed;
        public int Points;
        private int gravity;
        private Direction direction;
        public static double spawnTime = 5.00;

        public Enemy(string textureName)
        {
            texture = Common.Content.Load<Texture2D>(textureName);
            texture_reversed = Common.Content.Load<Texture2D>(textureName + "_reversed");
            position = Vector2.Zero;
            speed = 1;
            Points = 10;
            gravity = 1;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Direction Walk 
        {
            get { return this.direction; }
            set { this.direction = value;
                this.speed *= (int)this.direction;  //multiplicate *1 || *-1 to adjust the speed value
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X,
                                     (int)position.Y,
                                     texture.Width,
                                     texture.Height);
            }
        }

        public void Update()
        {
            position.Y = MathHelper.Clamp(position.Y, 0, Common.GameHeight - texture.Height - 195);
            this.position.Y += this.gravity;
            this.position.X += this.speed;

            if(spawnTime < 1.0)
            {
                spawnTime = 1.0;
            }
        }

        public void Draw()
        {
            if ((int)this.direction == 1)   //if the direction is right
            {
                Common.SpriteBatch.Draw(texture, position, Color.White);
            }
            else
            {
                Common.SpriteBatch.Draw(texture_reversed, position, Color.White);
            }
        }

    }
}
