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

  internal List<Keep> GetAllKeeps()
  {
    string sql = @"
    SELECT
    k.*,
    creator.*
    FROM keeps k
    JOIN accounts creator ON k.creatorId = creator.id
    ;";
    List<Keep> keeps = _db.Query<Keep, Account, Keep>(sql, (keep, creator) =>
    {
    return keep;
    }).ToList();
    return keeps;
  }

  internal Keep GetById(int keepId)
  {
    string sql = @"
    SELECT
    k.*,
    creator.*
    FROM keeps k 
    JOIN accounts creator ON k.creator.id
    WHERE k.id = @keepId
    ;";

    Keep keep = _db.Query<Keep, Account, Keep>(sql, (keep, creator) =>
    {
      keep.Creator = creator;
      return keep;
    }, new { keepId}).FirstOrDefault();
    return keep;
  }

  internal void EditKeep(Keep original)
  {
    string sql = @"
    UPDATE keeps
    SET
    name = @name,
    creatorId = @creatorId,
    description = @description,
    img = @img,
    views = @views,
    kept = @Kept
    WHERE id = @id
    ;";

    _db.Execute(sql, original);
  }

  internal int DeleteKeep(int keepId)
  {
    string sql = @"
    DELETE FROM keeps
    WHERE id = @keepId LIMIT 1
    ;";

    int rows = _db.Execute(sql, new {keepId});
    return rows;
  }
}