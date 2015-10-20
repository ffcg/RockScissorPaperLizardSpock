namespace RSPS.Model
{
    public interface ILocalStorage
    {
        void StoreFile(string fileName, string content);
        string LoadFileAsString(string fileName);

        void StoreFile(string fileName, byte[] content);
        byte[] LoadFileAsBytes(string fileName);
    }
}