using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.Repositories
{
    public interface IStorageScoresRepository<T>
    {
        Task<StorageScores> AddScores(StorageScores scores);
        Task<StorageScores> GetById(Guid id);
        Task<List<StorageScores>> GetScoresByUserId(Guid id);
    }
}