namespace keepr_redux.Services;

public class KeepsService
{
  private readonly KeepsRepository _repo;

  public KeepsService(KeepsRepository repo)
  {
    _repo = repo;
  }

  internal Keep CreateKeep(KeepsRepository keepData)
  {
    Keep keep = _repo.CreateKeep(keepData);
    return keep;
  }
}