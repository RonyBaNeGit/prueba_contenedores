namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios
{
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.Mensajeria.Auronix.Dominio;

    /// <summary>
    /// Propósito: Interfaz que define los métodos necesarios para la administración de notificaciones y plantillas.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public interface IRepositorioWhatsApp
    {
        #region Métodos

        /// <summary>
        /// Consulta los parámetros asociados a una plantilla específica.
        /// </summary>
        /// <param name="idPlantilla">Identificador de la plantilla para la cual se consultan los parámetros.</param>
        /// <returns>Una colección de parámetros asociados a la plantilla, o null si ocurre un error.</returns>
        Task<IEnumerable<Parametro>?> ConsultarParametrosPlantilla(string idPlantilla);

        /// <summary>
        /// Inserta una notificación de WhatsApp en la base de datos.
        /// </summary>
        /// <param name="notificacionWhatsApp">Objeto que contiene la información de la notificación a insertar.</param>
        /// <returns>Devuelve true si la inserción fue exitosa; de lo contrario, false.</returns>
        Task<bool> InsertarNotificacionWA(NotificacionWhatsApp notificacionWhatsApp);

        /// <summary>
        /// Inserta un parámetro de evento en la base de datos.
        /// </summary>
        /// <param name="eventoParametro">Objeto que contiene la información del parámetro a insertar.</param>
        /// <returns>Devuelve true si la inserción fue exitosa; de lo contrario, false.</returns>
        Task<bool> InsertarEventoParametro(EventoParametro eventoParametro);

        #endregion
    }
}