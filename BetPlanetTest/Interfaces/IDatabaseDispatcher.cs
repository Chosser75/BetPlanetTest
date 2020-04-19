using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetPlanetTest.Interfaces
{
    public interface IDatabaseDispatcher
    {
        
        Users GetUserById(int id);
        IEnumerable<Users> GetUsers();
        int CreateUser(Users user);
        bool UpdateUser(Users user);
        bool DeleteUser(int id);
        Comments GetCommentById(int id);
        IEnumerable<Comments> GetComments();
        int CreateComment(Comments user);
        bool UpdateComment(Comments user);
        bool DeleteComment(int id);

    }
}
