using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FinesController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FinesController(IFineService fineService)
        {
            _fineService = fineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FineDto>>> GetAllFines()
        {
            var fines = await _fineService.GetAllFinesAsync();
            return Ok(fines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FineDto>> GetFine(int id)
        {
            var fine = await _fineService.GetFineByIdAsync(id);
            if (fine == null)
            {
                return NotFound();
            }
            return Ok(fine);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<FineDto>>> GetFinesByMember(int memberId)
        {
            var fines = await _fineService.GetFinesByMemberIdAsync(memberId);
            return Ok(fines);
        }

        [HttpPost]
        public async Task<ActionResult<FineDto>> CreateFine([FromBody] CreateFineDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdFine = await _fineService.CreateFineAsync(request);
                return CreatedAtAction(nameof(GetFine), new { id = createdFine.Id }, createdFine);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/pay")]
        public async Task<ActionResult<FineDto>> PayFine(int id)
        {
            var updatedFine = await _fineService.PayFineAsync(id);
            if (updatedFine == null)
            {
                return NotFound($"Fine with ID {id} was not found.");
            }
            return Ok(updatedFine);
        }
    }
}
