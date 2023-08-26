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

  [HttpGet]
  public ActionResult<List<Keep>> GetAllKeeps()
  {
    try
    {
      List<Keep> keeps = _keepsService.GetAllKeeps();
      return ok(keeps);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{keepId}")]
  public async Task <ActionResult<Keep>> GetById(int keepId)
  {
    try 
    {
      Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);

      Keep keep = _keepsService.GetById(keepId, userInfo?.Id);
      return Ok(keep);
    }
    catch (exception e)
    {
      return BadRequest(e.Message);
    }
  }
}