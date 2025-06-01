# Skrypter - Advanced Payload Crypter

---

## Overview

**Skrypter** is a sophisticated payload protection solution developed by **SkeemLabs**. This tool encrypts and obfuscates executables to evade security measures while preserving full functionality and maintaining operational integrity.

---

## Key Features

- **Enhanced Encryption** - Position-dependent XOR encryption preserving PE headers while thoroughly securing payload body
- **Advanced AMSI Bypass** - Implements string obfuscation, reflection-based loading, and variable-key XOR-encoded shellcode
- **ETW Bypass** - Provides reliable Event Tracing for Windows bypass on modern Windows systems
- **Polymorphic Generation** - Generates unique crypted payloads with randomized namespaces and class names
- **Process Handling** - Optimized process management for complete Pulsar compatibility
- **UAC Bypass** - Administrative privilege elevation capabilities
- **Persistence** - Scheduled task creation for reliable system execution
- **Anti-Analysis Measures** - Configurable execution delays to prevent dynamic analysis
- **Professional Obfuscation** - Optional Armdot integration for enterprise-grade protection

---

## Security Techniques

- **Control Flow Obfuscation** - Multiple patterns including branching, exception-based, and switch-based flow
- **Dead Code Injection** - Strategic insertion of non-functional code to confuse static analysis
- **Assembly Attribute Manipulation** - Modifies assembly attributes to appear legitimate
- **Compiler Pattern Mimicry** - Simulates compiler-generated code patterns for enhanced stealth
- **Execution Timing Variations** - Randomized delays to prevent runtime detection
- **Anti-Debug Implementation** - Advanced detection and countermeasures for debugging attempts
- **String Protection** - Obfuscates embedded strings to prevent signature detection

---

## Current Capabilities

- **AMSI Bypass** - Memory-based bypass technique
- **ETW Bypass** - Event Tracing for Windows evasion
- **Encryption** - Position-dependent XOR encryption
- **Control Flow Obfuscation** - Advanced flow manipulation
- **Assembly Attribute Spoofing** - Legitimate assembly appearance
- **Code Obfuscation** - Strategic code insertion and manipulation
- **Variable Protection** - Comprehensive variable name obfuscation

**In Development:**
- **~~Direct Syscalls~~**
- **~~Alternative Injection Methods~~**
- **~~Memory Only Execution~~**
- **~~Process Hollowing Techniques~~**
- **~~Anti-VM Enhancements~~**

---

## Setup

1. Clone the repository:
    ```bash
    git clone https://github.com/SkeemLabs/Skrypter
    cd Skrypter
    ```

2. Open the solution in **Visual Studio 2019/2022**.

3. Build in **Release** mode.

4. Execute with **Administrator** privileges.

---

## Compatibility

- Full **Pulsar** payload compatibility
- Windows 10/11 support
- .NET Framework 4.7.2+

---

## Development

- Developed by **SkeemLabs**
- Based on advanced crypting methodologies

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
