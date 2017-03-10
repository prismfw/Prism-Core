/*
Copyright (C) 2017  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System;
using System.Diagnostics;
using System.Globalization;

namespace Prism.Utilities
{
    /// <summary>
    /// Defines an object that can write messages to a file stream or console output.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the path to the file in which log messages should be written.
        /// </summary>
        string LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the minimum level a log message must be for it to be written to a debug console.
        /// By default, this is set to <see cref="LoggingLevel.Info"/>.
        /// </summary>
        int MinimumConsoleOutputLevel { get; set; }

        /// <summary>
        /// Gets or sets the minimum level a log message must be for it to be written to a file.
        /// By default, this is set to <see cref="LoggingLevel.Trace"/>.
        /// </summary>
        int MinimumFileOutputLevel { get; set; }

        /// <summary>
        /// Writes the specified message to the console or log file using the specified logging level.
        /// </summary>
        /// <param name="loggingLevel">An <see cref="Enum"/> describing the logging level to use.</param>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        void Log(Enum loggingLevel, IFormatProvider provider, string message, params object[] args);
    }

    /// <summary>
    /// Represents a tool for writing messages to a file stream or console output.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Gets or sets the path to the file in which log messages should be written.
        /// </summary>
        public static string LogFilePath
        {
            get { return Current.LogFilePath; }
            set { Current.LogFilePath = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static ILogger Current
        {
            get
            {
                var current = TypeManager.Default.Resolve<ILogger>();
                if (current == null)
                {
                    TypeManager.Default.RegisterSingleton(typeof(ILogger), (current = new CommonLogger()));
                }

                return current;
            }
        }

        /// <summary>
        /// Writes the specified debug message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        [Conditional("DEBUG")]
        public static void Debug(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Debug, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified debug message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        [Conditional("DEBUG")]
        public static void Debug(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Debug, provider, message, args);
        }

        /// <summary>
        /// Writes the specified error message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Error(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Error, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified error message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Error(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Error, provider, message, args);
        }

        /// <summary>
        /// Writes the specified fatal error message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Fatal(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Fatal, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified fatal message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Fatal(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Fatal, provider, message, args);
        }

        /// <summary>
        /// Writes the specified info message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Info(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Info, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified info message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Info(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Info, provider, message, args);
        }

        /// <summary>
        /// Writes the specified message to the console or log file using the specified logging level and current culture.
        /// </summary>
        /// <param name="loggingLevel">An <see cref="Enum"/> describing the logging level to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Log(Enum loggingLevel, string message, params object[] args)
        {
            Current.Log(loggingLevel, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified message to the console or log file using the specified logging level.
        /// </summary>
        /// <param name="loggingLevel">An <see cref="Enum"/> describing the logging level to use.</param>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Log(Enum loggingLevel, IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(loggingLevel, provider, message, args);
        }

        /// <summary>
        /// Sets the minimum level a log message must be for it to be written to a debug console.
        /// By default, this is set to <see cref="LoggingLevel.Info"/>.
        /// </summary>
        /// <param name="level">The minimum logging level for console ouput.</param>
        public static void SetMinimumConsoleOutputLevel(LoggingLevel level)
        {
            Current.MinimumConsoleOutputLevel = (int)level;
        }

        /// <summary>
        /// Sets the minimum level a log message must be for it to be written to a debug console.
        /// By default, this is set to <see cref="LoggingLevel.Info"/>.
        /// </summary>
        /// <param name="level">The minimum logging level for console ouput.  This can be any 32-bit enumeration.</param>
        public static void SetMinimumConsoleOutputLevel(Enum level)
        {
            Current.MinimumConsoleOutputLevel = Convert.ToInt32(level, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Sets the minimum level a log message must be for it to be written to a file.
        /// By default, this is set to <see cref="LoggingLevel.Trace"/>.
        /// </summary>
        /// <param name="level">The minimum logging level for file ouput.</param>
        public static void SetMinimumFileOutputLevel(LoggingLevel level)
        {
            Current.MinimumFileOutputLevel = (int)level;
        }

        /// <summary>
        /// Sets the minimum level a log message must be for it to be written to a file.
        /// By default, this is set to <see cref="LoggingLevel.Trace"/>.
        /// <param name="level">The minimum logging level for file ouput.  This can be any 32-bit enumeration.</param>
        /// </summary>
        public static void SetMinimumFileOutputLevel(Enum level)
        {
            Current.MinimumFileOutputLevel = Convert.ToInt32(level, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes the specified trace message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        [Conditional("TRACE")]
        public static void Trace(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Trace, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified trace message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        [Conditional("TRACE")]
        public static void Trace(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Trace, provider, message, args);
        }

        /// <summary>
        /// Writes the specified warning message to the console or log file using the current culture.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Warn(string message, params object[] args)
        {
            Current.Log(LoggingLevel.Warn, CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Writes the specified warning message to the console or log file.
        /// </summary>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Any string formatting arguments for the message.</param>
        public static void Warn(IFormatProvider provider, string message, params object[] args)
        {
            Current.Log(LoggingLevel.Warn, provider, message, args);
        }
    }
}
