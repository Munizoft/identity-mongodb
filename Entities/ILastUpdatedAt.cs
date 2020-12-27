using System;

namespace Munizoft.Identity.Entities
{
    public interface ILastUpdatedAt
    {
        DateTime LastUpdatedAtUtc { get; set; }
        DateTime LastUpdatedAtLocal { get; }
    }
}
