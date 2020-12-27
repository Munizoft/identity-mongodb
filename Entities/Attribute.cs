using System;

namespace Munizoft.Identity.Entities
{
    public class Attribute
    {
        public Guid Id { get; set; }

        public String Key { get; set; }
        public String Type { get; set; }
        public String Value { get; set; }

        public Attribute(String key, String value)
            : this(key, String.Empty, value)
        {

        }

        public Attribute(String key, String type, String value)
        {
            this.Id = Guid.NewGuid();
            Key = key;
            Type = type;
            Value = value;
        }
    }
}