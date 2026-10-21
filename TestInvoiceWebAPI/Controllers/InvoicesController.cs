using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestInvoiceWebAPI.Models;

namespace InvoiceWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    {
        var invoices = await _context.Invoices
            .Include(invoice => invoice.Items)
            .ToListAsync();

        return Ok(invoices);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Invoice>> GetInvoice(int id)
    {
        var invoice = await _context.Invoices
            .Include(invoice => invoice.Items)
            .FirstOrDefaultAsync(invoice => invoice.InvoiceID == id);

        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Accountant}")]
    public async Task<ActionResult<Invoice>> AddInvoice(
        [FromBody] InvoiceCreateDto dto)
    {
        var invoice = new Invoice
        {
            CustomerName = dto.CustomerName,
            Items = dto.Items.Select(item => new InvoiceItem
            {
                Name = item.Name,
                Price = item.Price
            }).ToList()
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetInvoice),
            new { id = invoice.InvoiceID },
            invoice);
    }
}
