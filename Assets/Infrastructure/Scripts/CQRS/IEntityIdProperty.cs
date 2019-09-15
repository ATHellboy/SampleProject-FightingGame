using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS
{
    public interface IEntityIdProperty
    {
        string EntityId { get; }
    }
}