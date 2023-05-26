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
        private Texture2D _texture;
        private Vector2 _rotation;
        private Rectangle _rectangle;
        private Vector2 _location;
        private Vector2 _target;
        private Vector2 _velocity;

        public Bullet(Texture2D texture, Vector2 location, Vector2 target)
        {
            _texture = texture;
            _location = location;
            _target = target;

            _rectangle = new Rectangle((int)_location.X, (int)_location.Y, 16, 16);

            _velocity = new Vector2((_target.X - _location.X) / Vector2.Distance(_location, _target), (_target.Y - _location.Y) / Vector2.Distance(_location, _target));
        }

        public void Update()
        {
            _location.X += _velocity.X;
            _location.Y += _velocity.Y;

            _rectangle.X = (int)Math.Round(_location.X);
            _rectangle.Y = (int)Math.Round(_location.Y); 

        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
