// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Linq;

namespace TakNotify
{
    /// <summary>
    /// The wrapper of email message that will be sent through SMTP
    /// </summary>
    public class EmailMessage
    {
        internal static string Parameter_ToAddresses = $"{SmtpProviderConstants.DefaultName}_{nameof(ToAddresses)}";
        internal static string Parameter_CCAddresses = $"{SmtpProviderConstants.DefaultName}_{nameof(CCAddresses)}";
        internal static string Parameter_BCCAddresses = $"{SmtpProviderConstants.DefaultName}_{nameof(BCCAddresses)}";
        internal static string Parameter_FromAddress = $"{SmtpProviderConstants.DefaultName}_{nameof(FromAddress)}";
        internal static string Parameter_Subject = $"{SmtpProviderConstants.DefaultName}_{nameof(Subject)}";
        internal static string Parameter_Body = $"{SmtpProviderConstants.DefaultName}_{nameof(Body)}";
        internal static string Parameter_IsHtml = $"{SmtpProviderConstants.DefaultName}_{nameof(IsHtml)}";

        /// <summary>
        /// Instantiate an <see cref="EmailMessage"/> object
        /// </summary>
        public EmailMessage()
        {
            ToAddresses = new List<string>();
            CCAddresses = new List<string>();
            BCCAddresses = new List<string>();
        }

        /// <summary>
        /// Instantiate <see cref="EmailMessage"/>
        /// </summary>
        /// <param name="parameters">The collection of the message parameters</param>
        public EmailMessage(MessageParameterCollection parameters)
        {
            if (parameters.ContainsKey(Parameter_ToAddresses))
                ToAddresses = parameters[Parameter_ToAddresses].Split(',').ToList();
            else
                ToAddresses = new List<string>();

            if (parameters.ContainsKey(Parameter_CCAddresses))
                CCAddresses = parameters[Parameter_CCAddresses].Split(',').ToList();
            else
                CCAddresses = new List<string>();

            if (parameters.ContainsKey(Parameter_BCCAddresses))
                BCCAddresses = parameters[Parameter_BCCAddresses].Split(',').ToList();
            else
                BCCAddresses = new List<string>();

            if (parameters.ContainsKey(Parameter_FromAddress))
                FromAddress = parameters[Parameter_FromAddress];

            if (parameters.ContainsKey(Parameter_Subject))
                Subject = parameters[Parameter_Subject];

            if (parameters.ContainsKey(Parameter_Body))
                Body = parameters[Parameter_Body];

            if (parameters.ContainsKey(Parameter_IsHtml))
                if (bool.TryParse(parameters[Parameter_IsHtml], out bool isHtml))
                    IsHtml = isHtml;
        }

        /// <summary>
        /// The collection of <b>TO</b> addresses of the email
        /// </summary>
        public List<string> ToAddresses { get; set; }

        /// <summary>
        /// The collection of <b>CC</b> addresses of the email
        /// </summary>
        public List<string> CCAddresses { get; set; }

        /// <summary>
        /// The collection of <b>BCC</b> addresses of the email
        /// </summary>
        public List<string> BCCAddresses { get; set; }

        /// <summary>
        /// The <b>FROM</b> address of the email. If empty, the <see cref="SmtpProviderOptions.DefaultFromAddress"/> will be used instead
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// The <b>Subject</b> of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The <b>Body</b> of the email
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Is the <see cref="Body"/> a HTML text? (If the value is <b>false</b>, the <see cref="Body"/> is a plain text.)
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Convert the properties into message parameters
        /// </summary>
        /// <returns></returns>
        public MessageParameterCollection ToParameters()
        {
            var parameters = new MessageParameterCollection();
            
            if (ToAddresses.Count > 0)
                parameters.Add(Parameter_ToAddresses, string.Join(",", ToAddresses));

            if (CCAddresses.Count > 0)
                parameters.Add(Parameter_CCAddresses, string.Join(",", CCAddresses));

            if (BCCAddresses.Count > 0)
                parameters.Add(Parameter_BCCAddresses, string.Join(",", BCCAddresses));

            if (!string.IsNullOrEmpty(FromAddress))
                parameters.Add(Parameter_FromAddress, FromAddress);

            if (!string.IsNullOrEmpty(Subject))
                parameters.Add(Parameter_Subject, Subject);

            if (!string.IsNullOrEmpty(Body))
                parameters.Add(Parameter_Body, Body);

            parameters.Add(Parameter_IsHtml, IsHtml.ToString());

            return parameters;
        }
    }
}
