using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace ArmoSystems.ArmoGet.ResourceToResEnum
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static int Main()
        {
            var commandLine = new Options();
            var oldConsoleEncoding = Console.OutputEncoding;
            Console.OutputEncoding = Encoding.GetEncoding( 866 );
            try
            {
                using ( var writer = new StreamWriter( commandLine.ResManagerIDcs ) )
                {
                    writer.WriteLine( "// ReSharper disable ClassNeverInstantiated.Global" );
                    writer.WriteLine( commandLine.ToolSet
                        ? @"namespace DatabaseToolset { internal class ResManager : ArmoSystems.Timex.Shared.CommonClasses {"
                        : @"namespace ArmoSystems.Timex.Shared.CommonClasses { public partial class ResManager{" );
                    writer.WriteLine( "// ReSharper restore ClassNeverInstantiated.Global" );
                    // ReSharper disable AccessToDisposedClosure
                    ReadResources( commandLine.LocalizableStringsNewDll ).Except( ReadResources( commandLine.LocalizableStringsOldDll ) ).OrderBy( s => s ).ForEach( s => writer.WriteLine( CreateResAndResId( s ) ) );
                    // ReSharper restore AccessToDisposedClosure
                    writer.WriteLine( "} }" );
                }
// ReSharper disable once LocalizableElement
                Console.WriteLine( "Успешно выполнено!" );
            }
            catch ( FileNotFoundException ex )
            {
                Console.WriteLine( ex.Message );
                return -9;
            }
            catch ( ArgumentNullException )
            {
// ReSharper disable once LocalizableElement
                Console.WriteLine( "Задайте все необходимые аргументы командной строки" );
                return -10;
            }
            finally
            {
                Console.OutputEncoding = oldConsoleEncoding;
            }
            return 0;
        }

        private static string CreateResAndResId( string name )
        {
            return "public static string " + name + "{ get { return GetString( \"" + name + "\" ); } }";
        }

        private static IEnumerable< string > ReadResources( string assemblyLocalizableStrings )
        {
            if ( assemblyLocalizableStrings == null )
                return Enumerable.Empty< string >();
            var assembly = Assembly.LoadFrom( assemblyLocalizableStrings );
            var rm = new ResourceManager( "ArmoSystems.Timex.LocalizableStrings.Timex", assembly );
            using ( var resourceSet = rm.GetResourceSet( CultureInfo.GetCultureInfo( "ru-RU" ), true, true ) )
                return resourceSet.Cast< DictionaryEntry >().Where( de => de.Value is string ).Select( de => de.Key.ToString() ).ToList();
        }
    }
}