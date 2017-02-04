/*
Copyright (C) 2016  Prism Framework Team

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


#define DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.IO;

namespace Prism.Utilities
{
    /// <summary>
    /// Provides a universally-compatible implementation for <see cref="ILogger"/>.
    /// </summary>
    internal class CommonLogger : ILogger
    {
        public string LogFilePath { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public int MinimumConsoleOutputLevel
        {
            get { return minimumConsoleOutputLevel; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MinimumConsoleOutputLevel), Resources.Strings.ValueCannotBeLessThanZero);
                }

                minimumConsoleOutputLevel = value;
            }
        }
        private int minimumConsoleOutputLevel;

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public int MinimumFileOutputLevel
        {
            get { return minimumFileOutputLevel; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MinimumFileOutputLevel), Resources.Strings.ValueCannotBeLessThanZero);
                }

                minimumFileOutputLevel = value;
            }
        }
        private int minimumFileOutputLevel;
        
        private Queue<string> messageQueue;

        public CommonLogger()
        {
            messageQueue = new Queue<string>();
            MinimumConsoleOutputLevel = (int)LoggingLevel.Info;
            MinimumFileOutputLevel = (int)LoggingLevel.Trace;
        }

        public void Log(Enum loggingLevel, IFormatProvider provider, string message, params object[] args)
        {
            if (loggingLevel == null)
            {
                return; 
            }

            int level = Convert.ToInt32(loggingLevel, CultureInfo.InvariantCulture);
            if (level > 0xFF)
            {
                // this enforces the rule that anything over 0xFF always means Off
                return;
            }

            message = string.Format(provider, "[{0}] ({1}): {2}", DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss", provider),
                Enum.GetName(loggingLevel.GetType(), loggingLevel), string.Format(provider, message ?? string.Empty, args));

            if (MinimumConsoleOutputLevel <= level)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }

            if (MinimumFileOutputLevel <= level && LogFilePath != null)
            {
                message += Environment.NewLine;

                bool output = false;
                lock (messageQueue)
                {
                    messageQueue.Enqueue(message);
                    output = messageQueue.Count == 1;
                }

                if (output)
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        while (true)
                        {
                            lock (messageQueue)
                            {
                                File.AppendAllTextAsync(LogFilePath, messageQueue.Dequeue()).GetAwaiter().GetResult();
                                if (messageQueue.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                    });
                }
            }
        }
    }
}
