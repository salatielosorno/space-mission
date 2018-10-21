using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace videojuegoPOO
{
    public class Enemigo
    {
        public Texture2D textura;
        public AnimacionElementos animacion;
        public Vector2 posicion;
        float velocidad;
        public int danio = 10;
        public int vida = 20;
        public bool activo { get; set; }
        int desplazamiento;
        public bool disparar;
        public bool yaDisparo;
        Viewport viewport;
       
        public void inicializar(Vector2 pPosicion, float pVelocidad, Viewport viewport, int pDesplazamiento, AnimacionElementos animacion)
        {
            try
            {
                this.animacion = animacion;
                this.viewport = viewport;
                posicion = pPosicion;
                velocidad = pVelocidad;
                activo = true;

                this.desplazamiento = pDesplazamiento;
            }
            catch (Exception) { }
        }
        public void inicializar(Vector2 pPosicion, float pVelocidad, Viewport viewport, int pDesplazamiento, bool pDisparar, AnimacionElementos animacion)
        {
            try
            {
                this.animacion = animacion;
                this.viewport = viewport;
                posicion = pPosicion;
                velocidad = pVelocidad;
                activo = true;

                this.desplazamiento = pDesplazamiento;

                disparar = pDisparar;
            }
            catch (Exception) { }
        }

        public void actualizar(GameTime gameTime)
        {
            try
            {
                if (activo)
                {
                    posicion.Y += velocidad;

                    if ((int)eDesplazamiento.izquierda == (int)this.desplazamiento)
                        posicion.X -= velocidad;
                    else if ((int)eDesplazamiento.derecha == (int)this.desplazamiento)
                        posicion.X += velocidad;
                }

                if (posicion.Y > viewport.Height || vida == 0)
                    activo = false;
                animacion.Position = posicion;
                animacion.Update(gameTime);
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
    }
}
