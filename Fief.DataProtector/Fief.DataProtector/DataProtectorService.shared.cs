using System;
using System.Threading.Tasks;

namespace Fief.DataProtector
{
    public partial class DataProtectorService
    {
        private readonly string protectionDescriptor;

        public DataProtectorService(string protectionDescriptor = null)
        {
            this.protectionDescriptor = protectionDescriptor;
        }

        public Task<string> Encrypt(string data)
            => PlatformEncrypt(data);

        public Task<string> Decrypt(string data)
            => PlatformDecrypt(data);

        public Task<byte[]> Encrypt(byte[] data)
            => PlatformEncrypt(data);

        public Task<byte[]> Decrypt(byte[] data)
            => PlatformDecrypt(data);
    }
}
