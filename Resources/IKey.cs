using Newtonsoft.Json;

namespace Munizoft.Identity.Resources
{
    public interface IKey<TKey>
    {
        [JsonProperty("id")]
        TKey Id { get; set; }
    }
}