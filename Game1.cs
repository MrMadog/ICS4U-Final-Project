using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ICS4U_Final_Project
{
    public class Game1 : Game
    {
        public static float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;
            float run = secondPoint.X - originPoint.X;
            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            else
                return (float)(Math.PI + Math.Atan(rise / run));
        }

        Random generator = new();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Button> upgradeButtons;
        List<Button> hudButtons;
        List<Bullet> bullets;
        List<Trail> planeTrail;
        List<EnemyPlane> enemyPlanes;
        List<Texture2D> userPlaneTextures;
        List<Texture2D> enemyPlaneTextures;
        List<Bomb> explosionsList;
        List<Texture2D> exploTexturesList;

        Texture2D targetTexture, coinTexture, cursorTexture, bulletTexture, planeTrailTexture;
        Texture2D plusButtonTexture, plusButtonTextureP, minusButtonTexture, minusButtonTextureP, dimScreen;

        // user planes
        Texture2D userPlane, userPlaneTexture1, userPlaneTexture2, userPlaneTexture3, userPlaneTexture4, userPlaneTexture5, userPlaneTexture6;
        Texture2D userPlaneTexture7, userPlaneTexture8, userPlaneTexture9, userPlaneTexture10, userPlaneTexture11;
        // enemy planes
        Texture2D enemyPlane, enemyPlaneTexture1, enemyPlaneTexture2, enemyPlaneTexture3, enemyPlaneTexture4, enemyPlaneTexture5, enemyPlaneTexture6;

        Texture2D exploSpritesheet, bombTexture;

        Rectangle targetRect, coinRect, cursorRect, cursorHoverRect;
        Rectangle dimScreenRect, upgradeMenuRect, upgradeMenuInfoRect, upgradeMenuPointsRect;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;

        float angle, prevAngle;

        Point mouse;

        enum Screen
        {
            Intro, Game, Pause, Outro, Upgrade
        }

        int coinSpawnX, coinSpawnY, points = 10000, totalPoints, bombs;
        int boostAmount, totalBoost, planeHealth, totalPlaneHealth, planeAmmo;

        Screen screen;

        Vector2 origin, planeLocation, prevPlaneLocation, planeShadowLocation, mousePos, planeDirection, target, coinSpawnCircle, coinPoints;
        Vector2 damagePoints, killPoints, groundCrosshair;

        Circle mouseCircle, targetCircle, coinCircle, userHitbox;

        Color colour;

        Stopwatch timer, timer2, timer3, timer4, timer5, timer6;
        TimeSpan elapsed, elapsed2, elapsed3, elapsed4, elapsed5, elapsed6;

        bool targetBool = false;
        bool coinPointsBool = false;
        bool enemyKillBool = false;
        bool buttonHover = false;
        bool done = false;
        bool fadeBool = false;
        bool bulletBool = false;
        bool enemyHitBool = false;
        bool explosion = false;
        bool userHit = false;
        bool crashBool = false;

        SpriteFont pointsFont, pointNumbers, followingFont, upgradeMenuFont, upgradeMenuInfoFont, currentFont, availablePointsFont, menuTitleFont;

        SoundEffect planeShot, enemyPlaneShot, bombExplosion, engineSound, plusButton, minusButton;
        SoundEffectInstance engineSoundInstance;

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

            upgradeButtons = new List<Button>();
            hudButtons = new List<Button>();
            bullets = new List<Bullet>();
            planeTrail = new List<Trail>();
            enemyPlanes = new List<EnemyPlane>();
            userPlaneTextures = new List<Texture2D>();
            enemyPlaneTextures = new List<Texture2D>();
            exploTexturesList = new List<Texture2D>();
            explosionsList = new List<Bomb>();

            planeLocation = new Vector2(540, 800);
            groundCrosshair = planeLocation;

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

            timer = new Stopwatch();
            timer2 = new Stopwatch();
            timer3 = new Stopwatch();
            timer4 = new Stopwatch();
            timer5 = new Stopwatch();
            timer6 = new Stopwatch();

            base.Initialize();

            // - minus buttons
            upgradeButtons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 240, 36, 36), minusButton)); // 0
            upgradeButtons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 320, 36, 36), minusButton)); // 1
            upgradeButtons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 400, 36, 36), minusButton)); // 2
            upgradeButtons.Add(new Button(minusButtonTexture, minusButtonTextureP, new Rectangle(540, 480, 36, 36), minusButton)); // 3
            // - plus buttons
            upgradeButtons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 240, 36, 36), plusButton)); // 4
            upgradeButtons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 320, 36, 36), plusButton)); // 5
            upgradeButtons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 400, 36, 36), plusButton)); // 6
            upgradeButtons.Add(new Button(plusButtonTexture, plusButtonTextureP, new Rectangle(590, 480, 36, 36), plusButton)); // 7

            // - user plane textures
            userPlaneTextures.Add(userPlaneTexture1);
            userPlaneTextures.Add(userPlaneTexture2);
            userPlaneTextures.Add(userPlaneTexture3);
            userPlaneTextures.Add(userPlaneTexture4);
            userPlaneTextures.Add(userPlaneTexture5);
            userPlaneTextures.Add(userPlaneTexture6);
            userPlaneTextures.Add(userPlaneTexture7);
            userPlaneTextures.Add(userPlaneTexture8);
            userPlaneTextures.Add(userPlaneTexture9);
            userPlaneTextures.Add(userPlaneTexture10);
            userPlaneTextures.Add(userPlaneTexture11);
            // - enemy plane textures
            enemyPlaneTextures.Add(enemyPlaneTexture1);
            enemyPlaneTextures.Add(enemyPlaneTexture2);
            enemyPlaneTextures.Add(enemyPlaneTexture3);
            enemyPlaneTextures.Add(enemyPlaneTexture4);
            enemyPlaneTextures.Add(enemyPlaneTexture5);
            enemyPlaneTextures.Add(enemyPlaneTexture6);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // - textures
            targetTexture = Content.Load<Texture2D>("target");
            coinTexture = Content.Load<Texture2D>("circle");
            cursorTexture = Content.Load<Texture2D>("cursor");
            plusButtonTexture = Content.Load<Texture2D>("plusbuttonnotpressed");
            minusButtonTexture = Content.Load<Texture2D>("minusbuttonnotpressed");
            plusButtonTextureP = Content.Load<Texture2D>("plusbuttonpressed");
            minusButtonTextureP = Content.Load<Texture2D>("minusbuttonpressed");
            dimScreen = Content.Load<Texture2D>("rectangle");
            bulletTexture = Content.Load<Texture2D>("planeBullet");
            planeTrailTexture = Content.Load<Texture2D>("circle");
            exploSpritesheet = Content.Load<Texture2D>("explosion_spritesheet");
            bombTexture = Content.Load<Texture2D>("tile_0012");

            // - user planes
            userPlaneTexture1 = Content.Load<Texture2D>("plane 1");
            userPlaneTexture2 = Content.Load<Texture2D>("plane 2");
            userPlaneTexture3 = Content.Load<Texture2D>("plane 3");
            userPlaneTexture4 = Content.Load<Texture2D>("plane 4");
            userPlaneTexture5 = Content.Load<Texture2D>("plane 5");
            userPlaneTexture6 = Content.Load<Texture2D>("plane 6");
            userPlaneTexture7 = Content.Load<Texture2D>("plane 7");
            userPlaneTexture8 = Content.Load<Texture2D>("plane 8");
            userPlaneTexture9 = Content.Load<Texture2D>("plane 9");
            userPlaneTexture10 = Content.Load<Texture2D>("plane 10");
            userPlaneTexture11 = Content.Load<Texture2D>("plane 11");
            // - enemy planes
            enemyPlaneTexture1 = Content.Load<Texture2D>("enemy plane 1");
            enemyPlaneTexture2 = Content.Load<Texture2D>("enemy plane 2");
            enemyPlaneTexture3 = Content.Load<Texture2D>("enemy plane 3");
            enemyPlaneTexture4 = Content.Load<Texture2D>("enemy plane 4");
            enemyPlaneTexture5 = Content.Load<Texture2D>("enemy plane 5");
            enemyPlaneTexture6 = Content.Load<Texture2D>("enemy plane 6");



            // - fonts
            pointsFont = Content.Load<SpriteFont>("points");
            pointNumbers = Content.Load<SpriteFont>("pointNumbers");
            followingFont = Content.Load<SpriteFont>("following");
            upgradeMenuFont = Content.Load<SpriteFont>("upgrade menu");
            upgradeMenuInfoFont = Content.Load<SpriteFont>("upgrade_menu_info");
            currentFont = Content.Load<SpriteFont>("Current Font");
            availablePointsFont = Content.Load<SpriteFont>("Available Points");
            menuTitleFont = Content.Load<SpriteFont>("Menu Title Font");

            // - sounds
            planeShot = Content.Load<SoundEffect>("planeShot");
            enemyPlaneShot = Content.Load<SoundEffect>("enemyPlaneShot");
            bombExplosion = Content.Load<SoundEffect>("explosion");
            engineSound = Content.Load<SoundEffect>("planeEngine");
            plusButton = Content.Load<SoundEffect>("PlusButtonSound");
            minusButton = Content.Load<SoundEffect>("MinusButtonSound");

            static void SpriteSheet(GraphicsDevice graphicsDevice, Texture2D _texture, List<Texture2D> _textureList, int imageCount)
            {
                Texture2D cropTexture;
                Rectangle sourceRect;
                int width = _texture.Width / imageCount;
                int height = _texture.Height;

                for (int x = 0; x < imageCount; x++)
                {
                    sourceRect = new Rectangle(x * width, 0, width, height);
                    cropTexture = new Texture2D(graphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    _texture.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    _textureList.Add(cropTexture);
                }
            }

            SpriteSheet(GraphicsDevice, exploSpritesheet, exploTexturesList = new List<Texture2D>(), 5);

            engineSoundInstance = engineSound.CreateInstance();
            engineSoundInstance.IsLooped = true;
            engineSoundInstance.Volume = 1f;
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            prevKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            mouse = new Point(mouseState.X, mouseState.Y);
            mousePos = new Vector2(mouseState.X, mouseState.Y);

            userPlane = userPlaneTexture1;

            coinSpawnCircle = new Vector2(coinSpawnX + coinRect.Width / 2, coinSpawnY + coinRect.Height / 2);

            mouseCircle = new Circle(mousePos, 10);
            coinCircle = new Circle(coinSpawnCircle, 30);
            userHitbox = new Circle(planeLocation, userPlane.Width / 2);

            timer.Start(); elapsed = timer.Elapsed;

            cursorRect.X = mouseState.X - 16;
            cursorRect.Y = mouseState.Y - 16;
            cursorHoverRect.X = mouseState.X - 14;
            cursorHoverRect.Y = mouseState.Y - 14;

            buttonHover = false;

            groundCrosshair.X = planeLocation.X;
            groundCrosshair.Y = planeLocation.Y + 30;

            if (screen == Screen.Intro)
                IntroScreenUpdate(gameTime);

            if (screen == Screen.Game)
                GameScreenUpdate(gameTime);

            if (screen == Screen.Upgrade)
                UpgradeScreenUpdate(gameTime);

            if (screen == Screen.Pause)
                PauseScreenUpdate(gameTime);

            if (screen == Screen.Outro)
                OutroScreenUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            _spriteBatch.Begin();

            if (screen == Screen.Intro)
                IntroScreenDraw(gameTime);

            if (screen == Screen.Game)
                GameScreenDraw(gameTime);

            if (screen == Screen.Upgrade)
                UpgradeScreenDraw(gameTime);

            if (screen == Screen.Pause)
                PauseScreenDraw(gameTime);

            if (screen == Screen.Outro)
                OutroScreenDraw(gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        // - UPDATE AND DRAW METHODS ----------------------------------------------------------------------------------------------------------------

        // - Updates
        public void IntroScreenUpdate(GameTime gameTime)
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
        public void GameScreenUpdate(GameTime gameTime)
        {
            engineSoundInstance.Play();

            timer2.Start(); elapsed2 = timer2.Elapsed;
            timer3.Start(); elapsed3 = timer3.Elapsed;
            timer4.Start(); elapsed4 = timer4.Elapsed;
            timer5.Start(); elapsed5 = timer5.Elapsed;
            timer6.Start(); elapsed6 = timer6.Elapsed;

            // - checking if mouse is in screen when target is attempted to be created
            if (mousePos.X > 0 && mousePos.X < 1080 && mousePos.Y > 0 && mousePos.Y < 720)
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

            // - rotation and direction of plane
            planeDirection = target - planeLocation;
            planeDirection.Normalize();
            origin = new Vector2(userPlane.Width / 2, userPlane.Height / 2);

            // - boost
            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                engineSoundInstance.Pitch = 0.7f;
                if (boostAmount > 0)
                    planeLocation += planeDirection * 2;
                else
                    planeLocation += planeDirection;
                boostAmount -= 1;
                if (boostAmount < 0)
                    boostAmount = 0;
                if (boostAmount == 0)
                    engineSoundInstance.Pitch = 0.4f;
            }
            else
            {
                engineSoundInstance.Pitch = 0.4f;
                planeLocation += planeDirection;
                boostAmount += 1;
                if (boostAmount > totalBoost)
                    boostAmount = totalBoost;
            }

            // - once plane reaches cursor, rotation and movement stops
            if (mouseCircle.Contains(planeLocation) && targetBool == false)
            {
                angle = prevAngle;
                planeLocation = prevPlaneLocation;
            }

            // - if plane reaches target >> target = mouse again
            if (targetBool == true)
                if (targetCircle.Contains(planeLocation))
                    targetBool = false;

            // - swapping to upgrade menu / back
            if (keyboardState.IsKeyDown(Keys.Tab))
                screen = Screen.Upgrade;
            else
                screen = Screen.Game;

            // - previous action things
            prevPlaneLocation = planeLocation;
            prevAngle = angle;

            // - coins and coin points
            if (coinCircle.Contains(planeLocation))
            {
                coinPoints = new Vector2(coinSpawnX - 40, coinSpawnY - 20);

                coinSpawnX = generator.Next(60, 1020);
                coinSpawnY = generator.Next(60, 660);

                coinRect = new Rectangle(coinSpawnX, coinSpawnY, 30, 30);

                points += 100;
                totalPoints += 100;
                timer.Restart();
                coinPointsBool = true;
            }

            // - plane shadow and trail
            planeShadowLocation = new Vector2(planeLocation.X - 30, planeLocation.Y + 75);

            if (elapsed3.TotalSeconds >= 0.4)
            {
                if (planeTrail.Count < 12)
                    planeTrail.Add(new Trail(planeTrailTexture, planeLocation, Color.Black));
                for (int i = 0; i < planeTrail.Count; i++)
                {
                    if (planeTrail[i].getAlpha <= 0f)
                    {
                        planeTrail.RemoveAt(i);
                    }
                }
                timer3.Restart();
            }

            // - bullets
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                bulletBool = true;
                if (planeAmmo > 0)
                {
                    if (targetBool)
                        bullets.Add(new Bullet(bulletTexture, planeLocation, target, 3, planeShot));
                    else 
                        bullets.Add(new Bullet(bulletTexture, planeLocation, mouseState.Position.ToVector2(), 3, planeShot));
                    planeAmmo -= 1;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i].BulletLocation.X > 1180 || bullets[i].BulletLocation.X < -100 || bullets[i].BulletLocation.Y > 820 || bullets[i].BulletLocation.Y < -100)
                    bullets.RemoveAt(i);

            if (bulletBool)
                foreach (Bullet bullet in bullets)
                    bullet.Update();

            // - enemies
            if (elapsed2.TotalSeconds >= 5)
            {
                enemyPlane = enemyPlaneTextures[generator.Next(0, enemyPlaneTextures.Count)];

                enemyPlanes.Add(new EnemyPlane(enemyPlane, bulletTexture, 2, enemyPlaneShot, gameTime));
                timer2.Restart();
            }
            
            // - enemy bullets hitting me
            foreach (EnemyPlane enemy in enemyPlanes)
            {
                for (int i = 0; i < enemy.GetBulletCount; i++)
                {
                    if (userHitbox.Contains(enemy[i]) && !enemy.Contains(planeLocation))
                    {
                        userHit = true;
                        enemy.HitIndex = i;
                        planeHealth -= 25;
                    }
                }
            }
           
            // - user bullets hitting enemies
            foreach (EnemyPlane enemy in enemyPlanes)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (enemy.Contains(bullets[i].BulletLocation))
                    {
                        enemyHitBool = true;
                        bullets.RemoveAt(i);
                        timer4.Restart();
                        damagePoints = new Vector2(enemy.GetLocation.X + 20, enemy.GetLocation.Y + 30);
                        enemy.planeHealth -= 50;
                    }
                }
            }

            // - crashing into enemies
            foreach (EnemyPlane enemy in enemyPlanes)
            {
                if (enemy.Contains(planeLocation))
                {
                    crashBool = true;
                    enemyPlanes.Remove(enemy);
                    planeHealth -= 75;
                    break;
                }
            }

            // - deleting enemies off screen
            for (int i = 0; i < enemyPlanes.Count; i++)
            {
                if (enemyPlanes[i].GetLocation.X > 1180 || enemyPlanes[i].GetLocation.X < -100 || enemyPlanes[i].GetLocation.Y > 820 || enemyPlanes[i].GetLocation.Y < -100)
                    enemyPlanes.RemoveAt(i);
            }

            // - killing enemies
            for (int i = 0; i < enemyPlanes.Count; i++)
            {
                if (enemyPlanes[i].planeHealth <= 0)
                {
                    killPoints = new Vector2(enemyPlanes[i].GetLocation.X - 40, enemyPlanes[i].GetLocation.Y - 20);
                    enemyPlanes.RemoveAt(i);
                    points += 250;
                    timer4.Restart();
                    enemyKillBool = true;
                }
            }

            // - plane trail and health correlation
            foreach (Trail circle in planeTrail)
            {
                if (planeHealth > totalPlaneHealth * 0.75)
                    circle.SetColor = Color.White;
                if (planeHealth > totalPlaneHealth * 0.50 && planeHealth <= totalPlaneHealth * 0.75)
                    circle.SetColor = Color.LightGray;
                if (planeHealth > totalPlaneHealth * 0.25 && planeHealth <= totalPlaneHealth * 0.50)
                    circle.SetColor = Color.Gray;
                if (planeHealth <= totalPlaneHealth * 0.25)
                    circle.SetColor = Color.Black;
            }

            // - updating enemies and bullets and trail
            foreach (EnemyPlane plane in enemyPlanes)
                plane.Update(gameTime);

            foreach (Trail PlaneTrail in planeTrail)
                PlaneTrail.Update();

            foreach (Bomb explosion in explosionsList)
                explosion.Update(gameTime);

            // - bombs
            if (keyboardState.IsKeyDown(Keys.M) && prevKeyboardState.IsKeyUp(Keys.M))
            {
                if (bombs >= 1)
                {
                    explosion = true;
                    explosionsList.Add(new Bomb(exploTexturesList, new Vector2(planeLocation.X, planeLocation.Y), bombTexture, angle, 0.05, bombExplosion));
                    timer5.Restart();
                    bombs -= 1;
                }
            }
            if (explosion)
            {
                for (int i = 0; i < explosionsList.Count; i++)
                {
                    if (explosionsList[i].Done())
                        explosionsList.RemoveAt(i);
                }
            }

            // - game over(health = 0)
            if (planeHealth <= 0)
            {
                screen = Screen.Outro;
            }

            // - random testing



            // - pause menu
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                screen = Screen.Pause;
            }

            // - ending game
            if (keyboardState.IsKeyDown(Keys.RightControl) && prevKeyboardState.IsKeyUp(Keys.RightControl))
            {
                screen = Screen.Outro;
            }
        }
        public void UpgradeScreenUpdate(GameTime gameTime)
        {
            engineSoundInstance.Stop();
            foreach (EnemyPlane enemy in enemyPlanes)
            {
                enemy.enemyTimer.Stop();
                enemy.enemyTimer2.Stop();
                timer2.Stop();
            }

            for (int i = 0; i < 8; i++)
            {
                if (upgradeButtons[i].IsHovering(mouse))
                    buttonHover = true;
            }

            // - row 1 (health cost 500 for +50, return for 400)
            if (upgradeButtons[4].IsPressed())
                if (points >= 500 && totalPlaneHealth < 500)
                {
                    totalPlaneHealth += 50;
                    if (planeHealth == totalPlaneHealth - 50)
                        planeHealth = totalPlaneHealth;
                    points -= 500;
                }
            if (upgradeButtons[0].IsPressed())
                if (totalPlaneHealth >= 150)
                {
                    totalPlaneHealth -= 50;
                    points += 400;
                }
            // - row 2 (boost cost 200 for +25, return for 100)
            if (upgradeButtons[5].IsPressed())
                if (points >= 200 && totalBoost < 1000)
                {
                    totalBoost += 25;
                    points -= 200;
                }
            if (upgradeButtons[1].IsPressed())
                if (totalBoost >= 225)
                {
                    totalBoost -= 25;
                    points += 100;
                }
            // - row 3 (ammo cost 200 for + 5, return for 50/bullet)
            if (upgradeButtons[6].IsPressed())
                if (planeAmmo < 40 && points >= 200)
                {
                    planeAmmo += 5;
                    points -= 200;
                }
            if (upgradeButtons[2].IsPressed())
                if (planeAmmo > 1)
                {
                    planeAmmo -= 2;
                    points += 50;
                }
            // - row 4 (bombs cost 400 for + 1, return for 300)
            if (upgradeButtons[7].IsPressed())
                if (bombs < 10 && points >= 400)
                {
                    bombs += 1;
                    points -= 400;
                }
            if (upgradeButtons[3].IsPressed())
                if (bombs >= 1)
                {
                    bombs -= 1;
                    points += 300;
                }

            for (int i = 0; i < 8; i++)
                upgradeButtons[i].Update();

            if (keyboardState.IsKeyUp(Keys.Tab))
            {
                foreach (EnemyPlane enemy in enemyPlanes)
                {
                    enemy.enemyTimer.Start();
                    enemy.enemyTimer2.Start();
                    timer2.Start();
                }
                screen = Screen.Game;
            }
        }
        public void PauseScreenUpdate(GameTime gameTime)
        {
            foreach (EnemyPlane enemy in enemyPlanes)
            {
                enemy.enemyTimer.Stop();
                enemy.enemyTimer2.Stop();
                timer2.Stop();
            }

            if (keyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
                screen = Screen.Game;
        }
        public void OutroScreenUpdate(GameTime gameTime)
        {

        }
        // - Draws
        public void IntroScreenDraw(GameTime gameTime)
        {
            // - game title
            _spriteBatch.DrawString(menuTitleFont, "To The Clouds", new Vector2(60, 80), Color.White);

            // - cursor
            if (buttonHover == true)
                _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
            else
                _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);

            // - screen fade
            _spriteBatch.Draw(dimScreen, dimScreenRect, colour);
        }
        public void GameScreenDraw(GameTime gameTime)
        {
            // - coins
            _spriteBatch.Draw(coinTexture, coinRect, Color.White);

            // - pin
            if (targetBool == true)
                _spriteBatch.Draw(targetTexture, targetRect, Color.White);

            // - plane shadow
            _spriteBatch.Draw(userPlane, planeShadowLocation, null, Color.Black * 0.4f, angle, origin, 1f, SpriteEffects.None, 0f);

            // - ground crosshair
            _spriteBatch.Draw(coinTexture, new Rectangle((int)groundCrosshair.X, (int)groundCrosshair.Y, 5, 5), Color.Black * 0.6f);

            // - bombs
            if (explosion)
                foreach (Bomb explosion in explosionsList)
                    explosion.Draw(_spriteBatch);

            // - enemy planes
            foreach (EnemyPlane plane in enemyPlanes)
                plane.Draw(_spriteBatch);

            // - bullets
            foreach (Bullet bullet in bullets)
                bullet.Draw(_spriteBatch);

            // - plane trail
            foreach (Trail PlaneTrail in planeTrail)
                PlaneTrail.Draw(_spriteBatch);

            // - plane
            _spriteBatch.Draw(userPlane, planeLocation, null, Color.White, angle, origin, 1f, SpriteEffects.None, 0f);


            // ---- HUD ------------------------------

            // - hud points
            _spriteBatch.DrawString(pointsFont, $"Points :  {points}", new Vector2(40, 40), Color.Black);

            // - points numbers
            if (elapsed.TotalSeconds < 3 && coinPointsBool == true)
                _spriteBatch.DrawString(pointNumbers, "+100", coinPoints, Color.White);
            if (elapsed4.TotalSeconds < 3 && enemyKillBool == true)
                _spriteBatch.DrawString(pointNumbers, "+250", killPoints, Color.White);

            // - boost amount
            _spriteBatch.DrawString(followingFont, $"Boost: {boostAmount}", new Vector2(40, 100), Color.White);

            // - plane target
            if (targetBool == true)
                _spriteBatch.DrawString(followingFont, "Following: Pin", new Vector2(40, 150), Color.White);
            else
                _spriteBatch.DrawString(followingFont, "Following: Cursor", new Vector2(40, 150), Color.White);

            // - plane health 
            _spriteBatch.DrawString(followingFont, $"Health: {planeHealth}", new Vector2(40, 200), Color.White);

            // - plane ammo
            _spriteBatch.DrawString(followingFont, $"Ammo: {planeAmmo}", new Vector2(40, 250), Color.White);

            // - bomb count
            _spriteBatch.DrawString(followingFont, $"Bombs: {bombs}", new Vector2(40, 300), Color.White);




            // - Testing area -------------------------------------------------------------------------------------------




            if (crashBool)
                _spriteBatch.DrawString(pointsFont, "BOOOOOOOOOOM", new Vector2(200, 400), Color.White);





            // - hitting enemies
            if (elapsed4.TotalSeconds < 2 && enemyHitBool == true)
                _spriteBatch.DrawString(pointNumbers, "50", damagePoints, Color.DarkRed);

            // - cursor
            if (buttonHover == true)
                _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
            else
                _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
        }
        public void UpgradeScreenDraw(GameTime gameTime)
        {
            GameScreenDraw(gameTime);

            // - dim areas
            _spriteBatch.Draw(dimScreen, dimScreenRect, Color.Black * 0.3f);
            _spriteBatch.Draw(dimScreen, upgradeMenuRect, Color.Black * 0.5f);
            _spriteBatch.Draw(dimScreen, upgradeMenuPointsRect, Color.Black * 0.5f);
            // buttons
            for (int i = 0; i < 8; i++)
                upgradeButtons[i].Draw(_spriteBatch);
            // lines
            _spriteBatch.DrawString(upgradeMenuFont, "Health   ..........................................", new Vector2(140, 250), Color.White);
            _spriteBatch.DrawString(upgradeMenuFont, "Boost     ..........................................", new Vector2(140, 330), Color.White);
            _spriteBatch.DrawString(upgradeMenuFont, "Ammo       ..........................................", new Vector2(140, 410), Color.White);
            _spriteBatch.DrawString(upgradeMenuFont, "Bombs    ..........................................", new Vector2(140, 490), Color.White);
            // available points
            _spriteBatch.DrawString(availablePointsFont, $"Available Points: {points}", new Vector2(110, 615), Color.White);
            // hover info area
            if (upgradeButtons[0].IsHovering(mouse) || upgradeButtons[4].IsHovering(mouse))
            {
                _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                _spriteBatch.DrawString(followingFont, "Plane Health", new Vector2(700, 220), Color.White);
                _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +50\r\nmax health cap.\r\nCost: 500 Points\r\n\r\nClicking the ''-'' will take -50\r\nfrom your max health cap, and\r\ngive you some credits back.\r\nRefund: 400 Points ", new Vector2(700, 250), Color.White);
                _spriteBatch.DrawString(currentFont, "Current Max\r\nHealth:", new Vector2(700, 600), Color.White);
                _spriteBatch.DrawString(pointsFont, totalPlaneHealth.ToString(), new Vector2(700, 660), Color.White);
            }
            if (upgradeButtons[1].IsHovering(mouse) || upgradeButtons[5].IsHovering(mouse))
            {
                _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                _spriteBatch.DrawString(followingFont, "Boost", new Vector2(700, 220), Color.White);
                _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +25\r\nmax boost cap.\r\nCost: 200 Points\r\n\r\nClicking the ''-'' will take -25\r\nfrom your max boost cap, and \r\ngive you some credits back.\r\nRefund: 100 Points ", new Vector2(700, 250), Color.White);
                _spriteBatch.DrawString(currentFont, "Current Max\r\nBoost:", new Vector2(700, 600), Color.White);
                _spriteBatch.DrawString(pointsFont, totalBoost.ToString(), new Vector2(700, 660), Color.White);
            }
            if (upgradeButtons[2].IsHovering(mouse) || upgradeButtons[6].IsHovering(mouse))
            {
                _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                _spriteBatch.DrawString(followingFont, "Ammo", new Vector2(700, 220), Color.White);
                _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +5\r\nammo.\r\nCost: 200 Points\r\n\r\nClicking the ''-'' will take -2 ammo,\r\nand give you some credits back.\r\nRefund: 50 Points ", new Vector2(700, 250), Color.White);
                _spriteBatch.DrawString(currentFont, "Current Ammo:", new Vector2(700, 600), Color.White);
                _spriteBatch.DrawString(pointsFont, planeAmmo.ToString(), new Vector2(700, 660), Color.White);
            }
            if (upgradeButtons[3].IsHovering(mouse) || upgradeButtons[7].IsHovering(mouse))
            {
                _spriteBatch.Draw(dimScreen, upgradeMenuInfoRect, Color.Black * 0.8f);
                _spriteBatch.DrawString(followingFont, "Bombs", new Vector2(700, 220), Color.White);
                _spriteBatch.DrawString(upgradeMenuInfoFont, "Clicking the ''+'' will give you +1\r\nbomb.\r\nCost: 400 Points\r\n\r\nClicking the ''-'' will take -1\r\nbomb, and \r\ngive you some credits back.\r\nRefund: 300 Points ", new Vector2(700, 250), Color.White);
                _spriteBatch.DrawString(currentFont, "Current Bombs:", new Vector2(700, 600), Color.White);
                _spriteBatch.DrawString(pointsFont, bombs.ToString(), new Vector2(700, 660), Color.White);
            }

            if (buttonHover == true)
                _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
            else
                _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
        }
        public void PauseScreenDraw(GameTime gameTime)
        {
            GameScreenDraw(gameTime);

            _spriteBatch.Draw(dimScreen, dimScreenRect, Color.Black * 0.3f);
        }
        public void OutroScreenDraw(GameTime gameTime)
        {
            _spriteBatch.DrawString(menuTitleFont, "Game Over!", new Vector2(350, 200), Color.Black);

            _spriteBatch.DrawString(menuTitleFont, $"You got {totalPoints} points!", new Vector2(275, 350), Color.White);

            // - cursor
            if (buttonHover == true)
                _spriteBatch.Draw(cursorTexture, cursorHoverRect, Color.DarkGray);
            else
                _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
        }
    }
}