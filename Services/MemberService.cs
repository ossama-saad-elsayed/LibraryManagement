using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryManagementDbContext _context;

        public MemberService(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        {
            var members = await _context.Members.ToListAsync();
            return members.Select(m => new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                CreatedAt = m.CreatedAt
            });
        }

        public async Task<MemberDto?> GetMemberByIdAsync(int id)
        {
            var m = await _context.Members.FindAsync(id);
            if (m == null) return null;

            return new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                CreatedAt = m.CreatedAt
            };
        }

        public async Task<MemberDto> CreateMemberAsync(CreateMemberDto request)
        {
            var member = new Member
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                PhoneNumber = member.PhoneNumber,
                Address = member.Address,
                CreatedAt = member.CreatedAt
            };
        }

        public async Task<MemberDto?> UpdateMemberAsync(int id, UpdateMemberDto request)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return null;

            member.FullName = request.FullName;
            member.PhoneNumber = request.PhoneNumber;
            member.Address = request.Address;

            await _context.SaveChangesAsync();

            return new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                PhoneNumber = member.PhoneNumber,
                Address = member.Address,
                CreatedAt = member.CreatedAt
            };
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return false;

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
