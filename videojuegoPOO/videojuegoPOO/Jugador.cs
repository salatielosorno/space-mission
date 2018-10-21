using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace videojuegoPOO
{
    public class Jugador
    {
        public List<Proyectil> proyectiles;
        public int numeroProyectiles = 5;
        public Vector2 velocidadProyectil = Vector2.Zero;
        public Vector2 posicion;
        public AnimacionElementos animacion;

        public int width
        {
            get { 
                return animacion.FrameWidth; 
            }
        }

        public int height
        {
            get { 
                return animacion.FrameHeight; 
            }
        }

        float velocidad;
        public int vida = 100;
        public int puntuacion = 0;
        ContentManager contenedor;
        Viewport viewport;
        SoundEffect disparo;

        public void inicializar(Viewport viewport, ContentManager contenedor, AnimacionElementos PlayerAnimation, Vector2 pPosicion, float pVelocidad, SoundEffect disparo)
        {
            try
            {
                animacion = PlayerAnimation;
                proyectiles = new List<Proyectil>();
                posicion = pPosicion;
                velocidad = pVelocidad;
                this.contenedor = contenedor;
                this.viewport = viewport;
                this.disparo = disparo;
            }
            catch (Exception) { }
        }

        public void actualizar(KeyboardState teclado, KeyboardState estadoAnterior, GameTime gameTime)
        {
            try
            {
                if (teclado.IsKeyDown(Keys.Up))
                    posicion.Y -= 5.0f;

                if (teclado.IsKeyDown(Keys.Down))
                    posicion.Y += 5.0f;

                if (teclado.IsKeyDown(Keys.Right))
                    posicion.X += 5.0f;

                if (teclado.IsKeyDown(Keys.Left))
                    posicion.X -= 5.0f;

                if (teclado.IsKeyDown(Keys.Space) && estadoAnterior.IsKeyUp(Keys.Space))
                {
                    if (proyectiles.Count < numeroProyectiles)
                    {
                        disparo.Play();
                        agregarProyectil();
                    }
                }

                animacion.Position = posicion;
                animacion.Update(gameTime);

                posicion.X = MathHelper.Clamp(posicion.X, 0 + animacion.FrameWidth / 2, viewport.Width - animacion.FrameWidth / 2);
                posicion.Y = MathHelper.Clamp(posicion.Y, viewport.Height / 4, viewport.Height - animacion.FrameHeight / 2);
            }
            catch (Exception) { }
        }

        public void dibujar(SpriteBatch spriteBatch)
        {
            try
            {
                animacion.Draw(spriteBatch);
            }
            catch (Exception) { }
        }

        void agregarProyectil()
        {
            try
            {
                Proyectil proyectil = new Proyectil();

                //se dividio entre ocho porque son 4 imagenes que conforman la animacion
                proyectil.inicializar("Imagenes/laser", contenedor, new Vector2(posicion.X + animacion.FrameWidth / (animacion.frameCount * 2),
                    posicion.Y - animacion.FrameHeight / 2), velocidadProyectil);

                proyectiles.Add(proyectil);
            }
            catch (Exception) { }
        }
    }
}
