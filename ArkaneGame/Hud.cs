using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arcane
{
    class Hud
    {
        private SpriteFont font;
        private Vector2 position;
        private Vector2 position_timer;
        private Color color;
        private Player player1;
        private Player player2;
        private Vector2 position_player1_death;
        private Vector2 position_health_p1;
        private Vector2 position_health_p2;
        private double timer;

        public Hud(Player player1, Player player2,  string fontName)
        {
            font = Common.Content.Load<SpriteFont>(fontName);
            this.player1 = player1;
            this.player2 = player2;
            position = new Vector2(20.0f + 0.0f, 120.0f + 0.0f);
            position_timer = new Vector2(468.0f + 0.0f, 56.0f + 0.0f);
            position_health_p1 = new Vector2(20.0f + 0.0f, 20.0f + 0.0f);
            position_player1_death = new Vector2(20.0f + 0.0f, 56.0f + 0.0f);
            position_health_p2 = new Vector2((Common.GameWidth - 20) / 1.2f, 20.0f + 0.0f);
            color = Color.White;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public void Update()
        {
            //timer = System.Math.Round((float)Common.GameTime.ElapsedGameTime.TotalSeconds, 2);
           // timer = (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw()
        {
            string text = "TEAM SCORE: " + player1.Score;
            Common.SpriteBatch.DrawString(font, text, position, color);
            string text_timer = "TIME: " + System.Math.Round((float)player1.timer, 3);
            Common.SpriteBatch.DrawString(font, text_timer, position_timer, color);
            string text_live_p1 = "lives Player 1:    " + player1.lives;
            Common.SpriteBatch.DrawString(font, text_live_p1, position_health_p1, color);
            if (!player1.alive)
            {
                string player1_death = "PLAYER 1 IS DEATH";
                Common.SpriteBatch.DrawString(font, player1_death, position_player1_death, color);
            }
            string text_live_p2 = "lives Player 2:    " + player2.lives;
            Common.SpriteBatch.DrawString(font, text_live_p2, position_health_p2, color);
        }
    }
}
