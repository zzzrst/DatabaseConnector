namespace DatabaseConnector
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Text;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of the SQL Database.
    /// </summary>
    public class SQLDatabase : DatabaseObject
    {
        private SqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        public SQLDatabase()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used in this library.</param>
        public SQLDatabase(ILogger logger)
            : this(string.Empty, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connnect to database.</param>
        public SQLDatabase(string connectionString)
            : this(connectionString, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connnect to database.</param>
        /// <param name="logger">The logger to be used in this library.</param>
        public SQLDatabase(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="userID">Username for login.</param>
        /// <param name="password">Password for login.</param>
        public SQLDatabase(string host, string port, string serviceName, string userID, string password)
            : this(host, port, serviceName, userID, password, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDatabase"/> class.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="userID">Username for login.</param>
        /// <param name="password">Password for login.</param>
        /// <param name="logger">The logger to be used in this library.</param>
        public SQLDatabase(string host, string port, string serviceName, string userID, string password, ILogger logger)
        {
            this.ConnectionString = CreateConnectionString(host, port, serviceName, userID, password);
            this.Connected = false;
            this.Logger = logger;
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
            return $"Data Source={host};Initial Catalog={serviceName};User ID={userID};Password={password}";
        }

        /// <inheritdoc/>
        public override bool Connect()
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.ConnectionString);
                this.sqlConnection.Open();
                this.Connected = this.sqlConnection.State == ConnectionState.Open;
            }
            catch (Exception e)
            {
                this.Logger?.LogError(e.ToString());
            }

            return this.Connected;
        }

        /// <inheritdoc/>
        public override bool Disconnect()
        {
            try
            {
                this.sqlConnection.Close();
            }
            catch
            {
            }

            this.Connected = !(this.sqlConnection.State == ConnectionState.Closed);
            return !this.Connected;
        }

        /// <inheritdoc/>
        public override bool ExecuteNonQuery(string nonQuery)
        {
            bool result = true;

            SqlCommand command = new SqlCommand(nonQuery, this.sqlConnection);
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

            return result;
        }

        /// <inheritdoc/>
        public override List<List<object>> ExecuteQuery(string query)
        {
            List<List<object>> resultTable = new List<List<object>>();

            SqlCommand command = new SqlCommand(query, this.sqlConnection);
            if (this.Connected)
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
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
        public override bool IsConnected()
        {
            return this.Connected;
        }
    }
}
