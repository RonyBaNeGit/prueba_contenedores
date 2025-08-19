namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion
{
    using System;
    using System.Reflection;
    using AutoMapper;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Extensiones;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Extensiones;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.Extensiones;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Extensiones;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Repositorios;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Extensiones.Configuracion.Core.Configuracion;
    using CPM.Extensiones.Configuracion.Core.Enumerados;
    using CPM.Extensiones.Configuracion.Core.Extensiones;
    using CPM.Logging.Xml.Extensiones;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Enumerados;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Repositorios;
    using CPM.Mensajeria.Arquitectura.Aplicacion.ServiciosExternos;
    using CPM.Mensajeria.Arquitectura.Persistencia.Repositorios;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Peticion;
    using CPM.Mensajeria.Auronix.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Dominio;
    using CPM.Mensajeria.Auronix.Infraestructura.Configuracion;
    using CPM.Mensajeria.Auronix.Persistencia.Configuracion;
    using CPM.Mensajeria.Auronix.Persistencia.Repositorios;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using NUnit.Framework;
    using static CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.Testing;

    /// <summary>
    /// Propósito: Implementa los métodos que se llaman una vez antes y después de ejecutar cualquier prueba secundaria.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [SetUpFixture]
    public class Testing
    {
        #region Constantes

        /// <summary>
        /// Número telefonico del destinatario.
        /// </summary>
        public const string Telefono = "4731245521";

        /// <summary>
        /// Número telefonico del destinatario incorrecto.
        /// </summary>
        public const string TelefonoIncorrecto = "4771234567890";

        /// <summary>
        /// Identificador del socio.
        /// </summary>
        public const string NumeroSocio = "25734";

        /// <summary>
        /// Identificador del canal que origina la solicitud.
        /// </summary>
        public const int IdCanal = 10;

        /// <summary>
        /// Nombre de la aplicación que realiza la solicitud.
        /// </summary>
        public const string NombreAplicacion = "Pruebas";

        /// <summary>
        /// Identificador de la plantilla dentro del catálogo de Auronix para mensajería de WhatsApp.
        /// </summary>
        public const string Plantilla = "72eeb2d2_8c12_4be4_89ce_2232e0ceb9ef:mensaje_activacion_cpm_movilplus";

        /// <summary>
        /// Idioma correspondiente a la plantilla de notificación de WhatsApp.
        /// </summary>
        public const string Lenguaje = "es_MX";

        /// <summary>
        /// Identificador de la plantilla dentro del catálogo de Auronix para mensajería de WhatsApp.
        /// </summary>
        public const int IdParametro = 1;

        /// <summary>
        /// Nombre del socio destinatario.
        /// </summary>
        public const string NombreSocio = "Ronaldo";

        /// <summary>
        /// Apellido paterno del socio destinatario.
        /// </summary>
        public const string ApellidoPaternoSocio = "Barrientos";

        /// <summary>
        /// Descripción de la plantilla de notificación de WhatsApp.
        /// </summary>
        public const string DescripcionPlantilla = "Plantilla de WhatsApp de recordatorio posterior a que venció la fecha compromiso de entrega de documentos de solicitudes de crédito de sucursal";

        /// <summary>
        /// Número telefonico remitente para el envío de la notificación de whatsapp.
        /// </summary>
        public const string NumeroRemitente = "5214771296048";

        /// <summary>
        /// Listado de parámetros para la solicitud a Auronix.
        /// </summary>
        public static readonly List<string> ListaParametros = new List<string> { NombreSocio, ApellidoPaternoSocio };

        #endregion

        #region Variables

        /// <summary>
        /// Representa la raíz de una jerarquía Microsoft.Extensions.Configuration.IConfiguration.
        /// </summary>
        private static IConfiguration? configuration;

        /// <summary>
        /// Una fábrica para crear instancias de Microsoft.Extensions.DependencyInjection.IServiceScope,
        /// que se utiliza para crear servicios dentro de un alcance.
        /// </summary>
        private static IServiceScopeFactory? scopeFactory;

        #endregion

        #region Métodos Estáticos Públicos

        /// <summary>
        /// Parámetros de trazabilidad en servicios Api.
        /// </summary>
        public TrazabilidadDTO? TrazabilidadDTO { get; set; }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Devuelve los parámetros de la sección Aplicacion en la configuración.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static IOptions<Aplicacion> ObtenerConfiguracionAplicacion()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<Aplicacion>>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IServicioMensajeria"/>.
        /// </summary>
        /// <param name="mock">Indica si la instancia deberá generarse como Mock.</param>
        /// <returns>Instancia del implementación para el tipo <see cref="IServicioMensajeria"/>.</returns>
        public static IServicioMensajeria CrearInstanciaServicioMensajeria(bool mock = false)
        {
            if (mock)
            {
                return Substitute.For<IServicioMensajeria>();
            }

            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IServicioMensajeria>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IRepositorioMensajeria"/>.
        /// </summary>
        /// <param name="mock">Indica si la instancia deberá generarse como Mock.</param>
        /// <returns>Instancia del implementación para el tipo <see cref="IRepositorioMensajeria"/>.</returns>
        public static IRepositorioMensajeria CrearInstanciaRepositorioMensajeria(bool mock = false)
        {
            if (mock)
            {
                return Substitute.For<IRepositorioMensajeria>();
            }

            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRepositorioMensajeria>();
        }

        /// <summary>
        /// Devuelve los parámetros de configuración para la integración con la base de datos para la consulta de notificaciones.
        /// </summary>
        /// <returns>Lista de cadenas de conexión.</returns>
        public static DbConfiguracionMensajeriaWhatsApp ObtenerDbConfiguracionMensajeriaWhatsApp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracionMensajeriaWhatsApp>>().Value;
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IRepositorioMensajeriaWhatsApp"/>.
        /// </summary>
        /// <param name="mock">Indica si la instancia deberá generarse como Mock.</param>
        /// <returns>Instancia del implementación para el tipo <see cref="IRepositorioMensajeriaWhatsApp"/>.</returns>
        public static IRepositorioMensajeriaWhatsApp CrearInstanciaRepositorioMensajeriaWhatsApp(bool mock = false)
        {
            if (mock)
            {
                return Substitute.For<IRepositorioMensajeriaWhatsApp>();
            }

            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRepositorioMensajeriaWhatsApp>();
        }

        /// <summary>
        /// Devuelve los parámetros de la sección Orquestacion en la configuración.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static IOptions<ConfiguracionMensajeriaWhatsApp> ObtenerConfiguracionMensajeriaWhatsApp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<ConfiguracionMensajeriaWhatsApp>>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IRepositorioWhatsApp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="IRepositorioWhatsApp"/>.</returns>
        public static IRepositorioWhatsApp CrearInstanciaRepositorioWhatsApp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRepositorioWhatsApp>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IServicioMensajeriaWhatsAppSimpleTransacciones"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="IServicioMensajeriaWhatsAppSimpleTransacciones"/>.</returns>
        public static IServicioMensajeriaWhatsAppSimpleTransacciones CrearInstanciaRepositorioWhatsAppSimpleTransaccione()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IServicioMensajeriaWhatsAppSimpleTransacciones>();
        }

        /// <summary>
        /// Devuelve los parámetros de la sección ProcedimientosAlmacenados en la configuración.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static ProcedimientosAlmacenados ObtenerConfiguracionProcedimientosAlmacenados()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracion>>().Value.Procedimientos;
        }

        /// <summary>
        /// Devuelve los parámetros de la sección ServiciosExternos en la configuración.
        /// </summary>
        /// <returns>Lista de URLs de servicios externos.</returns>
        public static Infraestructura.Configuracion.ServiciosExternos ObtenerConfiguracionServiciosExternos()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<Infraestructura.Configuracion.ServiciosExternos>>().Value;
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IOrquestadorServicioWhatsapp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="IOrquestadorServicioWhatsapp"/>.</returns>
        public static IOrquestadorServicioWhatsapp CrearInstanciaOrquestadorWhatsapp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOrquestadorServicioWhatsapp>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="IServicioConfiguracionWhatsapp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="IServicioConfiguracionWhatsapp"/>.</returns>
        public static IServicioConfiguracionWhatsapp CrearInstanciaObtenerConfiguracion()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IServicioConfiguracionWhatsapp>();
        }

        /// <summary>
        /// Agrega la configuración de la memoria cache a la aplicación.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static IMemoryCache ObtenerConfiguracionMemoryCache()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        }

        /// <summary>
        /// Crear instancia del tipo <see cref="TrazabilidadDTO"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="TrazabilidadDTO"/>.</returns>
        public static TrazabilidadDTO CrearInstanciaTrazabilidadDTO()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<TrazabilidadDTO>();
        }

        /// <summary>
        /// Crear instancia del tipo <see cref="Infraestructura.Configuracion.ConfiguracionCache"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="TrazabilidadDTO"/>.</returns>
        public static IOptions<Infraestructura.Configuracion.ConfiguracionCache> CrearInstanciaTiempoCache()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<ConfiguracionCache>>();
        }

        /// <summary>
        /// Crear instancia del tipo <see cref="IMemoryCache"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="TrazabilidadDTO"/>.</returns>
        public static IMemoryCache CrearInstanciaCache()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        }

        /// <summary>
        /// Devuelve los parámetros de la sección ConnectionStrings en la configuración.
        /// </summary>
        /// <returns>Lista de cadenas de conexión.</returns>
        public static CadenasConexion ObtenerConfiguracionCadenasConexion()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracion>>().Value.Cadenas;
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="ILogger"/> para la clase de <see cref="RepositorioMensajeriaWhatsApp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="ILogger"/>.</returns>
        public static ILogger<RepositorioMensajeriaWhatsApp> ObtenerConfiguracionLoggerRepositorioMensajeriaWhatsApp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ILogger<RepositorioMensajeriaWhatsApp>>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="ILogger"/> para la clase de <see cref="OrquestadorServicioWhatsapp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="ILogger"/>.</returns>
        public static ILogger<OrquestadorServicioWhatsapp> ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ILogger<OrquestadorServicioWhatsapp>>();
        }

        /// <summary>
        /// Obtiene una instancia de <see cref="IServicioMensajeriaWhatsAppSimpleTransacciones"/> desde el scope de DI.
        /// </summary>
        /// <returns>La instancia de <see cref="IServicioMensajeriaWhatsAppSimpleTransacciones"/>.</returns>
        public static IServicioMensajeriaWhatsAppSimpleTransacciones ObtenerServicioMensajeriaWhatsAppSimpleTransacciones()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IServicioMensajeriaWhatsAppSimpleTransacciones>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="ILogger"/> para la clase de <see cref="RepositorioWhatsApp"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="ILogger"/>.</returns>
        public static ILogger<RepositorioWhatsApp> ObtenerConfiguracionLoggerRepositorioWhatsApp()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ILogger<RepositorioWhatsApp>>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz <see cref="ILogger"/> para la clase de <see cref="ServicioMensajeriaWhatsAppSimpleTransacciones"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="ILogger"/>.</returns>
        public static ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones> ObtenerConfiguracionLoggerServicioMensajeriaWhatsAppSimpleTransacciones()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ILogger<ServicioMensajeriaWhatsAppSimpleTransacciones>>();
        }

        /// <summary>
        /// Crea una instancia de implementación para la interfaz para la clase de <see cref="PoliticasReintento"/>.
        /// </summary>
        /// <returns>Instancia del implementación para el tipo <see cref="PoliticasReintento"/>.</returns>
        public static IOptions<PoliticasReintento> ObtenerPoliticasReintento()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<PoliticasReintento>>();
        }

        /// <summary>
        /// Agrega la configuración de la memoria cache a la aplicación.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static IMapper ObtenerConfiguracionMapper()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Devuelve los parámetros de la sección ProcedimientosAlmacenados en la configuración.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static TrazabilidadDTO ObtenerParametrosTrazabilidad()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<TrazabilidadDTO>();
        }

        /// <summary>
        /// Devuelve las propiedades de la clase BbConfiguracion en la configuración.
        /// </summary>
        /// <returns>Lista de procedimientos almacenados.</returns>
        public static IOptions<DbConfiguracion> ObtenerConfiguracionGeneral()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracion>>();
        }

        /// <summary>
        /// Metodo para crear instancia del tipo <see cref="NotificacionWhatsApp"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="NotificacionWhatsApp"/>.</returns>     
        public static NotificacionWhatsApp CrearInstanciaNotificacionWhatsApp()
        {
            return new NotificacionWhatsApp
            {
                TelefonoDestinatario = Telefono,
                FechaInicio = DateTime.Now.ToString("yyyy-MM-dd"),
                FechaFinal = DateTime.Now.ToString("yyyy-MM-dd"),
                Estatus = EstatusNotificacion.EnviadoConExito,
                IdCanalCPM = IdCanal,
                NombreAplicacion = NombreAplicacion,
                FolioSolicitud = Guid.NewGuid().ToString().ToUpper(),
                IdPlantilla = Plantilla,
                Reintentos = 1,
                IdCampania = Guid.NewGuid().ToString().ToUpper(),
                IdTransaccionProceso = string.Empty,
                NumeroSocio = NumeroSocio,
            };
        }

        /// <summary>
        /// Metodo para crear instancia del tipo <see cref="EventoParametro"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="EventoParametro"/>.</returns>     
        public static EventoParametro CrearInstanciaEventoParametro()
        {
            return new EventoParametro
            {
                IdParametro = IdParametro,
                Valor = "valor", 
                Orden = 1,
            };
        }

        /// <summary>
        /// Metodo para crear instancia del tipo <see cref="PeticionWhatsAppSimpleDTO"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="PeticionWhatsAppSimpleDTO"/>.</returns>     
        public static PeticionWhatsAppSimpleDTO CrearInstanciaPeticionWhatsAppSimple()
        {
            return new PeticionWhatsAppSimpleDTO
            {
                Canal = NumeroRemitente,
                IdTransaccionCliente = Guid.NewGuid().ToString().ToUpper(),
                IdUsuario = Guid.NewGuid().ToString().ToUpper(),
                TelefonoDestinatario = $"52{Telefono}",
                Plantilla = new PlantillaDTO
                {
                    IdPlantilla = Plantilla,
                    Lenguaje = Lenguaje,
                    Url = string.Empty,
                    Parametros = new List<string>
                    {
                        NombreSocio,
                        ApellidoPaternoSocio,
                    },
                    TipoFormato = string.Empty,
                },
                ListaMensajeDatosMeta = Array.Empty<string>().ToList(),
                Descripcion = DescripcionPlantilla,
                ListaNegra = Array.Empty<string>().ToList(),
                ListaMetaDatos = Array.Empty<string>().ToList(),
            };
        }

        /// <summary>
        /// Método para crear una instancia de la clase <see cref="SolicitudDTO"/> con datos de ejemplo.
        /// </summary>
        /// <returns>Instancia de <see cref="SolicitudDTO"/> con datos predefinidos.</returns>
        public static SolicitudDTO CrearInstanciaSolicitud()
        {
            var random = new Random();

            return new SolicitudDTO
            {
                FolioSolicitud = random.Next(1000).ToString(),
                NumeroSocio = NumeroSocio,
                Telefono = Telefono,
                IdPlantilla = Plantilla,
                Parametros = ListaParametros,
            };
        }

        /// <summary>
        /// Método que se llama una vez para realizar la configuración antes de ejecutar cualquier prueba secundaria.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var builder = this.CreateBuilder();
            var services = new ServiceCollection();

            // 1. Carga configuración base de la aplicación.
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            builder.AgregarConfiguracionInicial(new Dictionary<string, string?>()
            {
                { "Aplicacion:TipoAplicacion", TipoAplicacion.WebServer.ToString() },
                { "Aplicacion:Titulo", "ApiNotificacionesWhatsapp" },
                { "Aplicacion:NombreAplicacion", assemblyName.Name!.Split('.').Length > 1 ? assemblyName.Name.Split('.')[1] : assemblyName.Name },
                { "Aplicacion:Version", assemblyName.Version!.ToString() },
            });
            configuration = builder.Build();

            // Inicializar logger.
            var logger = services.RegistrarLoggerFactory(configuration).CreateLogger<Testing>();
            logger.BeginScope(new { IdTransaccion = Guid.NewGuid().ToString().ToUpper(), Usuario = "SISTEMA" });

            // Obtener valores variables de ambiente a configuración y agregar a la sección ServiciosExternos.
            builder.AgregarVariablesEntorno(configuration, logger);

            // Modificar prioridad de archivos appsettings.{ENVIRONMENT}.json
            builder.ModificarPrioridadConfiguracion();

            // Configura las propiedades de trazabilidad
            this.TrazabilidadDTO = new TrazabilidadDTO()
            {
                Usuario = Environment.UserName.ToUpper(),
                IdTransaccion = Guid.NewGuid().ToString().ToUpper(),
                IdCanal = 6,
                NombreAplicacion = "MOVIL",
            };
            configuration = builder.Build();

            // Se obtiene la configuración básica de las secciones Aplicacion, y ServiciosExternos.
            var aplicacion = new AplicacionBase();
            var serviciosExternos = new ServiciosExternosBase();
            configuration.GetSection("Aplicacion").Bind(aplicacion);
            configuration.GetSection("ServiciosExternos").Bind(serviciosExternos);

            services.AddSingleton<IMemoryCache, MemoryCache>();

            // Registro de Configuración semilla, palabra clave y prefijo desde el servicio de seguridad.
            services.AddHttpClient();
            await builder.AgregarConfiguracionSeguridad(aplicacion, serviciosExternos, logger);

            // Registro de configuración adicional (COMMUNICATOR, REACHCORE, VERITAN, etc).
            //// await builder.Configuration.AgregarConfiguracionSeguridad(aplicacion, serviciosExternos, 1, typeof(Communicator), logger);

            // Carga del archivo appsettings alojado en el servicio de seguridad.
            await builder.AgregarArchivoJson(aplicacion, serviciosExternos, logger);
            configuration = builder.Build();

            // 2. Registro de Configuración común para cualquier tipo de solución (API,WST,WEB).
            //// builder.Services.Configure<Communicator>(configuration.GetSection(nameof(Communicator)));

            // 3. Configuración especifica para procesos de negocio.
            // CPM.ApiNotificacionesWhatsapp.Dominio
            services.RegistrarMapeosDominio();

            // CPM.ApiNotificacionesWhatsapp.Aplicacion
            services.RegistrarConfiguracionAplicacion(configuration, logger)
                    .RegistrarMapeosAplicacion()
                    .RegistrarValidadoresAplicacion();

            // CPM.TellerCentral.Infraestructura
            // Configuración capa Infraestructura.
            services.RegistrarConfiguracionInfraestructura(configuration, logger)
                .RegistrarServiciosInfraestructura();
            services.RegistrarConfiguracionInfraestructura(configuration, logger)
                    .RegistrarServiciosInfraestructura();

            // CPM.TellerCentral.Persistencia
            services.RegistrarConfiguracionPersistencia(configuration, logger)
                    .RegistrarRepositoriosPersistencia();

            services.AddScoped(sp =>
            {
                Aplicacion aplicacion = sp.GetRequiredService<IOptions<Aplicacion>>().Value;
                return new TrazabilidadDTO()
                {
                    IdCanal = 6,
                    NombreAplicacion = aplicacion.NombreAplicacion,
                    Usuario = "SISTEMA",
                    IdTransaccion = Guid.NewGuid().ToString().ToUpper(),
                };
            });

            services.RegistrarServiciosOrquestacion();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var repositorio = scope.ServiceProvider.GetRequiredService<IRepositorioMensajeriaWhatsApp>();
                var configuracionPaquete = scope.ServiceProvider.GetRequiredService<IOptions<ConfiguracionMensajeriaWhatsApp>>();
                ConfiguracionWhatsApp? parametros = await repositorio.ConsultarConfiguracion();
                services.Configure<ConfiguracionAuronix>(configuracion =>
                {
                    // Configuracion Wst.
                    configuracion.Canal = parametros!.Canal;
                    configuracion.ApiKey = parametros!.ApiKey;
                    configuracion.DistribucionUniforme = parametros!.DistribucionUniforme;
                    configuracion.EnvioEnLinea = parametros!.EnvioEnLinea;
                    configuracion.LimiteNotificacionesMeta = parametros!.LimiteNotificacionesMeta;
                });

                // Paquete nuget Auronix.
                var serviciosExternosAuronix = scope.ServiceProvider.GetRequiredService<IOptions<Infraestructura.Configuracion.ServiciosExternos>>();
                services.Configure<ConfiguracionMensajeriaWhatsApp>(mensajeria =>
                {
                    mensajeria.UrlApiAuronixTransaccion = serviciosExternosAuronix.Value.UrlApiAuronixTransaccion;
                    mensajeria.ApiKey = parametros!.ApiKey;
                });
            }

            scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            ////ValidarBaseDatos();
        }

        /// <summary>
        /// Método que se llamará una vez después de que se hayan ejecutado todas las pruebas secundarias. 
        /// Se garantiza que se llamará al método, incluso si se produce una excepción.
        /// </summary>
        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Inicializa una nueva instancia de la clase Microsoft.AspNetCore.Builder.WebApplicationBuilder
        /// con valores predeterminados preconfigurados.
        /// </summary>
        /// <returns>Representa un tipo utilizado para crear la configuración de la aplicación.</returns>
        private IConfigurationBuilder CreateBuilder()
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .AddJsonFile("appsettings.Development.json", true, false);
        }
        #endregion
    }
}
