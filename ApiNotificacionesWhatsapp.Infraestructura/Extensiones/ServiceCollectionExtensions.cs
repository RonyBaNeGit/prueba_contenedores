namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.Extensiones
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos;
    using CPM.Comun.Arquitectura.Excepciones;
    using CPM.Extensiones.Configuracion.Core;
    using CPM.Mensajeria.Arquitectura.Extensiones;
    using CPM.Mensajeria.Auronix.Extensiones;
    using CPM.Mensajeria.Auronix.Persistencia.Configuracion;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Propósito: Registra la colección de servicios de la capa de Infraestructura.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Agregar la configuración de la capa de Infraestructura a la colección de configuraciones de la aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección descriptores de servicios.</param>
        /// <param name="config">Representa un conjunto de propiedades de configuración de aplicación de clave/valor.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <returns>Colección descriptores de servicios.</returns>
        public static IServiceCollection RegistrarConfiguracionInfraestructura(this IServiceCollection services, IConfiguration config, ILogger logger)
        {
            // 1. Configuración de URLs base para servicios externos.
            IConfigurationSection seccion = config.GetSection(ServiciosExternos.Seccion);
            ServiciosExternos serviciosExternos = new ServiciosExternos();
            seccion.Bind(serviciosExternos);
            ConfiguracionHelper.ValidarConfiguracionRequerida(serviciosExternos, logger, propiedadesExcluir: new string[] { "PrefijoVariablesAmbiente", "UrlApiAdministracionAplicaciones", "UrlApiGateway", "ApiKey" });
            services.Configure<ServiciosExternos>(seccion);

            // 2. Politicas de reintento.
            seccion = config.GetSection(PoliticasReintento.Seccion);
            PoliticasReintento politicas = new PoliticasReintento();
            seccion.Bind(politicas);
            services.Configure<PoliticasReintento>(seccion);
            ConfiguracionHelper.ValidarConfiguracionRequerida(new object[] { serviciosExternos, politicas }, logger, propiedadesExcluir: new string[] { "PrefijoVariablesAmbiente", "UrlApiAdministracionAplicaciones", "UrlApiGateway", "ApiKey" });

            // 3. Registrar configuración cache.
            seccion = config.GetSection(ConfiguracionCache.Seccion);
            ConfiguracionCache configuracionCache = new ConfiguracionCache();
            seccion.Bind(configuracionCache);
            ValidarConfiguracionRequerida(configuracionCache, logger);
            services.Configure<ConfiguracionCache>(seccion);

            // 4. Configuración servicio mensajeria.
            services.RegistrarConfiguracionServicioMensajeriaWhatsApp(config);
           
            services.AddScoped<IServicioConfiguracionWhatsapp, ServicioConfiguracionWhatsApp>();
            services.AddScoped<IServicioMensajeriaWhatsAppSimpleTransacciones, ServicioMensajeriaWhatsAppSimpleTransacciones>();
            return services;
        }

        /// <summary>
        /// Agregar la lista de servicios de la capa de Infraestructura a la colección de servicios de la aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarServiciosInfraestructura(this IServiceCollection services)
        {
            services.RegistrarServicioMensajeria();
            services.RegistrarServicioMensajeriaWhatsApp();
            return services;
        }

        /// <summary>
        /// Valida que las propiedades de configuración de una instancia tengan asignado un valor.
        /// </summary>
        /// <typeparam name="T">Tipo clase de la instancia a validar.</typeparam>
        /// <param name="instancia">Instancia de la clase.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        public static void ValidarConfiguracionRequerida<T>(T instancia, ILogger logger)
        {
            IEnumerable<string> errores = ConfiguracionHelper.ValidarPropiedadesRequeridas(instancia, new string[] { "NombreAplicacion", "UrlApiGateway", "UrlApiAdministracionAplicaciones" });
            if (errores.Any())
            {
                string mensaje = $"Se produjo un error durante el arranque de la aplicación. La configuración no es válida., {string.Join(",", errores)}";
                logger.LogError(mensaje);
                throw new ConfiguracionInvalidaException(mensaje);
            }

            logger.LogInformation("Configuración {sección} registrada correctamente.", instancia!.GetType().Name);
        }

        #endregion
    }
}
