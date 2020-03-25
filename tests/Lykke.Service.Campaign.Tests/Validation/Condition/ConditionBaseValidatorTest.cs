using System;
using FluentValidation.TestHelper;
using Lykke.Service.Campaign.Client.Models.Condition;
using Lykke.Service.Campaign.Strings;
using Lykke.Service.Campaign.Validation.Condition;
using Xunit;

namespace Lykke.Service.Campaign.Tests.Validation.Condition
{
    public class ConditionBaseValidatorTest
    {
        private const string ValidBonusType = "signup";

        private readonly ConditionBaseValidator<ConditionBaseModel> _conditionValidator;

        public ConditionBaseValidatorTest()
        {
            _conditionValidator = new ConditionBaseValidator<ConditionBaseModel>();
        }

        [Fact]
        public void When_ConditionImmediateRewardIsWholeNumber_Expect_NoErrorsForImmediateRewardAreThrown()
        {
            var condition = new ConditionBaseModel
            {
                ImmediateReward = 1.0m,
                Type = ValidBonusType
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.ImmediateReward, condition);
        }


        [Fact]
        public void When_ConditionImmediateCompletionCountNull_Expect_NoErrorsForCompletionCountAreThrown()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = null
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.CompletionCount, condition);
        }

        [Fact]
        public void When_ConditionImmediateCompletionCountZero_Expect_ErrorsForCompletionCountAreThrown()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = 0
            };

            var result = _conditionValidator.ShouldHaveValidationErrorFor(c => c.CompletionCount, condition);

            result.WithErrorMessage(Phrases.ConditionCompletionCountValidation);
        }

        [Fact]
        public void When_ConditionImmediateCompletionCountMaxInt_Expect_NoErrorsForCompletionCountAreThrown()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.CompletionCount, condition);
        }

        [Fact]
        public void When_ConditionIsStakeableAndNoStakeInfoPassed_Expect_ThrowError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = true,
                StakingRule = null,
                StakeAmount = null,
                StakeWarningPeriod = null,
                StakingPeriod = null,
                BurningRule = null
            };

            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakeAmount, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakeWarningPeriod, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingPeriod, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsStakeableAndNotValidStakeInfoPassed_Expect_ThrowError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = true,
                StakingRule = -1,
                StakeAmount = -1,
                StakeWarningPeriod = -1,
                StakingPeriod = -1,
                BurningRule = -1
            };

            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingRule, condition)
                .WithErrorMessage("'Staking Rule' must be greater than or equal to '0'.");
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakeAmount, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakeWarningPeriod, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingPeriod, condition);
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsStakeableAndLargerStakeRulePassed_Expect_ThrowError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = true,
                StakingRule = 252,
                BurningRule = 252
            };

            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingRule, condition)
                .WithErrorMessage("'Staking Rule' must be less than or equal to '100'.");
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.BurningRule, condition)
                .WithErrorMessage("'Burning Rule' must be less than or equal to '100'.");
        }

        [Fact]
        public void When_ConditionIsStakeableValidStakeRulePassed_Expect_NoErrorThrow()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = true,
                StakingRule = 100.00m,
                BurningRule = 100.00m
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsStakeableAndNotValidStakeRulePassed_Expect_ThrowError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = true,
                StakingRule = 99.999m,
                BurningRule = 99.999m
            };

            _conditionValidator.ShouldHaveValidationErrorFor(c => c.StakingRule, condition)
                .WithErrorMessage("'Staking Rule' must not be more than 5 digits in total, with allowance for 2 decimals. 2 digits and 3 decimals were found.");
            _conditionValidator.ShouldHaveValidationErrorFor(c => c.BurningRule, condition)
                .WithErrorMessage("'Burning Rule' must not be more than 5 digits in total, with allowance for 2 decimals. 2 digits and 3 decimals were found.");
        }

        [Fact]
        public void When_ConditionIsNotStakeableAndNoStakeInfoPassed_Expect_ThrowNoError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = false,
                StakingRule = null,
                StakeAmount = null,
                StakeWarningPeriod = null,
                StakingPeriod = null,
                BurningRule = null
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakeAmount, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakeWarningPeriod, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingPeriod, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsNotStakeableAndNotValidStakeInfoPassed_Expect_ThrowNoError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = false,
                StakingRule = -1,
                StakeAmount = -1,
                StakeWarningPeriod = -1,
                StakingPeriod = -1,
                BurningRule = -1
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakeAmount, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakeWarningPeriod, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingPeriod, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsNotStakeableAndLargerStakeRulePassed_Expect_ThrowNoError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = false,
                StakingRule = 252,
                BurningRule = 262
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c=>c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsNoStakeableValidStakeRulePassed_Expect_NoErrorThrow()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = false,
                StakingRule = 100.00m,
                BurningRule = 100.00m
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.BurningRule, condition);
        }

        [Fact]
        public void When_ConditionIsNotStakeableAndNotValidStakeRulePassed_Expect_ThrowNotError()
        {
            var condition = new ConditionBaseModel
            {
                CompletionCount = int.MaxValue,
                HasStaking = false,
                StakingRule = 99.998m
            };

            _conditionValidator.ShouldNotHaveValidationErrorFor(c => c.StakingRule, condition);
        }
    }
}
