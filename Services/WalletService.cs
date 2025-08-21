using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class WalletService
{
    private readonly MainDbContext _db;

    public WalletService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<Wallet> GetWalletAsync(Guid userId, string system)
    {
        var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId && w.System == system);
        if (wallet == null)
        {
            wallet = new Wallet { Id = Guid.NewGuid(), UserId = userId, System = system };
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();
        }
        return wallet;
    }

    public async Task<bool> AddTransactionAsync(Guid userId, string system, double amount, string type)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");

        var wallet = await GetWalletAsync(userId, system);

        if (type == "debit")
        {
            if (wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient funds");
            wallet.Balance -= amount;
        }
        else if (type == "credit")
        {
            wallet.Balance += amount;
        }
        else
        {
            throw new ArgumentException("Invalid transaction type");
        }

        await _db.SaveChangesAsync();
        return true;
    }
}
