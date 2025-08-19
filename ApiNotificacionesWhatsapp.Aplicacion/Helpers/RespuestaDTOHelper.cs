namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers
{
    using System.Text.Json;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Enumerados;

    /// <summary>
    /// Propósito: Clase auxiliar para la generación de respuestas DTO para las distintas clase de servicio.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class RespuestaDTOHelper
    {
        #region Constantes

        /// <summary>
        /// Mensaje que indica que los encabezados o cuerpo de petición proporcionados por el cliente no cumple con las reglas de validación básicas.
        /// </summary>
        public const string MensajePeticionIncorrecta = "La solicitud no puede ser procesada debido a errores en la petición por parte del cliente";

        /// <summary>
        /// Mensaje que indica que el servicio proceso correctamente la solicitud.
        /// </summary>
        public const string MensajeCorrecto = "Solicitud procesada correctamente";

        /// <summary>
        /// Mensaje que indica un error en la petición por parte del cliente que consume el servicio.
        /// </summary>
        public const string MensajeInvalido = "La solicitud no puede ser procesada debido a errores en la petición";

        /// <summary>
        /// Mensaje que indica un error durante el procesamiento de la solicitud por parte de la aplicación.
        /// </summary>
        public const string MensajeError = "Ocurrió un error al procesar la solicitud";

        #endregion

        #region Métodos Estáticos Públicos

        /// <summary>
        /// Convierte una cadena de texto en formato json a un objeto del tipo <see cref="RespuestaDTO"/>.
        /// </summary>
        /// <param name="respuestaDTO">Cadena json.</param>
        /// <returns>Respuesta DTO.</returns>
        public static RespuestaDTO? Deserializar(string respuestaDTO)
        {
            if (string.IsNullOrEmpty(respuestaDTO))
            {
                return null;
            }

            return JsonSerializer.Deserialize<RespuestaDTO>(respuestaDTO)!;
        }

        /// <summary>
        /// Convierte una cadena de texto en formato json a un objeto del tipo <see cref="RespuestaDTO{T}"/>.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de información a regresar en la respuesta.</typeparam>
        /// <param name="respuestaDTO">Cadena json.</param>
        /// <returns>Respuesta DTO.</returns>
        public static RespuestaDTO<T>? Deserializar<T>(string respuestaDTO)
        {
            if (string.IsNullOrEmpty(respuestaDTO))
            {
                return null;
            }

            return JsonSerializer.Deserialize<RespuestaDTO<T>>(respuestaDTO)!;
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo correcto.
        /// </summary>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="codigo">        
        /// Código de respuesta de la aplicación: 
        /// 0: Correcto. 
        /// 1: Petición incorrecta por parte del cliente.
        /// 2: Error interno de la aplicación.
        /// Mayor o igual a 3: Error interno de la aplicación durante la ejecución de alguno de los proceso internos.
        /// </param>
        /// <param name="mensaje">Mensaje de respuesta.</param>
        /// <returns>Respuesta DTO de tipo correcto.</returns>
        public static RespuestaDTO RespuestaDTO(string idTransaccion, CodigoRespuesta codigo, string mensaje = MensajeCorrecto)
        {
            return new RespuestaDTO()
            {
                IdTransaccion = idTransaccion,
                Codigo = codigo,
                Mensaje = mensaje,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo correcto.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de información a regresar en la respuesta.</typeparam>
        /// <param name="objetoInformacion">Datos del objeto de información a regresar en la respuesta.</param>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="codigo">        
        /// Código de respuesta de la aplicación: 
        /// 0: Correcto. 
        /// 1: Petición incorrecta por parte del cliente.
        /// 2: Error interno de la aplicación.
        /// Mayor o igual a 3: Error interno de la aplicación durante la ejecución de alguno de los proceso internos.
        /// </param>
        /// <param name="mensaje">Mensaje de respuesta.</param>
        /// <returns>Respuesta DTO de tipo correcto.</returns>
        public static RespuestaDTO<T> RespuestaDTO<T>(T objetoInformacion, string idTransaccion, CodigoRespuesta codigo, string mensaje = MensajeCorrecto)
        {
            return new RespuestaDTO<T>()
            {
                ObjetoInformacion = objetoInformacion,
                IdTransaccion = idTransaccion,
                Codigo = codigo,
                Mensaje = mensaje,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo correcto.
        /// </summary>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="mensaje">Mensaje de respuesta.</param>
        /// <returns>Respuesta DTO de tipo correcto.</returns>
        public static RespuestaDTO RespuestaCorrecta(string idTransaccion, string mensaje = MensajeCorrecto)
        {
            return new RespuestaDTO()
            {
                IdTransaccion = idTransaccion,
                Mensaje = mensaje,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo correcto.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de información a regresar en la respuesta.</typeparam>
        /// <param name="objetoInformacion">Datos del objeto de información a regresar en la respuesta.</param>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="mensaje">Mensaje de respuesta.</param>
        /// <returns>Respuesta DTO de tipo correcto.</returns>
        public static RespuestaDTO<T> RespuestaCorrecta<T>(T objetoInformacion, string idTransaccion, string mensaje = MensajeCorrecto)
        {
            return new RespuestaDTO<T>()
            {
                IdTransaccion = idTransaccion,
                Mensaje = mensaje,
                ObjetoInformacion = objetoInformacion,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo invalida.
        /// </summary>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="mensaje">Mensaje que indica un error en la petición por parte del cliente que consume el servicio.</param>
        /// <returns>Respuesta DTO de tipo incorrecta.</returns>
        public static RespuestaDTO RespuestaInvalida(string idTransaccion, string mensaje = MensajeInvalido)
        {
            return new RespuestaDTO()
            {
                IdTransaccion = idTransaccion,
                Codigo = CodigoRespuesta.Invalido,
                Mensaje = mensaje,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo invalida.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de información a regresar en la respuesta.</typeparam>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="mensaje">Mensaje que indica un error en la petición por parte del cliente que consume el servicio.</param>
        /// <param name="objetoInformacion">Datos del objeto de información a regresar en la respuesta.</param>
        /// <returns>Respuesta DTO de tipo incorrecta.</returns>
        public static RespuestaDTO<T> RespuestaInvalida<T>(string idTransaccion, string mensaje = MensajeInvalido, T objetoInformacion = default!)
        {
            return new RespuestaDTO<T>()
            {
                IdTransaccion = idTransaccion,
                Codigo = CodigoRespuesta.Invalido,
                Mensaje = mensaje,
                ObjetoInformacion = objetoInformacion,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo error interno.
        /// </summary>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="codigo"> Código que identifica el error en la aplicación.</param>
        /// <param name="mensaje">Mensaje que indica un error durante el procesamiento de la solicitud por parte de la aplicación.</param>
        /// <returns>Respuesta DTO de tipo interno.</returns>
        public static RespuestaDTO RespuestaErrorInterno(string idTransaccion, CodigoRespuesta codigo = CodigoRespuesta.ErrorInterno, string mensaje = MensajeError)
        {
            return new RespuestaDTO()
            {
                IdTransaccion = idTransaccion,
                Codigo = codigo,
                Mensaje = mensaje,
            };
        }

        /// <summary>
        /// Devuelve una respuesta DTO de tipo error interno.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de información a regresar en la respuesta.</typeparam>
        /// <param name="idTransaccion">Identificador único de la transacción.</param>
        /// <param name="codigo"> Código que identifica el error en la aplicación.</param>
        /// <param name="mensaje">Mensaje que indica un error durante el procesamiento de la solicitud por parte de la aplicación.</param>
        /// <returns>Respuesta DTO de tipo error interno.</returns>
        public static RespuestaDTO<T> RespuestaErrorInterno<T>(string idTransaccion, CodigoRespuesta codigo = CodigoRespuesta.ErrorInterno, string mensaje = MensajeError)
        {
            return new RespuestaDTO<T>()
            {
                IdTransaccion = idTransaccion,
                Codigo = codigo,
                Mensaje = mensaje,
            };
        }

        #endregion
    }
}