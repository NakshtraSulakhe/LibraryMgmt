using LibraryMgmt.Data;
using LibraryMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMgmt.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _libraryContext;

        public BooksController(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _libraryContext.Books.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBook(int id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(); 
            }

            return book; 
        }

        
        [HttpPost]
        public async Task<ActionResult<Books>> CreateBook(Books book)
        {
            
            book.Id = 0;

            _libraryContext.Books.Add(book); 
            await _libraryContext.SaveChangesAsync(); 

           
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }


        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Books book)
        {
            if (id != book.Id)
            {
                return BadRequest(); 
            }

            _libraryContext.Entry(book).State = EntityState.Modified; 

            try
            {
                await _libraryContext.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound(); 
                }
                else
                {
                    throw; 
                }
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _libraryContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _libraryContext.Books.Remove(book); 
            await _libraryContext.SaveChangesAsync(); 

            return NoContent(); 
        }

        private bool BookExists(int id)
        {
            return _libraryContext.Books.Any(e => e.Id == id); 
        }
    }
}
