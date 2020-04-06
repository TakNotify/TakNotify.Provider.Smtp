// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using TakNotify.Test;
using Xunit;

namespace TakNotify.Provider.Smtp.Test
{
    public class SmtpProviderTest
    {
        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly Mock<ILogger<Notification>> _logger;
        private readonly Mock<ISmtpClient> _smtpClient;

        public SmtpProviderTest()
        {
            _loggerFactory = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger<Notification>>();
            _smtpClient = new Mock<ISmtpClient>();

            _loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(_logger.Object);
        }

        [Fact]
        public async void Send_Success()
        {
            _smtpClient.Setup(client => client.SendMailAsync(It.IsAny<MailMessage>()))
                .Returns(Task.CompletedTask);

            var provider = new SmtpProvider(_smtpClient.Object, _loggerFactory.Object);

            var message = new EmailMessage
            {
                FromAddress = "sender@example.com",
                ToAddresses = new List<string> { "user@example.com" },
                Subject = "Test Email"
            };

            var result = await provider.Send(message.ToParameters());

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);

            var startMessage = LoggerHelper.FormatLogValues(SmtpLogMessages.Sending_Start, message.Subject, message.ToAddresses);
            _logger.VerifyLog(LogLevel.Debug, startMessage);

            var endMessage = LoggerHelper.FormatLogValues(SmtpLogMessages.Sending_End, message.Subject, message.ToAddresses);
            _logger.VerifyLog(LogLevel.Debug, endMessage);

        }

        [Fact]
        public void Send_ThrowException()
        {
            _smtpClient.Setup(client => client.SendMailAsync(It.IsAny<MailMessage>()))
                .Throws(new Exception("Exception01"));

            var provider = new SmtpProvider(_smtpClient.Object, _loggerFactory.Object);

            var message = new EmailMessage
            {
                FromAddress = "sender@example.com",
                ToAddresses = new List<string> { "user@example.com" },
                Subject = "Test Email"
            };

            var exception = Record.ExceptionAsync(() => provider.Send(message.ToParameters()));

            Assert.Equal("Exception01", exception.Result.Message);

            var startMessage = LoggerHelper.FormatLogValues(SmtpLogMessages.Sending_Start, message.Subject, message.ToAddresses);
            _logger.VerifyLog(LogLevel.Debug, startMessage);

            var endMessage = LoggerHelper.FormatLogValues(SmtpLogMessages.Sending_End, message.Subject, message.ToAddresses);
            _logger.VerifyLog(LogLevel.Debug, endMessage, Times.Never());

        }
    }
}
