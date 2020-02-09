using System.Security;

namespace CloudChat
{
    /// <summary>
    /// An interface for a class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// The secure passord
        /// </summary>
        SecureString SecurePassword { get; }
    }
}