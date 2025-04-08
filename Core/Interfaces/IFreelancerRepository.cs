using Core.Entities;

namespace Core.Interfaces
{
    public interface IFreelancerRepository
    {

        //contract for freelancer
        Task<IEnumerable<Freelancer>> GetAllAsync(string? search, string? skill, string? hobby);
        Task<Freelancer?> GetByIdAsync(int id);
        Task<Freelancer?> GetByUsernameAsync(string username);
        Task<Freelancer?> GetByEmailAsync(string email);
        Task<IEnumerable<Freelancer>> SearchAsync(string query);
        Task AddAsync(Freelancer freelancer);
        Task UpdateAsync(Freelancer freelancer);
        Task DeleteAsync(int id);
        Task ArchiveAsync(int id);
        Task UnarchiveAsync(int id);
    }
}
