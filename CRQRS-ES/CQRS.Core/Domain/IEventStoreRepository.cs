using CQRS.Core.Events;

namespace CQRS.Core.Domain;

/// <summary>
/// Should only be able to create and retrieve, not update or delete.
/// </summary>
public interface IEventStoreRepository
{
    Task SaveAsync(EventModel @event);
    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
    Task<List<EventModel>> FindAllAsync();
}