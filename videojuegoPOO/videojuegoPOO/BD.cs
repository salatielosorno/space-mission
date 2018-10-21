using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Data.OleDb;

namespace videojuegoPOO
{
    public class BD
    {
        string [] letrasDisponibles = new string [36]{ "A", "B", "C", "D", "E", 
                                                       "F", "G", "H", "I", "J", 
                                                       "K", "L", "M", "N", "O", 
                                                       "P", "Q", "R", "S", "T", 
                                                       "U", "V", "W", "X", "Y", 
                                                       "Z", "0", "1", "2", "3", 
                                                       "4", "5", "6", "7", "8", "9" };
        List<Item> letrasEnPantalla;
        int letraActual = 0;
        int maximoLetras = 3;
        public bool finalizado;
        public bool activo;
        public Item identificador;
        ContentManager contenedor;
        Viewport viewport;

        public void inicializar(ContentManager contenedor, Viewport viewport, Jugador jugador)
        {
            try
            {
                this.contenedor = contenedor;
                this.viewport = viewport;

                letrasEnPantalla = new List<Item>();

                if (!conectar())
                {
                    Item iletra = new Item(contenedor, "");
                    letrasEnPantalla.Add(iletra);
                    letrasEnPantalla.Add(iletra);
                    letrasEnPantalla.Add(iletra);

                    foreach (Item l in letrasEnPantalla)
                    {
                        l.aceptada = true;
                    }
                    return;
                }

                letraActual = 0;

                identificador = new Item(contenedor, obtenerPosicionEnPuntaje(jugador).ToString() + ".-");
                identificador.posicionItem = new Vector2(viewport.Width / 2 - (identificador.fuente.MeasureString(identificador.texto).X), viewport.Height / 2 - (identificador.fuente.MeasureString(identificador.texto).Y / 2));

                Item letra = new Item(contenedor, letrasDisponibles[letraActual]);
                letra.posicionItem = new Vector2(identificador.posicionItem.X + identificador.fuente.MeasureString(identificador.texto).X, viewport.Height / 2 - (letra.fuente.MeasureString(letra.texto).Y / 2));
                letrasEnPantalla.Add(letra);
            }
            catch (Exception) { }
        }

        public void actualizar(GameTime gameTime, KeyboardState teclado, KeyboardState estadoAnterior, Jugador jugador)
        {
            try
            {
                if (teclado.IsKeyDown(Keys.Right) && estadoAnterior.IsKeyUp(Keys.Right))
                    siguiente();

                if (teclado.IsKeyDown(Keys.Left) && estadoAnterior.IsKeyUp(Keys.Left))
                    anterior();

                if (teclado.IsKeyDown(Keys.Enter) && estadoAnterior.IsKeyUp(Keys.Enter))
                    agregarLetra(contenedor, letrasDisponibles[letraActual], viewport);

                for (int letra = 0; letra < letrasEnPantalla.Count; letra++)
                {
                    if (!letrasEnPantalla[letra].aceptada)
                        letrasEnPantalla[letra].Actualizar(letrasDisponibles[letraActual]);
                }

                int total = 0;

                for (int letras = 0; letras < letrasEnPantalla.Count; letras++)
                {
                    if (letrasEnPantalla[letras].aceptada)
                        total++;
                }

                if (total == 3)
                {
                    guardarRecord(jugador);

                    finalizado = true;
                    activo = false;
                }
            }
            catch (Exception) { }
        }
        void siguiente()
        {
            if (letraActual < letrasDisponibles.Length - 1)
                letraActual++;
            else
                letraActual = 0;
        }

        void anterior()
        {
            if (letraActual <= 0)
                letraActual = letrasDisponibles.Length - 1;
            else
                letraActual--;
        }

        public void dibujar(SpriteBatch spriteBatch, Viewport viewport)
        {
            try
            {
                for (int letraDibujada = 0; letraDibujada < letrasEnPantalla.Count; letraDibujada++)
                {
                    if (letrasEnPantalla[letraDibujada].aceptada)
                        letrasEnPantalla[letraDibujada].color = Color.White;
                    else
                        letrasEnPantalla[letraDibujada].color = Color.Gold;

                    letrasEnPantalla[letraDibujada].Dibujar(spriteBatch, viewport);
                }

                if (identificador != null)
                    identificador.Dibujar(spriteBatch, viewport);
            }
            catch (Exception) { }
        }

        void agregarLetra(ContentManager contenedor, string strLetra, Viewport viewport)
        {
            try
            {
                foreach (Item iletra in letrasEnPantalla)
                {
                    iletra.aceptada = true;
                }

                if (letrasEnPantalla.Count < maximoLetras)
                {
                    Item letra = new Item(contenedor, strLetra);
                    letra.posicionItem = new Vector2(letra.fuente.MeasureString(letra.texto).X + letrasEnPantalla[letrasEnPantalla.Count - 1].posicionItem.X, viewport.Height / 2 - letra.fuente.MeasureString(letra.texto).Y / 2);
                    letrasEnPantalla.Add(letra);
                }
            }
            catch (Exception) { }
        }

        bool guardarRecord(Jugador jugador)
        {
            OleDbConnection conexion = null;
            try
            {
                string cadena = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Space.mdb";

                conexion = new OleDbConnection(cadena);

                conexion.Open();

                OleDbCommand comando = new OleDbCommand("Insert into Record(siglas,puntuacion) values (@siglas,@puntuacion)", conexion);

                string siglas = "";

                foreach(Item letra in letrasEnPantalla)
                {
                    siglas += letra.ToString();
                }

                comando.Parameters.AddWithValue("siglas",siglas);
                comando.Parameters.AddWithValue("puntuacion",jugador.puntuacion + jugador.vida);

                int resultado;

                resultado = comando.ExecuteNonQuery();

                conexion.Close();

                return (bool)Boolean.Parse(resultado.ToString());
            }
            catch (Exception)
            {
                if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                    conexion.Close();

                return false;
            }
        }

        int obtenerPosicionEnPuntaje(Jugador jugador)
        {
            try
            {
                string cadena = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Space.mdb";

                OleDbConnection conexion = new OleDbConnection(cadena);

                conexion.Open();

                OleDbCommand comandoSelect = new OleDbCommand("SELECT Id, Siglas, Puntuacion FROM Record where puntuacion > @puntuacion;", conexion);

                comandoSelect.Parameters.AddWithValue("puntuacion", jugador.puntuacion + jugador.vida);

                OleDbDataAdapter adaptador = new OleDbDataAdapter(comandoSelect);

                System.Data.DataTable listaPuntuaciones = new System.Data.DataTable();

                adaptador.Fill(listaPuntuaciones);

                int contador = 1;

                foreach (System.Data.DataRow fila in listaPuntuaciones.Rows)
                {
                    if ((int)fila["Puntuacion"] == jugador.puntuacion + jugador.vida)
                        contador--;
                    else
                        contador++;
                }

                conexion.Close();
                
                return contador;
            }
            catch (Exception) { throw; }
        }

        bool conectar()
        {
            try
            {
                string cadena = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Space.mdb";

                OleDbConnection conexion = new OleDbConnection(cadena);

                conexion.Open();

                if (conexion.State == System.Data.ConnectionState.Open)
                    conexion.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
