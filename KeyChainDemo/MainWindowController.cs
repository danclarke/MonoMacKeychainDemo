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
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace KeyChainDemo
{
    public partial class MainWindowController : MonoMac.AppKit.NSWindowController
    {
		#region Constructors
		
        // Called when created from unmanaged code
        public MainWindowController(IntPtr handle) : base (handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base (coder)
        {
            Initialize();
        }
		
        // Call to load from the XIB/NIB file
        public MainWindowController() : base ("MainWindow")
        {
            Initialize();
        }
		
        // Shared initialization code
        private void Initialize()
        {
        }
		
		#endregion

        partial void SetPasswordClicked(MonoMac.AppKit.NSButton sender)
        {
            string username, password;

            if (!GetUsernameAndPassword(out username, out password))
                return;

            KeychainAccess.SetPassword(username, password);
            NSAlert.WithMessage("Success", "OK", null, null, "Record set").BeginSheet(Window);
        }

        partial void FetchClicked(MonoMac.AppKit.NSButton sender)
        {
            string username, password;

            if (!GetUsername(out username))
                return;

            var success = KeychainAccess.GetPassword(username, out password);

            if (success)
                SavePasswordLabel.StringValue = password;
            else
                SavePasswordLabel.StringValue = "No record present for specified username";
        }

        partial void DeleteClicked(MonoMac.AppKit.NSButton sender)
        {
            string username;

            if (!GetUsername(out username))
                return;

            KeychainAccess.ClearPassword(username);
            NSAlert.WithMessage("Success", "OK", null, null, "Record cleared").BeginSheet(Window);
        }

        #region UI Access Util

        private bool GetUsernameAndPassword(out string username, out string password)
        {
            username = UsernameTextField.StringValue;
            password = PasswordTextField.StringValue;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                NSAlert.WithMessage("Error", "OK", null, null, "Enter a username and password").BeginSheet(Window);
                return false;
            }

            return true;
        }

        private bool GetUsername(out string username)
        {
            username = UsernameTextField.StringValue;

            if (string.IsNullOrWhiteSpace(username))
            {
                NSAlert.WithMessage("Error", "OK", null, null, "Enter a username").BeginSheet(Window);
                return false;
            }

            return true;
        }

        #endregion
		
        //strongly typed window accessor
        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }
    }
}

