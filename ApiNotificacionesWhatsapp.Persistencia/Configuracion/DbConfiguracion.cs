namespace CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion
{
    /// <summary>
    /// Propósito: Contiene configuraciones relacionadas con la base de datos.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class DbConfiguracion
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DbConfiguracion"/>
        /// </summary>
        public DbConfiguracion()
        {
            this.Cadenas = new CadenasConexion();
            this.Procedimientos = new ProcedimientosAlmacenados();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Cadenas de conexión a bases de datos.
        /// </summary>
        public CadenasConexion Cadenas { get; set; }

        /// <summary>
        /// Procedimientos almacenados.
        /// </summary>
        public ProcedimientosAlmacenados Procedimientos { get; set; }

        #endregion
    }
}
