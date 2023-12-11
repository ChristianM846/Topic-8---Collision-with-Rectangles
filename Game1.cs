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

        KeyboardState keyboardState;
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
        Rectangle barrierRect1, barrierRect2;

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

            barrierRect1 = new Rectangle(0, 250, 350, 75);
            barrierRect2 = new Rectangle(450, 250, 350, 75);

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

                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.Y = barrierRect1.Bottom;
                }

                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.Y = barrierRect2.Bottom;
                }

            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;

                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.Y = barrierRect1.Top - pacRect.Height;
                }

                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.Y = barrierRect2.Top - pacRect.Height;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;

                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.X = barrierRect1.Right;
                }

            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;

                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.X = barrierRect2.Left - pacRect.Width;
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

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if(exitRect.Contains(mouseState.X, mouseState.Y))
                {
                    Exit();
                }
            }

            if (exitRect.Contains(pacRect))
            {
                Exit();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(barrierTexture, barrierRect1, Color.White);
            _spriteBatch.Draw(barrierTexture, barrierRect2, Color.White);
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