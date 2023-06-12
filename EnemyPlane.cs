using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ICS4U_Final_Project
{
    internal class EnemyPlane
    {
        Random generator = new Random();
        private int _level;
        private int _speed;
        private int _planeHealth;
        private Texture2D _planeTexture;
        private Texture2D _bulletTexture;
        private Vector2 _location;
        private Vector2 _bulletLocation;
        private Vector2 _shadowLocation;
        private Vector2 _velocity;
        private Vector2 _rotationOrigin;
        private Vector2 _target;
        private Circle _planeHitbox;
        private float _rotation, seconds, startTime, seconds2, startTime2;
        private List<Bullet> enemyBullets = new List<Bullet>();
        private bool bulletBool = false;
        private bool bulletBool2 = false;
        private SoundEffect _bulletSound;

        public EnemyPlane(Texture2D planeTexture, Texture2D bulletTexture, int level, SoundEffect bulletSound, GameTime gameTime)
        {
            _planeTexture = planeTexture;
            _bulletTexture = bulletTexture;
            _level = level;
            _bulletSound = bulletSound;

            _planeHitbox = new Circle(_location, _planeTexture.Width / 2);

            _location = new Vector2(0, generator.Next(0, 720));

            _bulletLocation = _location;

            _target = new Vector2(1280, generator.Next(0, 720));

            startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            startTime2 = (float)gameTime.TotalGameTime.TotalSeconds;

            switch (_level)
            {
                case 1: _speed = 1; planeHealth = 100; break;
                case 2: _speed = 2; planeHealth = 200; break;
                case 3: _speed = 3; planeHealth = 400; break;
                default: _speed = 1; planeHealth = 100; break;
            }

            _velocity = new Vector2((_target.X - _location.X) / Vector2.Distance(_location, _target) * _speed, (_target.Y - _location.Y) / Vector2.Distance(_location, _target) * _speed);

            _rotation = GetAngle(_location, _target);

            _rotationOrigin = new Vector2(_planeTexture.Width / 2, _planeTexture.Height / 2);
        }

        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;
            float run = secondPoint.X - originPoint.X;
            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            else
                return (float)(Math.PI + Math.Atan(rise / run));
        }

        public bool Contains(Vector2 point)
        {
            return _planeHitbox.Contains(point);
        }

        public Vector2 GetLocation
        {
            get { return _location; }
        }

        public Vector2 GetBulletLocation
        {
            get { return _bulletLocation; }
        }

        public Vector2 GetTarget
        {
            get { return _target; }
        }

        public int planeHealth
        {
            get { return _planeHealth; }
            set { _planeHealth = value; }
        }

        public void Update(GameTime gameTime)
        {
            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
            seconds2 = (float)gameTime.TotalGameTime.TotalSeconds - startTime2;

            _location.X += _velocity.X;
            _location.Y += _velocity.Y;

            _shadowLocation.X = _location.X - 30;
            _shadowLocation.Y = _location.Y + 75;

            _bulletLocation.X += _velocity.X;
            _bulletLocation.Y += _velocity.Y;
            
            if (seconds >= 0 && bulletBool == false)
            {
                enemyBullets.Add(new Bullet(_bulletTexture, _bulletLocation, _target, 3, _bulletSound));
                bulletBool = true;
            }

            if (seconds2 >= 0.5 && bulletBool2 == false)
            {
                enemyBullets.Add(new Bullet(_bulletTexture, _bulletLocation, _target, 3, _bulletSound));
                bulletBool2 = true;
            }

            if (seconds >= 3)
            {
                enemyBullets.Add(new Bullet(_bulletTexture, _bulletLocation, _target, 3, _bulletSound));
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            if (seconds2 >= 3.5)
            {
                enemyBullets.Add(new Bullet(_bulletTexture, _bulletLocation, _target, 3, _bulletSound));
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                startTime2 = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            for (int i = 0; i < enemyBullets.Count; i++)
            {
                if (enemyBullets[i].BulletLocation.X > 1180 || enemyBullets[i].BulletLocation.X < -100 || enemyBullets[i].BulletLocation.Y > 820 || enemyBullets[i].BulletLocation.Y < -100)
                    enemyBullets.RemoveAt(i);
            }



            foreach (Bullet bullet in enemyBullets)
                bullet.Update();

            _planeHitbox.Center = _location;
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (Bullet bullet in enemyBullets)
                bullet.Draw(_spriteBatch);

            _spriteBatch.Draw(_planeTexture, _location, null, Color.White, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane
            _spriteBatch.Draw(_planeTexture, _shadowLocation, null, Color.Black * 0.4f, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane shadow
        }
    }
}
