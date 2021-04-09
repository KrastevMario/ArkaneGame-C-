using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace Arcane
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Hud hud;
        Player player1;
        Player player2;
        Background background;
        Sound song;
        Sound effect;
        Collision collision_p1;
        Collision collision_p2;
        Collision_shuriken shuriken_p1_collision;
        List<Enemy> enemies;
        List<Shuriken> shurikens;
        public bool shuriken_p1;
        float timeSinceLastLive_p1 = 0f;
        float timeSinceLastLive_p2 = 0f;
        float frame_lost_live_p1 = 0f;
        float frame_lost_live_p2 = 0f;
        float spawn_shuriken_p1_timer = 0f;
        double timeEnemySpawn = 0f;
        bool p1_lost_life = false;
        bool p2_lost_life = false;
        List<int> diedEnemies; //  list of enemies (killed) [removing]
        List<int> shurikenHit; // list of shurikens that hit the enemy
        float timer_spawner;
        private bool shuriken_p2;
        private float spawn_shuriken_p2_timer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Common.Content = Content;
            Window.Title = Common.GameTitle;
            
            graphics.PreferredBackBufferWidth = Common.GameWidth;
            graphics.PreferredBackBufferHeight = Common.GameHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            diedEnemies = new List<int>();
            shurikenHit = new List<int>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Common.SpriteBatch = spriteBatch;

            player1 = new Player("character1", "character1_reversed", "r", Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
            player2 = new Player("character2", "character2_reversed", "l", Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter);
            player1.Position = new Vector2(10, 465);
            player2.Position = new Vector2(Common.GameWidth - 25, 465);
            player1.Speed = 5;
            player2.Speed = 5;

            this.shuriken_p1 = player1.shuriken_shot;

            enemies = new List<Enemy>(10);  //  enemy list
            shurikens = new List<Shuriken>(10);  //  enemy list

            spawnEnemy(true);  //  the first enemy (left)

            //enemy1 = new Enemy("enemy1");
            //enemy1.Position = new Vector2(-70, 465);

            
            /*
            collision_p1 = new Collision(player1, enemies[0]);
            collision_p2 = new Collision(player2, enemies[0]);
            */
            hud = new Hud(player1, player2, "gameFont");
            hud.Position = new Vector2((Common.GameWidth - 25) / 2 / 1.3f, 10);
            background = new Background("background1");

            song = new Sound(SoundType.Song, "bg_music");
            effect = new Sound(SoundType.Effect, "hit");

            song.Play();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Common.GameTime = gameTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(player1.alive)
                 player1.Update();
            if (player2.alive)
                player2.Update();

            foreach (Enemy i in enemies)
            {
                i.Update();
                //check if objects are on map(screen)
                if(i.Position.X > Common.GameWidth+100 || i.Position.X < 0 - 100)
                {
                    // enemies.Remove(i);
                    diedEnemies.Add(enemies.IndexOf(i));  //  if the mob is not on the screen, add it to the index to the "remove list"
                }
                if (i.Position.X > Common.GameWidth + 100 || i.Position.X < 0 - 100)
                {
                    shurikenHit.Add(enemies.IndexOf(i)); 
                }

                //collisions --> player, item
                
                collision_p1 = new Collision(player1, i);
                collision_p2 = new Collision(player2, i);

                if (player1.alive)
                {
                    if (timeSinceLastLive_p1 > 2.0f)    //timer for the health (player)
                    {
                        if (collision_p1.Update())
                        {
                            p1_lost_life = true;
                            player1.lives -= 1;
                            timeSinceLastLive_p1 = 0f;
                            diedEnemies.Add(enemies.IndexOf(i));
                        }
                        else p1_lost_life = false;
                    }
                }

                if (player2.alive)
                {
                    if (timeSinceLastLive_p2 > 2.0f)
                    {
                        if (collision_p2.Update())
                        {
                            p2_lost_life = true;
                            player2.lives -= 1;
                            timeSinceLastLive_p2 = 0f;
                            diedEnemies.Add(enemies.IndexOf(i));
                        }
                        else p2_lost_life = false;
                    }
                }
            }

            foreach (Shuriken i in shurikens)
            {
                foreach (Enemy y in enemies)
                {
                    shuriken_p1_collision = new Collision_shuriken(i, y);
                    if (shuriken_p1_collision.Update())
                    {
                        player1.Score += y.Points;
                        diedEnemies.Add(enemies.IndexOf(y));
                        shurikenHit.Add(shurikens.IndexOf(i));
                        System.Console.Write(" hit ");
                    }
                }

                i.Update();
            }

            foreach (int i in diedEnemies)
            {
                removeEnemy(i);
            }
            foreach (int i in shurikenHit)
            {
                removeShuriken(i);
            }
            diedEnemies.Clear();
            shurikenHit.Clear();



         //   enemy1.Update();
            hud.Update();

            //timers for the player
            timeSinceLastLive_p1 += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastLive_p2 += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            frame_lost_live_p1 += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            frame_lost_live_p2 += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            spawn_shuriken_p1_timer += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            spawn_shuriken_p2_timer += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            timer_spawner += (float)Common.GameTime.ElapsedGameTime.TotalSeconds;
            timeEnemySpawn += Common.GameTime.ElapsedGameTime.TotalSeconds;

            //timer for waves (mobs are starting to spawn more often)
            if (timer_spawner > 21.0f)
            {
               Enemy.spawnTime /= 1.2;
               timer_spawner = 0.0f;
               System.Console.Write(" timer: " + Enemy.spawnTime + "  ");
            }

            if(timeEnemySpawn > Enemy.spawnTime)
            {
                spawnEnemy((new System.Random().Next(100)) > 50);  //  50% probabilita' che risulti vero e che quindi spawni all'inizio
                timeEnemySpawn = 0.0;
            }

            //check if players activated the shuriken _command_ [P1]
            this.shuriken_p1 = player1.shuriken_shot;
            if (this.shuriken_p1)
            {
                if (spawn_shuriken_p1_timer > 2.0f)
                {
                    spawnShuriken(true, player1.Position);
                    spawn_shuriken_p1_timer = 0.0f;
                }
                player1.shuriken_shot = false;
            }

            //check if players activated the shuriken _command_ [P2]
            this.shuriken_p2 = player2.shuriken_shot;
            if (this.shuriken_p2)
            {
                if (spawn_shuriken_p2_timer > 2.0f)
                {
                    spawnShuriken(false, player2.Position);
                    spawn_shuriken_p2_timer = 0.0f;
                }
                player2.shuriken_shot = false;
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Common.SpriteBatch.Begin();
            background.Draw();

            if (p2_lost_life)       //if the player didnt loose any lifes, draw it without "die effect"
            {
                if (frame_lost_live_p2 > 0.15f) //draw the image every 0.150 seconds so it creates the "die effect"
                {
                    if (player2.alive)
                        player2.Draw();
                    frame_lost_live_p2 = 0.0f;  //reset the timer
                }
            }
            else { if (player2.alive) player2.Draw(); }

            if (p1_lost_life)
            {
                if (frame_lost_live_p1 > 0.15f)
                {
                    if (player1.alive)
                        player1.Draw();
                    frame_lost_live_p1 = 0.0f;
                }
            }
            else { if (player1.alive) player1.Draw(); }

            foreach(Enemy i in enemies)
            {
                i.Draw();
            }
            foreach (Shuriken i in shurikens)
            {
                i.Draw();
            }

            //enemy1.Draw();
            hud.Draw();
            Common.SpriteBatch.End();

            base.Draw(gameTime);
        }

        //this function spawns a new enemy at the end or at the beginning of the screen (X)
        public void spawnEnemy(bool beginning)  //  spawn left or right (enemy)
        {
            Enemy enemy;
            if (beginning)
            {
                enemy = new Enemy("enemy1");
                enemy.Position = new Vector2(0 - 100, 465);   //  spawn point
                enemy.Walk = Enemy.Direction.RIGHT;
            }
            else
            {
                enemy = new Enemy("enemy1");  //  TODO: CHANGE
                enemy.Position = new Vector2(Common.GameWidth + 100, 465);
                enemy.Walk = Enemy.Direction.LEFT;
            }
            enemies.Add(enemy);   //  spawns the new Enemy

            System.Console.Write(" New spawned, NOW: " + enemies.Count + "  ");
            System.Console.Write(" timer: " + Enemy.spawnTime + "  ");

        }

        public void spawnShuriken(bool player, Vector2 beginning) 
        {
            Shuriken shurik;
            if (player) //check who started the "skill"
            {
                shurik = new Shuriken("shuriken");
                shurik.Position = beginning;
                if (this.player1.direction == "r")
                    shurik.Move = Shuriken.Direction.RIGHT;
                else shurik.Move = Shuriken.Direction.LEFT;
            }
            else
            {
                shurik = new Shuriken("shuriken");
                shurik.Position = beginning;
                if (this.player2.direction == "r")
                    shurik.Move = Shuriken.Direction.RIGHT;
                else shurik.Move = Shuriken.Direction.LEFT;
            }
            shurikens.Add(shurik);  
        }

        //enemies to remove
        public void removeEnemy(int indexToRemove)
        {
            System.Console.Write(" New died , NOW: " + enemies.Count + "  " );
            try
            {
                enemies.RemoveAt(indexToRemove);
            }
            catch { }
        }
        public void removeShuriken(int indexToRemove)
        {
            if (indexToRemove != null)
                //if the shuriken at RemoveAt[position] doesn't exist, ignore the error 
                try
                {
                    shurikens.RemoveAt(indexToRemove);
                }
                catch{ }
        }
    }
}
