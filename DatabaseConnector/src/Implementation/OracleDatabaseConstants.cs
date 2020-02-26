// <copyright file="OracleDatabaseConstants.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseConnector
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class to represent the constant for OracleDatabase.
    /// </summary>
    internal static class OracleDatabaseConstants
    {
        /// <summary>
        /// SQL*Plus command prefix.
        /// </summary>
        internal const string SQLPLusCommand = "/C exit | sqlplus ";

        /// <summary>
        /// SQL message string for disconnecting.
        /// </summary>
        internal const string DisconnectedSQL = "SQL> Disconnected";

        /// <summary>
        /// Oracle message prompt to enter user name.
        /// </summary>
        internal const string SQLEnterUsername = "Enter user-name:";

        /// <summary>
        /// Oracle message string for connection.
        /// </summary>
        internal const string SQLConnectedTo = "Connected to:";

        /// <summary>
        /// Oracle message string for database version.
        /// </summary>
        internal const string SQLOracle12 = "Oracle Database 12c Enterprise Edition Release";

        /// <summary>
        /// Oracle message string for last connection info.
        /// </summary>
        internal const string SQLLastConnectionInfo = "Last Successful login time: ";

        /// <summary>
        /// Oracle message string for additional info.
        /// </summary>
        internal const string SQLOracleInfo = "With the Partitioning, OLAP, Advanced Analytics and Real Application Testing options";

        /// <summary>
        /// Oracle message string for error.
        /// </summary>
        internal const string SQLERROR = "ERROR";
    }
}
