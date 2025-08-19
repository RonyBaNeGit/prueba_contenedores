namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Cache
{
    /// <summary>
    /// Propósito: Define los métodos necesarios para gestionar la información que debe almacenarse en caché para el servicio.
    /// Fecha de creación: 08/11/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó: No Aplica.
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    /// <typeparam name="T">Valor a almacenar en caché.</typeparam>
    public interface IServicioCache<T>
    {
        /// <summary>
        /// Almacena un objeto en caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        /// <param name="valor">Información a almacenar en caché.</param>
        /// <param name="tiempoExpiracion">Duración en minutos del tiempo de vida en caché.</param>
        void AlmacenarCache(string clave, T valor, int tiempoExpiracion);

        /// <summary>
        /// Almacena un objeto en caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        /// <returns>El objeto almacenado en caché o nulo.</returns>
        T? ObtenerCache(string clave);

        /// <summary>
        /// Elimina un objeto de la caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        void EliminarCache(string clave);
    }
}
