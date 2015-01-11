﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NerdBot.Http;
using NerdBot.Messengers;
using NerdBot.Mtg;
using NerdBot.Parsers;
using NerdBot.UrlShortners;
using Ninject;
using Ninject.Extensions.Logging;

namespace NerdBot.Plugin
{
    public abstract class PluginBase : IPlugin
    {
        protected IMtgStore mStore;
        protected ICommandParser mCommandParser;
        protected ILogger mLogger;
        protected IHttpClient mHttpClient;
        protected IUrlShortener mUrlShortener;

        public PluginBase()
        {
        }

        #region Properties
        [Inject]
        public ILogger Logger
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mLogger = value;
            }
            get { return this.mLogger; }
        }

        [Inject]
        public IMtgStore Store
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mStore = value;
            }
            get { return this.mStore;  }
        }

        [Inject]
        public ICommandParser CommandParser
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mCommandParser = value;
            }
            get { return this.mCommandParser; }
        }

        [Inject]
        public IHttpClient HttpClient
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mHttpClient = value;
            }

            get { return this.mHttpClient;  }
        }

        [Inject]
        public IUrlShortener UrlShortener
        {
            set
            {
                if (value == null)
                    throw new ArgumentException("value");

                this.mUrlShortener = value;
            }

            get { return this.mUrlShortener; }
        }

        public abstract string Name { get; }
        public abstract string Description { get; }
        #endregion

        public abstract void OnLoad();
        public abstract void OnUnload();
        public abstract Task<bool> OnMessage(IMessage message, IMessenger messenger);
    }
}