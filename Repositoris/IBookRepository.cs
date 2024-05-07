using Kolokwium_s27232.ModelDto;

namespace Kolokwium_s27232.Repositoris;

public interface IBookRepository
{
    Task<bool> DoesBookExist(int id);
    Task<bool> DoesGenresExist(int id);
    Task<bool> DoesAuthorExist(int id);
    Task<bool> DoesPublishHouseExist(int id);


    Task<int> AddBook(string title, List<int> genreIds);

    Task<BooksDto> GetBook(int id);
    Task<List<string>> GetBookGeners(int id);
}