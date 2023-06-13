using BLL.Services;
using DAL;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;

namespace TestProject1
{
	public class Tests
	{
		private readonly DataContext _dataContext;
		private Mock<BLL.Services.Activities.List> mock;

		public Tests()
		{
			var options = new DbContextOptionsBuilder<DataContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;

			_dataContext = new DataContext(options);

			mock = new Mock<IMediator>(_dataContext);
		}
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void AllAdsTest()
		{
			Activity[] allAds = new Activity[1];
			Activity u = new Activity();
			mock.Setup(a => a.);
			mock.Setup(a => a.AllAds()).Returns(allAds);

			Assert.That(mock.Object.AllAds(), Is.Not.Null);
		}
	}
}