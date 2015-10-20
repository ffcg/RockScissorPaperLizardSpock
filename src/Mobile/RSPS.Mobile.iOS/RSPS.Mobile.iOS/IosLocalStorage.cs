using System.IO;
using Foundation;
using RSPS.Model;

namespace RSPS.Mobile.iOS
{
    public class IosLocalStorage : ILocalStorage
    {
        public void StoreFile(string fileName, string content)
        {
            var data = NSData.FromString(content);
            data.Save(fileName, false);
        }

        public string LoadFileAsString(string fileName)
        {
            var data = NSData.FromFile(fileName);
            var content = data.ToString(NSStringEncoding.Unicode);
            return content;
        }

        public void StoreFile(string fileName, byte[] content)
        {
            //https://bortolu.wordpress.com/2014/03/21/xamarin-c-how-to-convert-byte-array-to-uiimage-with-an-extension-method/
            var data = NSData.FromArray(content);
            data.Save(fileName, false);
        }

        public byte[] LoadFileAsBytes(string fileName)
        {
            var data = NSData.FromFile(fileName);

            return data.ToArray();
        }
    }
}