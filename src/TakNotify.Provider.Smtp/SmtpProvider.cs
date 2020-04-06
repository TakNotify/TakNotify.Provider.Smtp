// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SmtpProvider(SmtpProviderOptions smtpOptions, ILoggerFactory loggerFactory)
            : base(smtpOptions, loggerFactory)
        {
            _smtpClient = SmtpClient.Create(smtpOptions);
        }

        /// <summary>
        /// Instantiate the <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="smtpClient">The SMT Client</param>
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
                From = new MailAddress(emailMessage.FromAddress),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml
            };

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
