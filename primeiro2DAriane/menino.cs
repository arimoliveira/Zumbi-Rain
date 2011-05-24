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
    public class menino : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected SpriteBatch sBatch;

        //largura e altura do sprite na textura
        protected const int MENINOWIDHT = 79;
        protected const int MENINOHEIGHT = 100;

        //area de tela
        protected Rectangle screenBounds;

        public menino(Game game, ref Texture2D theTexture)
            : base(game)
        {
            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();
            
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            //criando o retângulo fonte
            //isto representa aonde o sprite da figura estará na superfície

            spriteRectangle = new Rectangle(MENINOWIDHT - 78, MENINOHEIGHT-99, MENINOWIDHT, MENINOHEIGHT);
            

            screenBounds = new Rectangle(0, 0, 800,600);

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 

        
        public void PutinStartPosition()
        {
            position.X = screenBounds.Width / 2;
            position.Y = screenBounds.Height - MENINOHEIGHT;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Up))
            {
                position.Y -= 3;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                position.Y += 3;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                position.X -= 3;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                position.X += 3;
            }

            //Mantem o menino dentro da tela

            if (position.X < screenBounds.Left)
            {
                position.X = screenBounds.Left;
            }
            if (position.X > screenBounds.Width - MENINOWIDHT)
            {
                position.X = screenBounds.Width - MENINOWIDHT;
            }
            if (position.Y < screenBounds.Top)
            {
                position.Y = screenBounds.Top;
            }
            if (position.Y > screenBounds.Height - MENINOHEIGHT)
            {
                position.Y = screenBounds.Height - MENINOHEIGHT;
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Desenha o sprite do menino
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            //desenha o menino
            sBatch.Draw(texture, position, spriteRectangle, Color.White);

            base.Draw(gameTime);
        }
        /// <summary>
        /// Retorna um retângulo com o tamanho do menino na tela
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, MENINOWIDHT, MENINOHEIGHT);
        }



    }
}