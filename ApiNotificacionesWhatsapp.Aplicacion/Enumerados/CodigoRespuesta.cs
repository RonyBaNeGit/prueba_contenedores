namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Enumerados
{
    /// <summary>
    /// Propósito: Lista de posibles códigos de error para una solicitud o transacción.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public enum CodigoRespuesta
    {
        /// <summary>
        /// Estatus que indica que se ha realizado la operación correctamente.
        /// </summary>
        Correcto = 0,

        /// <summary>
        /// Estatus que indica que se ha consumido el servicio incorrectamente.
        /// </summary>
        Invalido = 1,

        /// <summary>
        /// Estatus que indica que el servicio ha producido un error.
        /// </summary>
        ErrorInterno = 2,
    }
}