using System.Text.Json.Serialization;

namespace FicWriter.API.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Orders
{
    LastUpdated = 0,
    LastCreated = 1,
    FirstUpdated = 2,
    FirstCreated = 3,
    Alphabetical = 4,
    AlphabeticalReverse = 5,
}
