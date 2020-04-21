using BetPlanetTest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetPlanetTest.Data
{
    /// <summary>
    /// Диспетчер по CRUD-коммуникациям с БД test
    /// Допускает только одно одномоментное обращение к таблице базы данных
    /// </summary>
    public class PostgresDispatcher: IDatabaseDispatcher
    {

        private static Object syncUsersObject;
        private static Object syncCommentsObject;
        private IConfiguration configuration;

        public PostgresDispatcher(IConfiguration config)
        {
            this.configuration = config;
        }

        static PostgresDispatcher()
        {
            syncUsersObject = new Object();
            syncCommentsObject = new Object();
        }

        #region -------------------------------- General ------------------------------------------

        public int Create<T>(IModel record)
        {
            T newRecord = (T)record;
            int id = -1;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncUsersObject)
                    {
                        if (newRecord.GetType() == typeof(Users))
                        {
                            context.Users.Add(newRecord as Users);
                            id = (newRecord as Users).Id;
                        }
                        else if (newRecord.GetType() == typeof(Comments))
                        {
                            context.Comments.Add(newRecord as Comments);
                            id = (newRecord as Comments).Id;
                        }
                        else
                        {
                            return -1;
                        }

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

            return id;
        }

        public IModel GetById<T>(int id)
        {            
            using (testContext context = new testContext())
            {
                lock (syncUsersObject)
                {
                    if (typeof(T) == typeof(Users))
                    {
                        return context.Users.Find(id);
                    }
                    else if (typeof(T) == typeof(Comments))
                    {
                        return context.Comments.Find(id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }            
        }

        public IEnumerable<IModel> GetRecords<T>()
        {
            using (testContext context = new testContext())
            {
                lock (syncUsersObject)
                {
                    if (typeof(T) == typeof(Users))
                    {
                        return context.Users.AsQueryable().ToList();
                    }
                    else if (typeof(T) == typeof(Comments))
                    {
                        return context.Comments.AsQueryable().ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }           
        }

        #endregion ----------------------------- General ------------------------------------------

        #region --------------------------------- Users -------------------------------------------

        public Users GetUserByName(string name)
        {
            Users user;

            try
            {
                using (testContext context = new testContext())
                {
                    lock (syncUsersObject)
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
                    lock (syncUsersObject)
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
        
        public bool UpdateUser(Users user)
        {                          
            bool isSuccess = false;
            bool isProcessed = false;
            int timeout = configuration.GetValue<int>("DbDispatcher:dbtimeout");
            DateTime startTime = DateTime.Now;

            while (!isProcessed)
            {
                try
                {
                    using (testContext context = new testContext())
                    {
                        lock (syncUsersObject)
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
                            isProcessed = true;
                        }
                    }
                }
                catch (DbUpdateConcurrencyException dbex)
                {
                    // должно быть залогировано
                    Debug.WriteLine(dbex.Message + " ===== " + dbex.StackTrace);
                    if (DateTime.Now > startTime.AddSeconds(timeout))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // должно быть залогировано
                    Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                    return false;
                }
            }

            return isSuccess;
        }

        public bool DeleteUser(int id)
        {
            bool isSuccess = false;
            bool isProcessed = false;
            int timeout = configuration.GetValue<int>("DbDispatcher:dbtimeout");
            DateTime startTime = DateTime.Now;

            while (!isProcessed)
            {
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
                                if (result > 0)
                                {
                                    isSuccess = true;
                                }
                            }
                            isProcessed = true;
                        }
                    }
                }
                catch (DbUpdateConcurrencyException dbex)
                {
                    // должно быть залогировано
                    Debug.WriteLine(dbex.Message + " ===== " + dbex.StackTrace);
                    if (DateTime.Now > startTime.AddSeconds(timeout))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // должно быть залогировано
                    Debug.WriteLine(ex.Message + " ===== " + ex.StackTrace);
                    return false;
                }
            }            

            return isSuccess;
        }

        #endregion ------------------------------ Users -------------------------------------------


        #region ------------------------------- Comments ------------------------------------------
                
        public Comments GetCommentById(int id)
        {
            Comments comment;

            using (testContext context = new testContext())
            {
                lock (syncCommentsObject)
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
                lock (syncCommentsObject)
                {
                    comments = context.Comments.Where(c => c.IdUser == id).AsQueryable().ToList();
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
                    lock (syncCommentsObject)
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
                    lock (syncUsersObject)
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

        #endregion ---------------------------- Comments ------------------------------------------


        public bool CheckIfRecordExists<T>(int id)
        {
            bool result = false;

            using (testContext context = new testContext())
            {
                if (typeof(T) == typeof(Users))
                {
                    result = context.Users.Any(u => u.Id == id);
                }
                else if (typeof(T) == typeof(Comments))
                {
                    result = context.Comments.Any(c => c.Id == id);
                }
            }

            return result;
        }
               

    }
}
