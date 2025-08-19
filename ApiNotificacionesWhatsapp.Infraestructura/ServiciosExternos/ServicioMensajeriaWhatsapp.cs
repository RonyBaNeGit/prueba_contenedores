namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos
{
    using System.Collections.Generic;
    using System.Net.Http;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using global::CPM.ApiNotificacionesWhatsapp.Infraestructura.Constantes;
    using global::CPM.Mensajeria.Auronix.Infraestructura.ServiciosExternos;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Propósito: Funcionalidad base para la integración con el servicio de Auronix.
    /// Fecha de creación: 28/11/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ServicioMensajeriaWhatsApp
    {
        #region Variables

        /// <summary>
        /// Lista de encabezados de petición que deben ser enviados en todas las solicitudes
        /// hacia el servicio.
        /// </summary>
        private readonly List<KeyValuePair<string, string>> defaultHeaders;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServicioMensajeriaWhatsAppTransacciones"/>.
        /// </summary>
        /// <param name="trazabilidadDTO">Proporciona información de la transacción actual.</param>
        /// <param name="configuracion">Proporciona los parámetro de configuración necesarios para el consumo del servicio de mensajeria.</param>
        /// <param name="httpClient">Proporciona una clase para enviar solicitudes HTTP y recibir respuestas HTTP de un recurso identificado por un URI.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <param name="repositorioMensajeria">Proporciona los métodos necesarios para la administración de información para envío de notificaciones por WhatsApp.</param>
        public ServicioMensajeriaWhatsApp(TrazabilidadDTO trazabilidadDTO, IOptions<ServiciosExternos> configuracion, HttpClient httpClient, ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones> logger, IRepositorioMensajeriaWhatsApp repositorioMensajeria)
        {
            this.HttpClient = httpClient;
            this.defaultHeaders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(ServicioConstantes.ApiKey, configuracion.Value.ApiKey),
            };
            this.AgregarEncabezados();
            this.Logger = logger;
            this.Logger.BeginScope(new
            {
                IdTransaccion = trazabilidadDTO.IdTransaccion,
                Usuario = trazabilidadDTO.Usuario,
            });
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// URL base del servicio.
        /// </summary>
        public string UrlBase { get; protected set; } = string.Empty;

        /// <summary>
        /// Proporciona los métodos necesarios para el registro de trazabilidad.
        /// </summary>
        public ILogger Logger { get; protected set; }

        /// <summary>
        /// Proporciona los métodos necesarios para el consumo de servicios REST.
        /// </summary>
        public HttpClient HttpClient { get; protected set; }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Agrega los encabezados de petición.
        /// </summary>
        private void AgregarEncabezados()
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            foreach (KeyValuePair<string, string> header in this.defaultHeaders)
            {
                this.HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        #endregion
    }
}
