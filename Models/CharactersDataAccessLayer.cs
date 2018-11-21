using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using tephraSystemEditor.Models;

namespace tephraSystemEditor.Models
{
    public class CharactersDataAccessLayer
    {
        string connectionString = "server=192.168.1.29;uid=test;pwd=12345;database=tephra_system";

        public IEnumerable<Character> GetCharacters(string userID)
        {
            var characters = new List<Character>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT Name, Description, CharacterID, Level FROM Characters WHERE UserID = @UserID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UserID", userID);

                con.Open();
                var rdrCharacter = cmd.ExecuteReader();

                while (rdrCharacter.Read())
                {
                    var character = new Character();
                    character.Name = rdrCharacter["Name"].ToString();
                    character.Description = rdrCharacter["Description"].ToString();
                    character.ID = (int) rdrCharacter["CharacterID"];
                    character.UserID = userID;
                    character.Level = (int) rdrCharacter["Level"];

                    characters.Add(character);
                }

                con.Close();                
            }
            return characters;
        }

        public Character GetCharacter(int id)
        {
            Character character = null;
            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT CharacterID, Name, Description, UserID, CharacterID, Level "
                +"FROM Characters WHERE CharacterID = @CharacterID"
                ,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CharacterID", id);

                con.Open();
                var rdrCharacter = cmd.ExecuteReader();

                while (rdrCharacter.Read())
                {
                    character = new Character();
                    character.ID = rdrCharacter.GetInt32("CharacterID");
                    character.Name = rdrCharacter["Name"].ToString();
                    character.Description = rdrCharacter["Description"].ToString();
                    character.UserID = rdrCharacter.GetString("UserID");
                    character.Level = (int) rdrCharacter["Level"];
                    character.Specialties = GetCharacterSpecialties(character).ToList();

                }

                con.Close();                
            }
            return character;

        }

       public IEnumerable<Specialty> GetCharacterSpecialties(Character character)
       {
            var specialties = new List<Specialty>();

            var systemdb = new TephraSystemDataAccessLayer();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "SELECT ID, CharacterID, Ch.SpecialtyID " + 
                    "FROM CharactersSpecialty AS Ch JOIN Specialties AS Sp ON Sp.SpecialtyID = Ch.SpecialtyID " +
                    "WHERE CharacterID = @CharacterID"
                    , con
                );
                cmd.Parameters.AddWithValue("@CharacterID", character.ID);
                
                con.Open();
                var rdrCharacter = cmd.ExecuteReader();
                while(rdrCharacter.Read())
                {
                    var specialtyID = rdrCharacter.GetInt32("SpecialtyID");
                    var specialty = systemdb.GetSpecialty(specialtyID);
                    specialty.ID = rdrCharacter.GetInt32("ID");
                    specialties.Add(specialty);
                }
                con.Close();

            }
            return specialties;
       }

       public void Add(Character character){

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "INSERT INTO Characters(UserID, Name, Description, Level) " + 
                    "VALUES(@UserID, @Name, @Description, @Level)"
                    , con
                );
                cmd.Parameters.AddWithValue("@UserID", character.UserID);
                cmd.Parameters.AddWithValue("@Name", character.Name);
                cmd.Parameters.AddWithValue("@Description", character.Description);
                cmd.Parameters.AddWithValue("@Level", character.Level);

                con.Open();
                cmd.ExecuteNonQuery();

                cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT LAST_INSERT_ID()", con);

                var CharacterID = Convert.ToInt32(cmd.ExecuteScalar());

                foreach(var specialty in character.Specialties)
                {
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "INSERT INTO CharactersSpecialty(CharacterID, SpecialtyID, Notes) VALUES(@CharacterID, @SpID, @Notes)"
                        , con
                    );
                    cmd.Parameters.AddWithValue("@Notes", specialty.Notes);
                    cmd.Parameters.AddWithValue("@CharacterID", CharacterID);
                    cmd.Parameters.AddWithValue("@SpID", specialty.ID);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }
        public void Delete(Character ch)
        {
            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "DELETE FROM CharactersSpecialty WHERE @CharacterID = CharacterID"
                    , con
                );
                cmd.Parameters.AddWithValue("@CharacterID", ch.ID);
                con.Open();
                cmd.ExecuteNonQuery();

                cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "DELETE FROM Characters WHERE @CharacterID = CharacterID"
                    , con
                );
                cmd.Parameters.AddWithValue("@CharacterID", ch.ID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Update(Character ch)
        {
            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "UPDATE Characters SET Name = @Name, Description = @Description, Level = @Level " + 
                    "WHERE @CharacterID = CharacterID"
                    , con
                );
                cmd.Parameters.AddWithValue("@CharacterID", ch.ID);
                cmd.Parameters.AddWithValue("@Name", ch.Name);
                cmd.Parameters.AddWithValue("@Description", ch.Description);
                cmd.Parameters.AddWithValue("@Level", ch.Level);

                con.Open();
                cmd.ExecuteNonQuery();

                foreach(var specialty in ch.Specialties)
                {
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "UPDATE CharactersSpecialty SET SpecialtyID = @SpecialtyID, Notes = @Notes)"
                        +" WHERE CharacterID = @CharacterID"
                        , con
                    );
                    cmd.Parameters.AddWithValue("@Notes", specialty.Notes);
                    cmd.Parameters.AddWithValue("@CharacterID", ch.ID);
                    cmd.Parameters.AddWithValue("@SpID", specialty.ID);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        } 
    }
}
