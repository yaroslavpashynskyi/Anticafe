using AutoFixture;
using Blog.BLL.Services.Activitys;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace BLL.Tests;

public class DeleteTests
{
    private readonly IFixture _fixture;
    private readonly DataContext _dataContext;
    private readonly Delete.Handler _handler;

    public DeleteTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dataContext = Substitute.For<DataContext>();

        _handler = new Delete.Handler(_dataContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteActivity_WithSpecificId()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();
        var Activity = _fixture.Create<Activity>();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(Activity);
        _dataContext.CommitAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _dataContext.Received().CommitAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenActivityIsNotFound()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Activity>(null!));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFailToDeleteActivity()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();
        var Activity = _fixture.Create<Activity>();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(Activity);
        _dataContext.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _dataContext.Received().CommitAsync();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося видалити активність");
    }
}