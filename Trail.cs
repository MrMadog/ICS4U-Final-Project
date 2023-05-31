using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ICS4U_Final_Project
{
    internal class Trail
    {
        private Rectangle _rectangle;
        private Vector2 _spawnLocation;
        private Texture2D _texture;
        float alpha;

        public Trail(Texture2D texture, Vector2 spawnLocation)
        {
            _texture = texture;
            _spawnLocation = spawnLocation;

            _rectangle = new Rectangle((int)_spawnLocation.X, (int)_spawnLocation.Y, 15, 15);
            alpha = 1f;

        }

        public int circleWidth
        {
            get { return _rectangle.Width; }
        }

        public void Update()
        {
            _rectangle.Width += 1;
            _rectangle.Height += 1;

            //_rectangle.X -= 1;
            //_rectangle.Y -= 1;
            alpha -= 0.007f;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _rectangle, Color.White * alpha);
        }
    }
}
