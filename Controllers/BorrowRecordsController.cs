using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers

{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordService _borrowRecordService;

        public BorrowRecordsController(IBorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetAllBorrowRecords()
        {
            var records = await _borrowRecordService.GetAllBorrowRecordsAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecordDto>> GetBorrowRecord(int id)
        {
            var record = await _borrowRecordService.GetBorrowRecordByIdAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetBorrowRecordsByMember(int memberId)
        {
            var records = await _borrowRecordService.GetBorrowRecordsByMemberIdAsync(memberId);
            return Ok(records);
        }

        [HttpPost("borrow")]
        public async Task<ActionResult<BorrowRecordDto>> BorrowBook([FromBody] CreateBorrowRecordDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdRecord = await _borrowRecordService.BorrowBookAsync(request);
                return CreatedAtAction(nameof(GetBorrowRecord), new { id = createdRecord.Id }, createdRecord);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/return")]
        public async Task<ActionResult<BorrowRecordDto>> ReturnBook(int id)
        {
            var updatedRecord = await _borrowRecordService.ReturnBookAsync(id);
            if (updatedRecord == null)
            {
                return NotFound($"BorrowRecord with ID {id} was not found.");
            }
            return Ok(updatedRecord);
        }
    }
}
