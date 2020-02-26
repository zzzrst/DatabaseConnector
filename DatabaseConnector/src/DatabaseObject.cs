// <copyright file="DatabaseObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseConnector
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class to represent a database through which the user can connect to and query from.
    /// </summary>
    public abstract class DatabaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        public DatabaseObject()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used in this library.</param>
        public DatabaseObject(ILogger logger)
            : this(string.Empty, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to be used to connect to the database.</param>
        public DatabaseObject(string connectionString)
            : this(connectionString, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to be used to connect to the database.</param>
        /// <param name="logger">The logger to be used in this library.</param>
        public DatabaseObject(string connectionString, ILogger logger)
        {
            this.ConnectionString = connectionString;
            this.Logger = logger;
            this.Connected = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the state of the database.
        /// </summary>
        protected bool Connected { get; set; }

        /// <summary>
        /// Gets or sets the connection string to connect to the database.
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the logger to be used by the database.
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// Connects to the database.
        /// </summary>
        /// <returns><code>true</code> if the database has been successfully connected to.</returns>
        public abstract bool Connect();

        /// <summary>
        /// Returns whether or not a connection has been established to the database.
        /// <returns><code>true</code> if the database has been connected to</returns>
        /// </summary>
        /// <returns><code>true</code> if the database has been connected to.</returns>
        public abstract bool IsConnected();

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        /// <returns><code>true</code> if the database has been successfully disconnected from.</returns>
        public abstract bool Disconnect();

        /// <summary>
        /// Executes the given query that retrieves data from the database.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Data queried from the database.</returns>
        public abstract List<List<object>> ExecuteQuery(string query);

        /// <summary>
        /// Executes the given query that does not return any data (e.g. update, insert, delete).
        /// </summary>
        /// <param name="nonQuery">Query to execute.</param>
        /// <returns><code>true</code> if query was successfully executed.</returns>
        public abstract bool ExecuteNonQuery(string nonQuery);
    }
}
