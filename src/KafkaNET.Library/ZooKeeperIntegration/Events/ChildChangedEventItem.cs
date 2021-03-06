﻿/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Kafka.Client.ZooKeeperIntegration.Events
{
    using log4net;
    using System.Linq;
    using Utils;

    /// <summary>
    /// Represents methods that will handle a ZooKeeper child events  
    /// </summary>
    internal class ChildChangedEventItem
    {
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ChildChangedEventItem));

        private ZooKeeperClient.ZooKeeperEventHandler<ZooKeeperChildChangedEventArgs> childChanged;

        /// <summary>
        /// Occurs when znode children changes
        /// </summary>
        public event ZooKeeperClient.ZooKeeperEventHandler<ZooKeeperChildChangedEventArgs> ChildChanged
        {
            add
            {
                this.childChanged -= value;
                this.childChanged += value;
            }
            remove
            {
                this.childChanged -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildChangedEventItem"/> class. 
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <remarks>
        /// Should use external logger to keep same format of all event logs
        /// </remarks>
        public ChildChangedEventItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildChangedEventItem"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="handler">
        /// The subscribed handler.
        /// </param>
        /// <remarks>
        /// Should use external logger to keep same format of all event logs
        /// </remarks>
        public ChildChangedEventItem(ZooKeeperClient.ZooKeeperEventHandler<ZooKeeperChildChangedEventArgs> handler)
        {
            this.ChildChanged += handler;
        }

        /// <summary>
        /// Invokes subscribed handlers for ZooKeeeper children changes event
        /// </summary>
        /// <param name="e">
        /// The event data.
        /// </param>
        public void OnChildChanged(ZooKeeperChildChangedEventArgs e)
        {
            var handlers = this.childChanged;
            if (handlers == null)
            {
                return;
            }

            foreach (var handler in handlers.GetInvocationList())
            {
                Logger.Debug(e + " sent to " + handler.Target);
            }

            handlers(e);
        }

        /// <summary>
        /// Gets the total count of subscribed handlers
        /// </summary>
        public int Count
        {
            get
            {
                return this.childChanged != null ? this.childChanged.GetInvocationList().Count() : 0;
            }
        }
    }
}
