using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using OpenEntity.Validation;

namespace OpenEntity.Specs.Validation
{
    [TestFixture]
    public class DataAnnotationsValidationRunnerTests
    {
        private const string requiredErrorMessage = "This property Is Required!";
        private const string rangeErrorMessage = "The value of this property is not within the specified range.";
        private const string stringLengthErrorMessage = "The string is too long.";
        private const string pastErrorMessage = "The date must be in the past.";

        [Test]
        public void Validation_Should_Find_All_Error_Messages()
        {
            var badModel = new ValidationModel()
            {
                RangeProperty = 0,
                StringLengthProperty = "ThisStringIsTooLong",
            };
            var errors = DataAnnotationsValidationRunner.GetErrors(badModel);

            Assert.AreEqual(3, errors.Count);
            Assert.That(errors.Any(x => x.ErrorMessage.Equals(requiredErrorMessage)), "Missing requiredErrorMessage");
            Assert.That(errors.Any(x => x.ErrorMessage.Equals(rangeErrorMessage)), "Missing rangeErrorMessage");
            Assert.That(errors.Any(x => x.ErrorMessage.Equals(stringLengthErrorMessage)), "Missing stringLengthErrorMessage");
        }

        [Test]
        public void Validation_Should_Find_No_Error_Messages()
        {
            var goodModel = new ValidationModel()
            {
                RequiredProperty = DateTime.MinValue,
                RangeProperty = 5,
                StringLengthProperty = "0123456789",
            };
            var errors = DataAnnotationsValidationRunner.GetErrors(goodModel);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void Validation_Should_Work_With_Custom_ValidationAttribute()
        {
            var badModel = new ValidationModel()
            {
                RequiredProperty = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)),
                RangeProperty = 5,
                StringLengthProperty = "0123456789",
            };

            var errors = DataAnnotationsValidationRunner.GetErrors(badModel);

            Assert.AreEqual(1, errors.Count);
            Assert.That(errors.Any(x => x.ErrorMessage.Equals(pastErrorMessage)), "Missing pastErrorMessage");
        }

        private class ValidationModel
        {
            [Required(ErrorMessage = requiredErrorMessage)]
            [Past(ErrorMessage= pastErrorMessage)]
            public DateTime? RequiredProperty { get; set; }

            [System.ComponentModel.DataAnnotations.Range(5, 10, ErrorMessage = rangeErrorMessage)]            
            public int RangeProperty { get; set; }

            [StringLength(10, ErrorMessage = stringLengthErrorMessage)]
            public string StringLengthProperty { get; set; }
        }

        private class Past : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;
                if (value is DateTime)
                    return (DateTime)value < DateTime.Now;
                return true;
            }
        }
    }
}
