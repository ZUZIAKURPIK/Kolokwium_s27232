using Kolokwium_s27232.Repositoris;
using System.Transactions;
using Kolokwium_s27232.ModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium_s27232.Controlers;

[ApiController]
[Route("api/[controller]")]
public class BookControler : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BookControler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet("{BookId}/geners")]
    public async Task<IActionResult> GetBookGeners(int BookId)
    {
        if (!await _bookRepository.DoesBookExist(BookId))
        {
            return NotFound($"Book with given ID - {BookId} dosen't exist");
        }

        var geners = await _bookRepository.GetBookGeners(BookId);
        var book = await _bookRepository.GetBook(BookId);

        var result = new
        {
            id = book.Id,
            title = book.Title,
            geners = geners
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] BooksDto booksDto)
    {
        var bookId = await _bookRepository.AddBook(booksDto.Title, booksDto.GenreIds);

        var genres = await _bookRepository.GetBookGeners(bookId);
        var book = await _bookRepository.GetBook(bookId);

        var result = new
        {
            id = book.Id,
            title = book.Title,
            genres = genres
        };

        return CreatedAtAction(nameof(GetBookGeners), new { BookId = bookId }, result);
    }
}