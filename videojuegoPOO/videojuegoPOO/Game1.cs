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

namespace videojuegoPOO
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState teclado;

        Jugador jugador;
        float velocidadJugador = 5.0f;

        KeyboardState estadoAnterior;

        Fondo fondoJuego;

       
        TimeSpan tiempoPrevio;
        TimeSpan tiempoMaximo;

        Nivel nivelJuego;

        Menu menu;

        Item marcaNivel;

        SoundEffect disparo;
        SoundEffect explosion;
        Song musicaFondo;

        AnimacionElementos animacionJugador;

        BD guardar;
        List<Item> puntajes = new List<Item>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
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
            try
            {
                jugador = new Jugador();

                tiempoPrevio = TimeSpan.Zero;
                tiempoMaximo = TimeSpan.FromSeconds(0.5f);

                nivelJuego = new Nivel();

                menu = new Menu();
                menu.inicializar(GraphicsDevice.Viewport, Content);

                marcaNivel = new Item(Content, "");

                //agregado para proveer animacion
                animacionJugador = new AnimacionElementos();
                guardar = new BD();
            }
            catch (Exception) { }
            
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

            try
            {
                Texture2D texturaJugador = Content.Load<Texture2D>("Imagenes/naveAnimacion");

                //se divide entre 8 debido a que son 3 imagenes que componen la textura y necesitamos la mitad de una de las texturas
                animacionJugador.Inicialize(texturaJugador, new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (texturaJugador.Width / 8),
                   (graphics.GraphicsDevice.Viewport.Height / 2) - (texturaJugador.Height / 2)), 58, 90, 4, 30, Color.White, 1f, true);

                disparo = Content.Load<SoundEffect>("Sonidos/efectoLaser");

                jugador.inicializar(GraphicsDevice.Viewport, Content, animacionJugador, new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (animacionJugador.FrameWidth / 2),
                   (graphics.GraphicsDevice.Viewport.Height / 2) - (animacionJugador.FrameHeight / 2)), velocidadJugador, disparo);

                fondoJuego = new Fondo();

                fondoJuego.inicializar(Content, "Imagenes/fondoFinal", GraphicsDevice.Viewport.Height, 1);

                explosion = Content.Load<SoundEffect>("Sonidos/efectoExplosion");

                musicaFondo = Content.Load<Song>("Sonidos/gameMusic");
            }
            catch (Exception) { }
            // TODO: use this.Content to load your game content here
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

            try
            {
                estadoAnterior = teclado;

                teclado = Keyboard.GetState();

                if ((nivelJuego.estado == (int)eEstadoNivel.iniciado || nivelJuego.estado == (int)eEstadoNivel.finalizado) && guardar != null && (!guardar.activo))
                    jugador.actualizar(teclado, estadoAnterior, gameTime);

                jugador.animacion.Update(gameTime);

                fondoJuego.actualizar();

                if (nivelJuego != null && nivelJuego.estado == (int)eEstadoNivel.iniciado || nivelJuego.estado == (int)eEstadoNivel.finalizado)
                    actualizarProyectiles(gameTime);

                if (nivelJuego != null && (nivelJuego.estado == (int)eEstadoNivel.iniciado || nivelJuego.estado == (int)eEstadoNivel.finalizado))
                    nivelJuego.actualizar(jugador, gameTime);

                actualizarNivel(gameTime, teclado, estadoAnterior);

                if (nivelJuego != null && !(nivelJuego.estado == (int)eEstadoNivel.iniciado) && !(nivelJuego.estado == (int)eEstadoNivel.finalizado) && menu != null && (!guardar.activo))
                    menu.Actualizar(teclado, estadoAnterior);

                if (guardar.activo)
                    guardar.actualizar(gameTime, teclado, estadoAnterior, jugador);

                foreach (Item p in puntajes)
                {
                    p.posicionItem.Y -= 1;
                    if (p.posicionItem.Y < GraphicsDevice.Viewport.Height / 3)
                        p.aceptada = false;
                }
            }
            catch (Exception) { }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            try
            {
                fondoJuego.Dibujar(spriteBatch);

                if (nivelJuego != null && nivelJuego.estado == (int)eEstadoNivel.iniciado || nivelJuego.estado == (int)eEstadoNivel.finalizado)
                    foreach (Proyectil proyectil in jugador.proyectiles)
                    {
                        proyectil.Dibujar(spriteBatch);
                    }

                jugador.dibujar(spriteBatch);

                //para dibujar aun cuando finalizo el nivel
                if (nivelJuego != null && (nivelJuego.estado == (int)eEstadoNivel.iniciado || nivelJuego.estado == (int)eEstadoNivel.finalizado))
                    nivelJuego.Dibujar(spriteBatch);

                if (!(nivelJuego.estado == (int)eEstadoNivel.iniciado) && !(nivelJuego.estado == (int)eEstadoNivel.finalizado) && menu != null && (!guardar.activo))
                    menu.Dibujar(spriteBatch);

                marcaNivel.Dibujar(spriteBatch, GraphicsDevice.Viewport);

                if (guardar != null && guardar.activo)
                    guardar.dibujar(spriteBatch, GraphicsDevice.Viewport);

                foreach (Item p in puntajes)
                {
                    if (p.posicionItem.Y > GraphicsDevice.Viewport.Height / 3 && p.posicionItem.Y < (GraphicsDevice.Viewport.Height / 3) * 2)
                        p.Dibujar(spriteBatch, GraphicsDevice.Viewport);
                }

                for (int x = 0; x < puntajes.Count; x++)
                {
                    if (!puntajes[x].aceptada)
                        puntajes.RemoveAt(x);
                }
            }
            catch (Exception) { }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        void actualizarProyectiles(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - tiempoPrevio > tiempoMaximo)
            {
                tiempoPrevio = gameTime.TotalGameTime;
                
                if(nivelJuego!=null && nivelJuego.estado == (int)eEstadoNivel.iniciado)
                    nivelJuego.agregarEnemigo();
            }
            
            foreach (Proyectil proyectil in jugador.proyectiles)
            {
                proyectil.Actualizar();
            }
            removerProyectil();
        }

        void removerProyectil()
        {
            for (int elemento = 0; elemento < jugador.proyectiles.Count; elemento++)
            {
                if(jugador.proyectiles[elemento].activo == false)
                    jugador.proyectiles.RemoveAt(elemento);
            }
        }
                               
        //metodo de prueba para controlar los niveles
        void actualizarNivel(GameTime gameTime, KeyboardState teclado, KeyboardState estadoAnterior)
        {

            if (menu == null && puntajes.Count <= 0)
            {
                nivelJuego.iniciar((int)nivel.facil, Content, GraphicsDevice.Viewport);
                nivelJuego.estado = -1;

                menu = new Menu();
                menu.inicializar(GraphicsDevice.Viewport, Content);
                jugador.proyectiles = new List<Proyectil>();
                jugador.vida = 100;
                jugador.puntuacion = 0;
            }

            if (menu != null && teclado.IsKeyDown(Keys.Enter) && estadoAnterior.IsKeyUp(Keys.Enter) && guardar != null && (!guardar.activo))
                if ((int)accionInicial.iniciar == menu.elementoActual && !(nivelJuego.estado == (int)eEstadoNivel.iniciado) && !(nivelJuego.estado == (int)eEstadoNivel.pausa))
                {
                    nivelJuego.iniciar((int)nivel.facil, Content, GraphicsDevice.Viewport);
                    nivelJuego.estado = (int)eEstadoNivel.iniciado;
                    iniciarMusica(musicaFondo);
                }

                else if (nivelJuego.estado == (int)eEstadoNivel.iniciado)
                {
                    MediaPlayer.Pause();
                    menu = null;
                    nivelJuego.estado = (int)eEstadoNivel.pausa;

                    string[] titulos = new string[3] { "Reanudar", "Salir", "" };
                    menu = new Menu();
                    menu.inicializar(GraphicsDevice.Viewport, Content, titulos);
                }

                else if ((int)accionEnPausa.iniciar == menu.elementoActual && nivelJuego.estado == (int)eEstadoNivel.pausa)
                {
                    nivelJuego.estado = (int)eEstadoNivel.iniciado;
                    MediaPlayer.Resume();
                }

                else if ((int)accionEnPausa.salir == menu.elementoActual && nivelJuego.estado == (int)eEstadoNivel.pausa)
                {
                    nivelJuego.iniciar((int)nivel.facil, Content, GraphicsDevice.Viewport);
                    nivelJuego.estado = -1;

                    menu = new Menu();
                    menu.inicializar(GraphicsDevice.Viewport, Content);
                    jugador.proyectiles = new List<Proyectil>();
                    jugador.vida = 100;
                    jugador.puntuacion = 0;
                }
                else if ((int)accionInicial.Puntuacion == menu.elementoActual && !(nivelJuego.estado == (int)eEstadoNivel.iniciado) && !(nivelJuego.estado == (int)eEstadoNivel.pausa))
                {
                    menu = null;

                    obtenerListaPuntuacion();
                }
                else if ((int)accionInicial.salir == menu.elementoActual && nivelJuego.estado == -1)
                    Exit();

            if (jugador.puntuacion > 40 && (nivel)nivelJuego.nivelActual == nivel.facil && jugador.vida > 0)
            {
                nivelJuego.estado = (int)eEstadoNivel.finalizado;
            }

            if (jugador.puntuacion > 90 && (nivel)nivelJuego.nivelActual == nivel.medio && jugador.vida>0)
                nivelJuego.estado = (int)eEstadoNivel.finalizado;

            if (jugador.puntuacion > 150 && (nivel)nivelJuego.nivelActual == nivel.dificil && jugador.vida > 0)
                nivelJuego.estado = (int)eEstadoNivel.finalizado;

            if (jugador.vida <= 0 && nivelJuego.estado == (int)eEstadoNivel.iniciado)
                nivelJuego.estado = (int)eEstadoNivel.finalizado;


            if (nivelJuego.estado == (int)eEstadoNivel.finalizado && (nivel)nivelJuego.nivelActual == nivel.facil && jugador.vida > 0)
            {
                foreach (Enemigo enemigo in nivelJuego.enemigos)
                {
                    if (enemigo.activo)
                    {
                        marcaNivel.Actualizar("Nivel 2");
                        marcaNivel.posicionItem.Y = GraphicsDevice.Viewport.Height / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).Y / 2;
                        marcaNivel.posicionItem.X = GraphicsDevice.Viewport.Width / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).X / 2;
                        return;
                    }
                }

                //le regalamos vida al jugador
                jugador.vida += 20;

                marcaNivel.Actualizar("");

                nivelJuego.iniciar((int)nivel.medio, Content, GraphicsDevice.Viewport);
                nivelJuego.estado = (int)eEstadoNivel.iniciado;
            }

            if (nivelJuego.estado == (int)eEstadoNivel.finalizado && (nivel)nivelJuego.nivelActual == nivel.medio && jugador.vida > 0)
            {

                foreach (Enemigo enemigo in nivelJuego.enemigos)
                {
                    if (enemigo.activo)
                    {
                        marcaNivel.Actualizar("Nivel 3");
                        marcaNivel.posicionItem.Y = GraphicsDevice.Viewport.Height / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).Y / 2;
                        marcaNivel.posicionItem.X = GraphicsDevice.Viewport.Width / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).X / 2;
                        return;
                    }
                }

                //regalamos vida al jugador
                jugador.vida += 40;

                marcaNivel.Actualizar("");

                nivelJuego.iniciar((int)nivel.dificil, Content, GraphicsDevice.Viewport);
                nivelJuego.estado = (int)eEstadoNivel.iniciado;
            }

            finalizarJuego(gameTime, teclado, estadoAnterior);
        }

        void finalizarJuego(GameTime gameTime, KeyboardState teclado, KeyboardState estadoAnterior)
        {
            if (nivelJuego.estado == (int)eEstadoNivel.finalizado && (nivel)nivelJuego.nivelActual == nivel.dificil && jugador.puntuacion > 150)
             {

                foreach (Enemigo enemigo in nivelJuego.enemigos)
                {
                    if (enemigo.activo)
                    {
                        marcaNivel.Actualizar("Mision cumplida.");
                        marcaNivel.posicionItem.Y = GraphicsDevice.Viewport.Height / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).Y / 2;
                        marcaNivel.posicionItem.X = GraphicsDevice.Viewport.Width / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).X / 2;
                        menu = null;
                        menu = new Menu();
                        menu.inicializar(GraphicsDevice.Viewport, Content);
                        return;
                    }
                }
            }

            if (jugador.vida <= 0 && nivelJuego.estado == (int)eEstadoNivel.finalizado)
            {
                foreach (Enemigo enemigo in nivelJuego.enemigos)
                {
                    if (enemigo.activo)
                    {
                        marcaNivel.Actualizar("Mision incompleta.");
                        marcaNivel.posicionItem.Y = GraphicsDevice.Viewport.Height / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).Y / 2;
                        marcaNivel.posicionItem.X = GraphicsDevice.Viewport.Width / 2 - marcaNivel.fuente.MeasureString(marcaNivel.texto).X / 2;
                        menu = null;
                        menu = new Menu();
                        menu.inicializar(GraphicsDevice.Viewport, Content);
                        return;
                    }
                }
            }
            else
                return;

            if (!guardar.activo && !(guardar.finalizado))
            {
                guardar.inicializar(Content, GraphicsDevice.Viewport, jugador);
                guardar.activo = true;
                return;
            }
            if (guardar.activo && !(guardar.finalizado))
            {
                marcaNivel.Actualizar("");
                return;
            }

            jugador.animacion.color = Color.White;
            MediaPlayer.Stop();
            marcaNivel.Actualizar("");
            nivelJuego = new Nivel();
            nivelJuego.estado = -1;
            jugador.puntuacion = 0;
            jugador.vida = 100;

            guardar = null;
            guardar = new BD();
        }

        void iniciarMusica(Song musica)
        {
            try
            {
                if (!(MediaPlayer.State == MediaState.Playing))
                {
                    MediaPlayer.Play(musica);
                    MediaPlayer.IsRepeating = true;
                }
            }
            catch (Exception e)
            {
            }
        }

        public void obtenerListaPuntuacion()
        {
            try
            {
                string cadena = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Space.mdb";

                System.Data.OleDb.OleDbConnection conexion = new System.Data.OleDb.OleDbConnection(cadena);

                conexion.Open();

                System.Data.OleDb.OleDbCommand comando = new System.Data.OleDb.OleDbCommand("Select top 10 puntuacion, siglas from Record Order by Puntuacion DESC, id DESC", conexion);

                comando.CommandType = System.Data.CommandType.Text;

                System.Data.OleDb.OleDbDataAdapter adaptador = new System.Data.OleDb.OleDbDataAdapter(comando);

                System.Data.DataTable listaPuntuaciones = new System.Data.DataTable();

                adaptador.Fill(listaPuntuaciones);

                puntajes = new List<Item>();

                int espacio = 0;
                int ePosicion = 1;

                int elementoConsulta = 0;
                int elementosMaximos = 10;

                foreach (System.Data.DataRow fila in listaPuntuaciones.Rows)
                {
                    if (!(elementoConsulta < elementosMaximos))
                        break;

                    Item e = new Item(Content, ePosicion + ".- " + fila["siglas"].ToString());
                    e.posicionItem = new Vector2(GraphicsDevice.Viewport.Width / 2, (GraphicsDevice.Viewport.Height / 3 * 2) + espacio);
                    e.aceptada = true;
                    puntajes.Add(e);
                    espacio += 40;
                    ePosicion++;
                    elementoConsulta++;
                }

                conexion.Close();
            }
            catch (Exception)
            {
                    Item e = new Item(Content, "Ocurrio un error al consultar la base de datos.");
                    e.posicionItem = new Vector2(GraphicsDevice.Viewport.Width / 2 - e.fuente.MeasureString(e.texto).X/2, (GraphicsDevice.Viewport.Height / 3 * 2));
                    e.aceptada = true;
                    puntajes.Add(e);
            }
        }
    } 
    
    enum nivel
    {
        facil, medio, dificil
    }

    enum eDesplazamiento
    {
        centro, izquierda, derecha
    }


    enum eEstadoNivel
    {
        iniciado, pausa, finalizado
    }

    enum eEstadoMenu
    {
        activo, inactivo
    }
}
