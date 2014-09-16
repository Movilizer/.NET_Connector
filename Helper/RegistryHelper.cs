using System;
using System.Security.Cryptography;
using System.Text;

using MWS.Log;

namespace MWS.Helper
{
    public static class RegistryHelper
    {
        private static DataProtectionScope DATA_PROTECTION_SCOPE = DataProtectionScope.LocalMachine;

        private static object ReadValue(string rootKey, string name, object defaultValue)
        {
            // try to read 32bit value first
            object value = RegistryWOW6432.GetRegKey32(RegHive.HKEY_LOCAL_MACHINE, rootKey, name);

            if(value == null)
            {
                //try 64bit value then
                value = RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, rootKey, name);
            }

            // otherwise take over the default value
            if(value == null)
            {
                value = defaultValue;
            }

            if (value == null)
            {
                // no registry entry found for the specified name
                string message = String.Format("Registry configuration value reading failed: {0} : {1}",
                    rootKey, name);

                if (defaultValue == null)
                {
                    LogFactory.WriteWarning(message);
                }
                else
                {
                    LogFactory.WriteError(message);
                    throw new InvalidProgramException(message);
                }
            }

            return value;
        }

        public static string ReadString(string rootKey, string name)
        {
            return ReadString(rootKey, name, null);
        }

        public static string ReadString(string rootKey, string name, string defaultValue)
        {
            object value = ReadValue(rootKey, name, defaultValue);

            if (!(value is String))
            {
                // registry entry found, but has a wrong data type
                string message = String.Format("Registry configuration string type expected for: {0} : {1}",
                    rootKey, name);

                LogFactory.WriteError(message);
                throw new InvalidProgramException(message);
            }

            return (string)value;
        }

        public static string ReadProtectedString(string rootKey, string name, string entropy)
        {
            return ReadProtectedString(rootKey, name, null, entropy);
        }

        public static string ReadProtectedString(string rootKey, string name, string defaultValue, string entropy)
        {
            //string test = Protect("tT2N-4ebWU1(F!wGl_HD3cBt8#Esb)n", "movilizer");

            object value = ReadValue(rootKey, name, defaultValue);

            if (!(value is String))
            {
                // registry entry found, but has a wrong data type
                string message = String.Format("Registry configuration string or binary type expected for: {0} : {1}",
                    rootKey, name);

                LogFactory.WriteError(message);
                throw new InvalidProgramException(message);
            }

            // skip empty values
            if (value.Equals(""))
            {
                return (string)value;
            }

            try
            {
                // unprotect the value
                return Unprotect((string)value, entropy);
            }
            catch (Exception e)
            {
                string message = String.Format("Registry value unprotecting failed for: {0} : \"{1}\" Message: {2}",
                    rootKey, name, e.Message);

                LogFactory.WriteError(message);
            }

            return null;
        }

        public static int ReadInt(string rootKey, string name)
        {
            return ReadInt(rootKey, name, null);
        }

        public static int ReadInt(string rootKey, string name, string defaultValue)
        {
            object value = ReadValue(rootKey, name, defaultValue);

            try
            {
                return Int32.Parse((string)value);
            }
            catch (Exception e)
            {
                // no registry entry found for the specified name
                string message = String.Format("Registry configuration int type expected for: {0} : {1}. Exception: {2}",
                    rootKey, name, e.Message);

                LogFactory.WriteError(message);
                throw new InvalidProgramException(message);
            }
        }

        public static long ReadLong(string rootKey, string name)
        {
            return ReadLong(rootKey, name, null);
        }

        public static long ReadLong(string rootKey, string name, string defaultValue)
        {
            object value = ReadValue(rootKey, name, defaultValue);

            try
            {
                return Int64.Parse((string)value);
            }
            catch (Exception e)
            {
                // no registry entry found for the specified name
                string message = String.Format("Registry configuration long type expected for: {0} : {1}. Exception: {2}",
                    rootKey, name, e.Message);

                LogFactory.WriteError(message);
                throw new InvalidProgramException(message);
            }
        }

        private static string Protect(string value, string entropy)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(value);
            byte[] entropyBytes = Encoding.UTF8.GetBytes(entropy);

            byte[] encryptedData = ProtectedData.Protect(plainBytes, entropyBytes, DATA_PROTECTION_SCOPE);

            return Convert.ToBase64String(encryptedData);
        }

        private static string Unprotect(string value, string entropy)
        {
            byte[] encryptedData = Convert.FromBase64String(value);
            byte[] entropyBytes = Encoding.UTF8.GetBytes(entropy);

            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, entropyBytes, DATA_PROTECTION_SCOPE);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
