using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.API.Helpers;
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

    [HttpGet]
    public async Task<ActionResult<CandidateProfileDto>> GetMyProfile()
    {
        var userId = ClaimsHelper.GetUserId(User);

        var profile = await _candidateProfileService.GetByUserIdAsync(userId);

        return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok(profile);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CandidateProfileDto>> CreateNewProfile(
        [FromForm] UpdateCandidateProfileDto request)
    {
        var userId = ClaimsHelper.GetUserId(User);

        try
        {
            var profile = await _candidateProfileService.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetMyProfile), profile);
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

    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CandidateProfileDto>> UpdateMyProfile(
        [FromForm] UpdateCandidateProfileDto request)
    {
        var userId = ClaimsHelper.GetUserId(User);

        try
        {
            var profile = await _candidateProfileService.UpdateAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Profile Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("skills")]
    public async Task<ActionResult<CandidateProfileDto>> AddSkill(
        [FromBody] CandidateSkillInputDto request)
    {
        var userId = ClaimsHelper.GetUserId(User);

        try
        {
            var profile = await _skillsService.AddSkillAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Skill Added Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("skills/{candidateSkillId:long}")]
    public async Task<ActionResult<CandidateProfileDto>> EditSkill(
        long candidateSkillId,
        [FromBody] CandidateSkillInputDto request)
    {
        var userId = ClaimsHelper.GetUserId(User);

        try
        {
            var profile = await _skillsService.UpdateSkillAsync(
                userId,
                candidateSkillId,
                request);

            return profile is null? NotFound(new { message = "Candidate profile or skill not found." }) : Ok("Skill Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("skills/{candidateSkillId:long}")]
    public async Task<ActionResult<CandidateProfileDto>> RemoveSkill(
        long candidateSkillId)
    {
        var userId = ClaimsHelper.GetUserId(User);

        var profile = await _skillsService.RemoveSkillAsync(
            userId,
            candidateSkillId);

        return profile is null? NotFound(new { message = "Candidate profile or skill not found." }) : Ok("Skill Removed Successfully");
    }

    [HttpPost("skills/bulk")]
    public async Task<ActionResult<CandidateProfileDto>> AddSkillsBulk(
        [FromBody] UpdateCandidateSkillsDto request)
    {
        var userId = ClaimsHelper.GetUserId(User);

        try
        {
            var profile = await _skillsService.UpdateSkillsAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Skills Added Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
