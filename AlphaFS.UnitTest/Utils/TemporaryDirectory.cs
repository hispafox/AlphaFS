/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using Alphaleonis;
using System;
using SysIODirectoryInfo = System.IO.DirectoryInfo;
using SysIOPath = System.IO.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>Used to create a temporary directory that will be deleted once this instance is disposed.</summary>
   internal sealed class TemporaryDirectory : IDisposable
   {
      public TemporaryDirectory(string prefix = null) : this(SysIOPath.GetTempPath(), prefix) { }
      
      public TemporaryDirectory(string root, string prefix)
      {
         if (Utils.IsNullOrWhiteSpace(prefix))
            prefix = "AlphaFS";

         do
         {
            Directory = new SysIODirectoryInfo(SysIOPath.Combine(root, prefix + "-" + Guid.NewGuid().ToString("N").Substring(0, 6)));

         } while (Directory.Exists);

         Directory.Create();
      }

      public SysIODirectoryInfo Directory { get; private set; }

      public string RandomFileFullPath
      {
         get { return SysIOPath.Combine(Directory.FullName, SysIOPath.GetRandomFileName()); }
      }

      #region Disposable Members

      ~TemporaryDirectory()
      {
         Dispose(false);
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      private void Dispose(bool isDisposing)
      {
         try
         {
            if (isDisposing)
            {
               Directory.Refresh();

               if (Directory.Exists)
                  Directory.Delete(true);

               else
                  Console.WriteLine("\n\nThe TemporaryDirectory was already removed.");
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\nFailed to delete TemporaryDirectory: [{0}]: [{1}]", Directory, ex.Message);
         }
      }

      #endregion // Disposable Members
   }
}
