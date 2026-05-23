using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAPI.Data;
using WebLabAPI.Models;

namespace WebLabAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContactsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        return await _context.Contacts.OrderBy(c => c.FirstName).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return NotFound(new { message = "Contact not found" });
        return contact;
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(Contact contact)
    {
        if (string.IsNullOrWhiteSpace(contact.FirstName) ||
            string.IsNullOrWhiteSpace(contact.Email))
            return BadRequest(new { message = "Required fields missing" });

        var existingContact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Email == contact.Email);
        if (existingContact != null)
            return BadRequest(new { message = "Email already exists" });

        contact.CreatedAt = DateTime.UtcNow;
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    {
        if (id != contact.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingContact = await _context.Contacts.FindAsync(id);
        if (existingContact == null)
            return NotFound(new { message = "Contact not found" });

        existingContact.FirstName = contact.FirstName;
        existingContact.LastName = contact.LastName;
        existingContact.Email = contact.Email;
        existingContact.PhoneNumber = contact.PhoneNumber;
        existingContact.Address = contact.Address;
        existingContact.City = contact.City;
        existingContact.Country = contact.Country;
        existingContact.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Update failed" });
        }

        return Ok(existingContact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return NotFound(new { message = "Contact not found" });

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Contact deleted successfully" });
    }
}
