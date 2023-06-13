using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.Services.Activitys;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace BLL.Tests;

public class CreateTests
{
    private readonly IFixture _fixture;
    private readonly DataContext _dataContext;
    private readonly IUserAccessor _userAccessor;
    private readonly UserManager<User> _userManager;
    private readonly Create.Handler _handler;

    public CreateTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dataContext = Substitute.For<DataContext>();
        var mapper = new MapperlyMapper();
        _userAccessor = Substitute.For<IUserAccessor>();
        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);

        _handler = new Create.Handler(_dataContext, mapper, _userAccessor, _userManager);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCreateActivity()
    {
        // Arrange
        var command = _fixture.Create<Create.Command>();
        var user = _fixture.Create<User>();
        var rubric = _fixture.Create<Rubric>();

        _dataContext.GetRepository<Rubric>().GetByIdAsync(Arg.Any<Guid>()).Returns(rubric);
        _userManager.Users.Returns(new[] { user }.AsQueryable());
        _userAccessor.GetUsername().Returns(user.UserName);
        _dataContext.CommitAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dataContext.GetRepository<Activity>().Received(1).Add(Arg.Any<Activity>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFailToCreateActivity()
    {
        // Arrange
        var command = _fixture.Create<Create.Command>();

        _dataContext.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _dataContext.Received().CommitAsync();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося створити статтю");
    }
}