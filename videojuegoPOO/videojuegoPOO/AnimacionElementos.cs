using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace videojuegoPOO
{
    public class AnimacionElementos
    {
        
            // La imagen animada representada por un grupo de imagenes
            Texture2D spriteStrip;

            //Valor para escalar el sprite
            float scale;

            //tiempo desde la ultima vez que se actualizo la imagen
            int elapsedTime;

            //tiempo de despliegue por imagen
            int frameTime;

            //numero de imagenes que conforman la animacion
            public int frameCount;

            //indice de la imagen actual
            int currentFrame;

            //color de la imagen a desplegar
            public Color color;

            //el area de la imagen que vamos a desplegar
            Rectangle sourceRect = new Rectangle();

            //el area donde queremos desplegar la imagen
            Rectangle destinationRect = new Rectangle();

            //ancho de la imagen
            public int FrameWidth;

            //alto de una imagen
            public int FrameHeight;

            //estado de la animacion
            public bool Active;

            //repetir animacion
            public bool Looping;

            //posicion del sprite
            public Vector2 Position;


            public void Inicialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
            {
                //Mantener copias locales de los valores pasados
                this.color = color;
                this.FrameWidth = frameWidth;
                this.FrameHeight = frameHeight;
                this.frameCount = frameCount;
                this.frameTime = frametime;
                this.scale = scale;

                Looping = looping;
                Position = position;
                spriteStrip = texture;

                //hacer reset a los tiempos
                elapsedTime = 0;
                currentFrame = 0;

                //activar la animacion por defecto
                Active = true;
            }

            public void Update(GameTime gameTime)
            {

                //No actualiza si la imagen esta desactivada
                if (Active == false)
                    return;

                //Actualizar el tiempo transcurrido
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;



                

                //si elapsedTime es mayor que frametime
                //debemos cambiar la imagen
                if (elapsedTime > frameTime)
                {
                    //movemos a la siguiente imagen
                    currentFrame++;

                    //si currentFrame es igual al frameCount
                    //hacemos reset a currentFrame a cero
                    if (currentFrame == frameCount)
                    {
                        currentFrame = 0;

                        //si no queremos repetir la animacion
                        //asignamos Active a falso
                        if (Looping == false)
                            Active = false;
                    }

                    //reiniciamos elapsedTime a cero
                    elapsedTime = 0;
                }

                
                /*
                //BLOQUE PARA INTENTAR CAMBIAR IMAGEN CUANDO SE PRESIONE LA TECLA HACIA DELANTE

                //SE DECLARA UN VARIABLE PARA DETECTAR EL ESTADO DEL TECLADO
                KeyboardState estadoTeclado;

                //SE CAPTURA EL ESTADO DEL TECLADO
                estadoTeclado = Keyboard.GetState();

                //SI HA TRANSCURRIDO EL TIEMPO DETERMINADO EN LA CLASE GAME1
                if (elapsedTime > frameTime)
                {

                    //SI SE HA PRESIONADO LA TECLA FLECHA DERECHA
                    if (estadoTeclado.IsKeyDown(Keys.Right))
                    { //Pasa al siguiente "frame" 
                        currentFrame++;
                        //Se controla no pasarse de todos los "frames" del personaje 
                        if (currentFrame > frameCount - 1)
                            currentFrame = frameCount - 1;
                        //Acumulador de tiempo a cero 
                        //CuentaTiempo = 0f; 
                    }
                    if (estadoTeclado.IsKeyDown(Keys.Left))
                    {
                        currentFrame--;
                        //Se controla no pasarse de todos los "frames" del personaje 
                        if (currentFrame < 0)
                            currentFrame = 0;
                    }

                    //SI SE HA DEJADO DE PRESIONA ALGUNA DE LAS TECLAS DE FLECHA COLOCA EL FRAME CENTRAL PARA ESTABILIZAR LA NAVE
                    if (estadoTeclado.IsKeyUp(Keys.Left) && estadoTeclado.IsKeyUp(Keys.Right))
                        currentFrame = 2;


                    elapsedTime = 0;
                }

                //******************** BLOQUE DE TECLADO PARA ANIMACION

                */

                //Tomamos la imagen correcta multiplicando el currentFrame
                //por el ancho de la imagen
                sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

                //Actualizamos la posicion de la imagen en caso de que esta 
                //se desplace por la pantalla
                destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2, (int)Position.Y - (int)(FrameHeight * scale) / 2, (int)(FrameWidth * scale), (int)(FrameHeight * scale));

            }

            public void Draw(SpriteBatch spriteBatch)
            {
                if (Active)
                {
                    spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                }
            }
        
    }
}
