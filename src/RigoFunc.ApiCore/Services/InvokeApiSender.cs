// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace RigoFunc.ApiCore.Services {
    /// <summary>
    /// Represents the default implementation of <see cref="ISmsSender"/>, <see cref="IEmailSender"/> and <see cref="IAppPush"/> interfaces by invoking Api delegate it's job.
    /// </summary>
    public class InvokeApiSender : IEmailSender, ISmsSender, IAppPush {
        private readonly InvokeApiOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeApiSender"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public InvokeApiSender(IOptions<InvokeApiOptions> options) {
            _options = options.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeApiSender" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public InvokeApiSender(InvokeApiOptions options) {
            _options = options;
        }

        /// <summary>
        /// Pushes the message to list clients asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message</typeparam>
        /// <param name="appId">The App identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="targets">The target clients that message will be push to.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        public async Task PushMessageToListAsync<TMessage>(string appId, TMessage message, params Target[] targets) where TMessage : class {
            using (var http = new HttpClient()) {
                var response = await http.PostAsync(_options.AppPushApiUrl, GetHttpContent(appId, message, targets));
                if (!response.IsSuccessStatusCode) {
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                }
            }
        }

        /// <summary>
        /// Pushes the message to application asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message</typeparam>
        /// <param name="appId">The App identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> represents the push operation.</returns>
        public async Task PushMessageToAppAsync<TMessage>(string appId, TMessage message) where TMessage : class {
            using (var http = new HttpClient()) {
                var response = await http.PostAsync(_options.AppPushApiUrl, GetHttpContent(appId, message));
                if (!response.IsSuccessStatusCode) {
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                }
            }
        }
        
        private HttpContent GetHttpContent<TMessage>(string appId, TMessage message, params Target[] targets) where TMessage : class {
            if(targets.Length != 0 && targets.Any(t => string.IsNullOrEmpty(t.ClientId) && string.IsNullOrEmpty(t.Alias))){
                throw new ArgumentException(nameof(targets));
            }

            using (var memory = new MemoryStream()) {
                using (var writer = new BinaryWriter(memory)) {
                    writer.Write(appId);

                    if(typeof(TMessage) == typeof(string)) {
                        writer.Write(message.ToString());
                    }
                    else {
                        var json = JsonConvert.SerializeObject(message);
                        writer.Write(json);
                    }

                    var length = targets.Length;
                    writer.Write(length);
                    foreach (var target in targets) {
                        writer.Write(target.ClientId ?? "");
                        writer.Write(target.Alias ?? "");
                    }
                }
                return new ByteArrayContent(memory.ToArray());
            }
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
        public static Dictionary<string, string> ToDictionary(this Tuple<string, string>[] tuples, InvokeApiOptions options) {
            var parameters = new Dictionary<string, string>();
            foreach (var item in tuples) {
                parameters[item.Item1] = item.Item2;
            }

            return parameters;
        }
    }
}
