﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NerdBot;
using NerdBot.Parsers;
using NerdBot.Plugin;
using NerdBotCommon.Http;
using NerdBotCommon.Messengers;
using NerdBotCommon.Mtg;
using NerdBotCommon.Mtg.Prices;
using NerdBotCommon.Parsers;
using NerdBotCommon.UrlShortners;
using NerdBotTappedOut.Fetchers;

namespace NerdBotTappedOut
{
    public class TappedOutComboDecksPlugin : PluginBase
    {
        private const int cLimit = 8;

        public override string Name
        {
            get { return "combodecks command"; }
        }

        public override string Description
        {
            get { return string.Format("Returns the first {0} combo decks on tappedout.net", cLimit); }
        }

        public override string ShortDescription
        {
            get { return string.Format("Returns the first {0} combo decks on tappedout.net", cLimit); }
        }

        public override string Command
        {
            get { return "combodecks"; }
        }

        public override string HelpCommand
        {
            get { return "help combodecks"; }
        }

        public override string HelpDescription
        {
            get { return string.Format("{0}: 'combodecks' or 'combodecks esper'", this.Command); }
        }

        public TappedOutComboDecksPlugin(
                IMtgStore store,
                ICardPriceStore priceStore,
                ICommandParser commandParser,
                IHttpClient httpClient,
                IUrlShortener urlShortener,
                BotConfig config
            )
            : base(
                store,
                priceStore,
                commandParser,
                httpClient,
                urlShortener,
                config)
        {
        }

        public override void OnLoad()
        {
        }

        public override void OnUnload()
        {
        }

        public override async Task<bool> OnMessage(IMessage message, IMessenger messenger)
        {
            return false;
        }

        public override async Task<bool> OnCommand(Command command, IMessage message, IMessenger messenger)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (message == null)
                throw new ArgumentNullException("message");

            if (messenger == null)
                throw new ArgumentNullException("messenger");

            var latestFetcher = new TappedOutLatestFetcher("http://tappedout.net/api/deck/latest/combo/", this.HttpClient);
            List<TappedOutLatestDeckData> latestData = await latestFetcher.GetLatest();

            if (latestData != null)
            {
                if (latestData.Any())
                {
                    int total = latestData.Count;

                    if (command.Arguments.Length == 1)
                    {
                        string filter = command.Arguments[0].ToLower();

                        latestData = latestData.Where(l => l.Name.ToLower().Contains(filter)).ToList();
                    }

                    string[] latestDecks = latestData.Select(s =>
                        string.Format("{0} [{1}]", s.Name, this.mUrlShortener.ShortenUrl(s.Url)))
                        .Take(cLimit)
                        .ToArray();

                    string msg = string.Format("Combo decks: {0} [{1}/{2}]",
                        string.Join(", ", latestDecks),
                        latestDecks.Count(),
                        total);

                    messenger.SendMessage(msg);

                    return true;
                }
            }
                    
            return false;
        }
    }
}
