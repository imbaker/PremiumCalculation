
using FluentAssertions;
using Xunit;

namespace PremiumCalculation.Tests
{
    public class PremiumCalculationTests
    {
        [Fact]
        public void CheckIncludingCalculations()
        {
            // Arrange
            var premiumExclIptAmount = 100;
            var iptPercent = 12;

            // Act
            var sut = new Premium(iptPercent)
            {
                PremiumExclIptAmount = premiumExclIptAmount
            };

            // Assert
            sut.PremiumInclIptAmount.Should().Be(112);
            sut.PremiumExclIptAmount.Should().Be(premiumExclIptAmount);
            sut.IptAmount.Should().Be(premiumExclIptAmount * (iptPercent / 100m));
        }

        [Fact]
        public void CheckExcludingCalculations()
        {
            // Arrange
            var premiumInclIptAmount = 112;
            var iptPercent = 12;

            // Act
            var sut = new Premium(iptPercent)
            {
                PremiumInclIptAmount = premiumInclIptAmount
            };

            // Assert
            sut.PremiumExclIptAmount.Should().Be(100);
            sut.PremiumInclIptAmount.Should().Be(premiumInclIptAmount);
            sut.IptAmount.Should().Be(100m * (iptPercent / 100m));
        }

        [Fact]
        public void CheckIfPremiumOverridden_ThenCorrectValuesReturned()
        {
            // Arrange
            var premiumInclIptAmount = 112m;
            var overridePremium = 110m;
            var iptPercent = 12m;
            var expectedPremiumExclIptAmount = overridePremium / (1 + (iptPercent / 100m));

            // Act
            var sut = new Premium(iptPercent)
            {
                PremiumInclIptAmount = premiumInclIptAmount,
                OverridePremium = overridePremium
            };

            // Assert
            sut.PremiumInclIptAmount.Should().Be(110);
            sut.PremiumExclIptAmount.Should().Be(expectedPremiumExclIptAmount);
            sut.OverridePremium.Should().Be(overridePremium);
            sut.IptAmount.Should().Be(overridePremium - expectedPremiumExclIptAmount);
        }

        [Theory]
        [InlineData(12.0, 15.0, 100.0, 105.0, 12.0)]
        [InlineData(12.0, 15.0, 1235.66, 1250.0, 12.0)]
        [InlineData(12.0, 15.0, 1235.66, 1250.0, 185.349)]
        public void CheckIfCommissionOverridden_ThenCorrectValuesReturned(decimal iptPercent, decimal commissionPercent, decimal premiumExclIptAmount, decimal overridePremium, decimal overrideCommission)
        {
            // Arrange
            var maxCommission = premiumExclIptAmount * (commissionPercent / 100);
            var commissionSacrificed = maxCommission - overrideCommission;
            var expectedPremiumExclIptAmount = (overridePremium / (1 + (iptPercent / 100))) - commissionSacrificed;
            var expectedIptAmount = expectedPremiumExclIptAmount * (iptPercent / 100);
            var expectedPremiumInclIptAmount = expectedPremiumExclIptAmount + expectedIptAmount;

            // Act
            var sut = new Premium(iptPercent, commissionPercent)
            {
                PremiumExclIptAmount = premiumExclIptAmount,
                OverridePremium = overridePremium,
                OverrideCommission = overrideCommission
            };

            // Assert
            sut.PremiumExclIptAmount.Should().Be(expectedPremiumExclIptAmount);
            sut.PremiumInclIptAmount.Should().Be(expectedPremiumInclIptAmount);
            sut.OverridePremium.Should().Be(overridePremium);
            sut.IptAmount.Should().Be(expectedIptAmount);
            sut.MaxCommission.Should().Be(premiumExclIptAmount * (commissionPercent / 100));
        }
    }
}
