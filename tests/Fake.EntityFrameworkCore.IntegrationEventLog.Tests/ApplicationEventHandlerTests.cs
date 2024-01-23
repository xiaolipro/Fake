using Application.IntegrationEvents;
using Fake;
using Fake.EntityFrameworkCore.IntegrationEventLog;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

public class
    ApplicationEventHandlerTests : ApplicationTestWithTools<FakeEntityFrameworkCoreIntegrationEventLogTestModule>
{
    private readonly IIntegrationEventLogService _integrationEventLogService;

    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    public ApplicationEventHandlerTests()
    {
        _integrationEventLogService = ServiceProvider.GetRequiredService<IIntegrationEventLogService>();
    }

    [Fact]
    void 发布集成日志()
    {
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(AppTestDataBuilder.UserId);
        _integrationEventLogService.SaveEventAsync(orderStartedIntegrationEvent);
    }
}