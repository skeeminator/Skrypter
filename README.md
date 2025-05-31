# 🛡️ Malware Binder V2 🛡️
![CSHARP](https://img.shields.io/badge/Language-CSHARP-boldgreen?style=for-the-badge&logo=csharp)
<img src="https://img.shields.io/github/v/release/K3rnel-Dev/HellSing-Binder?style=for-the-badge&color=blue">
<img src="https://img.shields.io/github/downloads/K3rnel-Dev/HellSing-Binder/total?style=for-the-badge&color=purple">

<p>
  <img src="banner.png" alt="Project Banner" />
</p>

# 🛠️ File Merger & Encryption Tool

This project is a file merger and encryption tool that combines two executable files into one using XOR encryption and embeds them into a stub. The stub is then compiled by a builder. When executed, the stub decrypts the two resources and extracts them to the disk location specified by the user in the builder options.

---

### > **[⬇️ Download for Windows](https://github.com/K3rnel-Dev/MalwareBinder/releases/tag/Release)**

---

## ✨ Features

- 🔐 **XOR Encryption:** Converts two executable files into ciphertext by applying XOR encryption.
- 📦 **Stub Resource Embedding:** The builder embeds two encrypted resources into the stub, which is compiled and executed later.
- 💾 **File Extraction:** The stub decrypts and drops the two files to a location of the user's choice.
- ⚙️ **Flexible Configuration:**
  - 👁️ **HideFiles:** Option to hide the dropped files.
  - 🗑️ **Self-Delete:** Automatically deletes the stub after it completes its task.
  - 🤖 **High Mutation:** Obfuscation technique to make the code harder to analyze.
  - 🎃 **AMSI/ETW Patcher** Patch to prevent several functions from the amsi and ntdll library from working

---

## 📚 Dependencies

To build this project, you need the following dependencies:

- **MetroFramework** - A .NET WinForms framework used for the UI.  
  [Download MetroFramework](https://github.com/thielj/MetroFramework)
  
- **dnlib** - A library to work with .NET assemblies for obfuscation purposes.  
  [Download dnlib](https://github.com/0xd4d/dnlib)

Make sure to install these libraries before building the project.

---

## 🚀 Usage

### Builder Configuration
1. Add the two executable files that you want to merge.
2. Select the options you need:
   - 👁️ **HideFiles:** Hide the extracted files after dropping them.
   - 🗑️ **Self-Delete:** Automatically delete the stub after execution.
   - 🤖 **Obfuscate:** Apply basic obfuscation to the build file.
3. Compile the stub with the embedded encrypted-files.

### Execution
Once the stub is executed:
- It decrypts the embedded files.
- Extracts them to the specified disk location.
- Executes any optional features (hide files, self-delete).

---

## 📸 Screenshots

> ![](./1.png)
> ![](./2.png)

---

## ⚠️ Disclaimer

**This tool is created for educational purposes only.**  
Any misuse of this project for malicious purposes is strictly prohibited.  
The author is not responsible for any illegal use or damages caused by this tool.

---

## ⚙️ Credits

- **MetroFramework**  
  [MetroFramework on GitHub](https://github.com/thielj/MetroFramework)

- **dnlib**  
  [dnlib on GitHub](https://github.com/0xd4d/dnlib)

# Advanced Binder & Crypter

A powerful file binder and crypter application for Windows with multiple features for executable management. This tool allows binding multiple files into a single executable with advanced encryption options.

## Features

- File binding capability (combine two executables into one)
- Single-file crypter mode (encrypt individual executables)
- Advanced polymorphic encryption for maximum FUD capability
- Custom stub settings for execution control
- Built-in file hiding functionality
- Self-removal options
- Randomized execution patterns

## Technical Details

The application uses a multi-layered approach to file encryption:
- Content-based key generation
- Dynamic block-based encryption
- Variable bit rotation and XOR operations
- Special handling for PE files to preserve header structures
- Multiple fallback decryption mechanisms

Created by SkeemAI
