# Skrypter - Advanced Payload Crypter

---

## Overview

**Skrypter** is the biggest mess Ive made lol.

---

## Key Features

- **Enhanced Encryption** - Position-dependent XOR encryption preserving PE headers while thoroughly securing payload body
- **Advanced AMSI Bypass** - Implements string obfuscation, reflection-based loading, and variable-key XOR-encoded shellcode
- **ETW Bypass** - Now provides both direct and indirect syscall-based ETW bypasses on modern Windows systems
- **Polymorphic Generation** - Generates unique crypted payloads with randomized namespaces and class names
- **Process Masquerading** - Mimics legitimate Windows processes to avoid detection
- **Indirect Syscalls** - Implements the Evilbytecode-Gate approach for stealthy system call execution
- **UAC Bypass** - Administrative privilege elevation capabilities
- **Multi-layered Persistence** - Comprehensive persistence mechanisms including:
  - Standard registry startup
  - Windows Recovery Environment (WinRE) persistence that survives factory resets
  - Scheduled tasks and registry RunOnce keys
  - Multiple backup persistence locations for high resilience
- **Anti-Analysis Measures** - Configurable execution delays to prevent dynamic analysis
- **Professional Obfuscation** - Optional Armdot integration for enterprise-grade protection

---

## Security Techniques

- **Indirect Syscalls** - Uses advanced techniques to resolve system service numbers (SSNs) from ntdll.dll and execute syscalls without direct API calls
- **Control Flow Obfuscation** - Multiple patterns including branching, exception-based, and switch-based flow
- **Dead Code Injection** - Strategic insertion of non-functional code to confuse static analysis
- **Assembly Attribute Manipulation** - Modifies assembly attributes to appear legitimate
- **Compiler Pattern Mimicry** - Simulates compiler-generated code patterns for enhanced stealth
- **Execution Timing Variations** - Randomized delays to prevent runtime detection
- **Anti-Debug Implementation** - Advanced detection and countermeasures for debugging attempts
- **String Protection** - Obfuscates embedded strings to prevent signature detection
- **WinRE Persistence** - Embeds payload in Windows Recovery Environment to survive system resets

---

## Current Capabilities

- **AMSI Bypass** - Memory-based bypass technique
- **ETW Bypass** - Both direct and indirect syscall methods for Event Tracing for Windows evasion
- **Encryption** - Position-dependent XOR encryption with optional polymorphic AES layer
- **Control Flow Obfuscation** - Advanced flow manipulation
- **Assembly Attribute Spoofing** - Legitimate assembly appearance
- **Code Obfuscation** - Strategic code insertion and manipulation
- **Variable Protection** - Comprehensive variable name obfuscation
- **Process Masquerading** - Makes the process appear as legitimate Windows software
- **Indirect Syscalls** - Evilbytecode-Gate implementation for stealthy system calls
- **WinRE Persistence** - Payload persistence through Windows Recovery Environment

**Recently Implemented:**
- **✓ WinRE Persistence** - Multi-layered approach for survival through system factory resets
- **✓ Indirect Syscalls** - Using Evilbytecode-Gate approach for resolution and execution
- **✓ Process Masquerading** - Legitimate process impersonation with behavioral mimicking
- **✓ Polymorphic AES** - Additional encryption layer with dynamic key generation

**In Development:**
- **Memory Only Execution**
- **Process Hollowing Techniques**
- **Anti-VM Enhancements**
- **Kernel-Level Persistence Options**

---

## Technical Details

### Indirect Syscalls Implementation

The crypter now includes an implementation of indirect syscalls based on the Evilbytecode-Gate approach:

1. **SSN Resolution** - Extracts System Service Numbers by analyzing ntdll.dll exports
2. **Syscall Address Location** - Identifies the actual syscall instruction address
3. **Dynamic Shellcode Generation** - Creates and executes shellcode that:
   - Sets up syscall registers correctly (mov r10, rcx)
   - Loads the correct SSN (mov eax, ssn)
   - Jumps to the syscall instruction in ntdll (jmp r11)

This technique avoids direct API calls and significantly reduces detection by EDRs and AVs.

### WinRE Persistence

The WinRE persistence mechanism provides comprehensive survival capabilities through system factory resets:

1. **Primary Recovery Environment Integration**:
   - Places payload in the Windows Recovery Environment directory (`C:\Recovery\OEM`)
   - Modifies `ResetConfig.xml` to execute the payload during system reset
   - Base64-encodes the executable in `recoverypayload.xml`

2. **Secondary Persistence Locations**:
   - Creates hidden `CustomRecovery` directories in multiple system locations:
     - `C:\Windows\System32\Recovery\CustomRecovery`
     - `C:\Windows\System32\Config\SystemProfile\AppData\Local\Microsoft\Windows\WinRE\CustomRecovery`
     - `C:\ProgramData\Microsoft\Windows\WinRE\CustomRecovery`

3. **WinRE.wim Image Modification**:
   - Attempts to locate and mount the Windows Recovery Image (WinRE.wim)
   - Injects payload scripts into the mounted image
   - Adds registry RunOnce entries to the mounted system configuration
   - Unmounts and commits changes to ensure persistence survives image usage

4. **Recovery Script Execution**:
   - PowerShell script that decodes and executes the payload
   - Batch file wrappers for compatibility and error handling
   - Registry entries pointing to recovery scripts

5. **Fallback Persistence Mechanisms**:
   - Scheduled Task creation with ONLOGON trigger
   - Registry RunOnce keys for next-boot execution
   - Multiple file and registry locations to ensure redundancy

This multi-layered approach provides exceptional persistence capabilities that can survive even a complete system reset to factory settings.

---

## Setup

1. Clone the repository:
    ```bash
    git clone https://github.com/SkeemLabs/Skrypter
    cd Skrypter
    ```

2. Open the solution in **Visual Studio 2019/2022**.

3. Build in **Release** mode.

4. Execute with **Administrator** privileges (required for WinRE persistence).

---

## Compatibility

- Full **Pulsar** payload compatibility
- Windows 10/11 support
- .NET Framework 4.8
- Administrator privileges required for all features

---

## Development

- Developed by **SkeemLabs**
- Based on advanced crypting methodologies
- Incorporates Evilbytecode-Gate indirect syscall techniques
- Implements multi-layered WinRE persistence inspired by Reset Survival techniques
- Continuously updated to maintain effectiveness against modern security solutions

---

## Legal Disclaimer

This tool is provided for **educational and authorized testing purposes only**.  
Users are responsible for ensuring proper authorization for all usage.  
Unauthorized computer system access is illegal.

**Legitimate Use Cases**:
- Authorized Red Team operations
- Professional penetration testing
- Security research and development
- Malware protection research

**Usage Restriction**: Do not deploy on systems without explicit authorization.

---

## Support

For professional inquiries and custom implementation services, contact SkeemLabs directly.

---

# Skrypter - Enterprise-Grade Payload Protection
