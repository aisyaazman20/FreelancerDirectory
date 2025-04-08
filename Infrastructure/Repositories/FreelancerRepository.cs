using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class FreelancerRepository : IFreelancerRepository
    {
        private readonly FreelancerDbContext _context;

        public FreelancerRepository(FreelancerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Freelancer>> GetAllAsync(string? search, string? skill, string? hobby)
        {
            //retrieve active freelancers including skills & hobbies from db
            var query = _context.Freelancers
                .Include(f => f.Skills)
                .Include(f => f.Hobbies)
                .Where(f => !f.IsArchived)
                .AsQueryable();

            //search filter using LINQ
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f =>
                    f.Username.Contains(search) ||
                    f.Email.Contains(search) ||
                    f.PhoneNumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(skill))
            {
                query = query.Where(f =>
                    f.Skills.Any(s => s.Name.Contains(skill)));
            }

            if (!string.IsNullOrWhiteSpace(hobby))
            {
                query = query.Where(f =>
                    f.Skills.Any(h => h.Name.Contains(hobby)));
            }

            return await query.ToListAsync();
        }

        public async Task<Freelancer?> GetByIdAsync(int id)
        {
            return await _context.Freelancers
                .Include(f => f.Skills)
                .Include(f => f.Hobbies)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsArchived);
        }

        public async Task<Freelancer?> GetByUsernameAsync(string username)
        {
            return await _context.Freelancers
                .Include(f => f.Skills)
                .Include(f => f.Hobbies)
                .FirstOrDefaultAsync(f => f.Username == username && !f.IsArchived);
        }

        public async Task<Freelancer?> GetByEmailAsync(string email)
        {
            return await _context.Freelancers
                .FirstOrDefaultAsync(f => f.Email == email && !f.IsArchived);
        }


        public async Task<IEnumerable<Freelancer>> SearchAsync(string query)
        {
            return await _context.Freelancers
                .Include(f => f.Skills)
                .Include(f => f.Hobbies)
                .Where(f => (f.Username.Contains(query) || f.Email.Contains(query)) && !f.IsArchived)
                .ToListAsync();
        }

        public async Task AddAsync(Freelancer freelancer)
        {
            await _context.Freelancers.AddAsync(freelancer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Freelancer freelancer)
        {
            _context.Freelancers.Update(freelancer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer is not null)
            {
                _context.Freelancers.Remove(freelancer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ArchiveAsync(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer is not null)
            {
                freelancer.IsArchived = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnarchiveAsync(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer is not null)
            {
                freelancer.IsArchived = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
