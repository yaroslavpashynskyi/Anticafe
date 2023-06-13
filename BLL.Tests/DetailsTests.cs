using AutoFixture;
using Blog.BLL.Core;
using Blog.BLL.Services.Activitys;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using NSubstitute;

namespace BLL.Tests;

public class DetailsTests
{
    private readonly IFixture _fixture;
    private readonly DataContext _dataContext;
    private readonly MapperlyMapper _mapper;
    private readonly Details.Handler _handler;

    public DetailsTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dataContext = Substitute.For<DataContext>();
        _mapper = new MapperlyMapper();

        _handler = new Details.Handler(_dataContext, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnActivity_WithSpecificId()
    {
        // Arrange
        var ActivityId = Guid.NewGuid();
        var query = _fixture.Build<Details.Query>()
            .With(x => x.Id, ActivityId)
            .Create();
        var Activity = _fixture.Build<Activity>()
            .With(x => x.Id, ActivityId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();

        _dataContext.GetRepository<Activity>().GetQueryable().Returns(new[] { Activity }.AsAsyncQueryable());

        var ActivityDto = _mapper.Map(Activity);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(ActivityDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenActivityIsNotFound()
    {
        // Arrange
        var query = _fixture.Create<Details.Query>();
        var Activity = _fixture.Create<Activity>();

        _dataContext.GetRepository<Activity>().GetQueryable().Returns(new[] { Activity }.AsAsyncQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}