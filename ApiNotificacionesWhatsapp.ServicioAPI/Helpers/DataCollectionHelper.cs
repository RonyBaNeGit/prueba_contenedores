namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Helpers
{
    using System.Reflection;

    /// <summary>
    /// Propósito: Clase auxiliar para la conversión de colecciones de datos.
    /// Fecha de creación: 17/10/2023.
    /// Creador: Juan Carlos Miranda Méndez (MIMJ22037).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class DataCollectionHelper
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Convierte un diccionario de datos a una instancia de la clase especificada.
        /// </summary>
        /// <typeparam name="T">Tipo de dato destino.</typeparam>
        /// <param name="dict">Diccionario de valores.</param>
        /// <returns>Instancia de la clase destino.</returns>
        public static T DictionaryToObject<T>(IDictionary<string, string> dict)
            where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Encuentra el tipo de dato de la propiedad ACTUAL (int, cadena, doble, etc.)
                Type tPropertyType = t.GetType().GetProperty(property.Name)!.PropertyType;

                // Reparar valores anulables.
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...y cambia el tipo.
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name)!.SetValue(t, newA, null);
            }

            return t;
        }

        #endregion
    }
}
