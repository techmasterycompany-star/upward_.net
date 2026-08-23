using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Candidate;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers;

[ApiController]
[Route("api/candidate-profiles")]
[Authorize(Roles = "Candidate")]
public class CandidateProfileController : ControllerBase
{
    private readonly ICandidateProfileService _candidateProfileService;
    private readonly ISkillsService _skillsService;

    public CandidateProfileController(
        ICandidateProfileService candidateProfileService,
        ISkillsService skillsService)
    {
        _candidateProfileService = candidateProfileService;
        _skillsService = skillsService;
    }

    [HttpGet("{userId:long}")]
    public async Task<ActionResult<CandidateProfileDto>> GetByUserId(long userId)
    {
        var profile = await _candidateProfileService.GetByUserIdAsync(userId);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost("{userId:long}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CandidateProfileDto>> Create(long userId, [FromForm] UpdateCandidateProfileDto request)
    {
        try
        {
            var profile = await _candidateProfileService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetByUserId), new { userId }, profile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{userId:long}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CandidateProfileDto>> Update(long userId, [FromForm] UpdateCandidateProfileDto request)
    {
        try
        {
            var profile = await _candidateProfileService.UpdateAsync(userId, request);
            return profile is null ? NotFound() : Ok("Profile Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{userId:long}/skills")]
    public async Task<ActionResult<CandidateProfileDto>> AddSkill(long userId, [FromBody] CandidateSkillInputDto request)
    {
        try
        {
            var profile = await _skillsService.AddSkillAsync(userId, request);
            return profile is null ? NotFound() : Ok("Skill Added Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{userId:long}/skills/{candidateSkillId:long}")]
    public async Task<ActionResult<CandidateProfileDto>> EditSkill(long userId, long candidateSkillId, [FromBody] CandidateSkillInputDto request)
    {
        try
        {
            var profile = await _skillsService.UpdateSkillAsync(userId, candidateSkillId, request);
            return profile is null ? NotFound() : Ok("Skill Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{userId:long}/skills/{candidateSkillId:long}")]
    public async Task<ActionResult<CandidateProfileDto>> RemoveSkill(long userId, long candidateSkillId)
    {
        var profile = await _skillsService.RemoveSkillAsync(userId, candidateSkillId);
        return profile is null ? NotFound() : Ok("Skill Removed Successfully");
    }

    [HttpPost("{userId:long}/skills/bulk")]
    public async Task<ActionResult<CandidateProfileDto>> AddSkillsBulk(long userId, [FromBody] UpdateCandidateSkillsDto request)
    {
        var profile = await _skillsService.UpdateSkillsAsync(userId, request);
        return profile is null ? NotFound() : Ok("Skills Added Successfully");
    }
}
