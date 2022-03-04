using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly DataContext _dataContext;
    
    public ItemController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Item>>> GetAllItems()
    {
        return Ok(await _dataContext.Items.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItemById(int id)
    {
        Item? itemFromDb = await _dataContext.Items.FindAsync(id);
        if (itemFromDb == null)
        {
            return NotFound("Item is not found");
        }

        return Ok(itemFromDb);
    }

    [HttpPost]
    public async Task<ActionResult> AddItem(Item item)
    {
        if (item.Id != 0)
        {
            item.Id = 0;
        }
        
        item.Date = DateTime.Now;

        await _dataContext.Items.AddAsync(item);
        await _dataContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateItem(Item item)
    {
        Item? itemFromDb = await _dataContext.Items.FindAsync(item.Id);

        if (itemFromDb == null)
        {
            return BadRequest($"Item with id={item.Id} not found or data is not correct");
        }

        itemFromDb.Name = item.Name;
        itemFromDb.Author = item.Author;
        itemFromDb.Description = item.Description;
        itemFromDb.Date = DateTime.Now;

        _dataContext.Items.Update(itemFromDb);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        Item? itemFromDb = await _dataContext.Items.FindAsync(id);
        
        if (itemFromDb == null)
        {
            return BadRequest($"Item with id={id} not found");
        }

        _dataContext.Items.Remove(itemFromDb);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }

}