namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Constantes;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Helpers;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Respuesta;
    using CPM.Mensajeria.Auronix.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Dominio;
    using CPM.RestConexion.Standard;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Servicio para gestionar el envío de mensajes de WhatsApp simples a través del servicio externo Auronix.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// </summary>
    public class ServicioMensajeriaWhatsAppSimpleTransacciones : ServicioMensajeriaWhatsApp, IServicioMensajeriaWhatsAppSimpleTransacciones
    {
        /// <summary>
        /// Cliente HTTP para realizar llamadas al API externo.
        /// </summary>
        private readonly ClienteHttp clienteHttp;

        /// <summary>
        /// URL base del servicio externo Auronix.
        /// </summary>
        private readonly string urlBase;

        /// <summary>
        /// Logger para registrar eventos y errores.
        /// </summary>
        private readonly ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones> logger;

        /// <summary>
        ///  Interfaz que define los métodos necesarios para la consulta de configuración para envió de notificaciones de WhatsApp.
        /// </summary>
        private readonly IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServicioMensajeriaWhatsAppSimpleTransacciones"/>.
        /// </summary>
        /// <param name="trazabilidadDTO">Información de trazabilidad.</param>
        /// <param name="configuracion">Configuración que contiene la URL del API Auronix.</param>
        /// <param name="httpClient">HttpClient para las llamadas.</param>
        /// <param name="logger">Logger para registros.</param>
        /// <param name="repositorioMensajeria">Repositorio para gestionar notificaciones de mensajería.</param>
        /// <param name="servicioConfiguracionWhatsapp">Servicio para gestionar la configuración de WhatsApp.</param>
        public ServicioMensajeriaWhatsAppSimpleTransacciones(
            TrazabilidadDTO trazabilidadDTO,
            IOptions<ServiciosExternos> configuracion,
            IHttpClientFactory httpClient,
            ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones> logger,
            IRepositorioMensajeriaWhatsApp repositorioMensajeria,
            IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp)
            : base(trazabilidadDTO, configuracion, httpClient.CreateClient(), logger, repositorioMensajeria)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.servicioConfiguracionWhatsapp = servicioConfiguracionWhatsapp ?? throw new ArgumentNullException(nameof(servicioConfiguracionWhatsapp));

            urlBase = configuracion.Value?.UrlApiAuronixTransaccion
                ?? throw new InvalidOperationException("La URL de Auronix Transacción no está configurada correctamente.");

            // Configuramos ClienteHttp con la URL base
            var httpCliente = httpClient.CreateClient();
            this.clienteHttp = new ClienteHttp(httpCliente, urlBase);
        }

        /// <summary>
        /// Envía una notificación WhatsApp simple a través del API externo.
        /// </summary>
        /// <param name="plantillaWhatsAppDTO">Datos de la plantilla y notificaciones.</param>
        /// <returns>Respuesta con el estado del envío.</returns>
        public async Task<RespuestaServicioAuronixWASimpleDTO?> EnviarWhatsAppSimple(PeticionWhatsAppSimpleDTO plantillaWhatsAppDTO)
        {
            string operacion = $"{ServicioConstantes.EnvioSimpleController}";
            var respuesta = new RespuestaServicioAuronixWASimpleDTO();

            var configuracionWhatsapp = await servicioConfiguracionWhatsapp.ObtenerConfiguracionWhatsappAsync();
            if (configuracionWhatsapp == null)
            {
                this.logger.LogError("Ocurrió un error al consultar la configuración para envío de la notificación de WhatsApp.");       
                return null;
            }

            string apiKey = configuracionWhatsapp?.ApiKey ?? throw new InvalidOperationException("API Key no disponible.");

            try
            {
                this.logger.LogDebug("Iniciando comunicación con el servicio de Auronix para envío de WhatsApp.");
                this.clienteHttp.AddRequestHeader(ServicioConstantes.ApiKey, apiKey);

                // Realiza la llamada POST
                var respuestaHttp = await this.clienteHttp.PostAsync(operacion, HttpHelper.GetHttpJsonContent(plantillaWhatsAppDTO));

                if (!string.IsNullOrEmpty(respuestaHttp.Contenido))
                {
                    respuesta = JsonSerializer.Deserialize<RespuestaServicioAuronixWASimpleDTO>(respuestaHttp.Contenido);
                }

                if (respuestaHttp.EstatusHttp == System.Net.HttpStatusCode.NotFound)
                {
                    this.logger.LogError($"Ocurrió un error al intentar establecer comunicación con el servicio de Auronix: {respuestaHttp.EstatusHttp}");
                    return null;
                }

                if (!string.IsNullOrEmpty(respuestaHttp.Resultado))
                {
                    respuesta = JsonSerializer.Deserialize<RespuestaServicioAuronixWASimpleDTO>(respuestaHttp.Resultado);
                }               
            }
            catch (Exception ex)
            {             
                this.logger.LogCritical(ex, "Error al comunicar con el API de Auronix {@@url}.", $"{this.urlBase}{operacion}");
                return null;
            }

            // Validar respuesta
            if (!respuesta!.Errores.Any() && string.IsNullOrEmpty(respuesta.Mensaje) && string.IsNullOrEmpty(respuesta.IdTransaccion))
            {
                this.logger.LogError("Respuesta vacía o no válida del API de Auronix.");
                respuesta.Descripcion = "Respuesta vacía o no válida.";
                return respuesta;
            }

            if (respuesta.Errores.Any() || !string.IsNullOrEmpty(respuesta.Mensaje))
            {
                string erroresJson = JsonSerializer.Serialize(respuesta);
                string mensajeError = !string.IsNullOrEmpty(respuesta.Mensaje) ? respuesta.Mensaje : erroresJson;
                respuesta.Descripcion = respuesta.Errores.FirstOrDefault()!.Titulo;
                this.logger.LogError("Error en envío de WhatsApp. {@@trazabilidad}", mensajeError);
                return respuesta;
            }

            this.logger.LogInformation("Envío de WhatsApp exitoso. IdTransacción: {idTransaccion}", respuesta.IdTransaccion);
            return respuesta;
        }
    }
}