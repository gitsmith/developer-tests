using System.Data;

namespace HockeyApi.Common
{
    public interface IDb
    {
        IDbConnection CreateConnection();
    }
}
