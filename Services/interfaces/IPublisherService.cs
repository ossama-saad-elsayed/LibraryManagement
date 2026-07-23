using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();
        Task<PublisherDto?> GetPublisherByIdAsync(int id);
        Task<PublisherDto> CreatePublisherAsync(CreatePublisherDto request);
        Task<PublisherDto?> UpdatePublisherAsync(int id, CreatePublisherDto request);
        Task<bool> DeletePublisherAsync(int id);
    }
}
