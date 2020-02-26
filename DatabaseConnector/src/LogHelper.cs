// <copyright file="LogHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseConnector
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="LogHelper" />.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Gets or sets the logger to be used.
        /// </summary>
        public static ILogger Logger { get; set; }

        /// <summary>
        /// Logs a line with 4 tabs and Stdout:.
        /// </summary>
        public static void LogStdout()
        {
            Logger?.LogInformation($"{Tab(4)}Stdout:");
        }

        /// <summary>
        /// Logs the provided 'log' with 5 tabs in front.
        /// </summary>
        /// <param name="log">The log<see cref="string"/>.</param>
        public static void LogWithFiveTabs(string log)
        {
            Logger?.LogInformation($"{Tab(5)}{log}");
        }

        /// <summary>
        /// Returns a sequence of whitespaces of fixed length to represent tabs in the print log.
        /// </summary>
        /// <param name="indents">Number of tabs.</param>
        /// <returns>Sequence of tabs represented as whitespaces.</returns>
        private static string Tab(int indents = 1)
        {
            return string.Concat(Enumerable.Repeat("    ", indents));
        }
    }
}
