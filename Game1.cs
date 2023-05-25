using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        List<Button> buttons;

        Texture2D planeTexture, targetTexture, coinTexture, cursorTexture, bulletTexture;
        Texture2D plusButtonTexture, plusButtonTextureP, minusButtonTexture, minusButtonTextureP, dimScreen;

        Rectangle targetRect, coinRect, cursorRect, cursorHoverRect, bulletRect;
        Rectangle dimScreenRect, upgradeMenuRect, upgradeMenuInfoRect, upgradeMenuPointsRect;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;

        float angle, prevAngle, seconds, startTime, bulletRotation;

        Point mouse;

        enum Screen
        {
            Intro, Game, Outro
        }

        int coinSpawnX, coinSpawnY, points, totalPoints;
        int boostAmount, prevBoostAmount, totalBoost, planeHealth, totalPlaneHealth, planeAmmo;

        Screen screen;

        Vector2 origin, planeLocation, prevPlaneLocation, planeShadowLocation, mousePos, planeDirection, target, coinSpawnCircle, coinPoints;
        Vector2 bulletVelocity, bulletPosition;

        Circle mouseCircle, targetCircle, coinCircle;

        Color colour;

        bool targetBool = false;
        bool coinPointsBool = false;
        bool menuOpenBool = false;
        bool buttonHover = false;
        bool done = false;
        bool fadeBool = false;
        bool bulletBool = false;

        SpriteFont pointsFont, pointNumbers, followingFont, upgradeMenuFont, upgradeMenuInfoFont, currentFont, availablePointsFont;

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

            buttons = new List<Button>();

            planeLocation = new Vector2(540, 800);

            coinSpawnX = generator.Next(50, 1020);
            coinSpawnY = generator.Next(50, 300);

            coinRect = new Rectangle(coinSpawnX, coinSpawnY, 30, 30);

            cursorRect = new Rectangle(0, 0, 32, 32);
            cursorHoverRect = new Rectangle(0, 0, 28, 28);

            boostAmount = 200;
            totalBoost = boostAmount;

            planeHealth = 100;
            totalPlaneHealth = planeHealth;

            dimScreenRect = new Rectangle(0, 0, 1080, 720);
            upgradeMenuRect = new Rectangle(90, 200, 580, 350);
            upgradeMenuInfoRect = new Rectangle(690, 200, 300, 500);
            upgradeMenuPointsRect = new Rectangle(90, 570, 580, 130);

            colour = new Color(0, 0, 0, 0);

            base.Initialize();


            // - minus buttons
            buttons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 240, 36, 36))); // 0
            buttons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 320, 36, 36))); // 1
            buttons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 400, 36, 36))); // 2
            buttons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 480, 36, 36))); // 3
            // - plus buttons
            buttons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 240, 36, 36))); // 4
            buttons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 320, 36, 36))); // 5
            buttons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 400, 36, 36))); // 6
            buttons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 480, 36, 36))); // 7

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
            bulletTexture = Content.Load<Texture2D>("tile_0000");

            // - fonts
            pointsFont = Content.Load<SpriteFont>("points");
            pointNumbers = Content.Load<SpriteFont>("pointNumbers");
            followingFont = Content.Load<SpriteFont>("following");
            upgradeMenuFont = Content.Load<SpriteFont>("upgrade menu");
            upgradeMenuInfoFont = Content.Load<SpriteFont>("upgrade_menu_info");
            currentFont = Content.Load<SpriteFont>("Current Font");
            availablePointsFont = Content.Load<SpriteFont>("Available Points");

        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            prevKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            int x = mouseState.X;
            int y = mouseState.Y;

            mouse = new Point(mouseState.X, mouseState.Y);
            mousePos = new Vector2(mouseState.X, mouseState.Y);

            coinSpawnCircle = new Vector2(coinSpawnX + coinRect.Width / 2, coinSpawnY + coinRect.Height / 2);

            mouseCircle = new Circle(mousePos, 10);
            coinCircle = new Circle(coinSpawnCircle, 30);

            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;

            cursorRect.X = mouseState.X - 16;
            cursorRect.Y = mouseState.Y - 16;
            cursorHoverRect.X = mouseState.X - 14;
            cursorHoverRect.Y = mouseState.Y - 14;

            for (int i = 0; i < 8; i++)
                buttons[i].Update();

            buttonHover = false;

            // - Intro --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (screen == Screen.Intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    fadeBool = true;
                }
                if (fadeBool == true)
                    colour.A += 5;

                if (colour.A >= 255)
                    done = true;

                if (done == true)
                {
                    screen = Screen.Game;
                }
            }

            // - Game ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                planeDirection = target - planeLocation;
                planeDirection.Normalize();
                origin = new Vector2(planeTexture.Width / 2, planeTexture.Height / 2);
                
                // - boost
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (boostAmount > 0)
                        planeLocation += planeDirection * 2;
                    else
                        planeLocation += planeDirection;
                    boostAmount -= 1;
                    if (boostAmount < 0)
                        boostAmount = 0;
                }
                else
                {
                    planeLocation += planeDirection;
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

                    for (int i = 0; i < 8; i++)
                    {
                        if (buttons[i].IsHovering(mouse))
                            buttonHover = true;
                    }

                    // - row 1 (health cost 500 for +50, return for 400)
                    if (buttons[4].IsPressed())
                        if (points >= 500 && totalPlaneHealth < 500)
                        {
                            totalPlaneHealth += 50;
                            if (planeHealth == totalPlaneHealth - 50)
                                planeHealth = totalPlaneHealth;
                            points -= 500;
                        }
                    if (buttons[0].IsPressed())
                        if (totalPlaneHealth >= 150)
                        {
                            totalPlaneHealth -= 50;
                            points += 400;
                        }
                    // - row 2 (boost cost 200 for +25, return for 100)
                    if (buttons[5].IsPressed())
                        if (points >= 200 && totalBoost < 1000)
                        {
                            totalBoost += 25;
                            points -= 200;
                        }
                    if (buttons[1].IsPressed())
                        if (totalBoost >= 225)
                        {
                            totalBoost -= 25;
                            points += 100;
                        }
                    // - row 3 (ammo cost 200 for + 5, return for 50/bullet)
                    if (buttons[6].IsPressed())
                        if (planeAmmo < 40 && points >= 200)
                        {
                            planeAmmo += 5;
                            points -= 200;
                        }
                    if (buttons[2].IsPressed())
                        if (planeAmmo > 1)
                        {
                            planeAmmo -= 2;
                            points += 50;
                        }
                    // - row 4
                    if (buttons[7].IsPressed())
                    {

                    }

                    if (buttons[3].IsPressed())
                    {

                    }




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

                // - bullets
                if (keyboardState.IsKeyDown(Keys.M) && prevKeyboardState.IsKeyUp(Keys.M))
                {
                    bulletBool = true;

                    bulletRotation = GetAngle(planeLocation, new Vector2(mouseState.X, mouseState.Y));

                    bulletVelocity = new Vector2((float)Math.Cos(bulletRotation), (float)Math.Sin(bulletRotation)) * 5f;
                    bulletVelocity *= 2;

                    bulletPosition += bulletVelocity;
                    bulletRect.X = (int)bulletPosition.X;
                    bulletRect.Y = (int)bulletPosition.Y;
                }


                if (keyboardState.IsKeyDown(Keys.RightControl) && prevKeyboardState.IsKeyUp(Keys.RightControl))
                {
                    screen = Screen.Outro;
                }
            }

            // - Outro --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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

                // - cursor
                if (buttonHover == true)
                    _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
                else
                    _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);

                // - screen fade
                _spriteBatch.Draw(dimScreen, dimScreenRect, colour);
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
                _spriteBatch.DrawString(pointsFont, $"Points :  {points}", new Vector2(40, 40), Color.Black);

                // - points numbers
                if (seconds < 3 && coinPointsBool == true)
                    _spriteBatch.DrawString(pointNumbers, "+100", coinPoints, Color.White);

                // - boost amount
                _spriteBatch.DrawString(followingFont, $"Boost: {boostAmount.ToString()}", new Vector2(40, 100), Color.White);

                // - plane target
                if (targetBool == true)
                    _spriteBatch.DrawString(followingFont, "Following: Pin", new Vector2(40, 150), Color.White);
                else
                    _spriteBatch.DrawString(followingFont, "Following: Cursor", new Vector2(40, 150), Color.White);

                // - plane health
                _spriteBatch.DrawString(followingFont, $"Health: {planeHealth.ToString()}", new Vector2(40, 200), Color.White);

                // - plane ammo
                _spriteBatch.DrawString(followingFont, $"Ammo: {planeAmmo.ToString()}", new Vector2(40, 250), Color.White);


                // - upgrade menu
                if (menuOpenBool == true)
                {
                    // - dim areas
                    _spriteBatch.Draw(dimScreen, dimScreenRect, Color.Black * 0.3f);
                    _spriteBatch.Draw(dimScreen, upgradeMenuRect, Color.Black * 0.5f);
                    _spriteBatch.Draw(dimScreen, upgradeMenuPointsRect, Color.Black * 0.5f);
                    // buttons
                    for (int i = 0; i < 8; i++)
                        buttons[i].Draw(_spriteBatch);
                    // lines
                    _spriteBatch.DrawString(upgradeMenuFont, "Health   ..........................................", new Vector2(140, 250), Color.White);
                    _spriteBatch.DrawString(upgradeMenuFont, "Boost     ..........................................", new Vector2(140, 330), Color.White);
                    _spriteBatch.DrawString(upgradeMenuFont, "Ammo       ..........................................", new Vector2(140, 410), Color.White);
                    _spriteBatch.DrawString(upgradeMenuFont, "Bombs    ..........................................", new Vector2(140, 490), Color.White);
                    // available points
                    _spriteBatch.DrawString(availablePointsFont, $"Available Points: {points}", new Vector2(110, 615), Color.White);
                    // info areas
                    if (buttons[0].IsHovering(mouse) || buttons[4].IsHovering(mouse))
                    {
                        _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                        _spriteBatch.DrawString(followingFont, "Plane Health", new Vector2(700, 220), Color.White);
                        _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +50\r\nmax health cap.\r\nCost: 500 Points\r\n\r\nClicking the ''-'' will take -50\r\nfrom your max health cap, and\r\ngive you some credits back.\r\nRefund: 400 Points ", new Vector2(700, 250), Color.White);
                        _spriteBatch.DrawString(currentFont, "Current Max\r\nHealth:", new Vector2(700, 600), Color.White);
                        _spriteBatch.DrawString(pointsFont, totalPlaneHealth.ToString(), new Vector2(700, 660), Color.White);
                    }
                    if (buttons[1].IsHovering(mouse) || buttons[5].IsHovering(mouse))
                    {
                        _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                        _spriteBatch.DrawString(followingFont, "Boost", new Vector2(700, 220), Color.White);
                        _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +25\r\nmax boost cap.\r\nCost: 200 Points\r\n\r\nClicking the ''-'' will take -25\r\nfrom your max boost cap, and \r\ngive you some credits back.\r\nRefund: 100 Points ", new Vector2(700, 250), Color.White);
                        _spriteBatch.DrawString(currentFont, "Current Max\r\nBoost:", new Vector2(700, 600), Color.White);
                        _spriteBatch.DrawString(pointsFont, totalBoost.ToString(), new Vector2(700, 660), Color.White);
                    }
                    if (buttons[2].IsHovering(mouse) || buttons[6].IsHovering(mouse))
                    {
                        _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                        _spriteBatch.DrawString(followingFont, "Ammo", new Vector2(700, 220), Color.White);
                        _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +5\r\nammo.\r\nCost: 200 Points\r\n\r\nClicking the ''-'' will take -1 ammo,\r\nand give you some credits back.\r\nRefund: 50 Points ", new Vector2(700, 250), Color.White);
                        _spriteBatch.DrawString(currentFont, "Current Ammo:", new Vector2(700, 600), Color.White);
                        _spriteBatch.DrawString(pointsFont, planeAmmo.ToString(), new Vector2(700, 660), Color.White);
                    }
                    if (buttons[3].IsHovering(mouse) || buttons[7].IsHovering(mouse))
                    {
                        _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                        _spriteBatch.DrawString(followingFont, "Bombs", new Vector2(700, 220), Color.White);
                        _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +25\r\nmax boost cap.\r\nCost: 200 Points\r\n\r\nClicking the ''-'' will take -25\r\nfrom your max boost cap, and \r\ngive you some credits back.\r\nRefund: 100 Points ", new Vector2(700, 250), Color.White);
                    }
                }

                // - bullets
                if (bulletBool == true)
                {
                    _spriteBatch.Draw(bulletTexture, bulletRect, null, Color.White, bulletRotation, null, SpriteEffects.None, 0f);
                }

                // - cursor
                if (buttonHover == true)
                    _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
                else
                    _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
            }

            if (screen == Screen.Outro)
            {
                _spriteBatch.DrawString(followingFont, $"You got {totalPoints} points!", new Vector2(300, 400), Color.Black);

                // - cursor
                if (buttonHover == true)
                    _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
                else
                    _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}