namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.Repositorios
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Auronix.Dominio;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas integrales para insertar en BD un registro de la relación entre un parámetro y el identificador del evento de una notificación de whatsapp enviada.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestInsertarEventoParametro))]
    public class TestInsertarEventoParametro
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestInsertarEventoParametro"/>.
        /// </summary>
        public TestInsertarEventoParametro()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Simula un caso de prueba correcto para insertar un registro de relación evento-parámetro de  una notificación vía whatsapp enviada.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase]
        public async Task InsertarEventoParametro()
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();
            EventoParametro evento = CrearInstanciaEventoParametro();
            evento.IdTransaccionRegistro = trazabilidadDTO.IdTransaccion;
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            bool resultado = await repositorioWhatsApp.InsertarEventoParametro(evento);
            Assert.That(resultado);
        }

        /// <summary>
        /// Simula un caso de prueba incorrecto para insertar un registro de relación evento-parámetro de  una notificación vía whatsapp enviada, siendo el escenario que el procedimiento almacenado no exista o sea incorrecto.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase]
        public async Task InsertarNotificacionSPIncorrecto()
        {
            TrazabilidadDTO trazabilidadDTO = CrearInstanciaTrazabilidadDTO();

            ProcedimientosAlmacenados procedimientos = ObtenerConfiguracionProcedimientosAlmacenados();
            string procedimientoAlmacenado = procedimientos.SPInsertarNotificacionWAEventoParametro;
            procedimientos.SPInsertarNotificacionWAEvento = "SP_Test";

            EventoParametro evento = CrearInstanciaEventoParametro();
            evento.IdTransaccionRegistro = trazabilidadDTO.IdTransaccion;
            IRepositorioWhatsApp repositorioWhatsApp = CrearInstanciaRepositorioWhatsApp();
            bool resultado = await repositorioWhatsApp.InsertarEventoParametro(evento);
            Assert.That(!resultado);
        }

        #endregion
    }
}
