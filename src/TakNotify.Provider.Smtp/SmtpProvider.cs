// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TakNotify
{
    /// <summary>
    /// The implementation of <see cref="SmtpProvider"/>
    /// </summary>
    public class SmtpProvider : NotificationProvider
    {
        private readonly ISmtpClient _smtpClient;
        private readonly SmtpProviderOptions _smtpOptions;

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SmtpProvider(SmtpProviderOptions smtpOptions, ILoggerFactory loggerFactory)
            : base(smtpOptions, loggerFactory)
        {
            _smtpClient = SmtpClient.Create(smtpOptions);
            _smtpOptions = smtpOptions;
        }

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="smtpClient">The SMTP Client</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SmtpProvider(ISmtpClient smtpClient, ILoggerFactory loggerFactory)
            : base(smtpClient.Options, loggerFactory)
        {
            _smtpClient = smtpClient;
        }

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SmtpProvider(IOptions<SmtpProviderOptions> smtpOptions, ILoggerFactory loggerFactory)
            : base(smtpOptions.Value, loggerFactory)
        {
            _smtpClient = SmtpClient.Create(smtpOptions.Value);
        }

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// <para>Please be aware in using this constructor because it was originally intended for testing and it could cause
        /// unwanted behavior if it is used in the real application</para>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="smtpClient">The SMTP Client</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SmtpProvider(SmtpProviderOptions smtpOptions, ISmtpClient smtpClient, ILoggerFactory loggerFactory)
            : base(smtpClient.Options, loggerFactory)
        {
            _smtpClient = smtpClient;
            _smtpOptions = smtpOptions;
        }

        /// <summary>
        /// <inheritdoc cref="NotificationProvider.Name"/>
        /// </summary>
        public override string Name => SmtpProviderConstants.DefaultName;

        /// <summary>
        /// <inheritdoc cref="NotificationProvider.Send(MessageParameterCollection)"/>
        /// </summary>
        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var emailMessage = new EmailMessage(messageParameters);

            // mail message
            var message = new MailMessage
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml
            };

            if (!string.IsNullOrEmpty(emailMessage.FromAddress))
                message.From = new MailAddress(emailMessage.FromAddress);
            else if (_smtpOptions != null && !string.IsNullOrEmpty(_smtpOptions.DefaultFromAddress))
                message.From = new MailAddress(_smtpOptions.DefaultFromAddress);
            else
                return new NotificationResult(new List<string> { "From Address should not be empty" });

            foreach (var address in emailMessage.ToAddresses)
            {
                message.To.Add(new MailAddress(address));
            }

            foreach (var address in emailMessage.CCAddresses)
            {
                message.CC.Add(new MailAddress(address));
            }

            foreach (var address in emailMessage.BCCAddresses)
            {
                message.Bcc.Add(new MailAddress(address));
            }

            // send email
            Logger.LogDebug(SmtpLogMessages.Sending_Start, emailMessage.Subject, emailMessage.ToAddresses);

            await _smtpClient.SendMailAsync(message);

            Logger.LogDebug(SmtpLogMessages.Sending_End, emailMessage.Subject, emailMessage.ToAddresses);

            return new NotificationResult(true);
        }
    }
}
