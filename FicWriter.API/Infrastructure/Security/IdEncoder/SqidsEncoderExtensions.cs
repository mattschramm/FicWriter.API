using FicWriter.API.Infrastructure.Errors.Exceptions;
using Sqids;

namespace FicWriter.API.Infrastructure.Security.IdEncoder;

public static class SqidsEncoderExtensions
{
    public static long DecodeSingle(this SqidsEncoder<long> encoder, string encryptedId)
    {
        long id;

        try
        {
            id = encoder.Decode(encryptedId).Single();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidWorkIdException();
        }

        return id;
    }
}
