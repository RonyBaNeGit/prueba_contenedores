namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs
{
    using System;

    /// <summary>
    /// Propósito: Parámetros de configuración para el control del flujo de orquestación.
    /// Fecha de creación: 23/10/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class LimiteNotificacionesDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LimiteNotificacionesDTO"/>.
        /// </summary>
        public LimiteNotificacionesDTO()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Contador de notificaciones enviadas por día a META.
        /// </summary>
        public int TotalEnviadas { get; set; }

        /// <summary>
        /// Fecha del último envío.
        /// </summary>
        public DateTime FechaUltimoEnvio { get; set; } = DateTime.Now;

        #endregion
    }
}
