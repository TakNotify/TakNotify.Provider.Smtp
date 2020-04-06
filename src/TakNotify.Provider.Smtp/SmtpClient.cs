// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using System.Net;
using NetMail = System.Net.Mail;

namespace TakNotify
{
    public class SmtpClient : NetMail.SmtpClient, ISmtpClient
    {
        /// <summary>
        /// Initialize a new instance of <see cref="SmtpClient"/>
        /// </summary>
        public SmtpClient()
        {
            Options = new SmtpProviderOptions();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="SmtpClient"/>
        /// </summary>
        /// <param name="host"></param>
        public SmtpClient(string host)
            : base(host)
        {
            Options = new SmtpProviderOptions
            {
                Server = host
            };
        }

        /// <summary>
        /// Initialize a new instance of <see cref="SmtpClient"/>
        /// </summary>
        /// <param name="host">The SMTP host</param>
        /// <param name="port">The SMTP port</param>
        public SmtpClient(string host, int port)
            : base(host, port)
        {
            Options = new SmtpProviderOptions
            {
                Server = host,
                Port = port
            };
        }

        /// <summary>
        /// Initialize a new instance of <see cref="SmtpClient"/>
        /// </summary>
        /// <param name="host">The SMTP host</param>
        /// <param name="port">The SMTP port</param>
        /// <param name="username">The SMTP Username</param>
        /// <param name="password">The SMTP Password</param>
        /// <param name="enableSsl">Enable SSL</param>
        public SmtpClient(string host, int port, string username, string password, bool enableSsl)
            : base(host, port)
        {
            EnableSsl = enableSsl;

            if (!string.IsNullOrEmpty(username))
            {
                UseDefaultCredentials = false;
                Credentials = new NetworkCredential(username, password);
            }

            Options = new SmtpProviderOptions
            {
                Server = host,
                Port = port,
                Username = username,
                Password = password,
                UseSSL = enableSsl
            };
        }

        /// <summary>
        /// <inheritdoc cref="ISmtpClient.Options"/>
        /// </summary>
        public SmtpProviderOptions Options { get; }

        public static SmtpClient Create(SmtpProviderOptions smtpOptions)
        {
            return new SmtpClient(
                smtpOptions.Server,
                smtpOptions.Port,
                smtpOptions.Username,
                smtpOptions.Password,
                smtpOptions.UseSSL);
        }
    }
}
