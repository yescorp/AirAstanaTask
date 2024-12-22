using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    public class PasswordHashEvaluatorTests
    {
        [Fact]
        public void PasswordHashEvaluatorTest()
        {
            var sut = new PasswordHashEvaluator();
            var password = "P@ssw0rd";

            var (hash, salt) = sut.HashPassword(password);

            var isMatch = sut.PasswordMatch(password, salt, hash);

            Assert.True(isMatch);
        }
    }
}
