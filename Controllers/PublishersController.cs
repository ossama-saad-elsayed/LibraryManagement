using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAllPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] CreatePublisherDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdPublisher = await _publisherService.CreatePublisherAsync(request);
            return CreatedAtAction(nameof(GetPublisher), new { id = createdPublisher.Id }, createdPublisher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherDto>> UpdatePublisher(int id, [FromBody] CreatePublisherDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedPublisher = await _publisherService.UpdatePublisherAsync(id, request);
            if (updatedPublisher == null)
            {
                return NotFound();
            }
            return Ok(updatedPublisher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var result = await _publisherService.DeletePublisherAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
