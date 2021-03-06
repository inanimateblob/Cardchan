﻿
using System;
using System.Threading.Tasks;
using NerdBot.Parsers;
using NerdBotCommon.Http;
using NerdBotCommon.Messengers;
using NerdBotCommon.Mtg;
using NerdBotCommon.Mtg.Prices;
using NerdBotCommon.UrlShortners;
using SimpleLogging.Core;

namespace NerdBot.Plugin
{
    public abstract class MessagePluginBase : IMessagePlugin
    {
        protected string mBotName;
        protected IMtgStore mStore;
        protected ICardPriceStore mPriceStore;
        protected ICommandParser mCommandParser;
        protected IHttpClient mHttpClient;
        protected IUrlShortener mUrlShortener;
        protected ILoggingService mLoggingService;

        public MessagePluginBase(
            IMtgStore store,
            ICardPriceStore priceStore,
            ICommandParser commandParser,
            IHttpClient httpClient,
            IUrlShortener urlShortener)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            if (priceStore == null)
                throw new ArgumentNullException("priceStore");

            if (commandParser == null)
                throw new ArgumentNullException("commandParser");

            if (httpClient == null)
                throw new ArgumentNullException("httpClient");

            if (urlShortener == null)
                throw new ArgumentNullException("urlShortener");

            this.mStore = store;
            this.mPriceStore = priceStore;
            this.mCommandParser = commandParser;
            this.mHttpClient = httpClient;
            this.mUrlShortener = urlShortener;
        }

        #region Properties
        public string BotName
        {
            set
            {
                this.mBotName = value;
            }

            get { return this.mBotName; }
        }

        public IMtgStore Store
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mStore = value;
            }
            get { return this.mStore; }
        }

        public ICardPriceStore PriceStore
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mPriceStore = value;
            }
            get { return this.mPriceStore; }
        }

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

        public IHttpClient HttpClient
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.mHttpClient = value;
            }

            get { return this.mHttpClient; }
        }

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

        public ILoggingService LoggingService
        {
            get { return this.mLoggingService; }
            set { this.mLoggingService = value; }
        }

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string ShortDescription { get; }
        #endregion

        public abstract void OnLoad();
        public abstract void OnUnload();
        public abstract Task<bool> OnMessage(IMessage message, IMessenger messenger);
    }
}
