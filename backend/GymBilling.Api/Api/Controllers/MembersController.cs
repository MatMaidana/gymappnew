using GymBilling.Api.Api.Contracts;
using GymBilling.Api.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBilling.Api.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController(MemberService memberService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberResponse>>> List([FromQuery] string? filter, [FromQuery] int dueSoonDays = 7, CancellationToken cancellationToken = default)
    {
        var result = await memberService.ListAsync(filter, dueSoonDays, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var member = await memberService.GetAsync(id, cancellationToken);
        return member is null ? NotFound() : Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponse>> Create([FromBody] CreateMemberRequest request, CancellationToken cancellationToken)
    {
        var member = await memberService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = member.Id }, member);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> Update(Guid id, [FromBody] UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        var updated = await memberService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await memberService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/payments")]
    public async Task<ActionResult<PaymentResponse>> MarkPayment(Guid id, [FromBody] MarkPaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await memberService.MarkPaymentAsync(id, request, cancellationToken);
        return payment is null ? NotFound() : Ok(payment);
    }
}
