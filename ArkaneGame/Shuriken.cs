using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcane
{
    internal class Shuriken
    {
        public enum Direction : int
        {
            RIGHT = 1,
            LEFT = -1
        };

        private Texture2D texture;
        private Texture2D texture1;
        private Texture2D texture2;
        private Texture2D[] texture_buffer;
        private Vector2 position;
        private int speed;
        private Direction direction;
        private float frame_time_change;
        private float frame_time_change_time;
        private float frame_time;

        //inizialize variables
        public Shuriken(string textureName)
        {
            texture1 = Common.Content.Load<Texture2D>(textureName + "0");
            texture2 = Common.Content.Load<Texture2D>(textureName + "1");
            texture_buffer = new Texture2D[3];
            texture = texture1;
            texture_buffer[1] = texture1;
            texture_buffer[2] = texture2;
            position = Vector2.Zero;
            speed = 7;
            frame_time_change = 0.10f;
            frame_time_change_time = 0.10f;
            frame_time = 0.10f;
            
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

        public Direction Move
        {
            get { return this.direction; }
            set
            {
                this.direction = value;
                this.speed *= (int)this.direction;
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
            this.position.X += this.speed;

            //take the time from game (real time)
            frame_time += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;

            //shuriken frame change (after X time, change the frame)
            if (frame_time > frame_time_change)
            {
                texture_buffer[0] = texture_buffer[1];
                texture_buffer[1] = texture_buffer[2];
                texture_buffer[2] = texture_buffer[0];

                texture = texture_buffer[1];
                frame_time_change = frame_time + frame_time_change_time;
            }
            //else texture = texture2;
        }

        public void Draw()
        {
            Common.SpriteBatch.Draw(texture, position, Color.White);
        }

    }
}