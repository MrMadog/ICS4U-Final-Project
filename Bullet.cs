using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;


namespace ICS4U_Final_Project
{
    internal class Bullet
    {
        MouseState mouseState;

        private Texture2D _texture;
        private float rotation;
        private Vector2 _target;
        private Vector2 _position;
        private Vector2 _origin;
        private Rectangle rectangle;
        private Vector2 direction;

        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;

            float run = secondPoint.X - originPoint.X;

            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            else
                return (float)(Math.PI + Math.Atan(rise / run));
        }

        public Bullet(Texture2D texture, Vector2 origin, Vector2 position, Vector2 target)
        {
            _texture = texture;
            _origin = origin;
            _target = target;
            _position = position;

            direction = _target - _origin;
            rectangle = new Rectangle((int)origin.X, (int)origin.Y, 16, 16);
        }

        public void Update()
        {
            rotation = GetAngle(_origin, new Vector2(_target.X, _target.Y));

            _position += direction;



            rectangle.X = (int)_position.X;
            rectangle.Y = (int)_position.Y;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, rectangle, null, Color.White, rotation, new Vector2(_texture.Width/2, _texture.Height/2), SpriteEffects.None, 0f);
        }
    }
}
