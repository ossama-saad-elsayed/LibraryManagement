using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly LibraryManagementDbContext _context;

        public PublisherService(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
        {
            var publishers = await _context.Publishers
                .Select(p => new PublisherDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    ContactNumber = p.ContactNumber
                })
                .ToListAsync();

            return publishers;
        }

        public async Task<PublisherDto?> GetPublisherByIdAsync(int id)
        {
            var p = await _context.Publishers.FindAsync(id);
            if (p == null) return null;

            return new PublisherDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                ContactNumber = p.ContactNumber
            };
        }

        public async Task<PublisherDto> CreatePublisherAsync(CreatePublisherDto request)
        {
            var publisher = new Publisher
            {
                Name = request.Name,
                Address = request.Address,
                ContactNumber = request.ContactNumber
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                ContactNumber = publisher.ContactNumber
            };
        }

        public async Task<PublisherDto?> UpdatePublisherAsync(int id, CreatePublisherDto request)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return null;

            publisher.Name = request.Name;
            publisher.Address = request.Address;
            publisher.ContactNumber = request.ContactNumber;

            await _context.SaveChangesAsync();

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                ContactNumber = publisher.ContactNumber
            };
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return false;

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
