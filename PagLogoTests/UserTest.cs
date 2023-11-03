using Microsoft.EntityFrameworkCore;
using Moq;
using PagLogo;
using PagLogo.Exceptions;
using PagLogo.Models;
using PagLogo.Services;
using System.Reflection.Metadata;

namespace PagLogoTests
{
    public class UserTest
    {
        [Fact]
        public async Task Test_When_Return_Success()
        {

            var data = new List<Tradesman>
            {
                new Tradesman{
                    Id = 1,
                    Cnpj = "123456/4000",
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Tradesman>>();
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Tradesmans).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            string identifier = "123456/4000";
            var result = await service.GetUserAsync(identifier);

            Assert.Equal("Paulo Rocha", result.Name);
        }

        [Fact]
        public async Task Test_When_User_Not_Found()
        {

            var data = new List<Tradesman>
            {
                new Tradesman{
                    Id = 1,
                    Cnpj = "123456/4000",
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Tradesman>>();
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tradesman>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Tradesmans).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            string identifier = "123456/1000";
            var result = async () => await service.GetUserAsync(identifier);

            Assert.Equal(new UserException("Não encontrado").Message, 
                Assert.ThrowsAsync<UserException>(result).Result.Message);
        }
    }
}