using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace videojuegoPOO
{
    public class Proyectil
    {
        public Vector2 posicion;
        public Texture2D textura;
        Vector2 velocidad;
        public bool activo;
        public int danio = 10;
        public float rotacion = 0.0f;
        public bool esEnemigo;

        public void inicializar(String rutaTextura, ContentManager contenedor, Vector2 pPosicion, Vector2 pVelocidad, bool esEnemigo = false)
        {
            try
            {
                textura = contenedor.Load<Texture2D>(rutaTextura);
                posicion.X = pPosicion.X - (textura.Width / 2);
                posicion.Y = pPosicion.Y - (textura.Height / 2);
                velocidad = pVelocidad;
                activo = true;
                this.esEnemigo = esEnemigo;
            }
            catch (Exception) { }
        }

        public void Actualizar()
        {
            try
            {
                Vector2 direccion = Vector2.Zero;

                direccion.X = (float)Math.Sin(rotacion);

                if (esEnemigo)
                    direccion.Y = (float)Math.Cos(rotacion);
                else
                    direccion.Y = -(float)Math.Cos(rotacion);

                velocidad = velocidad + direccion;

                if (activo == true)
                    posicion += velocidad;

                if (posicion.Y <= -textura.Height)
                    activo = false;
            }
            catch (Exception) { }
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(textura, posicion, null, Color.White, -rotacion, new Vector2(textura.Width / 2, textura.Height / 2), 1, SpriteEffects.None, 0);
            }
            catch (Exception) { }
        }
    }
}
