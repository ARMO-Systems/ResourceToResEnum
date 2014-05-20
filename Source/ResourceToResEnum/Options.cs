using System;
using CommandLine;

namespace ArmoSystems.ArmoGet.ResourceToResEnum
{
    internal sealed class Options
    {
        public Options()
        {
            new Parser().ParseArguments( Environment.GetCommandLineArgs(), this );
        }
#pragma warning disable 0649
        [Option( 'n', "LocalizableStringsNewDll", HelpText = "LocalizableStrings dll new version." )]
        public string LocalizableStringsNewDll { get; set; }

        [Option( 'o', "LocalizableStringsOldDll", HelpText = "LocalizableStrings dll old version." )]
        public string LocalizableStringsOldDll { get; set; }

        [Option( 'c', "ResManagerID", HelpText = "ResManagerID.cs path file name ." )]
        public string ResManagerIDcs { get; set; }

        [Option( 't', "toolset", HelpText = "toolset ResManagerIDs.cs generated." )]
        public bool ToolSet { get; set; }
#pragma warning restore 0649
    }
}