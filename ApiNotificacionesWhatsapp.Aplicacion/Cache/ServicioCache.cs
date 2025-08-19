namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Cache
{
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Propósito: Implementa los métodos necesarios para gestionar la información que debe almacenarse en caché para el servicio.
    /// Fecha de creación: 08/11/2024.
    /// Creador: Adrian Velazquez Rocha (vera26024).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>    
    /// <typeparam name="T">Valor a almacenar en caché.</typeparam>
    public class ServicioCache<T> : IServicioCache<T>
    {
        #region Variables

        /// <summary>
        /// Interfaz que nos provee de los métodos necesarios para la administración de cache en el servicio.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        #endregion

        #region Constructores

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioCache{T}"/> class.
        /// </summary>
        /// <param name="memoryCache">Interfaz que nos provee de los métodos necesarios para la administración de cache en el servicio.</param>
        public ServicioCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        #endregion        

        #region Métodos Públicos

        /// <summary>
        /// Almacena un objeto en caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        /// <param name="valor">Información a almacenar en caché.</param>
        /// <param name="tiempoExpiracion">Duración en minutos del tiempo de vida en caché.</param>
        public void AlmacenarCache(string clave, T valor, int tiempoExpiracion)
        {
            MemoryCacheEntryOptions opcionesCache = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(tiempoExpiracion));
            this.memoryCache.Set(clave, valor, opcionesCache);
        }

        /// <summary>
        /// Almacena un objeto en caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        /// <returns>El objeto almacenado en caché o nulo.</returns>
        public T? ObtenerCache(string clave)
        {
            this.memoryCache.TryGetValue(clave, out T? valor);
            return valor;
        }

        /// <summary>
        /// Elimina un objeto de la caché.
        /// </summary>
        /// <param name="clave">Clave con el cual se identifica el objeto almacenado.</param>
        public void EliminarCache(string clave)
        {
            this.memoryCache.Remove(clave);
        }

        #endregion
    }
}