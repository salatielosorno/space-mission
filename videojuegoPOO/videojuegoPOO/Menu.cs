using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace videojuegoPOO
{
    public class Menu
    {
        List<Item> elementos = new List<Item>();
        List<string> textos;
        Vector2 posicion;
        public int elementoActual = 0;
        Viewport viewport;
        ContentManager contenedor;
        
        /// <summary>
        /// Incializa el menu con valores predeterminados. Iniciar, Puntuaciones, Salir
        /// </summary>
        /// <param name="viewport">Ventana grafica</param>
        /// <param name="contenedor">Contenedor</param>
        public void inicializar(Viewport viewport, ContentManager contenedor)
        {
            try
            {
                this.viewport = viewport;
                this.contenedor = contenedor;

                posicion = new Vector2(viewport.Width / 3, viewport.Height / 3);

                textos = new List<string>();
                textos.Add("Iniciar");
                textos.Add("Puntuaciones");
                textos.Add("Salir");

                for (int elemento = 0; elemento < textos.Count; elemento++)
                {
                    Item oItem = new Item(contenedor, textos[elemento]);
                    oItem.posicionItem = posicion;
                    elementos.Add(oItem);

                    posicion.Y += oItem.fuente.MeasureString(oItem.texto).Y;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Inicializa el menu con la opcion de personalizar los titulos predeterminados
        /// </summary>
        /// <param name="viewport">Ventana gráfica</param>
        /// <param name="contenedor">Contenedor</param>
        /// <param name="titulos">Arreglo con los titulos a colocar en el menú. Importante agregar un enum especifico para controlar la posicion de cada titulo.</param>
        public void inicializar(Viewport viewport, ContentManager contenedor, string [] titulos)
        {
            try
            {
                textos = new List<string>();

                foreach (string texto in titulos)
                {
                    textos.Add(texto);
                }

                this.viewport = viewport;
                this.contenedor = contenedor;

                posicion = new Vector2(viewport.Width / 3, viewport.Height / 3);

                for (int elemento = 0; elemento < textos.Count; elemento++)
                {
                    Item oItem = new Item(contenedor, textos[elemento]);
                    oItem.posicionItem = posicion;
                    elementos.Add(oItem);

                    posicion.Y += oItem.fuente.MeasureString(oItem.texto).Y;
                }
            }
            catch (Exception) { }
        }

        public void Actualizar(KeyboardState teclado, KeyboardState tecladoAnterior)
        {
            try
            {
                if (teclado.IsKeyDown(Keys.Up) && tecladoAnterior.IsKeyUp(Keys.Up))
                    anterior();
                if (teclado.IsKeyDown(Keys.Down) && tecladoAnterior.IsKeyUp(Keys.Down))
                    siguiente();

                for (int elemento = 0; elemento < elementos.Count; elemento++)
                {
                    if (elemento == elementoActual)
                        elementos[elemento].color = Color.Red;
                    else
                        elementos[elemento].color = Color.White;
                }
            }
            catch (Exception) { }
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            try
            {
                foreach (Item oItem in elementos)
                {
                    oItem.Dibujar(spriteBatch, viewport);
                }
            }
            catch (Exception) { }
        }

        void siguiente()
        {
            try
            {
                if (elementoActual < elementos.Count - 1)
                    elementoActual++;
                else
                    elementoActual = elementos.Count - 1;
            }
            catch (Exception) { }
        }

        void anterior()
        {
            try
            {
                if (elementoActual <= 0)
                    elementoActual = 0;
                else
                    elementoActual--;
            }
            catch (Exception) { }
        }
    }

    enum accionInicial
    {
        iniciar, Puntuacion, salir
    }
    enum accionEnPausa
    {
        iniciar, salir
    }
}
