using System.IO;
using WLM_WLMN.Common.DTOs;

namespace WLM_WLMN.Common.Interfaces
{
    public interface IUserUpdateStorage
    {
        void Save(UserUpdate userUpdate);
        void Backup(string targetLocation, int numberOfUpdates);
        void Restore(DirectoryInfo directoryInfo);
    }
}