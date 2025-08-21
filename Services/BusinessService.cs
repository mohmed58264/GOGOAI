using FixoraBackend.DTOs.DTOs;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BusinessService
{
    private readonly MainDbContext _db;

    public BusinessService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<BusinessClient> CreateClientAsync(BusinessClientRequest request)
    {
        var client = new BusinessClient
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            ManagerName = request.ManagerName,
            Phone = request.Phone,
            Region = request.Region,
            PaymentType = request.PaymentType
        };

        _db.BusinessClients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<BusinessSite> AddSiteAsync(BusinessSiteRequest request)
    {
        var site = new BusinessSite
        {
            Id = Guid.NewGuid(),
            BusinessClientId = request.BusinessClientId,
            SiteName = request.SiteName,
            Address = request.Address,
            ContactPhone = request.ContactPhone
        };

        _db.BusinessSites.Add(site);
        await _db.SaveChangesAsync();
        return site;
    }

    public async Task<BusinessOrder> CreateOrderAsync(BusinessOrderRequest request)
    {
        var order = new BusinessOrder
        {
            Id = Guid.NewGuid(),
            BusinessClientId = request.BusinessClientId,
            BusinessSiteId = request.BusinessSiteId,
            ServiceType = request.ServiceType,
            Price = request.Price
        };

        _db.BusinessOrders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<List<BusinessOrder>> GetClientOrders(Guid businessClientId)
    {
        return await _db.BusinessOrders
            .Where(o => o.BusinessClientId == businessClientId)
            .ToListAsync();
    }
}
