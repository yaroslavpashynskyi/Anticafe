using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.Services.Activitys;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using NSubstitute;

namespace BLL.Tests;
public class ListTests
{
    private readonly IFixture _fixture;
    private readonly DataContext _dataContext;
    private readonly MapperlyMapper _mapper;
    private readonly IUserAccessor _userAccessor;
    private readonly List.Handler _handler;

    public ListTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dataContext = Substitute.For<DataContext>();
        _mapper = new MapperlyMapper();
        _userAccessor = Substitute.For<IUserAccessor>();

        _handler = new List.Handler(_dataContext, _mapper, _userAccessor);
    }

    [Theory]
    [MemberData(nameof(GetParameters))]
    public async Task Handle_ShouldReturnSuccessResultWithListOfActivities_111(Guid rubricId, bool isAuthor, Guid authorId, string authorUserName)
    {
        // Arrange
        var query = Substitute.For<List.Query>();
        var ActivityParams = _fixture.Build<ActivityParams>()
            .With(x => x.PageSize, 1)
            .With(x => x.PageNumber, 1)
            .With(x => x.RubricId, rubricId)
            .With(x => x.IsAuthor, isAuthor)
            .Create();
        query.ActivityParams = ActivityParams;

        var Activity = _fixture.Build<Activity>()
            .With(x => x.Author, _fixture.Build<User>()
                .With(x => x.Id, authorId.ToString)
                .With(x => x.UserName, authorUserName)
                .Create())
            .With(x => x.Rubric, _fixture.Build<Rubric>()
                .With(x => x.Id, rubricId)
                .Create())
            .Without(x => x.Comments)
            .Create();
        var Activitys = Substitute.For<PagedList<Activity>>(new[] { Activity }, 1, ActivityParams.PageNumber, ActivityParams.PageSize);

        _dataContext.GetRepository<Activity>().GetQueryable().Returns(new[] { Activity }.AsAsyncQueryable());

        var ActivityDto = _mapper.Map(Activity);
        var ActivitysDto = Activitys.Map(_ => ActivityDto);

        _userAccessor.GetUsername().Returns(authorUserName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(ActivitysDto);
    }

    public static IEnumerable<object[]> GetParameters()
    {
        yield return new object[] { Guid.Empty, false, Guid.Empty, string.Empty};
        yield return new object[] { Guid.NewGuid(), false, Guid.Empty, string.Empty};
        yield return new object[] { Guid.Empty, true, Guid.NewGuid(), "UserName"};
        yield return new object[] { Guid.NewGuid(), true, Guid.NewGuid(), "UserName"};
    }
}