using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS4U_Final_Project
{
    internal class Bullet
    {
        private Texture2D _texture { get; set; }
        private float _rotation { get; set; }
        private Vector2 _position { get; set; }
        private Rectangle _rectangle { get; set; }
        private Vector2 bulletVelocity;

        public Bullet(Texture2D texture, Rectangle rectangle, Vector2 position, float rotation)
        {
            _texture = texture;
            _rectangle = rectangle;
            _position = position;
            _rotation = rotation;
        }

        public void Update()
        {
            bulletVelocity = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation)) * 5f;

            _position += bulletVelocity;

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _rectangle, null, Color.White, _rotation, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
