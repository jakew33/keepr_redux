namespace keepr_redux.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeepsController : ControllerBase
{
  private readonly KeepsService _keepsService;
  private readonly Auth0Provider _auth;

  public KeepsController(KeepsService keepsService)
  {
    _keepsService = keepsService;
    _auth = auth;
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Keep>> CreateKeep([FromBody] Keep keepData)
  {
    try
    {
      Account userInfo = await _auth.GetUserInfoAsync<Accout>(HttpContext);
      keepData.CreatorId = userInfo.Id;

      Keep keep = _keepsService.CreateKeep(keepData);
      return new ActionResult<Keep>(Ok(keep));
    }
    catch (Exception e)
    {
      return new ActionResult<Keep>(BadRequest(e.Message));
    }
  }
}