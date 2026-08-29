using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.API.Helpers;
using Upwork.Application.DTOs.Candidate;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers;

[ApiController]
[Route("api/candidate/profile")]
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
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(User);

            var profile = await _candidateProfileService.GetByUserIdAsync(userId);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok(profile);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateNewProfile([FromForm] UpdateCandidateProfileDto request)
    {

        try
        {
            var userId = ClaimsHelper.GetUserId(User);
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
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateCandidateProfileDto request)
    {

        try
        {
            var userId = ClaimsHelper.GetUserId(User);
            var profile = await _candidateProfileService.UpdateAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Profile Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPut("resume")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadResume([FromForm] UploadResumeRequest request)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(User);

            var profile = await _candidateProfileService.UploadResumeAsync(userId, request.File);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok(profile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost("skills")]
    public async Task<IActionResult> AddSkill(
        [FromBody] CandidateSkillInputDto request)
    {

        try
        {
            var userId = ClaimsHelper.GetUserId(User);
            var profile = await _skillsService.AddSkillAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Skill Added Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPut("skills/{candidateSkillId:long}")]
    public async Task<IActionResult> EditSkill(long candidateSkillId, [FromBody] CandidateSkillInputDto request)
    {

        try
        {
            var userId = ClaimsHelper.GetUserId(User);
            var profile = await _skillsService.UpdateSkillAsync(userId, candidateSkillId, request);

            return profile is null? NotFound(new { message = "Candidate profile or skill not found." }) : Ok("Skill Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpDelete("skills/{candidateSkillId:long}")]
    public async Task<IActionResult> RemoveSkill(long candidateSkillId)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(User);

            var profile = await _skillsService.RemoveSkillAsync(userId, candidateSkillId);

            return profile is null? NotFound(new { message = "Candidate profile or skill not found." }) : Ok("Skill Removed Successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPut("skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateCandidateSkillsDto request)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(User);
            var profile = await _skillsService.UpdateSkillsAsync(userId, request);

            return profile is null? NotFound(new { message = "Candidate profile not found." }) : Ok("Skills Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}

