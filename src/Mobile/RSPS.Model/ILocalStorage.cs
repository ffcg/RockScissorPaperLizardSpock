namespace RSPS.Model
{
    public interface ILocalStorage
    {
        void StoreFile(string fileName, string content);
        string LoadFile(string fileName);
    }
}