using System;
using System.Text;

namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Service credentials.
    /// </summary>
    public class DirectPaymentCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectPaymentCredentials"/> class.
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the service ID is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if the username/password is invalid.</exception>
        public DirectPaymentCredentials(int serviceId, string username, string password)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serviceId));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Argument is null or empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Argument is null or empty", nameof(password));
            }

            ServiceId = serviceId;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Gets the service ID.
        /// </summary>
        public int ServiceId { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Converts the <see cref="Username"/> and <see cref="Password"/> to a basic auth base-64 string.
        /// </summary>
        /// <returns>The base-64 encoded basic auth string.</returns>
        public string ToBasicAuthBase64String() => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ServiceId: {ServiceId}, Username: {Username}, Password: {Password}";
    }
}