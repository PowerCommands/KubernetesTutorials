namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IStorageService<T> where T : new()
    {
        string StoreObject(T storeObject, string fileName = "");
        string DeleteObject(string fileName = "");
        T GetObject(string fileName = "");
        string Backup(string fileName = "");
    }
}