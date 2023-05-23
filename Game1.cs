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



        Random generator = new Random();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D planeTexture, targetTexture, coinTexture, cursorTexture;
        Texture2D plusButtonTexture, plusButtonTextureP, minusButtonTexture, minusButtonTextureP;
        Texture2D plusButton, minusButton, plusButton1, minusButton1, plusButton2, minusButton2, dimScreen;

        Rectangle targetRect, coinRect, cursorRect;
        Rectangle plusButtonRect, minusButtonRect, plusButtonRect1, minusButtonRect1, plusButtonRect2, minusButtonRect2;
        Rectangle dimScreenRect;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState;

        float angle, prevAngle, seconds, startTime;

        Point mouse;

        enum Screen
        {
            Intro, Game, Outro
        }

        int coinSpawnX, coinSpawnY, points, totalPoints;
        int boostAmount, prevBoostAmount, totalBoost;

        Screen screen;

        Vector2 origin, planeLocation, prevPlaneLocation, planeShadowLocation, mousePos, direction, target, coinSpawnCircle, coinPoints;

        Circle mouseCircle, targetCircle, coinCircle;

        bool targetBool = false;
        bool coinPointsBool = false;
        bool menuOpenBool = false;
        bool buttonHover = false;

        SpriteFont pointsFont, pointNumbers, followingFont, upgradeMenuFont;

        Button button;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            screen = Screen.Intro;

            Mouse.SetPosition(540, 360);

            planeLocation = new Vector2(540, 800);

            coinSpawnX = generator.Next(50, 1020);
            coinSpawnY = generator.Next(50, 300);

            coinRect = new Rectangle(coinSpawnX, coinSpawnY, 30, 30);

            cursorRect = new Rectangle(0, 0, 32, 32);

            boostAmount = 200;
            totalBoost = boostAmount;

            plusButtonRect = new Rectangle(700, 300, 36, 36);
            minusButtonRect = new Rectangle(650, 300, 36, 36);
            plusButtonRect1 = new Rectangle(700, 380, 36, 36);
            minusButtonRect1 = new Rectangle(650, 380, 36, 36);
            plusButtonRect2 = new Rectangle(700, 460, 36, 36);
            minusButtonRect2 = new Rectangle(650, 460, 36, 36);

            dimScreenRect = new Rectangle(0, 0, 1080, 720);

            base.Initialize();

            button = new Button(plusButtonTexture, plusButtonTextureP, plusButtonRect);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // - textures
            planeTexture = Content.Load<Texture2D>("plane");
            targetTexture = Content.Load<Texture2D>("target");
            coinTexture = Content.Load<Texture2D>("circle");
            cursorTexture = Content.Load<Texture2D>("cursor");
            plusButtonTexture = Content.Load<Texture2D>("plusbuttonnotpressed");
            minusButtonTexture = Content.Load<Texture2D>("minusbuttonnotpressed");
            plusButtonTextureP = Content.Load<Texture2D>("plusbuttonpressed");
            minusButtonTextureP = Content.Load<Texture2D>("minusbuttonpressed");
            dimScreen = Content.Load<Texture2D>("rectangle");

            // - fonts
            pointsFont = Content.Load<SpriteFont>("points");
            pointNumbers = Content.Load<SpriteFont>("pointNumbers");
            followingFont = Content.Load<SpriteFont>("following");
            upgradeMenuFont = Content.Load<SpriteFont>("upgrade menu");

        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            int x = mouseState.X;
            int y = mouseState.Y;

            mouse = new Point(mouseState.X, mouseState.Y);
            mousePos = new Vector2(mouseState.X, mouseState.Y);

            screen = Screen.Game;

            coinSpawnCircle = new Vector2(coinSpawnX + coinRect.Width / 2, coinSpawnY + coinRect.Height / 2);

            mouseCircle = new Circle(mousePos, 10);
            coinCircle = new Circle(coinSpawnCircle, 30);

            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;

            cursorRect.X = mouseState.X - 16;
            cursorRect.Y = mouseState.Y - 16;

            plusButton = plusButtonTexture;
            minusButton = minusButtonTexture;
            plusButton1 = plusButtonTexture;
            minusButton1 = minusButtonTexture;
            plusButton2 = plusButtonTexture;
            minusButton2 = minusButtonTexture;

            buttonHover = false;

            if (screen == Screen.Intro)
            {

            }

            if (screen == Screen.Game)
            {
                // - checking if mouse is in screen when target is attempted to be created
                if (mousePos.X > 0 && mousePos.X < 1080 && mousePos.Y > 0 && mousePos.Y < 720 && menuOpenBool == false)
                { // - making a target on mouse click
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        target = new Vector2(mouseState.X, mouseState.Y);
                        targetCircle = new Circle(target, 5);
                        targetBool = true;
                        angle = GetAngle(planeLocation, new Vector2(target.X, target.Y));
                        targetRect = new Rectangle((int)target.X - 16, (int)target.Y - 30, 32, 32);
                    }
                }

                // - cancel target and make cursor target again
                if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                    targetBool = false;

                // - checking if there is a target
                if (targetBool == false)
                {
                    target = mousePos;
                    angle = GetAngle(planeLocation, new Vector2(mouseState.X, mouseState.Y));
                }

                // - rotation, direction and movement of plane
                direction = target - planeLocation;
                direction.Normalize();
                origin = new Vector2(planeTexture.Width / 2, planeTexture.Height / 2);
                
                // - boost
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (boostAmount > 0)
                        planeLocation += direction * 2;
                    else
                        planeLocation += direction;
                    boostAmount -= 1;
                    if (boostAmount < 0)
                    {
                        boostAmount = 0;
                    }
                }
                else
                {
                    planeLocation += direction;
                    boostAmount += 1;
                    if (boostAmount > totalBoost)
                        boostAmount = totalBoost;
                }

                // - once plane reaches cursor, rotation and movement stops
                if (mouseCircle.Contains(planeLocation))
                {
                    angle = prevAngle;
                    planeLocation = prevPlaneLocation;
                }

                // - if plane reaches target >> target = mouse again
                if (targetBool == true)
                    if (targetCircle.Contains(planeLocation))
                        targetBool = false;

                // - upgrade menu
                if (keyboardState.IsKeyDown(Keys.Tab))
                {
                    menuOpenBool = true;
                    angle = prevAngle;
                    planeLocation = prevPlaneLocation;
                    boostAmount = prevBoostAmount;

                    if (button.IsHovering(mouse))
                        buttonHover = true;

                    if (button.IsPressed(mouse))
                        totalBoost += 100;

                    button.Update();

                    /*
                    if (minusButtonRect.Contains(mouse) || plusButtonRect.Contains(mouse) || minusButtonRect1.Contains(mouse) || plusButtonRect1.Contains(mouse) || minusButtonRect2.Contains(mouse) || plusButtonRect2.Contains(mouse))
                    {
                        //buttonHover = true;

                        if (plusButtonRect.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                               // plusButton = plusButtonTextureP;
                                if (points >= 100)
                                    points -= 100;
                            }                                
                        if (minusButtonRect.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                                minusButton = minusButtonTextureP;

                        if (plusButtonRect1.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                plusButton1 = plusButtonTextureP;
                                if (points >= 100)
                                {
                                    points -= 100;
                                    totalBoost += 100;
                                }                                    
                            }

                        if (minusButtonRect1.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                                minusButton1 = minusButtonTextureP;

                        if (plusButtonRect2.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                                plusButton2 = plusButtonTextureP;
                        if (minusButtonRect2.Contains(mouse))
                            if (mouseState.LeftButton == ButtonState.Pressed)
                                minusButton2 = minusButtonTextureP;
                    }*/

                }

                if (keyboardState.IsKeyUp(Keys.Tab))
                    menuOpenBool = false;

                // - previous action things
                prevPlaneLocation = planeLocation;
                prevAngle = angle;
                prevMouseState = mouseState;
                prevBoostAmount = boostAmount;

                // - coins and coin points
                if (coinCircle.Contains(planeLocation))
                {
                    coinPoints = new Vector2(coinSpawnX - 40, coinSpawnY - 20);

                    coinSpawnX = generator.Next(60, 1020);
                    coinSpawnY = generator.Next(60, 660);

                    coinRect = new Rectangle(coinSpawnX, coinSpawnY, 30, 30);

                    points += 100;
                    totalPoints += 100;
                    startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    coinPointsBool = true;
                }

                // - plane shadow
                planeShadowLocation = new Vector2(planeLocation.X - 30, planeLocation.Y + 75);

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
                // - coins
                _spriteBatch.Draw(coinTexture, coinRect, Color.White);

                // - pin
                if (targetBool == true)
                    _spriteBatch.Draw(targetTexture, targetRect, Color.White);

                // - plane shadow
                _spriteBatch.Draw(planeTexture, planeShadowLocation, null, Color.Black * 0.4f, angle, origin, 1f, SpriteEffects.None, 0f);

                // - plane
                _spriteBatch.Draw(planeTexture, planeLocation, null, Color.White, angle, origin, 1f, SpriteEffects.None, 0f);

                // - hud points
                _spriteBatch.DrawString(pointsFont, $"Points :  {points}", new Vector2(40, 60), Color.Black);

                // - points numbers
                if (seconds < 3 && coinPointsBool == true)
                    _spriteBatch.DrawString(pointNumbers, "+100", coinPoints, Color.White);

                // - boost amount
                _spriteBatch.DrawString(pointsFont, boostAmount.ToString(), new Vector2(40, 100), Color.White);

                // - plane target
                if (targetBool == true)
                    _spriteBatch.DrawString(followingFont, "Following: Pin", new Vector2(40, 150), Color.White);
                else
                    _spriteBatch.DrawString(followingFont, "Following: Cursor", new Vector2(40, 150), Color.White);


                // - upgrade menu
                if (menuOpenBool == true)
                {
                    _spriteBatch.Draw(dimScreen, dimScreenRect, Color.Black * 0.3f);

                    button.Draw(_spriteBatch);



                    // - row 1
                   // _spriteBatch.Draw(plusButton, plusButtonRect, Color.White);
                   // _spriteBatch.Draw(minusButton, minusButtonRect, Color.White);
                   // _spriteBatch.DrawString(upgradeMenuFont, "Health   ...................................", new Vector2(300, 310), Color.White);

                    // - row 2
                    _spriteBatch.Draw(plusButton1, plusButtonRect1, Color.White);
                    _spriteBatch.Draw(minusButton1, minusButtonRect1, Color.White);
                    _spriteBatch.DrawString(upgradeMenuFont, "Boost     ...................................", new Vector2(300, 390), Color.White);

                    // - row 3
                    _spriteBatch.Draw(plusButton2, plusButtonRect2, Color.White);
                    _spriteBatch.Draw(minusButton2, minusButtonRect2, Color.White);
                    _spriteBatch.DrawString(upgradeMenuFont, "Ammo       ...................................", new Vector2(300, 470), Color.White);
                }

                // - cursor
                _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
                if (buttonHover == true)
                    _spriteBatch.Draw(cursorTexture, cursorRect, Color.Black);
            }

            if (screen == Screen.Outro)
            {

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}