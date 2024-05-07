using Kolokwium_s27232.ModelDto;
using Microsoft.Data.SqlClient;

namespace Kolokwium_s27232.Repositoris;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM books WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> DoesGenresExist(int id)
    {
        var query = "SELECT 1 FROM genres WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> DoesAuthorExist(int id)
    {
        var query = "SELECT 1 FROM authors WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> DoesPublishHouseExist(int id)
    {
        var query = "SELECT 1 FROM publishing_houses WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<List<string>> GetBookGeners(int id)
    {
        var query = @"Select g.name From genres g Join books_genres bg ON g.PK = bg.FK_genre Where bg.FK_book = @PK";
        
        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var geners = new List<string>();
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            geners.Add(reader.GetString(0));
        }

        return geners;
    }

    public async Task<BooksDto> GetBook(int id)
    {
        var query = @"Select PK as Id, title as Title From books WHERE PK = @PK";
        
        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", id);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new BooksDto
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1)
            };
        }

        return null;
    }

    public async Task<int> AddBook(string title, List<int> genreIds)
    {
        var query = @"INSERT INTO books (title) OUTPUT INSERTED.PK VALUES (@title)";

        await using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        await using SqlCommand command = new SqlCommand(query, connection);

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@title", title);

        await connection.OpenAsync();

        var bookId = (int) await command.ExecuteScalarAsync();

        foreach (var genreId in genreIds)
        {
            var query2 = @"INSERT INTO books_genres (FK_book, FK_genre) VALUES (@bookId, @genreId)";

            await using SqlCommand command2 = new SqlCommand(query2, connection);

            command2.Parameters.AddWithValue("@bookId", bookId);
            command2.Parameters.AddWithValue("@genreId", genreId);

            await command2.ExecuteNonQueryAsync();
        }

        return bookId;
    }
}