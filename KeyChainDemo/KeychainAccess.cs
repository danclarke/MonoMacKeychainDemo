/*
    Copyright (c) 2012, Dan Clarke
    All rights reserved.

    Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

        Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
        Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
        Neither the name of Dan Clarke nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text;

using MonoMac.Security;
using MonoMac.Foundation;

namespace KeyChainDemo
{
    public static class KeychainAccess
    {
        // Update to the name of your service
        private const string ServiceName = "KeyChain Demo";

        /// <summary>
        /// Gets the password from the OSX keychain
        /// </summary>
        /// <returns>
        /// Password is present in the keychain
        /// </returns>
        /// <param name='username'>
        /// The username
        /// </param>
        /// <param name='password'>
        /// The stored password
        /// </param>
        public static bool GetPassword(string username, out string password)
        {
            SecRecord searchRecord;
            var record = FetchRecord(username, out searchRecord);

            if (record == null)
            {
                password = string.Empty;
                return false;
            }

            password = NSString.FromData(record.ValueData, NSStringEncoding.UTF8);
            return true;
        }

        /// <summary>
        /// Sets a password in the OSX keychain
        /// </summary>
        /// <param name='username'>
        /// Username
        /// </param>
        /// <param name='password'>
        /// Password
        /// </param>
        public static void SetPassword(string username, string password)
        {
            SecRecord searchRecord;
            var record = FetchRecord(username, out searchRecord);

            if (record == null)
            {
                record = new SecRecord(SecKind.InternetPassword)
                {
                    Service = ServiceName,
                    Label = ServiceName,
                    Account = username,
                    ValueData = NSData.FromString(password)
                };

                SecKeyChain.Add(record);
                return;
            }

            record.ValueData = NSData.FromString(password);
            SecKeyChain.Update(searchRecord, record);
        }

        /// <summary>
        /// Clear a password from the keychain
        /// </summary>
        /// <param name='username'>
        /// Username of user to clear
        /// </param>
        public static void ClearPassword(string username)
        {
            var searchRecord = new SecRecord(SecKind.InternetPassword)
            {
                Service = ServiceName,
                Account = username
            };

            SecKeyChain.Remove(searchRecord);
        }

        /// <summary>
        /// Fetchs the record from the keychain
        /// </summary>
        /// <returns>
        /// The record or NULL
        /// </returns>
        /// <param name='username'>
        /// Username of record to fetch
        /// </param>
        /// <param name='searchRecord'>
        /// The search record used to fetch the returned record
        /// </param>
        private static SecRecord FetchRecord(string username, out SecRecord searchRecord)
        {
            searchRecord = new SecRecord(SecKind.InternetPassword)
            {
                Service = ServiceName,
                Account = username
            };

            SecStatusCode code;
            var data = SecKeyChain.QueryAsRecord(searchRecord, out code);

            if (code == SecStatusCode.Success)
                return data;
            else
                return null;
        }
    }
}

