using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TygaSoft.ProcessorCA
{
    //public class MachineKey
    //{
    //    const int validationKeyLength = 64;
    //    const int decryptionKeyLength = 24;
    //    private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    //    public string GenerateKey()
    //    {
    //        return string.Format("<machineKey validationKey=\"{0}\"\r\ndecryptionKey=\"{1}\"\r\nvalidation=\"SHA1\"/>",
    //          BytesToHex(GenerateKeyBytes(validationKeyLength)), BytesToHex(GenerateKeyBytes(decryptionKeyLength)));
    //    }

    //    private byte[] GenerateKeyBytes(int cb)
    //    {
    //        byte[] rndData = new byte[cb];
    //        rng.GetBytes(rndData);
    //        return rndData;
    //    }

    //    private string BytesToHex(byte[] key)
    //    {
    //        StringBuilder sb = new StringBuilder();

    //        for (int i = 0; i < key.Length; ++i)
    //        {
    //            sb.Append(String.Format("{0:X2}", key[i]));
    //        }

    //        return sb.ToString();
    //    }
    //}
}
