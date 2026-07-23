using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdMember = await _memberService.CreateMemberAsync(request);
            return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, createdMember);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MemberDto>> UpdateMember(int id, [FromBody] UpdateMemberDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedMember = await _memberService.UpdateMemberAsync(id, request);
            if (updatedMember == null)
            {
                return NotFound();
            }
            return Ok(updatedMember);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
