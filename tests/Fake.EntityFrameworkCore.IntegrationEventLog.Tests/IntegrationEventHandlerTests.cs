using Application.IntegrationEvents;
using Fake;
using Fake.EntityFrameworkCore.IntegrationEventLog;
using Fake.Testing;

public class
    IntegrationEventHandlerTests : FakeIntegrationTestWithTools<FakeEntityFrameworkCoreIntegrationEventLogTestModule>
{
    private readonly IIntegrationEventLogService _integrationEventLogService;

    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    public IntegrationEventHandlerTests()
    {
        _integrationEventLogService = GetRequiredService<IIntegrationEventLogService>();
    }

    [Fact]
    void 发布集成日志()
    {
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(AppTestDataBuilder.UserId);
        _integrationEventLogService.SaveEventAsync(orderStartedIntegrationEvent);
    }
}