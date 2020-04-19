using BetPlanetTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetPlanetTest.Data
{
    public class PostgresDispatcher: IDatabaseDispatcher
    {

        private static Object syncUsersObject;
        private static Object syncCommentsObject;

        static PostgresDispatcher()
        {
            syncUsersObject = new Object();
            syncCommentsObject = new Object();
        }

        #region ------------- Users----------------
        
        public int CreateUser(Users user)
        {
            Users newUser = user;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncUsersObject)
                    {
                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // должно быть залогировано
                Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                return -1;
            }

            return user.Id;
        }

        public Users GetUserById(int id)
        {
            Users user;

            using (testContext context = new testContext())
            {
                lock (syncUsersObject)
                {
                    user = context.Users.Find(id);
                }
            }

            return user;
        }

        public IEnumerable<Users> GetUsers()
        {
            List<Users> users;

            using (testContext context = new testContext())
            {
                lock (syncUsersObject)
                {
                    users = context.Users.AsQueryable().ToList();
                }
            }

            return users;
        }

        public bool UpdateUser(Users user)
        {
            bool isSuccess = false;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncUsersObject)
                    {
                        Users userToUpdate = context.Users.Find(user.Id);
                        if (userToUpdate != null)
                        {
                            userToUpdate = user;
                            int result = context.SaveChanges();
                            if (result > 0)
                            {
                                isSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // должно быть залогировано
                Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                return false;
            }

            return isSuccess;
        }

        public bool DeleteUser(int id)
        {
            bool isSuccess = false;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncUsersObject)
                    {
                        Users userToDelete = context.Users.Find(id);
                        if (userToDelete != null)
                        {
                            context.Users.Remove(userToDelete);
                            int result = context.SaveChanges();                            
                            if(result > 0)
                            {
                                isSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // должно быть залогировано
                Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                return false;
            }

            return isSuccess;
        }

        #endregion ----------- Users ---------------

        public int CreateComment(Comments user)
        {
            throw new NotImplementedException();
        }     

        public Comments GetCommentById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comments> GetComments()
        {
            throw new NotImplementedException();
        }
        
        public bool UpdateComment(Comments user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteComment(int id)
        {
            throw new NotImplementedException();
        }

    }
}
