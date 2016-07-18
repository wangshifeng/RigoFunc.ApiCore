// Copyright (c) RigoFunc (xuyingting). All rights reserved.

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents all the options you can user to configure the service.
    /// </summary>
    public class InvokeApiOptions {
        /// <summary>
        /// Gets or sets the send Sms API URL.
        /// </summary>
        /// <value>The send Sms API URL.</value>
        public string SmsApiUrl { get; set; }
        /// <summary>
        /// Gets or sets the send email API URL.
        /// </summary>
        /// <value>The send email API URL.</value>
        public string EmailApiUrl { get; set; }
        /// <summary>
        /// Gets or sets the App push API URL.
        /// </summary>
        /// <value>The App push API URL.</value>
        public string AppPushApiUrl { get; set; }
    }
}
