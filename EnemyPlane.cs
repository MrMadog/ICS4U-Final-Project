﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ICS4U_Final_Project
{
    internal class EnemyPlane
    {
        Random generator = new Random();
        private int _level;
        private int _speed;
        private int _bulletSpeed;
        private int _planeHealth;
        private Texture2D _planeTexture;
        private Texture2D _bulletTexture;
        private Vector2 _location;
        private Vector2 _bulletLocation;
        private Vector2 _shadowLocation;
        private Vector2 _velocity;
        private Vector2 _bulletVelocity;
        private Vector2 _rotationOrigin;
        private Vector2 _target;
        private Circle _hitbox;
        private float _rotation, seconds, startTime;
        private List<Bullet> enemyBullets = new List<Bullet>();
        private bool bulletBool = false;
        private SoundEffect _bulletSound;

        public EnemyPlane(Texture2D planeTexture, Texture2D bulletTexture, int level, SoundEffect bulletSound)
        {
            _planeTexture = planeTexture;
            _bulletTexture = bulletTexture;
            _level = level;
            _bulletSound = bulletSound;

            _hitbox = new Circle(_location, _planeTexture.Width / 2);

            _location = new Vector2(0, generator.Next(0, 720));

            _bulletLocation = _location;
            _bulletLocation.X += _planeTexture.Width / 2;
            _bulletLocation.X -= _bulletTexture.Width / 2;
            _bulletLocation.Y += _planeTexture.Height / 2;
            _bulletLocation.Y -= _bulletTexture.Height / 2;

            _target = new Vector2(1280, generator.Next(0, 720));

            switch (_level)
            {
                case 1: _speed = 1; planeHealth = 100; _bulletSpeed = 2; break;
                case 2: _speed = 2; planeHealth = 200; _bulletSpeed = 3; break;
                case 3: _speed = 3; planeHealth = 400; _bulletSpeed = 4; break;
                default: _speed = 1; planeHealth = 100; _bulletSpeed = 2; break;
            }


            _velocity = new Vector2((_target.X - _location.X) / Vector2.Distance(_location, _target) * _speed, (_target.Y - _location.Y) / Vector2.Distance(_location, _target) * _speed);
            //_bulletVelocity = new Vector2((_target.X - _location.X) / Vector2.Distance(_location, _target) * _bulletSpeed, (_target.Y - _location.Y) / Vector2.Distance(_location, _target) * _bulletSpeed);

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
            return _hitbox.Contains(point);
        }

        public Vector2 GetLocation
        {
            get { return _location; }
        }

        public int planeHealth
        {
            get { return _planeHealth;  }
            set { _planeHealth = value; }
        }

        public void Update(GameTime gameTime)
        {
            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;


            _location.X += _velocity.X;
            _location.Y += _velocity.Y;

            _shadowLocation.X = _location.X - 30;
            _shadowLocation.Y = _location.Y + 75;

            _bulletLocation.X += _velocity.X;
            _bulletLocation.Y += _velocity.Y;

            if (seconds >= 5)
            {
                enemyBullets.Add(new Bullet(_bulletTexture, _bulletLocation, _target, 3, _bulletSound));
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            foreach (Bullet bullet in enemyBullets)
                bullet.Update();

            _hitbox.Center = _location;
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_planeTexture, _location, null, Color.White, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane
            _spriteBatch.Draw(_planeTexture, _shadowLocation, null, Color.Black * 0.4f, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane shadow

            foreach (Bullet bullet in enemyBullets)
                bullet.Draw(_spriteBatch);
        }

    }
}
