namespace Fake.IdGenerators.GuidGenerator;

public class FastGuidGenerator
{
    // Base32 encoding - in ascii sort order for easy text based sorting
    private static readonly char[] s_encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV".ToCharArray();
}