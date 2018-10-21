using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace videojuegoPOO
{
    public class Fondo
    {
        Vector2[] posicion;
        Texture2D textura;
        public int velocidad;

        public void inicializar(ContentManager contenedor, String rutaTextura, int altoPantalla, int pVelocidad)
        {
            try
            {
                velocidad = pVelocidad;
                textura = contenedor.Load<Texture2D>(rutaTextura);

                posicion = new Vector2[(altoPantalla / textura.Height + 1)];

                for (int elemento = 0; elemento < posicion.Length; elemento++)
                {
                    posicion[elemento] = new Vector2(0, elemento * textura.Height);
                }
            }
            catch (Exception) { }
        }

        public void actualizar()
        {
            try
            {
                for (int elemento = 0; elemento < posicion.Length; elemento++)
                {
                    posicion[elemento].Y += velocidad;

                    if (posicion[elemento].Y >= textura.Height * (posicion.Length - 1))
                    {
                        posicion[elemento].Y = -textura.Height + velocidad;
                    }
                }
            }
            catch (Exception) { }
        }

        public void Dibujar(SpriteBatch spriteBach)
        {
            try
            {
                for (int elemento = 0; elemento < posicion.Length; elemento++)
                {
                    spriteBach.Draw(textura, posicion[elemento], Color.White);
                }
            }
            catch (Exception) { }
        }
    }
}
