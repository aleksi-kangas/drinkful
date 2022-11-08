using Drinkful.Domain.Common;
using Drinkful.Domain.Common.Models;
using Drinkful.Domain.Like.ValueObjects;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.Like;

public class Like : AggregateRoot<LikeId> {
  public UserId UserId { get; private set; }
  public ILikeableId SubjectId { get; private set; }
  
  private Like(LikeId likeId, UserId userId, ILikeableId subjectId) : base(likeId) {
    UserId = userId;
    SubjectId = subjectId;
  }
  
  public static Like Create(UserId userId, ILikeableId subjectId) {
    return new Like(LikeId.CreateUnique(), userId, subjectId);
  }
}