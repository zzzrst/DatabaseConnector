// <copyright file="ErrorQueryingData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseConnector
{
    using System;

    /// <summary>
    /// Defines the <see cref="ErrorQueryingData" />.
    /// </summary>
    public class ErrorQueryingData : Exception
    {
        /// <summary>
        /// Defines the ErrorMsg.
        /// </summary>
        public const string ErrorMsg = "Error executing command:";

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorQueryingData"/> class.
        /// </summary>
        public ErrorQueryingData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorQueryingData"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public ErrorQueryingData(string message)
            : base(message)
        {
        }
    }
}
