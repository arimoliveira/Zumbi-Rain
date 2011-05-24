using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace primeiro2DAriane
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class zumbi : Microsoft.Xna.Framework.DrawableGameComponent
    {

        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected int Yspeed;
        protected int Xspeed;
        protected Random random;
        protected SpriteBatch sBatch;

        //largura e altura do sprite na textura
        protected const int ZUMBIWIDHT = 48;
        protected const int ZUMBIHEIGHT = 80;


        public zumbi(Game game, ref Texture2D theTexture)
            : base(game)
        {
            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();

            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));


            //criando o retângulo fonte
            //isto representa aonde o sprite da figura estará na superfície

            spriteRectangle = new Rectangle(ZUMBIWIDHT - 47, ZUMBIHEIGHT - 79, ZUMBIWIDHT, ZUMBIHEIGHT);

            //INICIALIZAR O NÚMERO RAMDOMICO DE ONDE ELE VAI APARECER NA TELA
            //SUA POSIÇÃO INICIAL
            random = new Random(this.GetHashCode());
            PutinStartPosition();
            
        }

        /// <summary>
        /// inicializa a posição e velocidade do zumbi
       /// </summary>
        public void PutinStartPosition()
        {
            position.X = random.Next(Game.Window.ClientBounds.Width - ZUMBIWIDHT);
            position.Y = 0;
            Yspeed = 1 + random.Next(9);
            Xspeed = random.Next(3) -1;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }


        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Draw the zumbi
            sBatch.Draw(texture, position, spriteRectangle, Color.White);

            base.Draw(gameTime);
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Check if the zumbi still visible
            if ((position.Y >= Game.Window.ClientBounds.Height) ||
                (position.X >= Game.Window.ClientBounds.Width) || (position.X <= 0))
            {
                PutinStartPosition();
            }

            // Move zumbi
            position.Y += Yspeed;
            position.X += Xspeed;


            base.Update(gameTime);
        }

        /// <summary>
        /// Check if the meteor intersects with the specified rectangle
        /// </summary>
        /// <param name="rect">test rectangle</param>
        /// <returns>true, if has a collision</returns>
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y,
                ZUMBIWIDHT, ZUMBIHEIGHT);
            return spriterect.Intersects(rect);
        }



    }
}