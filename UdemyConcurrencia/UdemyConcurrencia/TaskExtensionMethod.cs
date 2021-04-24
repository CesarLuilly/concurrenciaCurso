using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdemyConcurrencia
{
    public static class TaskExtensionMethod
    {
        public static async Task<T> WithCancellation<T>(

            //                          //va a recibir como parametro un Task<T>, 
            //                          //  que es la misma tarea que va a retorna, 
            //                          //Se le pone this para referirse que va a
            //                          //  retornar lo mismo que va a recibir como
            //                          //  parametro.
            this Task<T> task,
            CancellationToken cancellationToken
            )
        {
            //                          //TaskCompletionSource es la tarear que vamos
            //                          //  a evaluar.
            var tcs = new TaskCompletionSource<object>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            //                          //.Register me permite registrar un delegado, 
            //                          //  es decir que puedo poner una pieza de codigo
            //                          //  que se va a ejecutar cuando el token sea cancelado.
            //                          //En este caso una funcion que va a ejecutarse 
            //                          //  cuando el token este cancelado.
            using (cancellationToken.Register(state => {
                //                      //Si el cancelation token es cancelado se va 
                //                      //  ejecutar este delegado.
                //                      //Y aqui lo que esta haciendo es completando la 
                //                      //  tarea.
                ((TaskCompletionSource<object>)state).TrySetResult(null);
            }, tcs))
            {
                var tareaResultante = await Task.WhenAny(
                    //                  //COn esta instruccion sirve para ver q tarea 
                    //                  //  termina primero,
                    //                  //  en este caso si es la tarea que recibe el 
                    //                  //  metodo WithCancellation Ó la tarea del
                    //                  //  completionsource.

                    task, 
                    //                  //Esta tarea tcs.Task va a completar si el 
                    //                  //  cancelationToken es cancelado.
                    tcs.Task);
                if (
                    //                  //El usuario cancelo la tarea.
                    //                  //Aqui estamos verificando si la tarea que se 
                    //                  //  fue la tarea del TaskCompletionSource.
                    tareaResultante == tcs.Task
                    )
                {
                    //                  //Lanzamos una excetion para detener la tarea               
                    throw new OperationCanceledException(cancellationToken);
                }
                return await task;
            }
        }
    }
}
