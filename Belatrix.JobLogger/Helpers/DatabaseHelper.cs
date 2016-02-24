using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Belatrix.JobLogger.Helpers
{
    public interface IDatabaseHelper
    {
        string ConnectionString { get; set; }
        void Insert(string message, int messageType);
    }

    [ExcludeFromCodeCoverage]
    public class DatabaseHelper : IDatabaseHelper
    {
        public string ConnectionString { get; set; }

        public DatabaseHelper()
        {
            
        }

        public void Insert(string message, int messageType)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO log VALUES(@message, @type)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("message", message);
                    command.Parameters.AddWithValue("type", messageType);
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
