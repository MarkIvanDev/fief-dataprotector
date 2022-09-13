using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fief.DataProtector
{
    public partial class DataProtectorService
    {
        static Task<string> PlatformEncrypt(string data)
            => throw new NotImplementedException();

        static Task<string> PlatformDecrypt(string data)
            => throw new NotImplementedException();

        static Task<byte[]> PlatformEncrypt(byte[] data)
            => throw new NotImplementedException();

        static Task<byte[]> PlatformDecrypt(byte[] data)
            => throw new NotImplementedException();
    }
}
