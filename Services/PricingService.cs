using System;

public class PricingService
{
    public double CalculateEstimate(PricingRequest input)
    {
        double basePrice = 50;

        if (input.ServiceType == "premium")
            basePrice += 30;

        if (input.Region.ToLower() == "vip")
            basePrice += 20;

        if (input.Time.Hour >= 18 || input.Time.Hour <= 6)
            basePrice += 15;

        return basePrice;
    }
}

