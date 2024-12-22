namespace Application.UnitTests
{
    public class CommandHandlerTests
    {
        [Fact]
        public async Task AddFlightCommandHandler_ShouldAddFlightAndSaveResult()
        {
            var dto = new AddFlightDto() { Origin = "test1", Destination = "test2", Arrival = new DateTimeOffset(), Departure = new DateTimeOffset(), Status = FlightStatus.InTime };
            var command = new AddFlightCommand(dto);

            var mockRepository = new Mock<IFlightsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockRepository.Setup(x => x.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(dto.ToFlight()));

            var commandHandler = new AddFlightCommandHandler(mockRepository.Object, mockUnitOfWork.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            mockRepository.Verify(x => x.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeFlightStatus_FlightExists_UpdatesFlightStatusAndSavesChanges()
        {
            var dto = new ChangeFlightStatusDto() { Status = FlightStatus.Delayed };
            var id = 1;
            var command = new ChangeFlightStatusCommand(id, dto);
            Flight? flight = new Flight() { Id = 1, Origin = "test1", Destination = "test2", Arrival = new DateTimeOffset(), Departure = new DateTimeOffset(), Status = FlightStatus.InTime };

            var mockRepository = new Mock<IFlightsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockRepository.Setup(x => x.GetByIdAsync(id, CancellationToken.None)).Returns(Task.FromResult<Flight?>(flight));
            mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Flight>(), CancellationToken.None)).Returns(Task.FromResult(flight));

            var commandHandler = new ChangeFlightStatusCommandHandler(mockRepository.Object, mockUnitOfWork.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            mockRepository.Verify(x => x.UpdateAsync(It.Is<Flight>(f => f.Status == dto.Status), It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeFlightStatus_FlightDoesNotExist_DoNotUpdateFlightAndReturnErrorResult()
        {
            var dto = new ChangeFlightStatusDto() { Status = FlightStatus.Delayed };
            var id = 1;
            var command = new ChangeFlightStatusCommand(id, dto);
            Flight? flight = new Flight() { Id = 1, Origin = "test1", Destination = "test2", Arrival = new DateTimeOffset(), Departure = new DateTimeOffset(), Status = FlightStatus.InTime };

            var mockRepository = new Mock<IFlightsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockRepository.Setup(x => x.GetByIdAsync(id, CancellationToken.None)).Returns(Task.FromResult<Flight?>(null));

            var commandHandler = new ChangeFlightStatusCommandHandler(mockRepository.Object, mockUnitOfWork.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Never);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUser_FirstUserIsModerator()
        {
            string username = "first-user";
            string password = "P@ssw0rd";
            var dto = new RegisterUserDto() { Username = username, Password = password };
            var command = new RegisterUserCommand(dto);
            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockRoleRepository = new Mock<IRoleRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var passwordHashEvaluator = new Mock<IPasswordHashEvaluator>();

            mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None)).Returns(Task.FromResult(false));
            mockRoleRepository
                .Setup(x => x.GetAll(CancellationToken.None))
                .Returns(Task.FromResult(new Role[]
                {
                    new Role() { Id = 1, Code = RoleNames.Moderator },
                    new Role() { Id = 2, Code = RoleNames.User }
                }.AsEnumerable()));

            var commandHandler = new RegisterUserCommandHandler(mockUserRepository.Object, mockRoleRepository.Object, mockUnitOfWork.Object, passwordHashEvaluator.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.RoleId == 1), CancellationToken.None), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_StartingFromSecondUsersHaveUserRole()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new RegisterUserDto() { Username = username, Password = password };
            var command = new RegisterUserCommand(dto);

            var mockUserRepository = new Mock<IUserRepository>();
            var mockRoleRepository = new Mock<IRoleRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var passwordHashEvaluator = new Mock<IPasswordHashEvaluator>();

            mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None)).Returns(Task.FromResult(true));
            mockRoleRepository
                .Setup(x => x.GetAll(CancellationToken.None))
                .Returns(Task.FromResult(new Role[]
                {
                    new Role() { Id = 1, Code = RoleNames.Moderator },
                    new Role() { Id = 2, Code = RoleNames.User }
                }.AsEnumerable()));

            var commandHandler = new RegisterUserCommandHandler(mockUserRepository.Object, mockRoleRepository.Object, mockUnitOfWork.Object, passwordHashEvaluator.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.RoleId == 2), CancellationToken.None), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AuthorizeUser_UserNotFound_Fails()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new AuthorizeUserDto() { Username = username, Password = password };
            var command = new AuthorizeUserQuery(dto);

            var mockUserRepository = new Mock<IUserRepository>();
            var accessTokenGenerator = new Mock<ITokenGenerator>();
            var passwordHashEvaluator = new Mock<IPasswordHashEvaluator>();

            mockUserRepository.Setup(x => x.GetUserWithRole(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<User?>(null));

            var commandHandler = new AuthorizeUserQueryHandler(mockUserRepository.Object, passwordHashEvaluator.Object, accessTokenGenerator.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            accessTokenGenerator.Verify(x => x.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AuthorizeUser_PasswordDoesNotMatch_Fails()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new AuthorizeUserDto() { Username = username, Password = password };
            var command = new AuthorizeUserQuery(dto);

            var mockUserRepository = new Mock<IUserRepository>();
            var accessTokenGenerator = new Mock<ITokenGenerator>();
            var passwordHashEvaluator = new Mock<IPasswordHashEvaluator>();

            mockUserRepository.Setup(x => x.GetUserWithRole(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<User?>(new User () { Username = username, Password = "anotherPassword", Salt = "some salt"}));
            passwordHashEvaluator.Setup(x => x.PasswordMatch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var commandHandler = new AuthorizeUserQueryHandler(mockUserRepository.Object, passwordHashEvaluator.Object, accessTokenGenerator.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            accessTokenGenerator.Verify(x => x.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task AuthorizeUser_PasswordMatches_ReturnsAccessToken()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new AuthorizeUserDto() { Username = username, Password = password };
            var command = new AuthorizeUserQuery(dto);
            var accessToken = "accessToken";

            var mockUserRepository = new Mock<IUserRepository>();
            var accessTokenGenerator = new Mock<ITokenGenerator>();
            var passwordHashEvaluator = new Mock<IPasswordHashEvaluator>();

            mockUserRepository.Setup(x => x.GetUserWithRole(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<User?>(new User() { Username = username, Password = password, Salt = "some salt", Role = new Role() { Code = RoleNames.User } }));
            passwordHashEvaluator.Setup(x => x.PasswordMatch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            accessTokenGenerator.Setup(x => x.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>())).Returns(accessToken);

            var commandHandler = new AuthorizeUserQueryHandler(mockUserRepository.Object, passwordHashEvaluator.Object, accessTokenGenerator.Object);

            // Action
            var result = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Equal(result.Data!.AccessToken, accessToken);
            accessTokenGenerator.Verify(x => x.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}