using EFdigitalLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EFdigitalLibrary.Repositoriess
{
    public class UserRepository
    {
        private readonly AppContext db;

        public UserRepository(AppContext dbContext)
        {
            db = dbContext;
        }

        public enum Commands
        {
            Create = 1,
            ShowAll = 2,
            ShowById = 3,
            UpdateEmail = 4,
            Delete = 5,
            ReturnToMain = 6,
        }

        public void ExecuteCommand(int userCommand)
        {
            switch (userCommand)
            {
                case (int)Commands.Create:
                    CreateUser();
                    break;

                case (int)Commands.ShowAll:
                    ShowAllUsers();
                    break;

                case (int)Commands.ShowById:
                    ShowUserbyId();
                    break;

                case (int)Commands.UpdateEmail:
                    UpdateUserEmail();
                    break;

                case (int)Commands.Delete:
                    DeleteUser();
                    break;

                case (int)Commands.ReturnToMain:
                    break;

                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }

        private void CreateUser()
        {
            Console.WriteLine("Введите имя нового пользователя");
            var nameToCreate = Console.ReadLine();
            Console.WriteLine("Введите E-mail");
            var emailToCreate = Console.ReadLine();
            Create(nameToCreate, emailToCreate);
            Console.WriteLine($"Пользователь {nameToCreate} c E-mail: {emailToCreate} добавлен в Библиотеку");
        }


        private void ShowUserbyId()
        {
            Console.WriteLine("Введите id пользователя для отображения его данных");
            int id = Convert.ToInt32(Console.ReadLine());
            ShowUserbyId(id);
        }

        private void ShowAllUsers()
        {
            var allUsers = db.Users.ToList();

            if (allUsers.Any())
            {
                foreach (var user in allUsers)
                {
                    Console.WriteLine($"Id: {user.Id} Имя: {user.Name} E-mail: {user.Email}");
                }
            }
            else
            {
                Console.WriteLine("В библиотеке не добавлено ни одного пользователя");
            }
        }

        public void Create(string name, string email)
        {
            var user = new User { Name = name, Email = email };
            db.Users.Add(user);
            db.SaveChanges();
        }

        private void DeleteUser()
        {
            Console.WriteLine("Введите имя пользователя, которого хотите удалить");
            var nameToDelete = Console.ReadLine();
            var userToDelete = db.Users.Include(u => u.Books).FirstOrDefault(u => u.Name == nameToDelete);

            if (userToDelete != null)
            {
                foreach (var book in userToDelete.Books)
                {
                    book.UserId = null;       
                }

                db.Users.Remove(userToDelete);
                db.SaveChanges();
                Console.WriteLine($"Пользователь {nameToDelete} удален");
            }
            else
            {
                Console.WriteLine($"Пользователь с именем {nameToDelete} не найден");
            }
        }


        public void UpdateUserEmail()
        {

            Console.WriteLine("Введите имя пользователя, у которого хотите обновить E-mail");
            var nameToUpdate = Console.ReadLine();
            var firstUser = db.Users.FirstOrDefault(u => u.Name == nameToUpdate);
            if (firstUser != null)
            {
                Console.WriteLine("Введите новый E-mail");
                var email = Console.ReadLine();
                firstUser.Email = email;
                db.SaveChanges();
                Console.WriteLine($"E-mail пользователя изменен на {email}");
            }
            else
            {
                Console.WriteLine($"Пользователь с именем {nameToUpdate} не найден");
            }

        }

        public void ShowUserbyId(int id)
        {
            var userById = db.Users.FirstOrDefault(u => u.Id == id);

            if (userById != null)
            {
                Console.WriteLine($"Id: {userById.Id} Имя: {userById.Name} E-mail: {userById.Email}");
            }
            else
            {
                Console.WriteLine($"Пользователь с ID {id} не найден");
            }
        }
    }
}
