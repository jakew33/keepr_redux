namespace keepr_redux.Repositories;

public class KeepsRepository
{
  private readonly IDbConnection _db;

  public KeepsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Keep CreateKeep(Keep keepData)
  {
    string sql = @"
    INSERT INTO keeps
    (CreatorId, Name, Description, Img, Views, Kept)
    VALUES
    (@creatorId, @name, @description, @img, @views, @kept);

    SELECT
    k.*
    creator.*
    FROM keeps k
    JOIN accounts creator ON k.creatorId = creator.id
    WHERE k.id - LAST_INSERT_ID()
    ;";

    Keep keep = _db.Query<Keep, Account, Keep>(sql, (keep, creator) =>
    {
      keep.Creator = creator;
      return keep;
    }, keepData).FirstOrDefault();
    return keep;
  }
}