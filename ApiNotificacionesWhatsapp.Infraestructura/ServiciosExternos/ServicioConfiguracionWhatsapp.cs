namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;

    using CPM.Mensajeria.Auronix.Dominio;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Propósito: Implementa los métodos necesarios para el repositorio de MensajeriaWhatsapp.
    /// Fecha de creación: 20/05/2025 10:30:04.
    /// Creador: Ronaldo Barrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ServicioConfiguracionWhatsApp : IServicioConfiguracionWhatsapp
    {
        /// <summary>
        /// Proporciona los métodos necesarios para la administración de información para envío de notificaciones por WhatsApp.
        /// </summary>
        private readonly IRepositorioMensajeriaWhatsApp repositorioMensajeriaWhatsapp;

        /// <summary>
        /// Proporciona los métodos para el registro de trazabilidad.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Interfaz que nos provee de los métodos necesarios para la administración de cache en el servicio.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Contiene configuraciones relacionadas a la administración de caché.
        /// </summary>
        private readonly IOptions<ConfiguracionCache> configuracionCache;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServicioConfiguracionWhatsApp"/>.
        /// </summary>
        /// <param name="repositorioMensajeria"> Proporciona los métodos necesarios para la administración de información para envío de notificaciones por WhatsApp.</param>
        /// <param name="configuracionCache">Contiene configuraciones relacionadas a la administración de caché.</param>
        /// <param name="memoryCache">Interfaz que nos provee de los métodos necesarios para la administración de cache en el servicio.</param>
        /// <param name="logger">Proporciona los métodos para el registro de trazabilidad.</param>
        public ServicioConfiguracionWhatsApp(IRepositorioMensajeriaWhatsApp repositorioMensajeria, IOptions<ConfiguracionCache> configuracionCache, IMemoryCache memoryCache, ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones> logger)
        {
            repositorioMensajeriaWhatsapp = repositorioMensajeria;
            this.memoryCache = memoryCache;
            this.logger = logger;
            this.configuracionCache = configuracionCache;
        }

        /// <summary>
        /// Obtiene la información de configuración para la solicitud de mensajeria de whatsapp.
        /// </summary>
        /// <returns>Objeto con la información de la configuración..</returns>
        public async Task<ConfiguracionWhatsApp?> ObtenerConfiguracionWhatsappAsync()
        {
            const string cacheKey = "ConfiguracionWhatsApp";

            // Intentar obtener la configuración del caché
            if (!memoryCache.TryGetValue(cacheKey, out ConfiguracionWhatsApp? configuracion))
            {
                try
                {
                    this.logger.LogDebug("Consultando configuración de WhatsApp.");
                    configuracion = await repositorioMensajeriaWhatsapp.ConsultarConfiguracion();

                    if (configuracion == null)
                    {
                        this.logger.LogCritical("No se pudo obtener la configuración de WhatsApp.");
                        return null;
                    }

                    // Establecer la configuración en caché
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(this.configuracionCache.Value.ConfiguracionTiempoCache),
                    };

                    memoryCache.Set(cacheKey, configuracion, cacheOptions);
                    this.logger.LogInformation("Configuración de WhatsApp almacenada en caché.");
                }
                catch (Exception ex)
                {
                    this.logger.LogCritical(ex, "Error al obtener la configuración de WhatsApp.");
                    return null;
                }
            }

            return configuracion;
        }
    }
}