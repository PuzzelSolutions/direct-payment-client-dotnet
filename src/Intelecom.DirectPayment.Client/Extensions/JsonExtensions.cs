using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client.Extensions
{
    /// <summary>
    /// Extension methods for working with JSON.
    /// </summary>
    internal static class JsonExtensions
    {
        /// <summary>
        /// Converts an object to <see cref="StringContent"/>.
        /// </summary>
        /// <param name="obj">The object that should be converted.</param>
        /// <param name="serializerSettings">JSON serializer settings.</param>
        /// <returns>The object converted to <see cref="StringContent"/>.</returns>
        public static StringContent CreateStringContent(this object obj, JsonSerializerSettings serializerSettings)
        {
            var json = JsonConvert.SerializeObject(obj, serializerSettings);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Deserializes the HTTP content to an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="message">The HTTP response message.</param>
        /// <returns>The deserialized JSON converted to the specified type.</returns>
        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage message)
        {
            var responseJson = await message.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        /// <summary>
        /// Deserializes the HTTP content to an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="responseMessageTask">The task that returns an HTTP response message.</param>
        /// <returns>The deserialized JSON converted to the specified type.</returns>
        public static async Task<T> DeserializeAsync<T>(this Task<HttpResponseMessage> responseMessageTask)
        {
            var responseMessage = await responseMessageTask;

            return await DeserializeAsync<T>(responseMessage);
        }
    }
}