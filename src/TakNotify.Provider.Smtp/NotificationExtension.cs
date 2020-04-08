// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using System.Threading.Tasks;

namespace TakNotify
{
    /// <summary>
    /// The extensions for <see cref="INotification"/>
    /// </summary>
    public static class NotificationExtension
    {
        /// <summary>
        /// Send message through <see cref="SmtpProvider"/>
        /// </summary>
        /// <param name="notification">The notification object</param>
        /// <param name="message">The email message object</param>
        /// <returns></returns>
        public static Task<NotificationResult> SendEmailWithSmtp(this INotification notification, EmailMessage message)
        {
            return notification.Send(SmtpProviderConstants.DefaultName, message.ToParameters());
        }
    }
}
