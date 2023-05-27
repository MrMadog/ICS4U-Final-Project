using Microsoft.Xna.Framework;
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
    internal class Bullet
    {
        private Texture2D _texture;
        private float _rotation;
        private int _speed;
        private Rectangle _rectangle;
        private Vector2 _location;
        private Vector2 _shadowLocation;
        private Vector2 _target;
        private Vector2 _velocity;
        private Vector2 _rotationOrigin;
        private SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;

        public Bullet(Texture2D texture, Vector2 location, Vector2 target, int speed, SoundEffect soundEffect)
        {
            _texture = texture;
            _location = location;
            _target = target;
            _speed = speed;
            _soundEffect = soundEffect;

            _rectangle = new Rectangle((int)_location.X, (int)_location.Y, 16, 16);

            _velocity = new Vector2((_target.X - _location.X) / Vector2.Distance(_location, _target) * _speed, (_target.Y - _location.Y) / Vector2.Distance(_location, _target) * _speed);

            _rotation = GetAngle(_location, _target);

            _rotationOrigin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            _soundEffectInstance = _soundEffect.CreateInstance();

            _soundEffectInstance.Play();
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

        public Vector2 BulletLocation
        {
            get { return _location; }
        }

        public void Update()
        {
            _location.X += _velocity.X;
            _location.Y += _velocity.Y;

            _shadowLocation.X = _location.X - 30;
            _shadowLocation.Y = _location.Y + 75;

            _rectangle.X = (int)Math.Round(_location.X);
            _rectangle.Y = (int)Math.Round(_location.Y); 
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _location, null, Color.White, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // actual bullet
            _spriteBatch.Draw(_texture, _shadowLocation, null, Color.Black * 0.4f, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // bullet shadow
        }
    }
}
