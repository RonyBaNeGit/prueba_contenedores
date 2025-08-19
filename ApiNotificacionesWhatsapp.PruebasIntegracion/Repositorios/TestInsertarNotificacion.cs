namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.Repositorios
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Auronix.Dominio;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas integrales para insertar en BD un registro de notificación vía whatsapp enviada.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestInsertarNotificacion))]
    public class TestInsertarNotificacion
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestInsertarNotificacion"/>.
        /// </summary>
        public TestInsertarNotificacion()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Simula un caso de prueba correcto para insertar un registro de notificación vía whatsapp enviada.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase("72eeb2d2_8c12_4be4_89ce_2232e0ceb9ef:mensaje_de_recordatorio_posterior")]
        public async Task InsertarNotificacion()
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();
            NotificacionWhatsApp notificacionWhatsApp = CrearInstanciaNotificacionWhatsApp();
            notificacionWhatsApp.IdTransaccionProceso = trazabilidadDTO.IdTransaccion;
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            bool resultado = await repositorioWhatsApp.InsertarNotificacionWA(notificacionWhatsApp);
            Assert.That(resultado);
        }

        /// <summary>
        /// Simula un caso de prueba incorrecto para insertar un registro de notificación vía whatsapp enviada, siendo el escenario que el procedimiento almacenado no exista o sea incorrecto.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [Test]
        public async Task InsertarNotificacionSPIncorrecto()
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();

            ProcedimientosAlmacenados procedimientos = ObtenerConfiguracionProcedimientosAlmacenados();
            string procedimientoAlmacenado = procedimientos.SPInsertarNotificacionWAEvento;
            procedimientos.SPInsertarNotificacionWAEvento = "SP_Test";

            NotificacionWhatsApp notificacionWhatsApp = CrearInstanciaNotificacionWhatsApp();
            notificacionWhatsApp.IdTransaccionProceso = trazabilidadDTO.IdTransaccion;
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            bool resultado = await repositorioWhatsApp.InsertarNotificacionWA(notificacionWhatsApp);
            Assert.That(!resultado);
        }

        #endregion
    }
}
