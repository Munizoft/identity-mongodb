using System;

namespace Munizoft.Identity.Entities
{
    public interface ICreatedAt
    {
        DateTime CreatedAtUtc { get; set; }
        DateTime CreatedAtLocal { get; }
    }
}