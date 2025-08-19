namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Extensiones
{
    using System.Reflection;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Mappers;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion;
    using CPM.Extensiones.Configuracion.Core;
    using CPM.Extensiones.Configuracion.Core.Excepciones;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Persistencia.Repositorios;
    using FluentValidation;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Propósito: Registra la colección de servicios de la capa de Aplicacion.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Agregar la configuración de la capa de Aplicación a la colección de configuraciones de la aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección descriptores de servicios.</param>
        /// <param name="config">Representa un conjunto de propiedades de configuración de aplicación de clave/valor.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <returns>Colección descriptores de servicios.</returns>
        public static IServiceCollection RegistrarConfiguracionAplicacion(this IServiceCollection services, IConfiguration config, ILogger logger)
        {
            // 1. Configuración general de la solución.
            IConfigurationSection seccion = config.GetSection(Aplicacion.Seccion);
            Aplicacion aplicacion = new Aplicacion();
            seccion.Bind(aplicacion);
            ValidarConfiguracionRequerida(aplicacion, logger);
            services.Configure<Aplicacion>(seccion);

            // 2. Configuración para la orquestación de repositorios y servicios externos.
            seccion = config.GetSection(Orquestacion.Seccion);
            Orquestacion orquestacion = new Orquestacion();
            seccion.Bind(orquestacion);
            ValidarConfiguracionRequerida(orquestacion, logger);
            services.Configure<Orquestacion>(seccion);
            return services;
        }

        /// <summary>
        /// Agregar la lista de servicios de orquestación de la capa de aplicación a la colección de servicios.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarServiciosOrquestacion(this IServiceCollection services)
        {
            services.AddScoped<IOrquestadorServicioWhatsapp, OrquestadorServicioWhatsapp>();
            services.AddScoped<IRepositorioMensajeriaWhatsApp, RepositorioMensajeriaWhatsApp>();
            return services;
        }

        /// <summary>
        /// Agregar la lista de mapeos de modelos y entidades DTO a la colección de servicios de la capa de aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarMapeosAplicacion(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AplicacionProfile));
            return services;
        }

        /// <summary>
        /// Agregar la lista de validadores de modelos y entidades DTO a la colección de servicios de la capa de aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarValidadoresAplicacion(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
            IEnumerable<string> errores = ConfiguracionHelper.ValidarPropiedadesRequeridas(instancia);
            if (errores.Any())
            {
                string mensaje = $"Se produjo un error durante el arranque de la aplicación. La configuración no es válida., {string.Join(",", errores)}";
                logger.LogError(mensaje);
                throw new ConfiguracionInvalidaException(mensaje);
            }

            logger.LogInformation("Configuración {sección} registrada correctamente.", instancia!.GetType().Name);
        }
    }
}
