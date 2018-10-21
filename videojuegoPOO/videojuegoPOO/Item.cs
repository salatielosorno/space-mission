using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace videojuegoPOO
{
    public class Item
    {
        public SpriteFont fuente;
        public Vector2 posicionItem = Vector2.Zero;
        public string texto;
        public float scala = 1;
        public Color color = Color.White;
        public bool aceptada;

        public Item(ContentManager contenedor, string texto)
        {
            try
            {
                fuente = contenedor.Load<SpriteFont>("Fuente/fuenteJuego");
                this.texto = texto;
            }
            catch (Exception) { }
        }

        public void Actualizar(String texto)
        {
            try
            {
                this.texto = texto;
            }
            catch (Exception) { }
        }

        public void Dibujar(SpriteBatch spriteBatch, Viewport viewport)
        {
            try
            {
                spriteBatch.DrawString(fuente, texto, posicionItem, color, 0f, Vector2.Zero, scala, SpriteEffects.None, 0);
            }
            catch (Exception) { }
        }

        public String ToString()
        {
            return texto;
        }
    }
}
