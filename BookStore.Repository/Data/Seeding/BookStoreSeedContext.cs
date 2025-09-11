using BookStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace BookStore.Repository.Data.Seeding
{
    public static class BookStoreSeedContext
    {

        public static async Task SeedAsync(BookStoreDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            if (!dbContext.Categories.Any())
            {
                var categoriesJson = File.ReadAllText("../BookStore.Repository/Data/Seeding/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesJson);
                Console.WriteLine(categories.IsNullOrEmpty());
                await dbContext.AddRangeAsync(categories);
            }

            if (!dbContext.Authors.Any())
            {
                var authorsJson = File.ReadAllText("../BookStore.Repository/Data/Seeding/authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsJson);
                await dbContext.AddRangeAsync(authors);
            }
            if (!dbContext.Publishers.Any())
            {
                var publishersJson = File.ReadAllText("../BookStore.Repository/Data/Seeding/publishers.json");
                var publishers = JsonSerializer.Deserialize<List<Publisher>>(publishersJson);
                await dbContext.AddRangeAsync(publishers);
            }
            if (!dbContext.Books.Any())
            {
                var booksJson = File.ReadAllText("../BookStore.Repository/Data/Seeding/books.json");
                var booksDto = JsonSerializer.Deserialize<List<BookAuthorSeed>>(booksJson);
                List<Book> books = new List<Book>();
                foreach (var dto in booksDto)
                {
                    var book = new Book
                    {
                        Name = dto.Name,
                        CoverUrl = dto.CoverUrl,
                        Quantity = dto.Quantity,
                        Language = dto.Language,
                        Price = dto.Price,
                        PageCount = dto.PageCount,
                        PublisherId = dto.PublisherId,
                        CategoryId = dto.CategoryId,
                        Description = dto.Description,
                        PublishDate = dto.PublishDate

                    };
                    foreach (var authorId in dto.AuthorsIds)
                    {
                        var author = await dbContext.Authors.FindAsync(authorId);
                        if (author != null)
                            book.Authors.Add(author);
                    }
                    books.Add(book);
                }
                await dbContext.AddRangeAsync(books);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
