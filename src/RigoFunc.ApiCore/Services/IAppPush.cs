// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents the message push target.
    /// </summary>
    public class Target {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string Alias { get; set; }
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public string AppId { get; set; }
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId { get; set; }
    }

    /// <summary>
    /// Provides the interfaces for App message push
    /// </summary>
    public interface IAppPush {
        /// <summary>
        /// Pushes the message to single client asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="target">The target.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        Task PushMessageToSingleAsync<T>(T message, Target target);

        /// <summary>
        /// Pushes the message to list clients asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="targets">The targets.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        Task PushMessageToListAsync<T>(T message, IList<Target> targets);

        /// <summary>
        /// Pushes the message to application asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the message</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        Task PushMessageToAppAsync<T>(T message);
    }
}
