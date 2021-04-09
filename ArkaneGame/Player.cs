using Arcane;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcane
{
    class Player
    {
        private Texture2D texture;
        private Texture2D texture_reversed;
        private Texture2D textureContainer;
        private Vector2 position;
        private Vector2 buffer_starter_position;
        public int lives;
        private int speed;
        private Keys keyUp;
        private Keys keyDown;
        private Keys keyLeft;
        private Keys keyRight;
        private Keys keySpace;
        private int score;
        private float GRAVITY_CONST;
        private float gravity;
        public string direction;
        private bool jumping;
        public bool shuriken_shot;
        public float timer;
        public bool alive = true;
        private bool on_ground = true;
        private float remove_gravity;
        private float floor = 465.0f;
        float image_height_convert;

        public Player(string textureName, string textureName_reversed, string direction, Keys keyUp, Keys keyDown, Keys keyLeft, Keys keyRight, Keys keySpace)
        {
            texture = Common.Content.Load<Texture2D>(textureName);
            texture_reversed = Common.Content.Load<Texture2D>(textureName_reversed);
            position = Vector2.Zero;
            position.Y = MathHelper.Clamp(position.Y, 0, Common.GameHeight - texture.Height - 195);
            buffer_starter_position.Y = position.Y;
            speed = 0;
            score = 0;
            GRAVITY_CONST = 1.0F;
            gravity = GRAVITY_CONST;
            this.keyUp = keyUp;
            this.keyDown = keyDown;
            this.keyLeft = keyLeft;
            this.keyRight = keyRight;
            this.keySpace = keySpace;
            this.direction = direction;
            this.textureContainer = this.texture;
            this.jumping = false;
            this.lives = 5;
            this.image_height_convert = texture.Height - 107.0f;
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
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
            timer += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;

            if(this.lives <=0)
            {
                this.alive = !(this.alive);
            }

            if (Keyboard.GetState().IsKeyDown(keyUp) && (on_ground))
            {
                    this.position.Y -= speed;
                    this.remove_gravity = 16.0f;
                    this.position.Y -= 1.0f;
                      System.Diagnostics.Debug.WriteLine("s");
            }
            else if (Keyboard.GetState().IsKeyDown(keyDown))
            {
                //position.Y += speed;
            }
            if (Keyboard.GetState().IsKeyDown(keyLeft))
            {
                this.direction = "l";
                position.X -= speed;
                if(this.textureContainer != this.texture_reversed)
                      this.textureContainer = this.texture_reversed;
            }
            else if (Keyboard.GetState().IsKeyDown(keyRight))
            {
                this.direction = "r";
                position.X += speed;
                if (this.textureContainer != this.texture)
                    this.textureContainer = this.texture;
            }
            if (Keyboard.GetState().IsKeyDown(keySpace))
            {
                shuriken_shot = true;
            }

                position.Y = MathHelper.Clamp(position.Y, 0, Common.GameHeight - texture.Height - 195);
                position.X = MathHelper.Clamp(position.X, 0, Common.GameWidth - texture.Width);
            
            //check if player is on the ground
            if (this.position.Y >= floor - image_height_convert)
            {
                this.position.Y += this.gravity;
                on_ground = true;

            }
            else
            {
                on_ground = false;
                this.remove_gravity *= 0.799f;
                this.position.Y += (this.gravity - this.remove_gravity);
            }
        }

        public void Draw()
        {
            Common.SpriteBatch.Draw(this.textureContainer, position, Color.White);
        }

    }
}
