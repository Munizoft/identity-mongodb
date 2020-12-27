using System;

namespace Munizoft.Identity.Entities
{
    public interface IDeletedAt
    {
        DateTime? DeletedAtUtc { get; set; }
        DateTime? DeletedAtLocal { get; }
    }
}