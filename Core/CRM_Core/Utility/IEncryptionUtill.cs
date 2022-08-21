using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    public interface IEncryptionUtill
    {
        string EncryptString_Aes(string plainText);
        string DecryptString_Aes(string encryptedText);
    }
}
