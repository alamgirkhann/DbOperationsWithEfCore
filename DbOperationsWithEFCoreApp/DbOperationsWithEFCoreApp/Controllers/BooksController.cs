using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAllBooksAsync() { 
        
            var books = await appDbContext.Books.Select(x=> new
            {
                x.Id,
                BookName = x.Title,
                x.Description,
                AuthorName = x.Author != null ? x.Author.Name : "NA",
            }).ToListAsync();

            return Ok(books);
        
        }

        [HttpGet("GetAllBooksUsingSQLQuery")]
        public async Task<IActionResult> GetAllBooksUsingSQLQuery() {

            // Case 1
            //var books = await appDbContext.Books.FromSql($"select * from Books").ToListAsync();

            // Case 2
            //var id = 2;
            //var books = await appDbContext.Books.FromSql($"select * from Books where Id = {id}").ToListAsync();

            // Case 3
            //var books = await appDbContext.Books.FromSql($"select * from Books").Where(x=> x.Id > 5).ToListAsync();

            // Case 4
            var columnName = "Id";
            var columnValue = "1";

            var parameter = new SqlParameter("anyName", columnValue);

            var books = await appDbContext.Books.FromSqlRaw($"select * from Books where {columnName} = @anyName", parameter).ToListAsync();


            return Ok(books);

        }

        [HttpGet("useLazyLoading")]
        public async Task<IActionResult> GetAllBooksAsyncUsingLazyLoading()
        {
            var book = await appDbContext.Books.FirstAsync();
            var author = book.Author;

            return Ok(book);
        }

        [HttpGet("getAllBooksExplicitLoadingOneToOne")]
        public async Task<IActionResult> getAllBooksExplicitLoadingOneToOneAsync() {

            var book = await appDbContext.Books.FirstAsync();

            // for one to one relation use refrence
            await appDbContext.Entry(book).Reference(x=> x.Author).LoadAsync();
            await appDbContext.Entry(book).Reference(x => x.Language).LoadAsync();

            return Ok(book);
        
        }

        // comment book inside book entity for this
        //[HttpGet("getAllBooksExplicitLoadingOneToMany")]
        //public async Task<IActionResult> getAllBooksExplicitLoadingOneToManyAsync()
        //{

        //    var languages = await appDbContext.Languages.ToListAsync();

        //    // for one to many relation use collection

        //    foreach (var language in languages)
        //    {
        //        await appDbContext.Entry(language).Collection(x => x.Books)
        //            .Query()
        //            .Where(x=> x.NoOfPages > 100)
        //            .LoadAsync();
        //    }

        //    return Ok(languages);

        //}

        [HttpGet("usingIncludeEagerLoading")]
        public async Task<IActionResult> GetAllBooksWithAuthorAndLanguageAsync() // eager loading
        {

            //var books = await appDbContext.Books.Include(x=> x.Author).Include(x=> x.Language).Select(x => new
            //{
            //    x.Id,
            //    BookName = x.Title,
            //    x.Description,
            //    AuthorName = x.Author != null ? x.Author.Name : "NA",
            //    LanguageTitle = x.Language.Title,
            //    LanguageDescription = x.Language.Description,

            //}).ToListAsync();

            var books = await appDbContext.Books.Where(x=> x.Author.Category != null).Include(x => x.Author).ThenInclude(x=> x.Category).Include(x => x.Language).ToListAsync();
            //var books = await appDbContext.Languages.Include(x => x.Books).ToListAsync();

            return Ok(books);

        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {
            //var author = new Author()
            //{
            //    Name = "Ghani Khan",
            //    Email = "ghaniKhan@gmail.com"
            //};
            //model.Author = author;
            appDbContext.Books.Add(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> model)
        {
            //foreach (Book i in model) {
            //    appDbContext.Books.Add(i);
            //    await appDbContext.SaveChangesAsync();
            //}

            appDbContext.Books.AddRange(model);
            await appDbContext.SaveChangesAsync();


            return Ok(model);
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] Book model)
        {
            
            var book = await appDbContext.Books.FirstOrDefaultAsync(x=> x.Id == bookId);

            if (book == null) {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;
            book.Author = model.Author;
            book.NoOfPages = model.NoOfPages;

            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWWithSingleQuery([FromBody] Book model)
        {
            appDbContext.Books.Update(model);
            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBooksInBulk() {

            await appDbContext.Books
                .Where(x=> x.AuthorId == null)
                .ExecuteUpdateAsync(x=> x
            .SetProperty(p=> p.NoOfPages, 1233)
            .SetProperty(p=> p.Description, p=> p.Description + " Updated 2.")
            .SetProperty(p=> p.AuthorId, 3)
            );

            return Ok();
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] int bookId) { 
        
            //var book = await appDbContext.Books.FirstOrDefaultAsync(x=> x.Id == bookId);
            //if (book == null)
            //{
            //    return NotFound();
            //}

            //appDbContext.Books.Remove(book);

            var book = new Book { Id = bookId };
            appDbContext.Entry(book).State = EntityState.Deleted;
            await appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBooksInBulk() {

            // hit db multiple times
            //var books = await appDbContext.Books.Where(x=> x.Id > 11).ToListAsync();
            //appDbContext.Books.RemoveRange(books);
            //var result = await appDbContext.SaveChangesAsync();

            // hit db only one time
            var books = await appDbContext.Books.Where(x=> x.Id > 10).ExecuteDeleteAsync();

            return Ok(books);
        }
    }
}
