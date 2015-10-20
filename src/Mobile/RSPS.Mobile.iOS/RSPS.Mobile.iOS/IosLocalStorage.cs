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

        public string LoadFile(string fileName)
        {
            var data = NSData.FromFile(fileName);
            var content = data.ToString(NSStringEncoding.Unicode);
            return content;
        }
    }
}