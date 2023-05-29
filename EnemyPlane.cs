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
    internal class EnemyPlane
    {
        Random generator = new Random();
        private int _level;
        private Texture2D _planeTexture;
        private Texture2D _bulletTexture;
        private Vector2 _location;
        private Vector2 _shadowLocation;
        private Vector2 _velocity;
        private Vector2 _rotationOrigin;
        private float _rotation;
        private List<Bullet> enemyBullets = new List<Bullet>();

        public EnemyPlane(Texture2D planeTexture, Texture2D bulletTexture, int level)
        {
            _planeTexture = planeTexture;
            _bulletTexture = bulletTexture;
            _level = level;

            
        }

        public void Update()
        {


            switch(_level )
            {
                case 1:
                    
                    break;

                case 2:

                    break;

                case 3:

                    break;
            }
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _location, null, Color.White, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane
            _spriteBatch.Draw(_texture, _shadowLocation, null, Color.Black * 0.4f, _rotation, _rotationOrigin, 1f, SpriteEffects.None, 0f);    // enemy plane shadow
        }

    }
}
