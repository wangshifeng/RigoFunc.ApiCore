// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents the default implementation of <see cref="ISmsSender"/> and <see cref="IEmailSender"/> interfaces by invoking Api delegate it's job.
    /// </summary>
    public class ApiInvokingSender : IEmailSender, ISmsSender {
        private readonly SmsEmailOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiInvokingSender"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApiInvokingSender(IOptions<SmsEmailOptions> options) {
            _options = options.Value;
        }

        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task{TResult}" /> represents the send operation.</returns>
        public async virtual Task SendEmailAsync(string email, string subject, string message) {
            using (var http = new HttpClient()) {
                var value = new { Email = email, Subject = subject, Message = message };
                var response = await http.PostAsJsonAsync(_options.SmsApiUrl, value);
                if (!response.IsSuccessStatusCode) {
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                }
            }
        }

        /// <summary>
        /// Sends the Sms message asynchronous.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the send operation.</returns>
        public async virtual Task<SendSmsResult> SendSmsAsnyc(string phoneNumber, string message) {
            using (var http = new HttpClient()) {
                var value = new { PhoneNumber = phoneNumber, Message = message };
                var response = await http.PostAsJsonAsync(_options.SmsApiUrl, value);
                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsAsync<SendSmsResult>();
                    return result;
                }
                else {
                    return SendSmsResult.Failed($"send Sms failed: {await response.Content.ReadAsStringAsync()}");
                }
            }
        }

        /// <summary>
        /// Sends the Sms message asynchronous.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the send operation.</returns>
        public async virtual Task<SendSmsResult> SendSmsAsync(string template, string phoneNumber, params Tuple<string, string>[] parameters) {
            using (var http = new HttpClient()) {
                var value = new { Template = template, PhoneNumber = phoneNumber, Parameters = parameters.ToDictionary(_options) };
                var response = await http.PostAsJsonAsync(_options.SmsApiUrl, value);
                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsAsync<SendSmsResult>();
                    return result;
                }
                else {
                    return SendSmsResult.Failed($"send Sms failed: {await response.Content.ReadAsStringAsync()}");
                }
            }
        }
    }

    internal static class TupleToDictionaryExtensions {
        public static Dictionary<string, string> ToDictionary(this Tuple<string, string>[] tuples, SmsEmailOptions options) {
            var parameters = new Dictionary<string, string>();
            foreach (var item in tuples) {
                parameters[item.Item1] = item.Item2;
            }

            if (!string.IsNullOrWhiteSpace(options.ProductName) && !parameters.ContainsKey(options.ProductName)) {
                parameters[options.ProductName] = options.ProductValue;
            }

            return parameters;
        }
    }
}
