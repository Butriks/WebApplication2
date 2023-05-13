using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using WebApplication2.Models;

namespace WebApplication2.Database;

public class UsersTable
{

    private readonly DB db;

    public UsersTable(DB db) => this.db = db;

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            var hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hashedString;

        }
    }

    public void AddUser(string username, string email, string password)
    {
        string hashPass = HashPassword(password);

        string query = $"INSERT INTO users (username, email, password) VALUES ('{username}','{email}','{hashPass}')";

        db.ExecuteNonQuery(query);
    }

    public void DeleteUser(int userId)
    {
        string query = $"DELETE FROM users WHERE user_id = {userId}";

        db.ExecuteNonQuery(query);
    }

    public User? GetUser(int userId)
    {

        string query = $"SELECT * FROM users WHERE user_id = {userId}";

        MySqlDataReader reader = db.ExecuteReader(query);

        User findedUser = null;

        if (reader != null && reader.HasRows)
        {
            reader.Read();

            int user_id = reader.GetInt32("user_id");

            string username = reader.GetString("username");

            string email = reader.GetString("email");

            string password = reader.GetString("password");

            findedUser = new User
            {
                UserId = user_id,
                UserName = username,
                Email = email,
                PasswordHash = password
            };
        }

        DB.CloseDataReader(reader);

        return findedUser;
    }

    public string GetAllUsers()//List<User>
    {

        string query = "SELECT * FROM users";

        MySqlDataReader reader = db.ExecuteReader(query);



        StringBuilder sb = new StringBuilder();



        while (reader.Read())

        {

            int userId = reader.GetInt32(0);

            string username = reader.GetString(1);

            string email = reader.GetString(2);

            //string password = reader.GetString(3);



            //sb.AppendLine($"User ID: {userId}, Username: {username}, Email: {email}, HeshPass: {password}");

            sb.AppendLine($"User ID: {userId}, Username: {username}, Email: {email}");

        }



        DB.CloseDataReader(reader);



        return sb.ToString();

    }



    public bool CheckEmail(string email, int id)

    {

        string query = $"SELECT email FROM users WHERE user_id = {id}";



        MySqlDataReader reader = db.ExecuteReader(query);



        reader.Read();

        string emailFromDB = reader.GetString("email");



        return emailFromDB == email;

    }



    public bool CheckUserName(string username, int id)
    {
        string query = $"SELECT * FROM users WHERE username = {username}";

        MySqlDataReader reader = db.ExecuteReader(query);

        reader.Read();

        string usernameFromDB = reader.GetString("username");

        return usernameFromDB == username;
    }



    public bool CheckPassword(string password, int id)

    {

        string query = $"SELECT password FROM users WHERE user_id = {id}";



        MySqlDataReader reader = db.ExecuteReader(query);



        reader.Read();

        string hashedPasswordFromDB = reader.GetString("password");



        string hashedPassword = HashPassword(password);



        return hashedPassword == hashedPasswordFromDB;

    }

    public int GetNumberOfUsers()

    {

        int numberOfUsers = 0;

        string query = "SELECT COUNT(*) FROM users;";

        MySqlDataReader reader = db.ExecuteReader(query);



        if (reader.Read())

        {

            numberOfUsers = Convert.ToInt32(reader[0]);

        }



        return numberOfUsers;

    }

    public bool CheckAuthorised(string username, string password)

    {

        string query = $"SELECT password FROM users WHERE username = '{username}'";

        string hashedPasswordFromDB = db.ExecuteScalar(query) as string;



        if (hashedPasswordFromDB == null)

        {

            return false;

        }



        string hashedPassword = HashPassword(password);



        return hashedPasswordFromDB == hashedPassword;

    }

    public bool CheckUserName(string username)
    {
        string query = $"SELECT COUNT(*) FROM users WHERE username = '{username}';";

        int count = Convert.ToInt32(db.ExecuteScalar(query));

        return count > 0;
    }

    public bool CheckUserEmail(string email)

    {

        string query = $"SELECT COUNT(*) FROM users WHERE email = '{email}';";

        int count = Convert.ToInt32(db.ExecuteScalar(query));



        return count > 0;

    }



    public int GetUserId(string username)

    {

        string query = $"SELECT user_id FROM users WHERE username = '{username}'";



        MySqlDataReader reader = db.ExecuteReader(query);



        reader.Read();

        int userIdFromDB = reader.GetInt32("user_id");



        return userIdFromDB;

    }

}

