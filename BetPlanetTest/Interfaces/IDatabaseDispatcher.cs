﻿using System.Collections.Generic;

namespace BetPlanetTest.Interfaces
{
    public interface IDatabaseDispatcher
    {
        
        Users GetUserById(int id);
        Users GetUserByName(string name);
        Users GetUserByEmail(string email);
        IEnumerable<Users> GetUsers();
        int CreateUser(Users user);
        bool UpdateUser(Users user);
        bool DeleteUser(int id);
        Comments GetCommentById(int id);
        IEnumerable<Comments> GetCommentsByUserId(int id);
        IEnumerable<Comments> GetComments();
        int CreateComment(Comments user);
        bool UpdateComment(Comments user);
        bool DeleteComment(int id);

    }
}
