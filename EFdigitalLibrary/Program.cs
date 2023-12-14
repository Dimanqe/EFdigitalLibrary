using EFdigitalLibrary.Repositoriess;

namespace EFdigitalLibrary
{
    public class Program
    {
        private static AppContext dbContext = new AppContext();
        private static UserRepository userRepository = new UserRepository(dbContext);
        private static BookRepository bookRepository = new BookRepository(dbContext);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (var dbContext = new AppContext())
            {
                userRepository = new UserRepository(dbContext);
                bookRepository = new BookRepository(dbContext);

                Console.WriteLine("Добро пожаловать в электронную библиотеку!");

                bool exitFlag = false;

                do
                {
                    Console.WriteLine();
                    DisplayMainMenu();
                    string inputMainMenu = Console.ReadLine();

                    if (int.TryParse(inputMainMenu, out int mainMenuChoice))
                    {
                        switch (mainMenuChoice)
                        {
                            case 1:
                                ManageUserRepository();
                                break;

                            case 2:
                                ManageBookRepository();
                                break;

                            case 3:
                                exitFlag = true;
                                break;

                            default:
                                Console.WriteLine("Неизвестная команда");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод");
                    }

                } while (!exitFlag);
            }
        }

        private static void DisplayMainMenu()
        {
            Console.WriteLine("Выберите раздел:");
            Console.WriteLine("1. Работа с репозиторием пользователей");
            Console.WriteLine("2. Работа с репозиторием книг");
            Console.WriteLine("3. Выход");
        }

        private static void ManageUserRepository()
        {
            bool returnToMainFlag = false;

            do
            {
                DisplayUserRepositoryMenu();
                string inputUserRepo = Console.ReadLine();

                if (int.TryParse(inputUserRepo, out int userRepoChoice))
                {
                    try
                    {
                        userRepository.ExecuteCommand(userRepoChoice);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    if (userRepoChoice == (int)UserRepository.Commands.ReturnToMain)
                        returnToMainFlag = true;
                }
                else
                {
                    Console.WriteLine("Неверный ввод");
                }

            } while (!returnToMainFlag);
        }

        private static void DisplayUserRepositoryMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Выбран раздел 'Работа с репозиторием пользователей'");
            Console.WriteLine("Список команд:");
            Console.WriteLine($"{(int)UserRepository.Commands.Create}: добавить нового пользователя");
            Console.WriteLine($"{(int)UserRepository.Commands.ShowAll}: показать всех пользователей");
            Console.WriteLine($"{(int)UserRepository.Commands.ShowById}: показать пользователя по ID");
            Console.WriteLine($"{(int)UserRepository.Commands.UpdateEmail}: обновление E-mail У пользователя");
            Console.WriteLine($"{(int)UserRepository.Commands.Delete}: удаление пользователя");
            Console.WriteLine($"{(int)UserRepository.Commands.ReturnToMain}: вернуться в главное меню");
        }

        private static void ManageBookRepository()
        {
            bool returnToMainFlag = false;

            do
            {
                Console.WriteLine();
                DisplayBookRepositoryMenu();
                string inputBookRepo = Console.ReadLine();

                if (int.TryParse(inputBookRepo, out int bookRepoChoice))
                {
                    try
                    {
                        bookRepository.ExecuteCommand(bookRepoChoice);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    if (bookRepoChoice == (int)BookRepository.Commands.ReturnToMain)
                        returnToMainFlag = true;
                }
                else
                {
                    Console.WriteLine("Неверный ввод");
                }

            } while (!returnToMainFlag);
        }

        private static void DisplayBookRepositoryMenu()
        {
            Console.WriteLine("Выбран раздел 'Работа с репозиторием книг'");
            Console.WriteLine("Список команд:");
            Console.WriteLine($"{(int)BookRepository.Commands.Create}: добавить новую книгу");
            Console.WriteLine($"{(int)BookRepository.Commands.ShowAll}: показать все книги");
            Console.WriteLine($"{(int)BookRepository.Commands.ShowById}: показать книгу по ID");
            Console.WriteLine($"{(int)BookRepository.Commands.Update}: обновление даты издания книги");
            Console.WriteLine($"{(int)BookRepository.Commands.Delete}: удаление книги");
            Console.WriteLine($"{(int)BookRepository.Commands.Assign}: выдать книгу пользователю");
            Console.WriteLine($"{(int)BookRepository.Commands.ShowBooksByGenre}: показать книги по жанру");
            Console.WriteLine($"{(int)BookRepository.Commands.ShowBooksByGenreAndTimeRange}: поиск книг по жанру и периоду издания");
            Console.WriteLine($"{(int)BookRepository.Commands.ShowBooksAmountByAuthor}: показать количество книг по автору");
            Console.WriteLine($"{(int)BookRepository.Commands.СheckByAuthorAndBookName}: проверить есть ли книга определенного названия определенного автора");
            Console.WriteLine($"{(int)BookRepository.Commands.СheckIfBookISAssigned}: проверить выдана ли книга какому-нибудь пользователю");
            Console.WriteLine($"{(int)BookRepository.Commands.GetAssignedBooksAmount}: показать количество выданных книг");
            Console.WriteLine($"{(int)BookRepository.Commands.GetLatestIssuedBook}: показать книгу в библиотеке, изданную позже всех");
            Console.WriteLine($"{(int)BookRepository.Commands.GetBooksSortedByName}: показать книги в библиотеке, отсортированные по названию");
            Console.WriteLine($"{(int)BookRepository.Commands.GetBooksSortedByReleaseDateDesc}: показать книги в библиотеке, отсортированные по убыванию года их издания");
            Console.WriteLine($"{(int)BookRepository.Commands.ReturnToMain}: вернуться в главное меню");
        }
    }
}
