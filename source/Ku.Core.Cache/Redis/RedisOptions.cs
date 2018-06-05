﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ku.Core.Cache.Redis
{
    internal class RedisOptions
    {
        /// <summary>
        /// Gets or sets the password to be used to connect to the Redis server.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether to use SSL encryption.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is SSL; otherwise, <c>false</c>.
        /// </value>
        public bool IsSsl { get; set; } = false;

        /// <summary>
        /// Gets or sets the SSL Host.
        /// If set, it will enforce this particular host on the server's certificate.
        /// </summary>
        /// <value>
        /// The SSL host.
        /// </value>
        public string SslHost { get; set; } = null;

        public bool AllowAdmin { get; set; } = false;

        /// <summary>
        /// Gets or sets the timeout for any connect operations.
        /// </summary>
        /// <value>
        /// The connection timeout.
        /// </value>
        public int ConnectionTimeout { get; set; } = 5000;

        /// <summary>
        /// Gets the list of endpoints to be used to connect to the Redis server.
        /// </summary>
        /// <value>
        /// The endpoints.
        /// </value>
        public IList<ServerEndPoint> Endpoints { get; } = new List<ServerEndPoint>();

        /// <summary>
        /// Gets or sets the Redis database index the cache will use.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public int Database { get; set; } = 0;
    }

    internal class ServerEndPoint
    {
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; } = 6379;

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }
    }
}
