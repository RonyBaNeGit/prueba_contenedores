namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.Repositorios
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Arquitectura.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Dominio;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas  integrales para consultar los parámetros correspondientes de una plantilla en específico.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestConsultarParametros))]
    public class TestConsultarParametros
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestConsultarParametros"/>.
        /// </summary>
        public TestConsultarParametros()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Simula un caso de prueba correcto para consultar lista de usuarios y aplicaciones.
        /// </summary>
        /// <param name="idPlantilla">Identificador de la plantilla basado en el catálogo de de Auronix.</param>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase("72eeb2d2_8c12_4be4_89ce_2232e0ceb9ef:mensaje_de_recordatorio_posterior")]
        public async Task ConsultarParametrosPlantilla(string idPlantilla)
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            IEnumerable<Parametro>? lista = await repositorioWhatsApp.ConsultarParametrosPlantilla(idPlantilla);
            Assert.That(lista, Is.Not.Null);
            Assert.That(lista!.Count, Is.GreaterThanOrEqualTo(1));
        }

        /// <summary>
        /// Simula un caso de prueba incorrecto para insertar un registro de notificación vía whatsapp enviada, siendo el escenario que el procedimiento almacenado no exista o sea incorrecto.
        /// </summary>
        /// <param name="idPlantilla">Identificador de la plantilla basado en el catálogo de de Auronix.</param>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase("72eeb2d2_8c12_4be4_89ce_2232e0ceb9ef:mensaje_de_recordatorio_posterior")]
        public async Task ConsultarParametrosPlantillaSPIncorrecto(string idPlantilla)
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();

            ProcedimientosAlmacenados procedimientos = ObtenerConfiguracionProcedimientosAlmacenados();
            string procedimientoAlmacenado = procedimientos.SPInsertarNotificacionWAEvento;
            procedimientos.SPInsertarNotificacionWAEvento = "SP_Test";

            NotificacionWhatsApp notificacionWhatsApp = CrearInstanciaNotificacionWhatsApp();
            notificacionWhatsApp.IdTransaccionProceso = trazabilidadDTO.IdTransaccion;
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            IEnumerable<Parametro>? lista = await repositorioWhatsApp.ConsultarParametrosPlantilla(idPlantilla);
            Assert.That(lista, Is.Null);
        }

        #endregion
    }
}
