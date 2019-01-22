using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using tephraSystemEditor.Models;


//Change attributes and skills to use enums instead of fetching this stuff from the sql database, this is hella inefficent. 

namespace tephraSystemEditor.Models
{
    public class TephraSystemDataAccessLayer
    {
        string connectionString = "server=192.168.1.29;uid=test;pwd=12345;database=tephra_system";
        

        public IEnumerable<Attr> GetAllAttributes( bool fill = true)
        {
            var attributes = new List<Attr>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT AttributeID, Name, Description FROM Attributes Order By AttributeID", con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrAttributes = cmd.ExecuteReader();

                while (rdrAttributes.Read())
                {
                    var attribute = new Attr();

                    attribute.ID = Convert.ToInt32(rdrAttributes["AttributeID"]);
                    attribute.Name = rdrAttributes["Name"].ToString();
                    attribute.Description = rdrAttributes["Description"].ToString();
                    attribute.Skills = GetSkills(attribute.ID).ToList();

                    attributes.Add(attribute);
                }

                con.Close();                
            }
            return attributes;
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            var skills = new List<Skill>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SkillID, Name, Description FROM Skills", con);

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                while (rdrSkills.Read())
                {
                    var skill = new Skill();

                    skill.ID = Convert.ToInt32(rdrSkills["SkillID"]);
                    skill.Name = rdrSkills["Name"].ToString();
                    skill.Description = rdrSkills["Description"].ToString();

                    skill.Specialties = GetSpecialties(skill.ID);
                    
                    skills.Add(skill);
                }

