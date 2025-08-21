using System;
using System.Threading.Tasks;
using FixoraBackend.Data;
using Microsoft.EntityFrameworkCore;

public class WarrantyService
{
    private readonly MainDbContext _db;

    public WarrantyService(MainDbContext db)
    {
        _db = db;
    }

    public async Task SubmitAsync(WarrantyRequest request)
    {
        var existing = await _db.WarrantyCases.FirstOrDefaultAsync(w => w.OrderId == request.OrderId);
        if (existing != null)
            throw new InvalidOperationException("Warranty case already submitted");

        var warranty = new WarrantyCase
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            ClientId = request.ClientId,
            ProblemDescription = request.ProblemDescription
        };

        _db.WarrantyCases.Add(warranty);
        await _db.SaveChangesAsync();
    }

    public async Task ProviderReviewAsync(WarrantyReview review)
    {
        var warranty = await _db.WarrantyCases.FindAsync(review.CaseId);
        if (warranty == null) throw new Exception("Not found");

        warranty.ProviderResponse = review.ProviderResponse;
        warranty.Status = review.Accept ? "resolved" : "provider_rejected";
        await _db.SaveChangesAsync();
    }

    public async Task EscalateToSupervisorAsync(WarrantyEscalation escalation)
    {
        var warranty = await _db.WarrantyCases.FindAsync(escalation.CaseId);
        if (warranty == null) throw new Exception("Not found");

        warranty.SupervisorNote = escalation.SupervisorNote;
        warranty.Status = "escalated";
        await _db.SaveChangesAsync();
    }
}
