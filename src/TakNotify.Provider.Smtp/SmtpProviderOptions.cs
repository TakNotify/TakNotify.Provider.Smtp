// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
namespace TakNotify
{
    /// <summary>
    /// The SMTP provider options
    /// </summary>
    public class SmtpProviderOptions : NotificationProviderOptions
    {
        internal static string Parameter_Server = $"{SmtpProviderConstants.DefaultName}_{nameof(Server)}";
        internal static string Parameter_Port = $"{SmtpProviderConstants.DefaultName}_{nameof(Port)}";
        internal static string Parameter_Username = $"{SmtpProviderConstants.DefaultName}_{nameof(Username)}";
        internal static string Parameter_Password = $"{SmtpProviderConstants.DefaultName}_{nameof(Password)}";
        internal static string Parameter_UseSSL = $"{SmtpProviderConstants.DefaultName}_{nameof(UseSSL)}";
        internal static string Parameter_DefaultFromAddress = $"{SmtpProviderConstants.DefaultName}_{nameof(DefaultFromAddress)}";

        /// <summary>
        /// Instantiate the <see cref="SmtpProviderOptions"/>
        /// </summary>
        public SmtpProviderOptions()
        {
            Parameters.Add(Parameter_Server, "");
            Parameters.Add(Parameter_Port, 0);
            Parameters.Add(Parameter_Username, "");
            Parameters.Add(Parameter_Password, "");
            Parameters.Add(Parameter_UseSSL, false);
            Parameters.Add(Parameter_DefaultFromAddress, "");
        }

        /// <summary>
        /// The SMTP server
        /// </summary>
        public string Server
        {
            get => Parameters[Parameter_Server].ToString();
            set => Parameters[Parameter_Server] = value;
        }

        /// <summary>
        /// The SMTP port
        /// </summary>
        public int Port
        {
            get => int.Parse(Parameters[Parameter_Port].ToString());
            set => Parameters[Parameter_Port] = value;
        }

        /// <summary>
        /// The SMTP username
        /// </summary>
        public string Username
        {
            get => Parameters[Parameter_Username].ToString();
            set => Parameters[Parameter_Username] = value;
        }

        /// <summary>
        /// The SMTP password
        /// </summary>
        public string Password
        {
            get => Parameters[Parameter_Password].ToString();
            set => Parameters[Parameter_Password] = value;
        }

        /// <summary>
        /// The SMTP SSL status
        /// </summary>
        public bool UseSSL
        {
            get => bool.Parse(Parameters[Parameter_UseSSL].ToString());
            set => Parameters[Parameter_UseSSL] = value;
        }

        /// <summary>
        /// The default "From Address" that will be used if the <see cref="EmailMessage.FromAddress"/> is empty
        /// </summary>
        public string DefaultFromAddress
        {
            get => Parameters[Parameter_DefaultFromAddress].ToString();
            set => Parameters[Parameter_DefaultFromAddress] = value;
        }
    }
}
