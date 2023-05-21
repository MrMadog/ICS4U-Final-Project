using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ICS4U_Final_Project
{
    public class Game1 : Game
    {
        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;

            float run = secondPoint.X - originPoint.X;

            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)

                return (float)Math.Atan(rise / run);
            else

                return (float)(Math.PI + Math.Atan(rise / run));

        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D planeTexture, targetTexture;

        Rectangle targetRect;

        MouseState mouseState, prevMouseState;

        float angle, prevAngle;

        Point mouse;

        enum Screen
        {
            Intro, Game, Outro
        }

        Screen screen;

        Vector2 origin, planeLocation, prevPlaneLocation, mousePos, direction, target;

        Circle mouseCircle, targetCircle;

        bool targetBool = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            screen = Screen.Intro;

            Mouse.SetPosition(540, 360);

            planeLocation = new Vector2(300, 400);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            planeTexture = Content.Load<Texture2D>("plane");
            targetTexture = Content.Load<Texture2D>("target");


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();

            int x = mouseState.X;
            int y = mouseState.Y;

            mouse = new Point(mouseState.X, mouseState.Y);
            mousePos = new Vector2(mouseState.X, mouseState.Y);

            screen = Screen.Game;

            mouseCircle = new Circle(mousePos, 10);

            if (screen == Screen.Intro)
            {

            }

            if (screen == Screen.Game)
            {
                // - checking if mouse is in screen when target is attempted to be created
                if (mousePos.X > 0 && mousePos.X < 1080 && mousePos.Y > 0 && mousePos.Y < 720)
                { // - making a target on mouse click
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        target = new Vector2(mouseState.X, mouseState.Y);
                        targetCircle = new Circle(target, 5);
                        targetBool = true;
                        angle = GetAngle(planeLocation, new Vector2(target.X, target.Y));
                        targetRect = new Rectangle((int)target.X - 25, (int)target.Y - 45, 50, 50);
                    }
                }

                // - cancel target and make cursor target again
                if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                {
                    targetBool = false;
                }

                // - checking if there is a target
                if (targetBool == false)
                {
                    target = mousePos;
                    angle = GetAngle(planeLocation, new Vector2(mouseState.X, mouseState.Y));
                }

                // - rotation, direction and movement of plane
                Vector2 direction = target - planeLocation;
                direction.Normalize();
                origin = new Vector2(planeTexture.Width / 2, planeTexture.Height / 2);
                planeLocation += direction * 2;

                // - once plane reaches cursor, rotation and movement stops
                if (mouseCircle.Contains(planeLocation))
                {
                    angle = prevAngle;
                    planeLocation = prevPlaneLocation;
                }

                // - if plane reaches target >> target = mouse again
                if (targetBool == true)
                {
                    if (targetCircle.Contains(planeLocation))
                    {
                        targetBool = false;
                    }
                }

                // previous action things
                prevPlaneLocation = planeLocation;
                prevAngle = angle;
                prevMouseState = mouseState;
            }

            if (screen == Screen.Outro)
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            _spriteBatch.Begin();

            if (screen == Screen.Intro)
            {

            }

            if (screen == Screen.Game)
            {
                if (targetBool == true)
                    _spriteBatch.Draw(targetTexture, targetRect, Color.White);

                _spriteBatch.Draw(planeTexture, planeLocation, null, Color.White, angle, origin, 1f, SpriteEffects.None, 0f);
            }

            if (screen == Screen.Outro)
            {

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
