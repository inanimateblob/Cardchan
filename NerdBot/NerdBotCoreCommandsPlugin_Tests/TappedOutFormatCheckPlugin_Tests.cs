﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NerdBot.Http;
using NerdBot.Messengers;
using NerdBot.Messengers.GroupMe;
using NerdBot.Mtg;
using NerdBot.Mtg.Prices;
using NerdBot.Parsers;
using NerdBot.UrlShortners;
using NerdBotCoreCommands;
using NUnit.Framework;
using SimpleLogging.Core;

namespace NerdBotCoreCommandsPlugin_Tests
{
    [TestFixture]
    class TappedOutFormatCheckPlugin_Tests
    {
        private const string connectionString = "mongodb://localhost";
        private const string databaseName = "mtgdb";

        private IMtgStore mtgStore;
        private TappedOutFormatCheckPlugin plugin;

        private Mock<ICommandParser> commandParserMock;
        private Mock<IHttpClient> httpClientMock;
        private Mock<IUrlShortener> urlShortenerMock;
        private Mock<IMessenger> messengerMock;
        private Mock<ILoggingService> loggingServiceMock;
        private Mock<ICardPriceStore> priceStoreMock;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            loggingServiceMock = new Mock<ILoggingService>();

            mtgStore = new MtgStore(connectionString, databaseName, loggingServiceMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            // Setup ICardPriceStore Mocks
            priceStoreMock = new Mock<ICardPriceStore>();

            // Setup ICommandParser Mocks
            commandParserMock = new Mock<ICommandParser>();

            // Setup IHttpClient Mocks
            httpClientMock = new Mock<IHttpClient>();

            // Setup IUrlShortener Mocks
            urlShortenerMock = new Mock<IUrlShortener>();

            // Setup IMessenger Mocks
            messengerMock = new Mock<IMessenger>();

            plugin = new TappedOutFormatCheckPlugin(
                mtgStore,
                priceStoreMock.Object,
                commandParserMock.Object,
                httpClientMock.Object,
                urlShortenerMock.Object);
        }

        [Test]
        public void GetFormats()
        {
            var cmd = new Command()
            {
                Cmd = "formats",
                Arguments = new string[]
                {
                    "spore clou%",
                }
            };

            var msg = new GroupMeMessage();

            bool handled =
            plugin.OnCommand(
                cmd,
                msg,
                messengerMock.Object
                ).Result;

            messengerMock.Verify(m => m.SendMessage(It.Is<string>(c => c.StartsWith("Spore Cloud is legal in formats: "))), Times.Once);
        }
    }
}
