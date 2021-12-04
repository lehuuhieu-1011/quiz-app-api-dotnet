using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.Repositories
{
    public class StorageScoresRepository : IStorageScoresRepository<StorageScores>
    {
        private readonly DataContext _context;
        public StorageScoresRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<StorageScores> AddScores(StorageScores scores)
        {
            await _context.StorageScores.AddAsync(scores);
            await _context.SaveChangesAsync();
            return scores;
        }

        public async Task<StorageScores> GetById(Guid id)
        {
            StorageScores scores = await _context.StorageScores.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (scores == null)
            {
                return null;
            }
            return scores;
        }

        public async Task<List<StorageScores>> GetScoresByUserId(Guid id)
        {
            List<StorageScores> scores = await _context.StorageScores.Include(x => x.CourseQuiz).Include(x => x.User).Where(x => x.UserId.Equals(id)).ToListAsync();
            if (scores.Count == 0)
            {
                return null;
            }
            return scores;
        }
    }
}