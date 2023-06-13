using AutoFixture;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Activitys;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace BLL.Tests;

public class EditTests
{
    private readonly IFixture _fixture;
    private readonly DataContext _dataContext;
    private readonly MapperlyMapper _mapper;
    private readonly Edit.Handler _handler;

    public EditTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dataContext = Substitute.For<DataContext>();
        _mapper = new MapperlyMapper();

        _handler = new Edit.Handler(_dataContext, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldEditActivity_WhenNewActivityIdIsMatchingOldActivityId()
    {
        // Arrange
        var ActivityId = Guid.NewGuid();
        var editedActivity = _fixture.Build<ActivityDto>()
            .With(x => x.Id, ActivityId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create(); 
        var oldActivity = _fixture.Build<Activity>()
            .With(x => x.Id, ActivityId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Activity, editedActivity)
            .Create();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(oldActivity);
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
        var editedActivity = _fixture.Create<ActivityDto>();
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Activity, editedActivity)
            .Create();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Activity>(null!));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTryingToUpdateActivityWithNoChanges()
    {
        // Arrange
        var Activity = _fixture.Build<Activity>()
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();
        var unchangedActivity = _mapper.Map(Activity);
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Activity, unchangedActivity)
            .Create();

        _dataContext.GetRepository<Activity>().GetByIdAsync(Arg.Any<Guid>()).Returns(Activity);
        _dataContext.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося оновити активність");
    }
}