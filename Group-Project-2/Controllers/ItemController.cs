using Group_Project_2.DAL;
using Group_Project_2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Group_Project_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemRepository.GetAll();
            if (items == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            return Ok(items);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Item newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Invalid item data.");
            }
            //newItem.ItemId = GetNextItemId();
            bool returnOk = await _itemRepository.Create(newItem);

            if (returnOk)
            {
                var response = new { success = true, message = "Item " + newItem.Name + " created successfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Item creation failed" };
                return Ok(response);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItembyId(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            return Ok(item);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Item newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Invalid item data.");
            }
            bool returnOk = await _itemRepository.Update(newItem);
            if (returnOk)
            {
                var response = new { success = true, message = "Item " + newItem.Name + " updated successfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Item creation failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            bool returnOk = await _itemRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
                return BadRequest("Item deletion failed");
            }
            var response = new { success = true, message = "Item " + id.ToString() + " deleted succesfully" };
            return Ok(response);
        }
    }
}