                con.Close();                
            }
            return skills;
        }

        public IEnumerable<Skill> GetSkills(int attributeID, bool fill = true)
        {
            var skills = new List<Skill>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SkillID, Name, Description FROM Skills WHERE AttributeID = @attributeID Order By AttributeID", con);
                cmd.Parameters.AddWithValue("@attributeID", attributeID);

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                while (rdrSkills.Read())
                {
                    var skill = new Skill();

                    skill.ID = Convert.ToInt32(rdrSkills["SkillID"]);
                    skill.Name = rdrSkills["Name"].ToString();
                    skill.Description = rdrSkills["Description"].ToString();
                    if(fill)
                        skill.Specialties = GetSpecialties(skill.ID);

                    skills.Add(skill);
                }

                con.Close();                
            }
            return skills;
        }

        public Skill GetSkill(string name)
        {
            var skills = new List<Skill>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SkillID, Name, Description FROM Skills WHERE Name = @Name", con);
                cmd.Parameters.AddWithValue("@Name", name);

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                var skill = new Skill();

                skill.ID = Convert.ToInt32(rdrSkills["SkillID"]);
                skill.Name = rdrSkills["Name"].ToString();
                skill.Description = rdrSkills["Description"].ToString();
                
                skill.Specialties = GetSpecialties(skill.ID);

                con.Close();                
                
                return skill;
            }
        }

        public Specialty GetSpecialty(int specialtyID){
            
            Specialty specialty = null;

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SkillID, Name, Description FROM Specialties WHERE SpecialtyID = @SpecialtyID", con);
                cmd.Parameters.AddWithValue("@SpecialtyID", specialtyID);

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                while (rdrSkills.Read())
                {
                    specialty = new Specialty();

                    specialty.ID = specialtyID;
                    specialty.SkillID = rdrSkills.GetInt32("SkillID");
                    specialty.Name = rdrSkills["Name"].ToString();
                    specialty.Description = rdrSkills["Description"].ToString();

                    specialty.Bonuses = GetBonuses(specialty.ID).ToList();
                }

                con.Close();                
            }
            return specialty;
        }
        public IEnumerable<Specialty> GetSpecialties(int skillID)
        {
            var specialties = new List<Specialty>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SpecialtyID, Name, Description FROM Specialties WHERE SkillID = @SkillID", con);
                cmd.Parameters.AddWithValue("@SkillID", skillID);

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                while (rdrSkills.Read())
                {
                    var specialty = new Specialty();

                    specialty.SpecialtyID = Convert.ToInt32(rdrSkills["SpecialtyID"]);
                    specialty.SkillID = skillID;
                    specialty.Name = rdrSkills["Name"].ToString();
                    specialty.Description = rdrSkills["Description"].ToString();
                    specialty.Bonuses = GetBonuses(specialty.SpecialtyID).ToList();

                    specialties.Add(specialty);
                }

                con.Close();                
            }
            return specialties;
        }

        public IEnumerable<Specialty> GetAllSpecialties()
        {
            var specialties = new List<Specialty>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand
                ("SELECT SpecialtyID, SkillID, Name, Description FROM Specialties", con); 

                cmd.CommandType = CommandType.Text;

                con.Open();
                var rdrSkills = cmd.ExecuteReader();

                while (rdrSkills.Read())
                {
                    var specialty = new Specialty();

                    specialty.SpecialtyID = Convert.ToInt32(rdrSkills["SpecialtyID"]);
                    specialty.SkillID = rdrSkills.GetInt32("SkillID");
                    specialty.Name = rdrSkills["Name"].ToString();
                    specialty.Description = rdrSkills["Description"].ToString();
                    specialty.Bonuses = GetBonuses(specialty.SpecialtyID).ToList();

                    specialties.Add(specialty);
                }

                con.Close();                
            }
            return specialties;
        }

        public IEnumerable<Bonus> GetBonuses(int specialtyID){

            var bonuses = new List<Bonus>();

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                //need to grab bonuses somehow;
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "SELECT BonusID, Type, Value FROM SpecialtyValues WHERE SpecialtyID = @spID"
                    ,con
                    );
                cmd.Parameters.AddWithValue("@spID", specialtyID);
                cmd.CommandType = CommandType.Text;
                
                con.Open();
                var rdrBonus = cmd.ExecuteReader();
                while (rdrBonus.Read())
                {
                    var bonus = new Bonus();
                    bonus.ID = Convert.ToInt32(rdrBonus["BonusID"]);
                    bonus.Value = Convert.ToInt32(rdrBonus["Value"]);
                    if(Enum.TryParse(typeof(Mod), rdrBonus["Type"].ToString(), true, out var t))
                        bonus.mod = (Mod) t;
                    bonuses.Add(bonus);
                }
                con.Close();
            }
            return bonuses;
        }

        public void Delete(int specialtyID)
        {
            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "DELETE FROM SpecialtyValues WHERE @specialtyID = SpecialtyID"
                    , con
                );
                cmd.Parameters.AddWithValue("@specialtyID", specialtyID);
                con.Open();
                cmd.ExecuteNonQuery();

                cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "DELETE FROM Specialties WHERE @specialtyID = SpecialtyID"
                    , con
                );
                cmd.Parameters.AddWithValue("@specialtyID", specialtyID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Add(Specialty specialty){
            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "INSERT INTO Specialties(SkillID, Name, Description) VALUES(@SkillID, @Name, @Description)"
                    , con
                );
                cmd.Parameters.AddWithValue("@SkillID", specialty.SkillID);
                cmd.Parameters.AddWithValue("@Name", specialty.Name);
                cmd.Parameters.AddWithValue("@Description", specialty.Description);

                con.Open();
                cmd.ExecuteNonQuery();

                cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT LAST_INSERT_ID()", con);

                var spID = Convert.ToInt32(cmd.ExecuteScalar());

                foreach(var bonus in specialty.Bonuses)
                {
                    if(bonus.Value == 0)
                        continue;
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "INSERT INTO SpecialtyValues(SpecialtyID, Type, Value) VALUES(@SpID, @Type, @Value)"
                        , con
                    );
                    cmd.Parameters.AddWithValue("@SpID", spID);
                    cmd.Parameters.AddWithValue("@Type", bonus.mod.ToString());
                    cmd.Parameters.AddWithValue("@Value", bonus.Value);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public void Update(Specialty specialty)
        {

            using(var con = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {

                MySql.Data.MySqlClient.MySqlCommand cmd;
                con.Open();

                foreach(var bonus in specialty.Bonuses)
                {
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "UPDATE SpecialtyValues SET Type = @type, Value = @value WHERE @bonusID = BonusID"
                        , con
                    );
                    cmd.Parameters.AddWithValue("@bonusID", bonus.ID);
                    cmd.Parameters.AddWithValue("@type", bonus.mod.ToString());
                    cmd.Parameters.AddWithValue("@value", bonus.Value);
                    cmd.ExecuteNonQuery();
                }

                cmd = new MySql.Data.MySqlClient.MySqlCommand(
                    "UPDATE Specialties SET Name = @Name, Description = @Description WHERE @specialtyID = SpecialtyID"
                    , con
                );
                cmd.Parameters.AddWithValue("@specialtyID", specialty.ID);
                //cmd.Parameters.AddWithValue("@SkillID", specialty.SkillID);
                cmd.Parameters.AddWithValue("@Name", specialty.Name);
                cmd.Parameters.AddWithValue("@Description", specialty.Description);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

    }
}
