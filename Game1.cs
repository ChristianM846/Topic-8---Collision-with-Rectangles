using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Topic_8___Collision_with_Rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int pacSpeed;

        KeyboardState keyboardState, oldState;
        MouseState mouseState;

        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;

        Texture2D currentPacTexture;
        Rectangle pacRect;

        Texture2D exitTexture;
        Rectangle exitRect;

        Texture2D barrierTexture;
        List<Rectangle> barriers;

        Texture2D coinTexture;
        List<Rectangle> coins;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentPacTexture = pacRightTexture;

            base.Initialize();

            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);

            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));

            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 400, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(375, 280, coinTexture.Width, coinTexture.Height));

            exitRect = new Rectangle(700, 350, 100, 100);

            currentPacTexture = pacRightTexture;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //PacMan
            pacUpTexture = Content.Load<Texture2D>("PacUp");
            pacDownTexture = Content.Load<Texture2D>("PacDown");
            pacLeftTexture = Content.Load<Texture2D>("PacLeft");
            pacRightTexture = Content.Load<Texture2D>("PacRight");

            //Barrier
            barrierTexture = Content.Load<Texture2D>("rock_barrier");

            //Exit
            exitTexture = Content.Load<Texture2D>("hobbit_door");

            //Coin
            coinTexture = Content.Load<Texture2D>("coin");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if ( keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;

                foreach (Rectangle barrier in barriers)
                {
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Bottom;
                    }
                }

            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;

                foreach (Rectangle barrier in barriers)
                {
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Top - pacRect.Height;
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;

                foreach (Rectangle barrier in barriers)
                {
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Right;
                    }
                }

            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;

                foreach (Rectangle barrier in barriers)
                {
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Left - pacRect.Width;
                    }
                }

            }

            if (pacRect.Top <= 0)
            {
                pacRect.Y = 0;
            }

            if (pacRect.Bottom >= _graphics.PreferredBackBufferHeight)
            {
                pacRect.Y = _graphics.PreferredBackBufferHeight - pacRect.Height;
            }

            if (pacRect.X <= 0)
            {
                pacRect.X = 0;
            }

            if (pacRect.Right >= _graphics.PreferredBackBufferWidth)
            {
                pacRect.X = _graphics.PreferredBackBufferWidth - pacRect.Width;
            }


            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }


            }

            if (exitRect.Contains(pacRect))
            {
                if (oldState.IsKeyUp(Keys.Enter) && keyboardState.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            oldState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            foreach (Rectangle barrier in barriers)
            {
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
            }

            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);

            foreach(Rectangle coin in coins)
            {
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}