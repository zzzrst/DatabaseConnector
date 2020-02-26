// <copyright file="OracleDatabase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseConnector
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// Class to represent an Oracle database.
    /// </summary>
    public class OracleDatabase : DatabaseObject
    {
        /// <summary>
        /// Represents an open connection to the Oracle database.
        /// </summary>
        private OracleConnection myConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class.
        /// </summary>
        public OracleDatabase()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used in this library.</param>
        public OracleDatabase(ILogger logger)
            : this(string.Empty, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connnect to database.</param>
        public OracleDatabase(string connectionString)
            : this(connectionString, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connnect to database.</param>
        /// <param name="logger">The logger to be used in this library.</param>
        public OracleDatabase(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="userID">Username for login.</param>
        /// <param name="password">Password for login.</param>
        public OracleDatabase(string host, string port, string serviceName, string userID, string password)
        {
            this.ConnectionString = CreateConnectionString(host, port, serviceName, userID, password);
            this.Connected = false;
        }

        /// <summary>
        /// Creates and returns the command line for executing SQL scripts.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="userID">Username for login.</param>
        /// <param name="password">Password for login.</param>
        /// <param name="filePath">File path of SQL script.</param>
        /// <returns>The command line for executing SQL scripts.</returns>
        public static string CreateCommandHelper(string host, string port, string serviceName, string userID, string password, string filePath)
        {
            return $"{userID}/{password}@{host}:{port}/{serviceName} @\"{filePath}\"";
        }

        /// <summary>
        /// Creates and returns the connection string.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="userID">Username for login.</param>
        /// <param name="password">Password for login.</param>
        /// <returns>The connection string.</returns>
        public static string CreateConnectionString(string host, string port, string serviceName, string userID, string password)
        {
            // connection string with provder of OraOLEDB that comes when user installs Oracle Client
            return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={serviceName})));User ID={userID};Password={password};";
        }

        /// <inheritdoc/>
        public override bool Connect()
        {
            try
            {
                this.myConnection = new OracleConnection(this.ConnectionString);
                this.myConnection.Open();
                this.Connected = this.myConnection.State == ConnectionState.Open;
            }
            catch (Exception e)
            {
                this.Logger?.LogError(e.ToString());
            }

            return this.Connected;
        }

        /// <inheritdoc/>
        public override bool IsConnected()
        {
            return this.Connected;
        }

        /// <inheritdoc/>
        public override bool Disconnect()
        {
            try
            {
                this.myConnection.Close();
            }
            catch
            {
            }

            this.Connected = !(this.myConnection.State == ConnectionState.Closed);
            return !this.Connected;
        }

        /// <inheritdoc/>
        public override List<List<object>> ExecuteQuery(string query)
        {
            List<List<object>> resultTable = new List<List<object>>();

            OracleCommand command = new OracleCommand(query, this.myConnection);
            if (this.Connected)
            {
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int field = 0; field < reader.FieldCount; field++)
                        {
                            row.Add(reader.GetValue(field));
                        }

                        resultTable.Add(row);
                    }
                }
                catch (Exception e)
                {
                    throw new ErrorQueryingData($"{ErrorQueryingData.ErrorMsg} {e.Message}");
                }
            }

            return resultTable;
        }

        /// <inheritdoc/>
        public override bool ExecuteNonQuery(string nonQuery)
        {
            bool result = true;

            if (nonQuery.EndsWith(".sql\""))
            {
                return this.ExecuteSQLFromFile(nonQuery);
            }
            else
            {
                OracleCommand command = new OracleCommand(nonQuery, this.myConnection);
                try
                {
                    int row = command.ExecuteNonQuery();
                    if (row != -1)
                    {
                        this.Logger?.LogInformation(row + " rows affected by the command");
                    }
                    else
                    {
                        this.Logger?.LogInformation("No return result");
                    }
                }
                catch (Exception e)
                {
                    this.Logger?.LogError(e.ToString());
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query from the given SQL file.
        /// </summary>
        /// <param name="filePath">File path of SQL script.</param>
        /// <returns><code>true</code> if query was successfully executed.</returns>
        private bool ExecuteSQLFromFile(string filePath)
        {
            bool successfulRun = true;
            string cmdRun = OracleDatabaseConstants.SQLPLusCommand + filePath;

            // Console.WriteLine(cmdRun);
            try
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "cmd.exe",
                    Arguments = cmdRun,
                };
                p.StartInfo = startInfo;
                p.Start();
                LogHelper.LogStdout();
                string line;
                int lineCount = 0;
                while ((line = p.StandardOutput.ReadLine()) != null)
                {
                    if (line.StartsWith(OracleDatabaseConstants.DisconnectedSQL))
                    {
                        break;
                    }

                    lineCount++;
                    if (!this.SkipSQLPlusCommandLineInfo(lineCount, line))
                    {
                        if (line.StartsWith(OracleDatabaseConstants.SQLERROR))
                        {
                            successfulRun = false;
                        }

                        LogHelper.LogWithFiveTabs(line);
                    }
                }

                p.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                successfulRun = false;
            }

            return successfulRun;
        }

        /// <summary>
        /// Returns whether or not the SQL*Plus command-line info has been skipped.
        /// </summary>
        /// <param name="lineCount">Number of lines skipped.</param>
        /// <param name="line">Line to check for skip.</param>
        /// <returns><code>true</code> if the SQL*Plus command-line info has been skipped.</returns>
        private bool SkipSQLPlusCommandLineInfo(int lineCount, string line)
        {
            return lineCount <= 4 ||
                   line.Equals(string.Empty) ||
                   line.StartsWith(OracleDatabaseConstants.SQLEnterUsername) ||
                   line.StartsWith(OracleDatabaseConstants.SQLLastConnectionInfo) ||
                   line.StartsWith(OracleDatabaseConstants.SQLConnectedTo) ||
                   line.StartsWith(OracleDatabaseConstants.SQLOracle12) ||
                   line.StartsWith(OracleDatabaseConstants.SQLOracleInfo);
        }
    }
}
