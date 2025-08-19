namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.ServiciosOrquestacion
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Enumerados;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.ApiNotificacionesWhatsapp.Infraestructura.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Repositorios;
    using CPM.Comun.Arquitectura.Configuracion;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Dominio;
    using CPM.Mensajeria.Auronix.Persistencia.Configuracion;
    using CPM.Mensajeria.Auronix.Persistencia.Repositorios;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas para los métodos definidos en la interfaz <see cref="IOrquestadorServicioWhatsapp"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestEnvioWASimple))]
    public class TestEnvioWASimple
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestEnvioWASimple"/>.
        /// </summary>
        public TestEnvioWASimple()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Caso de prueba para realizar un envío de notificación simple por WhatsApp.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsapp()
        {
            IRepositorioMensajeriaWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioMensajeriaWhatsApp(true);
            IRepositorioMensajeria repositorioMensajeria = CrearInstanciaRepositorioMensajeria(true);
            IOrquestadorServicioWhatsapp servicio = CrearInstanciaOrquestadorWhatsapp();
            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);
            Assert.That(respuesta, Is.Not.Null);
            Assert.That(Is.Equals(respuesta.Codigo, CodigoRespuesta.Correcto));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando existe un error al obtener la configuración para envío de WhatsApp.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappErrorConfiguracionWhatsApp()
        {
            var random = new Random();
            ProcedimientosAlmacenados procedimientos = ObtenerConfiguracionProcedimientosAlmacenados();
            procedimientos.SPConsultarConfiguracionWhastapp = "SP_Test";

            IOptions<DbConfiguracion> dbConfiguracion = ObtenerConfiguracionGeneral();

            var services = new ServiceCollection();

            services.Configure<DbConfiguracionMensajeriaWhatsApp>(opts =>
            {
                opts.CadenaConexion = dbConfiguracion.Value.Cadenas.CadenaConexion;
                opts.LimiteRegistros = 100;
                opts.SPConsultarConfiguracionWhastapp = procedimientos.SPConsultarConfiguracionWhastapp;
            });

            var politicas = new PoliticasReintento
            {
                Reintento = new Reintento
                {
                    Cantidad = 3,
                    TiempoEspera = 10,
                },
            };
            var opcionesPoliticas = Options.Create(politicas);

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var configuracionOptions = scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracionMensajeriaWhatsApp>>();

            var repositorio = new RepositorioMensajeriaWhatsApp(
                ObtenerParametrosTrazabilidad(),
                configuracionOptions,
                ObtenerConfiguracionLoggerRepositorioMensajeriaWhatsApp(),
                opcionesPoliticas);

            var repositorioWhatsApp = new RepositorioWhatsApp(
                ObtenerParametrosTrazabilidad(),
                dbConfiguracion,
                ObtenerConfiguracionLoggerRepositorioWhatsApp(),
                ObtenerPoliticasReintento());

            var servicioMensajeria = new ServicioConfiguracionWhatsApp(
                repositorio,
                CrearInstanciaTiempoCache(),
                ObtenerConfiguracionMemoryCache(),
                ObtenerConfiguracionLoggerServicioMensajeriaWhatsAppSimpleTransacciones());

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                ObtenerParametrosTrazabilidad(),
                ObtenerConfiguracionMapper(),
                repositorioWhatsApp,
                ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                servicioMensajeria,
                ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                repositorio);

            IRepositorioMensajeriaWhatsApp repositorioWhatsApps = repositorio;
            IRepositorioWhatsApp repositorioWhatsApp1 = repositorioWhatsApp;
            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.ErrorInterno));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("Ocurrió un error al obtener la configuración para el envío del WhatsApp."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando existe un error al obtener las notificaciones procesadas correctamente.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappErrorObtenerNotificacionesProcesadas()
        {
            IOptions<DbConfiguracion> dbConfiguracion = ObtenerConfiguracionGeneral();

            var services = new ServiceCollection();

            services.Configure<DbConfiguracionMensajeriaWhatsApp>(opts =>
            {
                opts.CadenaConexion = dbConfiguracion.Value.Cadenas.CadenaConexion;
                opts.LimiteRegistros = 100;
                opts.SPConsultarNotificacionesProcesadasWhastapp = "SP_Test";
                opts.SPConsultarConfiguracionWhastapp = "pa_s_Consultar_Informacion_NotificacionWA_Configuracion_Cat";
            });

            var politicas = new PoliticasReintento
            {
                Reintento = new Reintento
                {
                    Cantidad = 3,
                    TiempoEspera = 10,
                },
            };
            var opcionesPoliticas = Options.Create(politicas);

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var configuracionOptions = scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracionMensajeriaWhatsApp>>();

            var repositorio = new RepositorioMensajeriaWhatsApp(
                ObtenerParametrosTrazabilidad(),
                configuracionOptions,
                ObtenerConfiguracionLoggerRepositorioMensajeriaWhatsApp(),
                opcionesPoliticas);

            var repositorioWhatsApp = new RepositorioWhatsApp(
                ObtenerParametrosTrazabilidad(),
                dbConfiguracion,
                ObtenerConfiguracionLoggerRepositorioWhatsApp(),
                ObtenerPoliticasReintento());

            var servicioMensajeria = new ServicioConfiguracionWhatsApp(
                repositorio,
                CrearInstanciaTiempoCache(),
                ObtenerConfiguracionMemoryCache(),
                ObtenerConfiguracionLoggerServicioMensajeriaWhatsAppSimpleTransacciones());

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                ObtenerParametrosTrazabilidad(),
                ObtenerConfiguracionMapper(),
                repositorioWhatsApp,
                ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                servicioMensajeria,
                ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                repositorio);

            IRepositorioMensajeriaWhatsApp repositorioWhatsApps = repositorio;
            IRepositorioWhatsApp repositorioWhatsApp1 = repositorioWhatsApp;
            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.ErrorInterno));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("Ocurrió un error al consultar las notificaciones procesadas correctamente."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando la cantidad notificaciones procesadas en el día superó el limite permitido por META.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappErrorLimiteMETASuperado()
        {
            var servicioConfigMock = Substitute.For<IServicioConfiguracionWhatsapp>();
            var repositorioMensajeriaMock = Substitute.For<IRepositorioMensajeriaWhatsApp>();

            // Configurar el método asincrónico
            servicioConfigMock.ObtenerConfiguracionWhatsappAsync()
                .Returns(Task.FromResult<ConfiguracionWhatsApp?>(new ConfiguracionWhatsApp
                {
                    LimiteNotificacionesMeta = 0,
                }));

            repositorioMensajeriaMock.ConsultarNotificacionesProcesadas(0)
       .Returns(Task.FromResult<IEnumerable<NotificacionWhatsApp>?>(new List<NotificacionWhatsApp>
       {
        new NotificacionWhatsApp { IdTransaccionRegistro = "1", },
        new NotificacionWhatsApp { IdTransaccionRegistro = "2" },
        new NotificacionWhatsApp { IdTransaccionRegistro = "3" },
       }));

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                ObtenerParametrosTrazabilidad(),
                ObtenerConfiguracionMapper(),
                CrearInstanciaRepositorioWhatsApp(),
                ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                servicioConfigMock,
                ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                repositorioMensajeriaMock);
            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.Invalido));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("Se alcanzó el límite de envío de notificaciones."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando la cantidad notificaciones procesadas en el día superó el limite permitido por META.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappErrorConsultarPlantillas()
        {
            IOptions<DbConfiguracion> dbConfiguracion = ObtenerConfiguracionGeneral();

            var services = new ServiceCollection();

            services.Configure<DbConfiguracionMensajeriaWhatsApp>(opts =>
            {
                opts.CadenaConexion = dbConfiguracion.Value.Cadenas.CadenaConexion;
                opts.LimiteRegistros = 100;
                opts.SPConsultarNotificacionesProcesadasWhastapp = "pa_s_Consultar_Informacion_NotificacionWA_Evento_Procesadas";
                opts.SPConsultarConfiguracionWhastapp = "pa_s_Consultar_Informacion_NotificacionWA_Configuracion_Cat";
                opts.SPConsultarPlantillasWhastapp = "SP_Test";
            });

            var politicas = new PoliticasReintento
            {
                Reintento = new Reintento
                {
                    Cantidad = 3,
                    TiempoEspera = 10,
                },
            };
            var opcionesPoliticas = Options.Create(politicas);

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var configuracionOptions = scope.ServiceProvider.GetRequiredService<IOptions<DbConfiguracionMensajeriaWhatsApp>>();

            var repositorio = new RepositorioMensajeriaWhatsApp(
                ObtenerParametrosTrazabilidad(),
                configuracionOptions,
                ObtenerConfiguracionLoggerRepositorioMensajeriaWhatsApp(),
                opcionesPoliticas);

            var repositorioWhatsApp = new RepositorioWhatsApp(
                ObtenerParametrosTrazabilidad(),
                dbConfiguracion,
                ObtenerConfiguracionLoggerRepositorioWhatsApp(),
                ObtenerPoliticasReintento());

            var servicioMensajeria = new ServicioConfiguracionWhatsApp(
                repositorio,
                CrearInstanciaTiempoCache(),
                ObtenerConfiguracionMemoryCache(),
                ObtenerConfiguracionLoggerServicioMensajeriaWhatsAppSimpleTransacciones());

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                ObtenerParametrosTrazabilidad(),
                ObtenerConfiguracionMapper(),
                repositorioWhatsApp,
                ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                servicioMensajeria,
                ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                repositorio);

            IRepositorioMensajeriaWhatsApp repositorioWhatsApps = repositorio;
            IRepositorioWhatsApp repositorioWhatsApp1 = repositorioWhatsApp;
            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.ErrorInterno));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("Error al consultar el registro de la plantilla solicitada."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando la cantidad notificaciones procesadas en el día superó el limite permitido por META.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappInvalidoConsultarPlantillas()
        {
            var repositorioMensajeriaMock = Substitute.For<IRepositorioMensajeriaWhatsApp>();

            repositorioMensajeriaMock.ConsultarPlantillas()
            .Returns(Task.FromResult<IEnumerable<PlantillaWhatsApp>?>(new List<PlantillaWhatsApp>()));

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                ObtenerParametrosTrazabilidad(),
                ObtenerConfiguracionMapper(),
                CrearInstanciaRepositorioWhatsApp(),
                ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                CrearInstanciaObtenerConfiguracion(),
                ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                repositorioMensajeriaMock);

            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            SolicitudDTO solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.Invalido));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("No existe la plantilla proporcionada."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando ocurre un error al consultar los parámetros de la plantilla solicitada en la solicitud.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappInvalidoConsultarParametros()
        {
            var repositorioWAMock = Substitute.For<IRepositorioWhatsApp>();

            repositorioWAMock.ConsultarParametrosPlantilla(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<Parametro>?>(new List<Parametro>()));

            var solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                        ObtenerParametrosTrazabilidad(),
                        ObtenerConfiguracionMapper(),
                        repositorioWAMock,
                        ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                        CrearInstanciaObtenerConfiguracion(),
                        ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                        CrearInstanciaRepositorioMensajeriaWhatsApp());

            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.Invalido));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("La plantilla solicitada no cuenta con parámetros configurados."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando el número de parámetros recibidos en la solicitud no coincide con los necesarios para la plantilla solicitada.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappInvalidoParametrosIncorrectos()
        {
            var repositorioWAMock = Substitute.For<IRepositorioWhatsApp>();

            repositorioWAMock.ConsultarParametrosPlantilla(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<Parametro>?>(new List<Parametro>()
            {
                new Parametro()
                {
                    Id = "IdPrueba",
                },
            }));

            var solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                        ObtenerParametrosTrazabilidad(),
                        ObtenerConfiguracionMapper(),
                        repositorioWAMock,
                        ObtenerServicioMensajeriaWhatsAppSimpleTransacciones(),
                        CrearInstanciaObtenerConfiguracion(),
                        ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                        CrearInstanciaRepositorioMensajeriaWhatsApp());

            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.Invalido));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("La cantidad de parámetros proporcionados no coincide con la cantidad requerida para la plantilla."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando ocurre un error al intentar comunicarse con el servicio de Auronix.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappErrorConexionAuronix()
        {
            var servicioWAMock = Substitute.For<IServicioMensajeriaWhatsAppSimpleTransacciones>();

            servicioWAMock.EnviarWhatsAppSimple(Arg.Any<PeticionWhatsAppSimpleDTO>())
            .Returns(Task.FromResult<RespuestaServicioAuronixWASimpleDTO?>(null));

            var solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                        ObtenerParametrosTrazabilidad(),
                        ObtenerConfiguracionMapper(),
                        CrearInstanciaRepositorioWhatsApp(),
                        servicioWAMock,
                        CrearInstanciaObtenerConfiguracion(),
                        ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                        CrearInstanciaRepositorioMensajeriaWhatsApp());

            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.ErrorInterno));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.EqualTo("Ocurrió un error al intentar establecer comunicación con el servicio de Auronix."));
        }

        /// <summary>
        /// Caso de prueba de error para realizar un envío de notificación simple por WhatsApp cuando se envían valores inválidos al servicio de Auronix.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioWhatsappInvalido()
        {
            var servicioWAMock = Substitute.For<IServicioMensajeriaWhatsAppSimpleTransacciones>();

            servicioWAMock.EnviarWhatsAppSimple(Arg.Any<PeticionWhatsAppSimpleDTO>())
            .Returns(Task.FromResult<RespuestaServicioAuronixWASimpleDTO?>(
                new
        RespuestaServicioAuronixWASimpleDTO()
                {
                    Descripcion = "Algo salio mal",
                }));

            var solicitudDTO = CrearInstanciaSolicitud();
            solicitudDTO.Telefono = TelefonoIncorrecto;

            var servicioWhatsApp = new OrquestadorServicioWhatsapp(
                        ObtenerParametrosTrazabilidad(),
                        ObtenerConfiguracionMapper(),
                        CrearInstanciaRepositorioWhatsApp(),
                        servicioWAMock,
                        CrearInstanciaObtenerConfiguracion(),
                        ObtenerConfiguracionLoggerOrquestadorServicioWhatsapp(),
                        CrearInstanciaRepositorioMensajeriaWhatsApp());

            IOrquestadorServicioWhatsapp servicio = servicioWhatsApp;

            RespuestaDTO respuesta = await servicio.EnviarWhatsAppIndividual(solicitudDTO);

            Assert.That(respuesta, Is.Not.Null);
            Assert.That(respuesta.Codigo, Is.EqualTo(CodigoRespuesta.Invalido));
            Assert.That(respuesta.IdTransaccion, Is.Not.Null.Or.Empty);
            Assert.That(respuesta.Mensaje, Is.Not.Null.Or.Empty);
        }
        #endregion
    }
}
