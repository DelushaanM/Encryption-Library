# AES-256 Encryption Library

## 📌 Overview
This library provides AES-256 encryption and decryption functionalities using a password-based key derivation mechanism. It ensures secure data encryption using industry best practices like PBKDF2 key derivation and CBC mode with proper padding.

## 📂 Data Structure
- **Key Size:** 256 bits
- **Salt Size:** 128 bits
- **IV Size:** 128 bits
- **Iterations:** 100,000 (PBKDF2)
- **Cipher Mode:** CBC (Cipher Block Chaining)
- **Padding Mode:** PKCS7

## 🚀 Usage
### Encryption
```csharp
string encryptedText = Encryptor.Encrypt("Your Secret Data", "StrongPassword");
Console.WriteLine(encryptedText);
```

### Decryption
```csharp
string decryptedText = Encryptor.Decrypt(encryptedText, "StrongPassword");
Console.WriteLine(decryptedText);
```

## 🔍 How It Works
1. **Key Derivation:** The password is processed using PBKDF2 to generate a strong 256-bit key.
2. **Salt Generation:** A random salt is generated for each encryption to prevent dictionary attacks.
3. **IV Generation:** A new Initialization Vector (IV) is created to ensure different ciphertexts for the same plaintext.
4. **AES Encryption:** The plaintext is encrypted using AES-256 in CBC mode with PKCS7 padding.
5. **Storage:** The salt, IV, and encrypted data are combined and stored securely.
6. **Decryption Process:** The salt and IV are extracted, the key is rederived using PBKDF2, and AES-256 decrypts the ciphertext back to plaintext.

## ✅ Advantages
- **Strong Security:** Uses AES-256, the highest standard in symmetric encryption.
- **Password-Based Key Derivation:** PBKDF2 makes brute-force attacks highly difficult.
- **Random Salt & IV:** Ensures unique encryption outputs for identical inputs.
- **Integrity Protection:** Prevents common vulnerabilities like key reuse.
- **Ease of Use:** Simple API for encrypting and decrypting data.

## 🔒 Best Practices
- Always use a **strong, unique password** for encryption.
- Do not hardcode passwords in your application; use a **secure vault**.
- Store the encrypted data securely, preferably in a **protected database**.
- Regularly **rotate passwords** to maintain security.
- Use **HMAC authentication** for additional integrity verification if required.
- Implement **secure key management practices** to avoid unauthorized access.

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

