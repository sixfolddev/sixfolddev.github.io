
using System.Collections.Generic;


namespace RoomAid.ServiceLayer
{
    public interface IArchiveService
    {
        bool FileOutPut(List<string> resultSet);
        bool DeleteLog(string fileName);

        string GetMessage();

    }
}
