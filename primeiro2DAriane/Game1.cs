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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //GRAPHICAL STUFF
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch= null;
        private Texture2D backgroundTexture,zumbiTexture; //fundo da tela
        private Texture2D meninoTexture; //textura do menino
        private SpriteFont gameFont;


        //AUDIO STUFF
        private SoundEffect explosao; //wav que deve ser tocado quando o zumbi colide com o menino
        private SoundEffect novoZumbi; //som que deve ser tocado quando entra um novo zumbi na tela
        private Song backmusic; //musica de fundo do jogo

        //GAME STUFF
        private menino player; //referencia o jogador, no caso o menino
        private const int STARTZUMBICOUNT = 2; //contagem de zumbis
        private int zumbiCount;
        private int zumbiHitCount;
        private int lastTickCount;
        private const int ADDZUMBITIME = 5000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
          //  graphics.PreferredBackBufferHeight = 800;
         //  graphics.PreferredBackBufferWidth = 600;
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("jardim"); 
            meninoTexture = Content.Load<Texture2D>("menino");
            zumbiTexture = Content.Load<Texture2D>("zumbi");

            //FONTE DO JOGO
            gameFont = Content.Load<SpriteFont>("font");

            //INICIALIZANDO OS SONS DO JOGO
            explosao = Content.Load<SoundEffect>("explosion");
            novoZumbi = Content.Load<SoundEffect>("novoZumbi");
            backmusic = Content.Load<Song>("backmusic");

            // Play the background music
            MediaPlayer.Play(backmusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //inicia o jogo se ainda não foi iniciado
            if (player == null)
            {
                Start();
            }
            //chamando a DOGameLogic
            DoGameLogic();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Run the game logic
        /// </summary>
        private void DoGameLogic()
        {
            // Check collisions
            bool hasColision = false;
            Rectangle shipRectangle = player.GetBounds();
            foreach (GameComponent gc in Components)
            {
                if (gc is zumbi)
                {
                    hasColision = ((zumbi)gc).CheckCollision(shipRectangle);
                    if (hasColision)
                    {
                        // ZUMBI PEGOU
                          explosao.Play();
                          zumbiHitCount++;
                        // Shake!
                        //   rumblePad.RumblePad(500, 1.0f, 1.0f);
                        // Remove all previous meteors
                        RemoveAllZumbis();
                        // Let's start again
                        Start();

                        break;
                    }
                }
            }

            // Add a new meteor if is time
            CheckforNewZumbi();
        }

        /// <summary>
        /// Check if is a moment for a new rock!
        /// </summary>
        private void CheckforNewZumbi()
        {
            // Add a rock each ADDMETEORTIME
            if ((System.Environment.TickCount - lastTickCount) > ADDZUMBITIME)
            {
                lastTickCount = System.Environment.TickCount;
                Components.Add(new zumbi(this, ref zumbiTexture));
                //newMeteor.Play();
                zumbiCount++;
                novoZumbi.Play();
            }
        }

        /// <summary>
        /// Remove all Zumbis
        /// </summary>
        private void RemoveAllZumbis()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is zumbi)
                {
                    Components.RemoveAt(i);
                    i--;
                }
            }
        }
        
        /// <summary>
        /// Inicializa o round de jogo
        /// </summary>
            private void Start()
        {
            //Cria (se necessário) e coloca o jogador na posição inicial

            if(player == null)
            {
                //adiciona o componente do jogador
                player = new menino(this, ref meninoTexture);
                Components.Add(player);
            }
            player.PutinStartPosition();
            for (int i = 0; i < STARTZUMBICOUNT; i++)
            {
                Components.Add(new zumbi(this, ref zumbiTexture));
            }

    
         }

       /// <summary>
       /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Draw background texture in a seperate pass
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0,800,600),Color.LightGray);
            spriteBatch.End();


            //começa a renderizar sprites
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //Desenha os componentes do jogo (sprites included)
            base.Draw(gameTime);
            //Termina de desenhar os sprites
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, "Zumbis que colidiram: " + zumbiHitCount.ToString(),
          new Vector2(15,15), Color.Blue);
          spriteBatch.End();
            

        }
      }

         

           
    }


