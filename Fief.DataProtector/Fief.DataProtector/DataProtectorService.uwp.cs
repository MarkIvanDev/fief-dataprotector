using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Fief.DataProtector
{
    public partial class DataProtectorService
    {
        async Task<string> PlatformEncrypt(string data)
        {
            var provider = new DataProtectionProvider(protectionDescriptor ?? "LOCAL=user");
            var buffer = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);
            var protectedBuffer = await provider.ProtectAsync(buffer);
            var protectedString = CryptographicBuffer.EncodeToBase64String(protectedBuffer);
            return protectedString;
        }

        async Task<string> PlatformDecrypt(string data)
        {
            var provider = new DataProtectionProvider();
            var buffer = CryptographicBuffer.DecodeFromBase64String(data);
            var unprotectedBuffer = await provider.UnprotectAsync(buffer);
            var unprotectedString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, unprotectedBuffer);
            return unprotectedString;
        }

        async Task<byte[]> PlatformEncrypt(byte[] data)
        {
            var provider = new DataProtectionProvider(protectionDescriptor ?? "LOCAL=user");
            var buffer = CryptographicBuffer.CreateFromByteArray(data);
            using (var source = await GetInputStream(buffer))
            using (var target = new InMemoryRandomAccessStream())
            {
                await provider.ProtectStreamAsync(source, target.GetOutputStreamAt(0));
                using (var reader = new DataReader(target.GetInputStreamAt(0)))
                {
                    var bytes = new byte[target.Size];
                    await reader.LoadAsync((uint)target.Size);
                    reader.ReadBytes(bytes);
                    return bytes;
                }
            }
        }

        async Task<byte[]> PlatformDecrypt(byte[] data)
        {
            var provider = new DataProtectionProvider();
            var buffer = CryptographicBuffer.CreateFromByteArray(data);
            using (var source = await GetInputStream(buffer))
            using (var target = new InMemoryRandomAccessStream())
            {
                await provider.UnprotectStreamAsync(source, target.GetOutputStreamAt(0));
                using (var reader = new DataReader(target.GetInputStreamAt(0)))
                {
                    var bytes = new byte[target.Size];
                    await reader.LoadAsync((uint)target.Size);
                    reader.ReadBytes(bytes);
                    return bytes;
                }
            }
        }

        async Task<IInputStream> GetInputStream(IBuffer buffer)
        {
            var stream = new InMemoryRandomAccessStream();
            using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
            {
                writer.WriteBuffer(buffer);
                await writer.StoreAsync();
                await writer.FlushAsync();
            }
            return stream.GetInputStreamAt(0);
        }
    }
}
