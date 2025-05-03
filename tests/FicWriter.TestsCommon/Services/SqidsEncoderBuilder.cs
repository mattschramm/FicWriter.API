using Sqids;

namespace CommonTestUtils.Services;

public static class SqidsEncoderBuilder
{
    public static SqidsEncoder<long> Build() => new(new SqidsOptions()
    {
        Alphabet = "Qpbkl3Fu8UdPT4hsq9BRIZ0tN6ADJfW7yxEY2nioC1eMjLKgS5rOamXHcVvwzG",
        MinLength = 5
    });
}
