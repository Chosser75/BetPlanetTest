using System.Collections.Generic;

namespace BetPlanetTest.Interfaces
{
    public interface IDatabaseDispatcher
    {

        int Create<T>(IModel record);
        IModel GetById<T>(int id);
        IEnumerable<IModel> GetRecords<T>();

        Users GetUserByName(string name);
        Users GetUserByEmail(string email); 
        bool UpdateUser(Users user);
        bool DeleteUser(int id);

        
        IEnumerable<Comments> GetCommentsByUserId(int id);
        bool UpdateComment(Comments user);
        bool DeleteComment(int id);

        bool CheckIfRecordExists<T>(int id);

    }
}
