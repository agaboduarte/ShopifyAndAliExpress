using ISAA.Helper.ConsoleHelper;
using ISAA.Helper.PhantomJS;
using ISAA.Rules.Ali.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ISAA.Suppliers.Ali.Automation
{
    public class Program
    {
        public static CultureInfo CulturePtBr = CultureInfo.GetCultureInfo("pt-br");

        public static void Main(string[] args)
        {
            Argument.SetArgument(args);

            PhantomConfig();

            var suppressHeader = Argument.GetArgumentBool("-SuppressHeader");

            if (!suppressHeader)
            {
                Print.PrintDescription(Assembly.GetExecutingAssembly());
            }

            if (Argument.HasArgument("-H"))
            {
                PrintHelper();

                return;
            }

            if (Run(suppressHeader))
            {
                return;
            }

            Print.SetOutToConsole();

            PrintHelper();
        }

        public static void PhantomConfig()
        {
            PhantomControl.CONFIG_LOAD_IMAGES = false;

            PhantomControl.CONFIG_PROXY = new[] { "true", "0" }.Contains(ConfigurationManager.AppSettings["ProxyCredential:UseProxy"].ToLower());
            PhantomControl.CONFIG_PROXY_ADDRESS = ConfigurationManager.AppSettings["ProxyCredential:Address"];
            PhantomControl.CONFIG_PROXY_PASSWORD = ConfigurationManager.AppSettings["ProxyCredential:Password"];
            PhantomControl.CONFIG_PROXY_USERNAME = ConfigurationManager.AppSettings["ProxyCredential:UserName"];
        }

        public static bool Run(bool suppressHeader)
        {
            var maxDegreeOfParallelism = default(int?);

            if (Argument.HasArgument("-MaxDegreeOfParallelism"))
            {
                maxDegreeOfParallelism = Argument.GetArgumentInt("-MaxDegreeOfParallelism");
            }

            if (Argument.HasArgument("-MethodName"))
            {
                var methodName = Argument.GetArgumentString("-MethodName");
                var keepAlive = Argument.GetArgumentBool("-KeepAlive");
                var awaitTime = Argument.GetArgumentInt("-AwaitTime");

                if (awaitTime == null)
                {
                    awaitTime = 250;
                }

                switch (methodName)
                {
                    case "AliGetProductById":
                        {
                            ChangeOutputType();

                            if (!suppressHeader)
                            {
                                Print.PrintText("AliGetProductById", "");

                                Console.WriteLine();
                            }

                            var storeId = Argument.GetArgumentInt("-StoreId", true);
                            var productId = Argument.GetArgumentLong("-ProductId", true);

                            var model = AliGetProduct.Run(storeId.Value, productId.Value);

                            Print.PrintText("JSON Result", JsonConvert.SerializeObject(model));

                            return true;
                        }
                    case "AliGetProduct":
                        {
                            ChangeOutputType();

                            if (!suppressHeader)
                            {
                                Print.PrintText("AliGetProduct", "");

                                Console.WriteLine();
                            }

                            do
                            {
                                AliGetProduct.Run(maxDegreeOfParallelism);

                                WaitLoop(keepAlive, awaitTime.Value);
                            }
                            while (keepAlive);

                            return true;
                        }
                    case "AliUpdateProduct":
                        {
                            ChangeOutputType();

                            if (!suppressHeader)
                            {
                                Print.PrintText("AliUpdateProduct", "");

                                Console.WriteLine();
                            }

                            do
                            {
                                AliUpdateProduct.Run(maxDegreeOfParallelism);

                                WaitLoop(keepAlive, awaitTime.Value);
                            }
                            while (keepAlive);

                            return true;
                        }
                    case "AliUpdateShopifyProduct":
                        {
                            ChangeOutputType();

                            if (!suppressHeader)
                            {
                                Print.PrintText("AliUpdateShopifyProduct", "");

                                Console.WriteLine();
                            }

                            do
                            {
                                AliUpdateShopifyProduct.Run(maxDegreeOfParallelism);

                                WaitLoop(keepAlive, awaitTime.Value);
                            }
                            while (keepAlive);

                            return true;
                        }
                }
            }

            return false;
        }

        public static void PrintHelper()
        {
            if (Argument.HasArgument("-MethodName"))
            {
                var methodName = Argument.GetArgumentString("-MethodName");

                switch (methodName)
                {
                    // TODO: helper for methods

                    //case "ShopifyUpdateProduct":
                    //    Print.PrintText("Help For", methodName);

                    //    Console.WriteLine();

                    //    Print.PrintText("-KeepAlive", "Process has to keep alive, default is FALSE [ BOOL ] [ OPTIONAL ].");
                    //    Print.PrintText("-MaxDegreeOfParallelism", "Max degree of parallelism [ INT ] [ OPTIONAL ].");

                    //    break;

                    default:
                        Print.PrintText("-MethodName", "Not Found: " + methodName);

                        Console.WriteLine();

                        break;
                }
            }

            Console.WriteLine();

            Print.PrintText("Help", "");

            Console.WriteLine();

            Print.PrintText("-MethodName", "Method name for execution. [ Enum {{ UpdateProduct | UpdateStock | UpdateAllStock | ShopifyUpdateProduct }} ] [ REQUIRED ].");
            Print.PrintText("-OutputType", "Output Type, default is Console [ Enum {{ Console | TextFile }} ] [ OPTIONAL ].");
            Print.PrintText("-SuppressHeader", "Suppress header, assembly information [ BOOL ] [ OPTIONAL ].");
            Print.PrintText("-H", "For help, with -MethodName for help for method [ BOOL ] [ OPTIONAL ].");
        }

        public static void ChangeOutputType()
        {
            if (Argument.HasArgument("-OutputType"))
            {
                var outputType = (OutputType)Enum.Parse(typeof(OutputType), Argument.GetArgumentString("-OutputType"));

                if (outputType == OutputType.TextFile)
                {
                    var outputFileName = Print.NewOutputFileName();

                    Print.PrintText("OutputType", "Change to TextFile: {0}", outputFileName.Substring(outputFileName.IndexOf("./")));

                    Print.SetOutToTextFile(outputFileName);
                }
            }
        }

        public static void WaitLoop(bool keepAlive, int millisecondsTimeout = 250)
        {
            if (Print.CurrentOutputType == OutputType.TextFile)
            {
                var fileInfo = new FileInfo(Print.CurrentOutputFile);

                if (fileInfo.Length >= 10000000)
                {
                    Console.WriteLine();

                    ChangeOutputType();
                }
            }

            if (keepAlive)
            {
                Thread.Sleep(millisecondsTimeout);
            }
        }

        public static bool Try(string refID, string text, int? maxAttempts, bool supressOutpuError, Action action)
        {
            var attemps = 0;

            do
            {
                attemps++;

                try
                {
                    action();

                    return true;
                }
                catch (Exception e)
                {
                    if (!supressOutpuError)
                    {
                        Print.PrintText(
                            ConsoleColor.Red,
                            DateTime.UtcNow.ToString(),
                            "{0} RefId {1} {2}",
                            "Error".PadRight(15, ' '),
                            refID.PadRight(8, ' '),
                            string.Format("{0}{1}Attemps: {2}{3}{4}",
                                text,
                                Environment.NewLine,
                                attemps,
                                Environment.NewLine,
                                e.ToString()));
                    }
                }
            }
            while (maxAttempts == null || attemps < maxAttempts);

            return false;
        }

        public static bool Try(string refID, string text, Action action)
        {
            return Try(refID, text, 3, false, action);
        }

        public static bool Try(string refID, string text, bool supressOutpuError, Action action)
        {
            return Try(refID, text, 3, supressOutpuError, action);
        }

        public static bool Try(string refID, string text, int? maxAttempts, Action action)
        {
            return Try(refID, text, maxAttempts, false, action);
        }

        public static string ToTitle(string text)
        {
            if (text == null)
            {
                return null;
            }

            var items = new[]
            {
                "\r\n",
                "\n",
            };

            foreach (var c in items)
            {
                text = text.Replace(c, " ");
            }

            return CulturePtBr.TextInfo.ToTitleCase(text.ToLower()).Trim().Replace("  ", " ");
        }
    }
}
