using Mapster;

namespace Fake.ObjectMapping.Mapster;

public class FakeMapsterOptions
{
    public TypeAdapterConfig TypeAdapterConfig { get; set; }

    public FakeMapsterOptions()
    {
        TypeAdapterConfig = new TypeAdapterConfig();
    }
}