using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Taxes.Application.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        private Mock<IValidator<MyRequest>> _mockValidator = default!;
        private ValidationBehavior<MyRequest, ErrorOr<MyResponse>> _validationBehavior = default!;

        [SetUp]
        public void Setup()
        {
            _mockValidator = new Mock<IValidator<MyRequest>>();
            _validationBehavior = new ValidationBehavior<MyRequest, ErrorOr<MyResponse>>(new List<IValidator<MyRequest>>
                { _mockValidator.Object });
        }

        [Test]
        public async Task Handle_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
        {
            _mockValidator
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());
            var nextDelegate = new Mock<RequestHandlerDelegate<ErrorOr<MyResponse>>>();
            nextDelegate.Setup(nd => nd.Invoke())
                .ReturnsAsync(new ErrorOr<MyResponse>())
                .Verifiable();
            var request = new MyRequest();

            var result = await _validationBehavior.Handle(request, nextDelegate.Object, It.IsAny<CancellationToken>());


            result.IsError.ShouldBeFalse();
            nextDelegate.VerifyAll();
        }

        [Test]
        public async Task Handle_WhenValidatorResultIsNotValid_ShouldReturnListOfErrors()
        {
            var failures = new List<ValidationFailure> { new("test", "bad test") };
            _mockValidator
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(failures));
            var nextDelegate = new Mock<RequestHandlerDelegate<ErrorOr<MyResponse>>>();
            nextDelegate.Setup(nd => nd.Invoke())
                .ReturnsAsync(new ErrorOr<MyResponse>())
                .Verifiable();
            var request = new MyRequest();

            var result = await _validationBehavior.Handle(request, nextDelegate.Object, It.IsAny<CancellationToken>());


            result.IsError.ShouldBeTrue();
            result.Errors[0].Code.ShouldBeEquivalentTo("test");
            result.Errors[0].Description.ShouldBeEquivalentTo("bad test");
            nextDelegate.Verify(x => x.Invoke(), Times.Never);
        }
    }
}