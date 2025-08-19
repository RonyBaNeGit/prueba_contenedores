namespace CPM.ApiNotificacionesWhatsapp.Persistencia.Extensiones
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Repositorios;
    using CPM.Extensiones.Configuracion.Core;
    using CPM.Extensiones.Configuracion.Core.Excepciones;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Repositorios;
    using CPM.Mensajeria.Arquitectura.Extensiones;
    using CPM.Mensajeria.Arquitectura.Persistencia.Repositorios;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Extensiones;
    using CPM.Mensajeria.Auronix.Persistencia.Configuracion;
    using CPM.Mensajeria.Auronix.Persistencia.Repositorios;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PoliticasReintento = Mensajeria.Auronix.Persistencia.Configuracion.PoliticasReintento;

    /// <summary>
    /// Propósito: Registra la colección de servicios de la capa de Persistencia\Datos.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Registra la configuración para la capa de persistencia como cadenas de conexión y procedimientos almacenados.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección descriptores de servicios.</param>
        /// <param name="config">Representa un conjunto de propiedades de configuración de aplicación de clave/valor.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <returns>Colección descriptores de servicios.</returns>
        public static IServiceCollection RegistrarConfiguracionPersistencia(this IServiceCollection services, IConfiguration config, ILogger logger)
        {
            // 1. Descifrado de cadenas de conexión.
            string mensaje = string.Empty;
            IConfigurationSection seccion;
            CadenasConexion cadenasConexion;
            try
            {
                seccion = config.GetSection(CadenasConexion.Seccion);
                cadenasConexion = ConfiguracionHelper.DescifrarCadenasConexion<CadenasConexion>(config, typeof(CadenasConexion));
                logger.LogInformation("Cadenas de conexión configuradas correctamente.");
            }
            catch (Exception ex)
            {
                mensaje = "La configuración de cadenas de conexión no es correcta, verificar que han sido generadas con los parámetros correctos";
                logger.LogError(ex, mensaje);
                throw new ConfiguracionInvalidaException(mensaje);
            }

            // 2. Registrar procedimientos almacenados.
            seccion = config.GetSection(ProcedimientosAlmacenados.Seccion);
            ProcedimientosAlmacenados procedimientos = new ProcedimientosAlmacenados();
            seccion.Bind(procedimientos);

            // 3. Registrar configuración.
            services.Configure<DbConfiguracion>(dbConfig =>
            {
                dbConfig.Cadenas = cadenasConexion;
                dbConfig.Procedimientos = procedimientos;
            });

            // 4. Politicas de reintento.
            seccion = config.GetSection(PoliticasReintento.Seccion);
            PoliticasReintento politicas = new PoliticasReintento();
            seccion.Bind(politicas);
            services.Configure<PoliticasReintento>(seccion);
            ConfiguracionHelper.ValidarConfiguracionRequerida(new object[] { procedimientos, politicas }, logger);

            // 5. Configuración base de datos mensajeria.
            services.RegistrarConfiguracionRepositorioMensajeriaWhatsApp(config, cadenasConexion.CadenaConexion);

            services.AddScoped<IRepositorioMensajeriaWhatsApp, RepositorioMensajeriaWhatsApp>();
            services.AddScoped<IRepositorioWhatsApp, RepositorioWhatsApp>();
            services.AddScoped<IRepositorioMensajeria, RepositorioMensajeria>();
            return services;
        }

        /// <summary>
        /// Agregar la lista de repositorios de la capa de Persistencia\Datos a la colección de servicios de la aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarRepositoriosPersistencia(this IServiceCollection services)
        {
            services.RegistrarRepositoriosMensajeria();
            services.RegistrarRepositoriosMensajeriaWhatsApp();
            return services;
        }

        #endregion
    }
}
