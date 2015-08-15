﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommandLine;

namespace RokuLoader
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var consoleLine = new string('-', 80);

            Console.WriteLine(consoleLine);
            Console.WriteLine("RokuLoader {0}", fvi.ProductVersion);
            Console.WriteLine("Roku command line interface for the Development Application Installer");
            Console.WriteLine("See http://sdkdocs.roku.com/display/sdkdoc/Developer+Guide");
            Console.WriteLine(consoleLine);
            Console.WriteLine();

            var options = new Options();
            var success = false;
  
            if (Parser.Default.ParseArguments(args, options))
            {
                if (options.Hostname != null && options.Password != null)
                {

                    if (options.ZipFilePath != null)
                    {
                        if (!File.Exists(options.ZipFilePath))
                        {
                            Console.WriteLine("File does not exist {0}", options.ZipFilePath);
                        }
                        else
                        {
                            if (!ZipCheck.CheckSignature(options.ZipFilePath))
                            {
                                Console.WriteLine("Not a valid zip file {0}", options.ZipFilePath);
                            }
                            else
                            {
                                //Upload the file to the Roku
                                success = HttpUpload.PostFile(options.Hostname, options.Username, options.Password, options.ZipFilePath);
                            }
                        }

                    }
                    else
                    {
                        if (options.CodeDirectoryPath != null)
                        {
                            Console.WriteLine("Option --code-directory-path (-c) is not implemented.");
                        }
                        else
                        {
                            Console.WriteLine("Must specify either --code-directory-path (-c) or --zip-file-path (-z)");
                        }
                    }
                }
            }

#if DEBUG
                Console.WriteLine("Debug Build. Hit any key to close.");
                Console.ReadKey();
#endif
            Environment.ExitCode = success ? 0 : 1;
        }
    }
}
