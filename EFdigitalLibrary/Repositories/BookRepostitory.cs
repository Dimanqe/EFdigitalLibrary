using EFdigitalLibrary.Models;

namespace EFdigitalLibrary.Repositoriess
{
    public class BookRepository
    {
        private readonly AppContext db;

        public BookRepository(AppContext dbContext)
        {
            db = dbContext;
        }

        public void ExecuteCommand(int command)
        {
            switch ((Commands)command)
            {
                case Commands.Create:
                    CreateBook();
                    break;
                case Commands.ShowAll:
                    ShowAllBooks();
                    break;
                case Commands.ShowById:
                    ShowBookById();
                    break;
                case Commands.Update:
                    UpdateBookReleaseDate();
                    break;
                case Commands.Delete:
                    DeleteBook();
                    break;
                case Commands.Assign:
                    AssignBookToUser();
                    break;
                case Commands.ShowBooksByGenre:
                    ShowBooksByGenre();
                    break;
                case Commands.ShowBooksByGenreAndTimeRange:
                    ShowBooksByGenreAndTimeRange();
                    break;
                case Commands.ShowBooksAmountByAuthor:
                    ShowBooksAmountByAuthor();
                    break;
                case Commands.СheckByAuthorAndBookName:
                    CheckBookByAuthorAndBookName();
                    break;
                case Commands.СheckIfBookISAssigned:
                    CheckIfBookIsAssigned();
                    break;
                case Commands.GetAssignedBooksAmount:
                    GetAssignedBooksAmount();
                    break;
                case Commands.GetLatestIssuedBook:
                    GetLatestIssuedBook();
                    break;
                case Commands.GetBooksSortedByName:
                    GetBooksSortedByName();
                    break;
                case Commands.GetBooksSortedByReleaseDateDesc:
                    GetBooksSortedByReleaseDateDesc();
                    break;
                case Commands.ReturnToMain:
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }

        public void CreateBook()
        {
            Console.WriteLine("Введите имя новой книги");
            var name = Console.ReadLine();
            Console.WriteLine("Введите автора книги");
            var author = Console.ReadLine();
            Console.WriteLine("Введите жанр книги");
            var genre = Console.ReadLine();

            DateTime releaseDate;
            do
            {
                Console.WriteLine("Введите дату издания книги (в формате ГГГГ-ММ-ДД):");
                var releaseDateInput = Console.ReadLine();

                if (DateTime.TryParse(releaseDateInput, out releaseDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный формат даты.");
                }
            } while (true);

            var newBook = new Book(name, author, genre, releaseDate);

            db.Books.Add(newBook);
            db.SaveChanges();

            Console.WriteLine($"Книга {name} добавлена в репозиторий");
        }


        public void ShowAllBooks()
        {
            var allBooks = db.Books.ToList();

            if (allBooks.Count > 0)
            {
                foreach (var book in allBooks)
                {
                    Console.WriteLine($"Id: {book.Id} Название: \"{book.Name}\" Дата издания: {book.ReleaseDate}");
                }
            }
            else
            {
                Console.WriteLine("В библиотеке не добавлено ни одной книги");
            }
        }

        public void ShowBookById()
        {
            Console.WriteLine("Введите id книги для отображения ее данных");
            int inputBookid = Convert.ToInt32(Console.ReadLine());
            var bookById = db.Books.FirstOrDefault(u => u.Id == inputBookid);

            if (bookById != null)
            {
                Console.WriteLine($"Id: {bookById.Id} Название: \"{bookById.Name}\" Дата издания: {bookById.ReleaseDate}");
            }
            else
            {
                Console.WriteLine($"Книга с ID {inputBookid} не найдена");
            }
        }

        public void UpdateBookReleaseDate()
        {
            Console.WriteLine("Введите название книги, у которой хотите обновить дату издания");
            var nameToUpdate = Console.ReadLine();

            DateTime newDate;
            do
            {
                Console.WriteLine("Введите новую дату издания (в формате ГГГГ-ММ-ДД):");
                var newDateInput = Console.ReadLine();

                if (DateTime.TryParse(newDateInput, out newDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный формат даты.");
                }
            } while (true);

            var bookToUpdate = db.Books.FirstOrDefault(u => u.Name == nameToUpdate);

            if (bookToUpdate != null)
            {
                bookToUpdate.ReleaseDate = newDate;
                db.SaveChanges();
                Console.WriteLine($"Дата издания книги {nameToUpdate} изменена на {newDate}");
            }
            else
            {
                Console.WriteLine($"Книга с названием \"{nameToUpdate}\" не найдена");
            }
        }

        public void DeleteBook()
        {
            Console.WriteLine("Введите название книги, которую хотите удалить");
            var nameToDelete = Console.ReadLine();
            var bookToDelete = db.Books.FirstOrDefault(u => u.Name == nameToDelete);

            if (bookToDelete != null)
            {
                db.Books.Remove(bookToDelete);
                db.SaveChanges();
                Console.WriteLine($"Книга {nameToDelete} удалена");
            }
            else
            {
                Console.WriteLine($"Книга с названием \"{nameToDelete}\" не найдена");
            }
        }

        public void AssignBookToUser()
        {
            Console.WriteLine("Введите название книги, которую хотите выдать");
            var bookNameToAssign = Console.ReadLine();
            Console.WriteLine("Введите имя пользователя, которому выдается книга");
            var userNameToAssign = Console.ReadLine();

            var bookByName = db.Books.FirstOrDefault(b => b.Name == bookNameToAssign);
            var userByName = db.Users.FirstOrDefault(u => u.Name == userNameToAssign);

            if (bookByName != null && userByName != null)
            {
                bookByName.UserId = userByName.Id;
                db.SaveChanges();
                Console.WriteLine($"Книга \"{bookNameToAssign}\" выдана пользователю \"{userNameToAssign}\"");
            }
            else
            {
                Console.WriteLine($"Книга с названием \"{bookNameToAssign}\" не найдена или пользователь с именем \"{userNameToAssign}\" не найден");
            }
        }

        public void ShowBooksByGenre()
        {
            Console.WriteLine("Введите жанр книги");
            var bookGenreToShow = Console.ReadLine();
            var booksByGenre = db.Books.Where(b => b.Genre == bookGenreToShow).ToList();

            foreach (var book in booksByGenre)
            {
                Console.WriteLine(book.Name);
            }
        }

        public void ShowBooksByGenreAndTimeRange()
        {
            Console.WriteLine("Введите жанр книги");
            var bookGenreToShow = Console.ReadLine();

            DateTime startOfRange, endOfRange;
            if (TryGetDateTime("Введите начало периода (в формате ГГГГ-ММ-ДД):", out startOfRange)
                && TryGetDateTime("Введите конец периода (в формате ГГГГ-ММ-ДД):", out endOfRange))
            {
                var booksByGenre = db.Books.Where(b => b.Genre == bookGenreToShow && b.ReleaseDate > startOfRange && b.ReleaseDate < endOfRange).ToList();

                foreach (var book in booksByGenre)
                {
                    Console.WriteLine(book.Name);
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод даты.");
            }
        }

        public void ShowBooksAmountByAuthor()
        {
            Console.WriteLine("Введите автора книги");
            var imputBookAuthor = Console.ReadLine();
            var bookByAuthor = db.Books.FirstOrDefault(b => b.Author == imputBookAuthor);
            if (bookByAuthor != null)
            {
                var booksAmountByAuthor = db.Books.Count(b => b.Author == imputBookAuthor);
                Console.WriteLine(booksAmountByAuthor);

            }
            else
            {
                Console.WriteLine("Такого автора нет в библиотеке");
            }
        }


        public void CheckBookByAuthorAndBookName()
        {
            Console.WriteLine("Введите автора книги");
            var inputBookAuthor = Console.ReadLine();
            var bookByAuthor = db.Books.Any(b => b.Author == inputBookAuthor);
            Console.WriteLine("Введите название книги");
            var inputBookname = Console.ReadLine();
            var bookByName = db.Books.Any(db => db.Name == inputBookname);
            bool result = bookByAuthor && bookByName;
            Console.WriteLine(result);

            if (bookByAuthor == false)
            {
                Console.WriteLine("Такого автора нет");

            }
            else if (bookByName == false)
            {
                Console.WriteLine("Такой книги нет");
            }

        }

        public void CheckIfBookIsAssigned()
        {
            Console.WriteLine("Введите название книги");
            var inputBookname = Console.ReadLine();
            var assignedBook = db.Books.FirstOrDefault(db => db.Name == inputBookname);
            if (assignedBook != null)
            {
                if (assignedBook.UserId == null)
                {
                    Console.WriteLine($"Книга {assignedBook.Name} никому не выдана");
                }
                else
                {
                    var assignedUser = db.Books.Join(db.Users, b => b.UserId, u => u.Id, (b, u) => new User { Name = u.Name }).FirstOrDefault();

                    Console.WriteLine($"Книга {assignedBook.Name} выдана пользователю {assignedUser.Name}");
                }
            }
            else
            {
                Console.WriteLine("Такой книги нет");
            }
        }


        public void GetAssignedBooksAmount()
        {
            Console.WriteLine($"Количество выданных книг - {db.Books.Where(b => b.UserId != null).Count()}");
        }

        private bool TryGetDateTime(string prompt, out DateTime result)
        {
            do
            {
                Console.WriteLine(prompt);
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out result))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Некорректный формат даты.");
                }
            } while (true);
        }

        public void GetLatestIssuedBook()
        {
            var books = db.Books.ToList();

            if (books.Any())
            {
                var latestBook = books.OrderByDescending(book => book.ReleaseDate).First();
                Console.WriteLine($"Книга {latestBook.Name} была издана позже всех в библиотеке");
            }
            else
            {
                Console.WriteLine("В библиотеке нет книг.");
            }
        }

        public void GetBooksSortedByName()
        {
            var sortedBooks = db.Books.OrderBy(b => b.Name);
            for (int i = 1; i < sortedBooks.Count(); i++)
            {
                Console.WriteLine($"{i} - \"{sortedBooks.ElementAt(i).Name}\"");
            }
        }
        public void GetBooksSortedByReleaseDateDesc()
        {
            var sortedBooks = db.Books.OrderByDescending(b => b.ReleaseDate);
            for (int i = 1; i < sortedBooks.Count(); i++)
            {
                Console.WriteLine($"{i} - \"{sortedBooks.ElementAt(i).Name} - {sortedBooks.ElementAt(i).ReleaseDate}\"");
            }
        }

        public enum Commands
        {
            Create = 1,
            ShowAll = 2,
            ShowById = 3,
            Update = 4,
            Delete = 5,
            Assign = 6,
            ShowBooksByGenre = 7,
            ShowBooksByGenreAndTimeRange = 8,
            ShowBooksAmountByAuthor = 9,
            СheckByAuthorAndBookName = 10,
            СheckIfBookISAssigned = 11,
            GetAssignedBooksAmount = 12,
            GetLatestIssuedBook = 13,
            GetBooksSortedByName = 14,
            GetBooksSortedByReleaseDateDesc = 15,
            ReturnToMain = 16,
        }
    }
}
