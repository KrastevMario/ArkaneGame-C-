using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcane
{
    class Background
    {
        private Texture2D texture;

        public Background(string textureName)
        {
            texture = Common.Content.Load<Texture2D>(textureName);
        }

        public void Draw()
        {
            Common.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
        }
    }
}
