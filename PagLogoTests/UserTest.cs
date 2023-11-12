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

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = "123456/4000",
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            string identifier = "123456/4000";
            var result = await service.GetUserAsync(identifier);

            Assert.Equal("Paulo Rocha", result.Name);
        }

        [Fact]
        public async Task Test_When_User_Not_Found()
        {

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = "123456/4000",
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            string identifier = "123456/1000";
            var result = async () => await service.GetUserAsync(identifier);

            Assert.Equal(new UserException("Usuário não encontrado.").Message, 
                Assert.ThrowsAsync<UserException>(result).Result.Message);
        }

        [Fact]
        public async Task Test_When_Save_Success()
        {

            string identifier = "123456/4000";
            var userNew = new User
            {
                Identifier = identifier,
                Name = "Rocha",
                Email = "rocha@gmail.com",
                Balance = 1300.45
            };

            var mockSet = new Mock<DbSet<User>>();
            var data = new List<User> { 
                new User{}
            }.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            await service.SaveUserAsync(userNew);

            mockContext.Verify(c => c.Users.Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Test_Save_When_Identifier_Exists()
        {
            string identifier = "123456/4000";
            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var userNew = new User
            {
                Identifier = identifier,
                Name = "Rocha",
                Email = "rocha@gmail.com",
                Balance = 1300.45
            };

            var mockSet = new Mock<DbSet<User>>();
            
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            var result = async() => await service.SaveUserAsync(userNew);

            Assert.Equal(new UserException("Usuário ou email já cadastrados").Message,
                Assert.ThrowsAsync<UserException>(result).Result.Message);
        }

        [Fact]
        public async Task Test_Save_When_Email_Exists()
        {
            string identifier = "123456/4000";
            string newIdentifier = "777666/1000";
            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var userNew = new User
            {
                Identifier = newIdentifier,
                Name = "Rocha",
                Email = "paulo@gmail.com",
                Balance = 1300.45
            };

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            var result = async () => await service.SaveUserAsync(userNew);

            Assert.Equal(new UserException("Usuário ou email já cadastrados").Message,
                Assert.ThrowsAsync<UserException>(result).Result.Message);
        }

        [Fact]
        public async Task Test_When_Update_Success()
        {

            string identifier = "123456/4000";

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var userNew = new User
            {
                Identifier = identifier,
                Name = "Paulo Rocha",
                Email = "paulo@gmail.com",
                Balance = 300.0
            };

            var mockSet = new Mock<DbSet<User>>();
            
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            await service.UpdateUserAsync(userNew);

            mockContext.Verify(c => c.Users.Update(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Test_Update_When_User_Not_Exists()
        {

            string identifier = "123456/4000";

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var userNew = new User
            {
                Identifier = identifier + "23",
                Name = "Paulo Rocha",
                Email = "paulo@gmail.com",
                Balance = 300.0
            };

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            var result = async() => await service.UpdateUserAsync(userNew);

            Assert.Equal(new UserException("Usuário não encontrado.").Message,
               Assert.ThrowsAsync<UserException>(result).Result.Message);
        }

        [Fact]
        public async Task Test_Update_When_Email_Exists()
        {

            string identifier = "123456/4000";
            string identifier2 = "777666/4000";

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                },
                new User
                {
                    Id = 10,
                    Identifier = identifier2,
                    Name = "Henrique Rocha",
                    Email = "henrique@gmail.com",
                    Balance = 6700.0
                }
            }.AsQueryable();

            var userNew = new User
            {
                Identifier = identifier2,
                Name = "Paulo Rocha",
                Email = "paulo@gmail.com",
                Balance = 300.0
            };

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            var result = async () => await service.UpdateUserAsync(userNew);

            Assert.Equal(new UserException("Usuário ou email já cadastrados.").Message,
               Assert.ThrowsAsync<UserException>(result).Result.Message);
        }

        [Fact]
        public async Task Test_When_Delete_Success()
        {

            string identifier = "123456/4000";

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            await service.DeleteUserAsync(identifier);

            mockContext.Verify(c => c.Users.Remove(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Test_Delete_When_User_Not_Exists()
        {

            string identifier = "123456/4000";

            var data = new List<User>
            {
                new User{
                    Id = 1,
                    Identifier = identifier,
                    Name = "Paulo Rocha",
                    Email = "paulo@gmail.com",
                    Balance = 200.0
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            string userIdentifierShouldDelete = "111111/9000";
            var result = async() => await service.DeleteUserAsync(userIdentifierShouldDelete);

            Assert.Equal(new UserException("Usuário não encontrado.").Message,
               Assert.ThrowsAsync<UserException>(result).Result.Message);
        }
    }
}