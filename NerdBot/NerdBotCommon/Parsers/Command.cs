﻿namespace NerdBotCommon.Parsers
{
    public class Command
    {
        public string Cmd { get; set; }
        public string[] Arguments { get; set; }

        public Command()
        {
            this.Arguments = new string[] {};
        }
    }
}
