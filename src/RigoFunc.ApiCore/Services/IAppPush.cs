// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System.Threading.Tasks;

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents a App push target.
    /// </summary>
    public class Target {
        /// <summary>
        /// Gets or sets the client Id.
        /// </summary>
        /// <value>The client Id.</value>
        public string ClientId { get; set; }
        /// <summary>
        /// Gets or sets the alias which binding to client Id.
        /// </summary>
        /// <value>The alias binding to client Id.</value>
        /// <remarks>
        /// In some case, we cann't or not easy to get the client Id, but the client Id had bind an alias to it.
        /// </remarks>
        public string Alias { get; set; }

        /// <summary>
        /// Create a new instance by the alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>A new instance of <see cref="Target"/>.</returns>
        public static Target FromAlias(string alias) {
            return new Target {
                Alias = alias
            };
        }
    }

    /// <summary>
    /// Provides the interfaces for App message push
    /// </summary>
    public interface IAppPush {
        /// <summary>
        /// Pushes the message to list clients asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message</typeparam>
        /// <param name="appId">The App identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="tagets">The target clients that message will be push to.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        Task PushMessageToListAsync<TMessage>(string appId, TMessage message, params Target[] tagets) where TMessage : class;

        /// <summary>
        /// Pushes the message to application asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message</typeparam>
        /// <param name="appId">The App identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        Task PushMessageToAppAsync<TMessage>(string appId, TMessage message) where TMessage : class;
    }
}
