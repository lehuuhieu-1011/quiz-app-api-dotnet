using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Services
{
    public class StorageScoresService
    {
        private readonly IStorageScoresRepository<StorageScores> _repo;

        public StorageScoresService(IStorageScoresRepository<StorageScores> repo)
        {
            _repo = repo;
        }

        public async Task<StorageScores> AddScores(StorageScores scores)
        {
            return await _repo.AddScores(scores);
        }

        public async Task<StorageScores> GetById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<List<StorageScores>> GetScoresByUserId(Guid id)
        {
            return await _repo.GetScoresByUserId(id);
        }

    }
}