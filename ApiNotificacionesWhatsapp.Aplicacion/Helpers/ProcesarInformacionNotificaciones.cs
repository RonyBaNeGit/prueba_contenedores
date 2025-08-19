namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers
{
    using System.Text;
    using System.Text.Json;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Constantes;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Peticion;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Respuesta;
    using CPM.Mensajeria.Auronix.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Dominio;

    /// <summary>
    /// Propósito: Implementa métodos auxiliares para procesar información necesaria para la orquestación de notificaciones de WhatsApp.
    /// Fecha de creación: 03/12/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ProcesarInformacionNotificaciones
    {
        #region Métodos Públicos

        /// <summary>
        /// Crear contenido HTML de respuesta del servicio Auronix.
        /// </summary>
        /// <param name="respuestaServicioAuronixDTO">Información de respuesta del servicio de Auronix para solicitudes con errores o no procesadas.</param>
        /// <returns>Instancia del tipo <see cref="RespuestaServicioAuronixDTO"/>.</returns>        
        public static string CrearContenidoHtml(RespuestaServicioAuronixDTO respuestaServicioAuronixDTO)
        {
            StringBuilder contenidoHtml = new StringBuilder($"<p>{WhatsApp.ErrorEncabezado}</p>");

            // Agregamos el error en caso de que la petición genere una excepción o no se haya mapeado correctamente la respuesta.
            if (!string.IsNullOrEmpty(respuestaServicioAuronixDTO.Descripcion) && string.IsNullOrEmpty(respuestaServicioAuronixDTO.IdRelacionado))
            {
                contenidoHtml.Append($"<div><strong>Error: </strong>{respuestaServicioAuronixDTO.Descripcion}.</div>");
            }

            // Validamos que la respuesta contenga el id relacionado y lo agregamos al contenido.
            if (!string.IsNullOrEmpty(respuestaServicioAuronixDTO.IdRelacionado))
            {
                contenidoHtml.Append($"<div><strong>Identificador Respuesta Auronix: </strong>{respuestaServicioAuronixDTO.IdRelacionado}</div>");
            }

            // Si encontramos errores, los agregamos como lista al contenido HTML.
            if (respuestaServicioAuronixDTO.Errores.Any())
            {
                contenidoHtml.Append("<ul>");
                foreach (var error in respuestaServicioAuronixDTO.Errores)
                {
                    contenidoHtml.Append($"<li>{error.Detalle}</li>");
                }

                contenidoHtml.Append("</ul>");
            }

            // Mensaje de error identificado cuando las credenciales no son correctas (401).
            if (!string.IsNullOrEmpty(respuestaServicioAuronixDTO.Mensaje))
            {
                contenidoHtml.Append($"<div><strong>Error: </strong>{respuestaServicioAuronixDTO.Mensaje}</div>");
            }

            return contenidoHtml.ToString();
        }

        /// <summary>
        /// Actualizar el estatus y el número de reintentos en una lista notificaciones de WhatsApp.
        /// </summary>
        /// <param name="listaNotificacionWhatsApp">Lista de notificaciones.</param>
        /// <param name="estatusNotificacion">Estatus de la notificación.</param>
        /// <param name="reintentos">Intentos de petición realizados.</param>
        /// <param name="aplicacion">Parámetros de la sección Aplicacion en la configuración.</param>
        /// <param name="idCampania">Identificador único de respuesta de Auronix para solicitudes exitosas.</param>
        public static void ActualizarPropiedadesNotificaciones(IEnumerable<NotificacionWhatsApp> listaNotificacionWhatsApp, EstatusNotificacion estatusNotificacion, byte reintentos, Aplicacion aplicacion, string idCampania = "")
        {
            // Validación para evitar referencias nulas
            if (listaNotificacionWhatsApp is null)
            {
                throw new ArgumentNullException(nameof(listaNotificacionWhatsApp));
            }

            // Actualiza propiedades de cada petición.
            foreach (var notificacion in listaNotificacionWhatsApp)
            {
                notificacion.Estatus = estatusNotificacion;
                notificacion.Reintentos = reintentos;
                notificacion.IdCampania = idCampania;
            }
        }

        /// <summary>
        /// Obtener rango de notificaciones pendientes de procesar.
        /// </summary>
        /// <param name="listaNotificacionWhatsApp">Lista de notificaciones.</param>        
        /// <param name="numeroNotificaciones">Número de notificaciones a obtener.</param>
        /// <param name="configuracionAuronix">Parámetros de configuración en base de datos para el consumo del servicio de mensajeria de Auronix.</param>
        /// <returns>Lista de notificaciones de WhatsApp.</returns>
        public static IEnumerable<NotificacionWhatsApp> ObtenerRangoNotificaciones(IEnumerable<NotificacionWhatsApp> listaNotificacionWhatsApp, int numeroNotificaciones, ConfiguracionAuronix configuracionAuronix)
        {
            IEnumerable<NotificacionWhatsApp> listaNotificacionesRango = listaNotificacionWhatsApp;
            if (configuracionAuronix.LimiteNotificacionesMeta > 0)
            {
                listaNotificacionesRango = listaNotificacionWhatsApp.Take(numeroNotificaciones);
            }

            return listaNotificacionesRango;
        }

        /// <summary>
        /// Mapear respuesta del servicio Auronix para inserción en bitácora.
        /// </summary>
        /// <param name="idTransaccionProceso">Identificador del conjunto de notificaciones enviadas en la petición.</param>        
        /// <param name="peticionWhatsAppMasivoDTO">Estructura de petición enviada a Auronix.</param>
        /// <param name="respuestaServicioAuronixDTO">Respuesta del servicio de Auronix.</param>
        /// <param name="aplicacion">Parámetros de la sección Aplicacion en la configuración.</param>
        /// <param name="trazabilidad">Parámetros con información para trazabilidad.</param>
        /// <returns>Lista de notificaciones de WhatsApp.</returns>
        public static RespuestaServicio MapearRespuestaAuronix(string idTransaccionProceso, PeticionWhatsAppMasivoDTO peticionWhatsAppMasivoDTO, RespuestaServicioAuronixDTO? respuestaServicioAuronixDTO, Aplicacion aplicacion, TrazabilidadDTO trazabilidad)
        {
            RespuestaServicio respuestaServicio = new RespuestaServicio()
            {
                IdTransaccionProceso = idTransaccionProceso,
                Peticion = JsonSerializer.Serialize(peticionWhatsAppMasivoDTO),
                Respuesta = JsonSerializer.Serialize(respuestaServicioAuronixDTO),
                UsuarioRegistro = trazabilidad.Usuario,
            };
            return respuestaServicio;
        }

        #endregion

    }
}
