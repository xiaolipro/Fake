using Application.IntegrationEvents;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EntityFrameworkCore.IntegrationEventLog.Tests;

public class ApplicationEventHandlerTests
    : ApplicationTestWithTools<FakeEntityFrameworkCoreIntegrationEventLogTestModule>
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