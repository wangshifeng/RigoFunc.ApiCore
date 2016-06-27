// Copyright (c) RigoFunc (xuyingting). All rights reserved.

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents all the options you can user to configure the service.
    /// </summary>
    public class SmsEmailOptions {
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
    }
}
