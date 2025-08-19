namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion
{
    using CPM.Extensiones.Configuracion.Core.Configuracion;

    /// <summary>
    /// Propósito: Parámetros de la sección Aplicacion en la configuración.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class Aplicacion : AplicacionBase
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en la configuración.
        /// </summary>
        public const string Seccion = nameof(Aplicacion);

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Aplicacion"/>.
        /// </summary>
        public Aplicacion()
            : base()
        {
        }

        #endregion

    }
}