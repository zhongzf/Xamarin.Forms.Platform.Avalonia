using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class AvaloniaPlatformServices : IPlatformServices
    {
        public bool IsInvokeRequired => !global::Avalonia.Application.Current.CheckAccess();

        public string RuntimePlatform => "Avalonia";

        public void BeginInvokeOnMainThread(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action);
        }

        public Ticker CreateTicker()
        {
            return new AvaloniaTicker();
        }

        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public string GetMD5Hash(string input)
        {
            // MSDN - Documentation -https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5(v=vs.110).aspx
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
        {
            // TODO:
            return 20;
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
        {
            return Platform.GetNativeSize(view, widthConstraint, heightConstraint);
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Stream>();

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(uri);
                request.BeginGetResponse(ar =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    try
                    {
                        Stream stream = request.EndGetResponse(ar).GetResponseStream();
                        tcs.TrySetResult(stream);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            return new AvaloniaIsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly());
        }

        public void OpenUriAction(Uri uri)
        {
            System.Diagnostics.Process.Start(uri.AbsoluteUri);
        }

        public void QuitApplication()
        {      
            // TODO:
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var timer = new DispatcherTimer() { Interval = interval };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                bool result = callback();
                if (!result)
                    timer.Stop();
            };
        }
    }
}
