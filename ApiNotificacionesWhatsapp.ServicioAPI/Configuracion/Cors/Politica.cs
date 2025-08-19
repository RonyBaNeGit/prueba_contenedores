namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Configuracion.Cors
{
    /// <summary>
    /// Propósito: Politica para habilitar el enrutamiento de puntos de conexión (CORS).
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class Politica
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Politica"/>.
        /// </summary>
        public Politica()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Nombre de la directiva.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Lista de puntos de conexión permitidos.
        /// </summary>
        public string[] Origenes { get; set; } = Array.Empty<string>();

        #endregion
    }
}
