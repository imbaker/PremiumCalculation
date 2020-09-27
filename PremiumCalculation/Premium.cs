namespace PremiumCalculation
{
    public class Premium
    {
        private decimal premiumExclIptAmount;
        private decimal premiumInclIptAmount;
        private decimal overriddenPremiumExclIptAmount;
        private decimal overriddenPremiumInclIptAmount;
        private decimal overridePremium = 0;
        private decimal overrideCommission = 0;

        public Premium(decimal iptPercent, decimal commissionPercent = 0)
        {
            IptPercent = iptPercent;
            CommissionPercent = commissionPercent;
        }

        public decimal PremiumExclIptAmount {
            get
            {
                return overriddenPremiumExclIptAmount != 0 ? overriddenPremiumExclIptAmount : premiumExclIptAmount;
            }
            set 
            {
                premiumExclIptAmount = value;
                IptAmount = premiumExclIptAmount * (IptPercent / 100);
                premiumInclIptAmount = premiumExclIptAmount + IptAmount;
            } 
        }

        public decimal PremiumInclIptAmount {
            get
            {
                return overriddenPremiumInclIptAmount != 0 ? overriddenPremiumInclIptAmount : premiumInclIptAmount;
            }
            set {
                premiumInclIptAmount = value;
                premiumExclIptAmount = premiumInclIptAmount / (1 + (IptPercent / 100));
                premiumExclIptAmount -= CommissionSacrificed;
                IptAmount = premiumInclIptAmount - premiumExclIptAmount;
            }
        }

        public decimal IptPercent { get; }
        
        public decimal CommissionPercent { get; }

        public decimal IptAmount { get; private set; }

        public decimal MaxCommission
        {
            get => premiumExclIptAmount * (CommissionPercent / 100);
        }

        public decimal OverridePremium
        {
            get => overridePremium;
            set
            {
                overridePremium = value;
                overriddenPremiumInclIptAmount = value;
                overriddenPremiumExclIptAmount = overriddenPremiumInclIptAmount / (1 + (IptPercent / 100));
                overriddenPremiumExclIptAmount -= CommissionSacrificed;
                IptAmount = overriddenPremiumInclIptAmount - overriddenPremiumExclIptAmount;
            }
        }

        public decimal OverrideCommission
        {
            get => OverrideCommission;
            set
            {
                overrideCommission = value;
                overriddenPremiumExclIptAmount -= CommissionSacrificed;
                IptAmount = overriddenPremiumExclIptAmount * (IptPercent / 100);
                overriddenPremiumInclIptAmount = overriddenPremiumExclIptAmount + IptAmount;
            }
        }

        private decimal CommissionSacrificed
        {
            get
            {
                return overrideCommission == 0 ? 0 : MaxCommission - overrideCommission;
            }
        }
    }
}
