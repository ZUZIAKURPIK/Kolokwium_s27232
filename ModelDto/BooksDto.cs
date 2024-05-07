namespace Kolokwium_s27232.ModelDto;

public class BooksDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<int> GenreIds { get; set; }
}

public class AuthorsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class GenersDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PublishingHouses
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OwnerFirstName { get; set; }
    public string OwnerLastName { get; set; }
}

public class BooksEditions
{
    public int Id { get; set; }
    public PublishingHouses PublishingHouses { get; set; }
    public BooksDto BooksDto { get; set; }
    public string EditionTitle { get; set; }
    public DateTime ReleaseDate { get; set; }
}

public class BooksAuthors
{
    public BooksDto BooksDto { get; set; }
    public AuthorsDto AuthorsDto { get; set; }
}

public class BooksGeners
{
    public BooksDto BooksDto { get; set; }
    public GenersDto GenersDto { get; set; }
}