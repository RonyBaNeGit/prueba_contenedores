namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using System.Text;
    using CPM.AspNetCore.OpenApi.Interfaces;

    /// <summary>
    /// Propósito: Establece los estatus HTTP de respuesta y los encabezados de petición requeridos
    /// para cada operación del servicio api que se visualizaran en el Swagger.
    /// Fecha de creación: 03/06/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó: No aplica.
    /// Dependencias de conexiones e interfaces: IOperationFilter.
    /// </summary>
    public class OperationRulesFilter : IOperationRulesFilter
    {
        /// <summary>
        /// Método que devuelve el listado de reglas o validaciones establecidas en la operación de la API.
        /// </summary>
        /// <param name="operacion">Nombre completo de la operación a consultar.</param>
        /// <returns>Cadena HTML que contiene las reglas especificadas en la operación.</returns>
        public string ObtenerReglasOperacion(string operacion)
        {
            StringBuilder reglasOperacion = new StringBuilder();
            switch (operacion)
            {
                case "Simple":
                    reglasOperacion.AppendLine("\n#1 El nuevo método debe soportar aproximadamente hasta 10,000 peticiones diarias.");
                    reglasOperacion.AppendLine("\n#2 Los siguientes campos son parámetros de entrada para enviar el mensaje de WhatsApp:");
                    reglasOperacion.AppendLine("\n   - Folio de solicitud:");
                    reglasOperacion.AppendLine("\n      1.Longitud máxima de 20 dígitos y mínima de 10 dígitos.");
                    reglasOperacion.AppendLine("\n      2.Solo valores de tipo númerico en formato de texto.");
                    reglasOperacion.AppendLine("\n      3.Opcional; se permite vacío o nulo.");
                    reglasOperacion.AppendLine("\n   - Número de socio:");
                    reglasOperacion.AppendLine("\n      1.Longitud mínima de 1 dígito y máxima de 10 dígitos.");
                    reglasOperacion.AppendLine("\n      2.Solo valores de tipo númerico en formato de texto.");
                    reglasOperacion.AppendLine("\n      3.Obligatorio;");
                    reglasOperacion.AppendLine("\n   - Número de teléfono destinatario:");
                    reglasOperacion.AppendLine("\n      1.Longitud de 10 dígitos.");
                    reglasOperacion.AppendLine("\n      2.Solo valores de tipo númerico en formato de texto.");
                    reglasOperacion.AppendLine("\n      3.Obligatorio;");
                    reglasOperacion.AppendLine("\n   - Id plantilla WhatsApp:");
                    reglasOperacion.AppendLine("\n      1.Debe ser tipo texto.");
                    reglasOperacion.AppendLine("\n      2.Obligatorio;");
                    reglasOperacion.AppendLine("\n   - Parámetros de la plantilla:");
                    reglasOperacion.AppendLine("\n      1.Debe ser tipo texto.");
                    break;
            }

            return reglasOperacion.ToString();
        }
    }
}
