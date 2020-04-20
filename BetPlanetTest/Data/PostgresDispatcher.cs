using BetPlanetTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetPlanetTest.Data
{
    /// <summary>
    /// Диспетчер по CRUD-коммуникациям с БД test
    /// Допускает только одно одномоентное обращение к базе данных
    /// </summary>
    public class PostgresDispatcher: IDatabaseDispatcher
    {

        private static Object syncObject;

        static PostgresDispatcher()
        {
            syncObject = new Object();
        }

        #region ------------- Users----------------
        
        public int CreateUser(Users user)
        {
            Users newUser = user;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
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

            return newUser.Id;
        }

        public Users GetUserById(int id)
        {
            Users user;

            using (testContext context = new testContext())
            {
                lock (syncObject)
                {
                    user = context.Users.Find(id);
                }
            }

            return user;
        }

        public Users GetUserByName(string name)
        {
            Users user;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
                    {
                        user = context.Users.Single(u => u.Name.Equals(name));
                    }
                }
            }
            catch (Exception ex)
            {
                // должно быть залогировано
                Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                return null;
            }

            return user;
        }

        public Users GetUserByEmail(string email)
        {
            Users user;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
                    {
                        user = context.Users.Single(u => u.Email.Equals(email));
                    }
                }
            }
            catch (Exception ex)
            {
                // должно быть залогировано
                Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                return null;
            }

            return user;
        }

        public IEnumerable<Users> GetUsers()
        {
            List<Users> users;

            using (testContext context = new testContext())
            {
                lock (syncObject)
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
                    lock (syncObject)
                    {
                        Users userToUpdate = context.Users.Find(user.Id);
                        if (userToUpdate != null)
                        {
                            userToUpdate.Name = user.Name;
                            userToUpdate.Email = user.Email;
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
                    lock (syncObject)
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

        public int CreateComment(Comments comment)
        {
            Comments newComment = comment;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
                    {
                        context.Comments.Add(newComment);
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

            return newComment.Id;
        }     

        public Comments GetCommentById(int id)
        {
            Comments comment;

            using (testContext context = new testContext())
            {
                lock (syncObject)
                {
                    comment = context.Comments.Find(id);
                }
            }

            return comment;
        }

        public IEnumerable<Comments> GetCommentsByUserId(int id)
        {
            List<Comments> comments;

            using (testContext context = new testContext())
            {
                lock (syncObject)
                {
                    comments = context.Comments.Where(c => c.IdUser == id).AsQueryable().ToList();
                }
            }

            return comments;
        }

        public IEnumerable<Comments> GetComments()
        {
            List<Comments> comments;

            using (testContext context = new testContext())
            {
                lock (syncObject)
                {
                    comments = context.Comments.AsQueryable().ToList();
                }
            }

            return comments;
        }
        
        public bool UpdateComment(Comments comment)
        {
            bool isSuccess = false;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
                    {
                        Comments commentToUpdate = context.Comments.Find(comment.Id);
                        if (commentToUpdate != null)
                        {
                            commentToUpdate.IdUser = comment.IdUser;
                            commentToUpdate.Txt = comment.Txt;
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

        public bool DeleteComment(int id)
        {
            bool isSuccess = false;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncObject)
                    {
                        Comments commentToDelete = context.Comments.Find(id);
                        if (commentToDelete != null)
                        {
                            context.Comments.Remove(commentToDelete);
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


    }
}
