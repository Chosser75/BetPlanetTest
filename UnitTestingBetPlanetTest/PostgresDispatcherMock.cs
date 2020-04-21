using BetPlanetTest;
using BetPlanetTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestingBetPlanetTest
{
    internal class PostgresDispatcherMock : IDatabaseDispatcher
    {
        private List<Users> users;
        private List<Comments> comments;

        public PostgresDispatcherMock()
        {
            PopulateLists();
        }
               

        public bool CheckIfRecordExists<T>(int id)
        {
            if (typeof(T) == typeof(Users))
            {                
                return users.Find(u => u.Id == id) == null ? false : true;
            }
            else if (typeof(T) == typeof(Comments))
            {
                return comments.Find(c => c.Id == id) == null ? false : true;
            }
            return false;
        }

        public int Create<T>(IModel record)
        {
            T rec = (T)record;

            if ((rec as Users).Id == 0) return 8;

            if (typeof(T) == typeof(Users))
            {
                return users.Find(u => u.Id == (rec as Users).Id) != null ? -1 : (rec as Users).Id;
            }
            else if (typeof(T) == typeof(Comments))
            {
                return comments.Find(c => c.Id == (rec as Comments).Id) != null ? -1 : (rec as Comments).Id;
            }
            return -1;
        }

        public bool DeleteComment(int id)
        {
            return comments.Find(c => c.Id == id) != null ? true : false;
        }

        public bool DeleteUser(int id)
        {
            return users.Find(u => u.Id == id) != null ? true : false;
        }

        public IModel GetById<T>(int id)
        {
            if (id > 0)
            {
                if (typeof(T) == typeof(Users))
                {
                    Console.WriteLine("--- DispatcherMock: returning user");
                    return users.Find(u => u.Id == id);
                }
                else if (typeof(T) == typeof(Comments))
                {
                    Console.WriteLine("--- DispatcherMock: returning comment");
                    return new Comments() { Id = 1, IdUser = 1, Txt = "test txt" };
                }
            }
                        
            Console.WriteLine("--- DispatcherMock: returning null");
            return null;
        }

        public IEnumerable<Comments> GetCommentsByUserId(int id)
        {
            return comments.FindAll(c => c.IdUser == id);
        }

        public IEnumerable<IModel> GetRecords<T>()
        {
            if (typeof(T) == typeof(Users))
            {
                Console.WriteLine("--- DispatcherMock: returning users");
                return users;
            }
            else if (typeof(T) == typeof(Comments))
            {
                Console.WriteLine("--- DispatcherMock: returning comments");
                return comments;
            }

            return null;
        }

        public Users GetUserByEmail(string email)
        {
            return users.Find(u => u.Name.Equals(email));
        }

        public Users GetUserByName(string name)
        {
            return users.Find(u => u.Name.Equals(name));
        }

        public bool UpdateComment(Comments comment)
        {
            return comments.Find(c => c.Id == comment.Id) != null ? true : false;
        }

        public bool UpdateUser(Users user)
        {
            return users.Find(c => c.Id == user.Id) != null ? true : false;
        }

        private void PopulateLists()
        {
            users = new List<Users>();
            comments = new List<Comments>();

            for (int i = 1; i < 8; i++)
            {                
                users.Add(new Users() { Id = i, Name = "test name" + i, Email = "test email" + i });
                comments.Add(new Comments() { Id = i, IdUser = (i < 5 ? 1 : 2), Txt = "test text" + i });
            }
        }
    }
}
