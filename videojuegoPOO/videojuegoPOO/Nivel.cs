using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace videojuegoPOO
{
    public class Nivel
    {
        public List<Enemigo> enemigos;
        public float velocidadEnemigo = 5.0f;

        public int estado = -1;

        string[] texturaEnemigo = new string[3] { "Imagenes/asteroideAnimacion", "Imagenes/animacionMarcianos", "Imagenes/animacionMarcianos" };
        public int nivelActual = -1;

        Rectangle rectangulo1;
        Rectangle rectangulo2;
        List<Proyectil> proyectilesEnemigos;
        TimeSpan tiempoDisparosEnemigos= TimeSpan.Zero, tiempoMaximoProyectilesEnemigos = TimeSpan.FromSeconds(0.6f);
        Vector2 velocidadProyectilEnemigo = Vector2.Zero;
        Item puntuacionJugador;
        Item saludJugador;
        int puntosEnemigoAbatido = 10;

        AnimacionElementos animacionEnemigo;

        List<AnimacionElementos> explosiones;

        SoundEffect explosion;
        ContentManager contenedor;
        Viewport viewport;

        public void iniciar(int nivel, ContentManager contenedor, Viewport viewport)
        {
            try
            {
                enemigos = new List<Enemigo>();
                proyectilesEnemigos = new List<Proyectil>();
                nivelActual = nivel;
                puntuacionJugador = new Item(contenedor, "Puntuacion: 0");
                saludJugador = new Item(contenedor, "Salud: 0");
                saludJugador.posicionItem.Y = puntuacionJugador.fuente.MeasureString(puntuacionJugador.texto).Y;
                explosiones = new List<AnimacionElementos>();

                explosion = contenedor.Load<SoundEffect>("Sonidos/efectoExplosion");
                this.contenedor = contenedor;
                this.viewport = viewport;
            }
            catch (Exception) { }
        }

        public void actualizar(Jugador jugador, GameTime gameTime)
        {
            try
            {
                actualizarEnemigos(gameTime);
                removerEnemigos();

                actualizarColisiones(jugador);
                disparoEnemigo(gameTime, jugador);

                actualizarProyectilesEnemigos();

                removerProyectilesEnemigos();

                if (jugador.vida >= 0)
                    puntuacionJugador.Actualizar("Puntuacion: " + jugador.puntuacion);
                
                saludJugador.Actualizar("Salud: " + jugador.vida);

                actualizarExplosiones(gameTime);
                removerExplosion();
            }
            catch (Exception) { }
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            try
            {
                foreach (Proyectil proyectil in proyectilesEnemigos)
                {
                    proyectil.Dibujar(spriteBatch);
                }
                foreach (Enemigo enemigo in enemigos)
                {
                    enemigo.dibujar(spriteBatch);
                }

                puntuacionJugador.Dibujar(spriteBatch, viewport);
                
                saludJugador.Dibujar(spriteBatch, viewport);

                DibujarExplosiones(spriteBatch);
            }
            catch (Exception) { }
        }


        private void actualizarProyectilesEnemigos()
        {
            try
            {
                foreach (Proyectil proyectil in proyectilesEnemigos)
                {
                    proyectil.Actualizar();
                }
            }
            catch (Exception) { }
        }

        public void agregarEnemigo()
        {
            try
            {
                Enemigo enemigo = new Enemigo();
                Texture2D texturaE;

                texturaE = contenedor.Load<Texture2D>(texturaEnemigo[(int)nivelActual]);

                animacionEnemigo = new AnimacionElementos();

                if ((int)texturaEnemigoNivel.asteoide == (int)nivelActual)
                    animacionEnemigo.Inicialize(texturaE, Vector2.Zero, 35, 35, 8, 150, Color.White, 1f, true);
                else
                    animacionEnemigo.Inicialize(texturaE, Vector2.Zero, 50, 45, 6, 150, Color.White, 1f, true);

                int posicionAncho = generarPosicionAleatoria(nivelActual);

                if (nivelActual == (int)nivel.facil)
                { enemigo.inicializar(new Vector2(posicionAncho, 0 - animacionEnemigo.FrameHeight), velocidadEnemigo, viewport, generarDesplazamiento(posicionAncho), animacionEnemigo); enemigo.danio = 15; }
                else if (nivelActual == (int)nivel.medio)
                { enemigo.inicializar(new Vector2(posicionAncho, 0 - animacionEnemigo.FrameHeight), velocidadEnemigo, viewport, generarDesplazamiento(posicionAncho), true, animacionEnemigo); enemigo.danio = 10; }
                else
                { enemigo.inicializar(new Vector2(posicionAncho, 0 - animacionEnemigo.FrameHeight), velocidadEnemigo, viewport, generarDesplazamiento(posicionAncho), true, animacionEnemigo); enemigo.danio = 5; }

                enemigos.Add(enemigo);
            }
            catch (Exception) { }
        }

        int generarPosicionAleatoria(int pnivel)
        {
            try
            {
                int posicion;
                Random r = new Random();

                if (pnivel == (int)nivel.medio)
                    posicion = r.Next(0, viewport.Width - animacionEnemigo.FrameWidth / 2);
                else
                    posicion = r.Next(0 - viewport.Width / 3, viewport.Width + viewport.Width / 3);

                return posicion;
            }
            catch (Exception) { throw; }
        }

        int generarDesplazamiento(int pPosicion)
        {
            try
            {
                if (nivelActual == (int)nivel.dificil || nivelActual == (int)nivel.facil)
                    if (pPosicion >= 0 - viewport.Width / 3 && pPosicion <= viewport.Width / 3)
                        return (int)eDesplazamiento.derecha;
                    else if (pPosicion >= (viewport.Width / 3) * 2 && pPosicion <= viewport.Width + viewport.Width / 3)
                        return (int)eDesplazamiento.izquierda;
                    else
                        return (int)eDesplazamiento.centro;
                else
                    return (int)eDesplazamiento.centro;
            }
            catch (Exception) { throw; }
        }

        void actualizarEnemigos(GameTime gameTime)
        {
            try
            {
                foreach (Enemigo enemigo in enemigos)
                {
                    enemigo.actualizar(gameTime);
                }
            }
            catch (Exception) { }
        }

        void removerEnemigos()
        {
            try
            {
                for (int elemento = 0; elemento < enemigos.Count; elemento++)
                {
                    if (!enemigos[elemento].activo)
                        enemigos.RemoveAt(elemento);
                }
            }
            catch (Exception) { }
        }

        void actualizarColisiones(Jugador jugador)
        {
            try
            {
                //coloca un color transparente cuando no hay colisión
                jugador.animacion.color = Color.White;

                colisionJugadorConEnemigo(jugador);

                colisionProyectilConEnemigo(jugador);

                colisionProyectilConJugador(jugador);
            }
            catch (Exception) { }
        }

        void colisionProyectilConJugador(Jugador jugador)
        {
            try
            {
                rectangulo1 = new Rectangle((int)jugador.posicion.X, (int)jugador.posicion.Y, jugador.width, jugador.height);//jugador.textura.Width, jugador.textura.Height);

                foreach (Proyectil proyectil in proyectilesEnemigos)
                {
                    rectangulo2 = new Rectangle((int)proyectil.posicion.X, (int)proyectil.posicion.Y, proyectil.textura.Width, proyectil.textura.Height);

                    if (rectangulo1.Intersects(rectangulo2))
                    {
                        if (!(estado == (int)eEstadoNivel.finalizado))
                            jugador.vida -= proyectil.danio;

                        jugador.animacion.color = Color.Red;
                        proyectil.activo = false;
                    }
                }
            }
            catch (Exception) { }
        }

        void colisionProyectilConEnemigo(Jugador jugador)
        {
            try
            {
                for (int i = 0; i < enemigos.Count; i++)
                {
                    for (int j = 0; j < jugador.proyectiles.Count; j++)
                    {
                        rectangulo1 = new Rectangle((int)enemigos[i].posicion.X, (int)enemigos[i].posicion.Y, enemigos[i].animacion.FrameWidth, enemigos[i].animacion.FrameHeight);

                        rectangulo2 = new Rectangle((int)jugador.proyectiles[j].posicion.X, (int)jugador.proyectiles[j].posicion.Y, jugador.proyectiles[j].textura.Width, jugador.proyectiles[j].textura.Height);

                        if (rectangulo1.Intersects(rectangulo2))
                        {
                            enemigos[i].vida -= jugador.proyectiles[j].danio;
                            jugador.proyectiles[j].activo = false;

                            if (enemigos[i].vida <= 0)
                            {
                                explosion.Play();
                                agregarExplosion(enemigos[i].posicion);

                                if (!(estado == (int)eEstadoNivel.finalizado))
                                    jugador.puntuacion += puntosEnemigoAbatido;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        void colisionJugadorConEnemigo(Jugador jugador)
        {
            try
            {
                rectangulo1 = new Rectangle((int)jugador.posicion.X, (int)jugador.posicion.Y, jugador.width, jugador.height);

                foreach (Enemigo enemigo in enemigos)
                {
                    rectangulo2 = new Rectangle((int)enemigo.posicion.X, (int)enemigo.posicion.Y, animacionEnemigo.FrameWidth, animacionEnemigo.FrameHeight);

                    if (rectangulo1.Intersects(rectangulo2))
                    {
                        explosion.Play();
                        agregarExplosion(enemigo.posicion);

                        if (!(estado == (int)eEstadoNivel.finalizado))
                            jugador.vida -= enemigo.danio;

                        if (jugador.vida < 0)
                            jugador.vida = 0;

                        jugador.animacion.color = Color.Red;
                        enemigo.activo = false;
                    }
                }
            }
            catch (Exception) { }
        }

        void disparoEnemigo(GameTime gameTime, Jugador jugador)
        {
            try
            {
                if (gameTime.TotalGameTime - tiempoDisparosEnemigos > tiempoMaximoProyectilesEnemigos)
                    foreach (Enemigo enemigo in enemigos)
                    {
                        enemigo.yaDisparo = sobrepasoAlJugador(enemigo, jugador);

                        if (enemigo.disparar && !enemigo.yaDisparo)
                        {
                            tiempoDisparosEnemigos = gameTime.TotalGameTime;
                            Proyectil proyectil = new Proyectil();
                            proyectil.inicializar("Imagenes/laser",contenedor, new Vector2(enemigo.posicion.X + animacionEnemigo.FrameWidth / 2, enemigo.posicion.Y + (animacionEnemigo.FrameHeight * 2)), velocidadProyectilEnemigo, true);
                            proyectil.rotacion = (float)calcularRotacion(enemigo, jugador);

                            if (!(nivelActual == (int)nivel.facil))
                                proyectil.danio = 5;

                            proyectilesEnemigos.Add(proyectil);
                        }
                    }
            }
            catch (Exception) { }
        }

        void removerProyectilesEnemigos()
        {
            try
            {
                if (proyectilesEnemigos != null)
                    for (int elemento = 0; elemento < proyectilesEnemigos.Count; elemento++)
                    {
                        if (!proyectilesEnemigos[elemento].activo)
                            proyectilesEnemigos.RemoveAt(elemento);
                    }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Calcula la rotación que debe tener el proyectil, en relación al Jugador.
        /// </summary>
        /// <param name="enemigo">Objeto de la clase Enemigo</param>
        /// <param name="jugador">Objeto de la clase Jugador</param>
        /// <returns></returns>
        double calcularRotacion(Enemigo enemigo, Jugador jugador)
        {
            try
            {
                double pendiente;

                //muy importante para ubicar al jugador y atacarlo
                pendiente = (jugador.posicion.X - enemigo.posicion.X) / (jugador.posicion.Y - enemigo.posicion.Y);

                double radianes = Math.Atan(pendiente);

                return radianes;
            }
            catch (Exception) { throw; }
        }

        bool sobrepasoAlJugador(Enemigo enemigo, Jugador jugador)
        {
            try
            {
                if (enemigo.posicion.Y >= jugador.posicion.Y - animacionEnemigo.FrameHeight)
                    return true;
                else
                    return false;
            }
            catch (Exception) { throw; }
        }

        void agregarExplosion(Vector2 posicion)
        {
            try
            {
                AnimacionElementos animacionExplosion = new AnimacionElementos();

                if ((int)texturaEnemigoNivel.asteoide == (int)nivelActual)
                {
                    Texture2D texturaAnimacion = contenedor.Load<Texture2D>("Imagenes/ExplosionAsteoide");
                    animacionExplosion.Inicialize(texturaAnimacion, posicion, 134, 134, 12, 30, Color.White, 1f, false);
                }
                else
                {
                    Texture2D texturaAnimacion = contenedor.Load<Texture2D>("Imagenes/ExplosionMarciano");

                    animacionExplosion.Inicialize(texturaAnimacion, posicion, 60, 64, 21, 30, Color.White, 1f, false);
                }

                explosiones.Add(animacionExplosion);
            }
            catch (Exception) { }
        }
        void actualizarExplosiones(GameTime gameTime)
        {
            try
            {
                if (explosiones != null)
                    foreach (AnimacionElementos explosion in explosiones)
                    {
                        explosion.Update(gameTime);
                    }
            }
            catch (Exception) { }
        }

        void removerExplosion()
        {
            try
            {
                if (explosiones != null)
                    for (int explosionActual = 0; explosionActual < explosiones.Count; explosionActual++)
                    {
                        if (!explosiones[explosionActual].Active)
                            explosiones.RemoveAt(explosionActual);
                    }
            }
            catch (Exception) { }
        }

        void DibujarExplosiones(SpriteBatch spriteBatch)
        {
            try
            {
                foreach (AnimacionElementos explosion in explosiones)
                {
                    explosion.Draw(spriteBatch);
                }
            }
            catch (Exception) { }
        }
    }

    enum texturaEnemigoNivel
    {
        asteoide, marciano
    }
}
