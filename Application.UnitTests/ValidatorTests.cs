namespace Application.UnitTests
{
    public class ValidatorTests
    {
        [Fact]
        public async Task RegisterUserValidator_UsernameAlreadyExists_ValidationFails()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new RegisterUserDto() { Username = username, Password = password };
            var command = new RegisterUserCommand(dto);
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

            var registerUserValidator = new RegisterUserValidator(userRepository.Object);

            // Act
            var result = await registerUserValidator.ValidateAsync(command);

            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task RegisterUserValidator_UsernameDoesNotExistYet_ValidationSucceeds()
        {
            string username = "second-user";
            string password = "P@ssw0rd";
            var dto = new RegisterUserDto() { Username = username, Password = password };
            var command = new RegisterUserCommand(dto);
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

            var registerUserValidator = new RegisterUserValidator(userRepository.Object);

            // Act
            var result = await registerUserValidator.ValidateAsync(command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
