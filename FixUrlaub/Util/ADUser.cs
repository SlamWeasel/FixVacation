using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub.Util
{
    internal class ADUser
    {
        public string Username,
                        FullName,
                        ID,
                        Department;

        public ADUser Leader;
        public bool IsLeader = false, IsHR = false;
        public DateTime? Birthday;
        public VacationInfo VI;
        private UserPrincipal UsP;
        

        public ADUser()
        {
            Username = "CFixemer";
            FullName = "Fixemer Christian";
            ID = "1";
            Department = "Management";
            Leader = null;
            Birthday = null;
        }
        public ADUser(string _Username)
        {
            Username = _Username;

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal u = UserPrincipal.FindByIdentity(ctx, this.Username);
            UsP = u;

            PullUserData(u);
        }
        public ADUser(UserPrincipal u)
        {
            Username = u.Name;
            UsP = u;

            PullUserData(u);
        }

        /// <summary>
        /// Pulls the Data of the User from the ActiveDirectory by using a <see cref="UserPrincipal"/> created usually with a UserName
        /// </summary>
        /// <param name="u"></param>
        [HandleProcessCorruptedStateExceptions]
        private void PullUserData(UserPrincipal u)
        {
            FullName = u.DisplayName;
            ID = u.EmployeeId;
            if (u.GetUnderlyingObjectType() == typeof(DirectoryEntry))
            {
                DirectoryEntry entry = (DirectoryEntry)u.GetUnderlyingObject();
                
                if (entry.Properties["department"] != null)
                    Department = entry.Properties["department"].Value.ToString();

                //Creates a new Instance of ADUser by using the "manager" Property to search for it in the AD.
                //If there is no Manager put in, CFixemer is automatically set as Manager 
                if (entry.Properties["manager"].Value != null)
                    Leader = new ADUser(
                        UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain),
                        IdentityType.DistinguishedName,
                        entry.Properties["manager"].Value.ToString()));
                else if (u.Surname == "Fixemer")
                    Leader = null;
                else
                    Leader = new ADUser();

                //Goes through all Groups and looks for the Team_Manager-Group
                foreach (string v in entry.Properties["memberOf"])
                    if (new PropertyValue(v).CN == "Team_Manager")
                        IsLeader = true;
                //Goes through all Groups and looks for the HR-Group
                foreach (string v in entry.Properties["memberOf"])
                    if (new PropertyValue(v).CN == "Human Resources")
                        IsHR = true;


                try
                {
                    using (SqlConnection cn = new SqlConnection(Settings.sqlConnectionString))
                    using (SqlCommand cmd = new SqlCommand(
                        "IF EXISTS(SELECT * FROM [Users] WHERE [UserName] = '" + Username + "')" + "\n" +
                        "BEGIN" + "\n" +
                        "   SELECT 'True' AS [Bool]" + "\n" +
                        "END" + "\n" +
                        "ELSE" + "\n" +
                        "BEGIN" + "\n" +
                        "   SELECT 'False' AS [Bool]" + "\n" +
                        "END"
                        , cn)
                    { CommandTimeout = 600 })
                    {
                        cn.Open();

                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            _ = read.Read();

                            //If the User does not already Exist
                            if (bool.Parse(read.GetString(0)))
                            {
                                read.Close();

                                using (SqlCommand getBirthday = new SqlCommand("SELECT [Birthday] FROM [Users] WHERE [UserID] = " + ID, cn) { CommandTimeout = 600 })
                                using (SqlDataReader readBirthday = getBirthday.ExecuteReader())
                                {
                                    _ = readBirthday.Read();

                                    Birthday = readBirthday.GetDateTime(0);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Tries to get a Team from the Groups the AD-User is a member of
        /// </summary>
        /// <returns></returns>
        public string GetTeamGroup()
        {
            DirectoryEntry entry = (DirectoryEntry)UsP.GetUnderlyingObject();

            foreach (string v in entry.Properties["memberOf"])
            {
                if (new PropertyValue(v).CN.Equals("Management"))
                    return "Management";
                if (new PropertyValue(v).CN.Equals("Facility Management"))
                    return "Facility Management";
                if (new PropertyValue(v).CN.Equals("Human Resources"))
                    return "Human Resources";
            }
            foreach (string v in entry.Properties["memberOf"])
            {
                if (new PropertyValue(v).CN.Equals("EDV"))
                    return "EDV";
                if (new PropertyValue(v).CN.Equals("Werkstatt"))
                    return "Werkstatt";
            }
            foreach (string v in entry.Properties["memberOf"])
                if (new PropertyValue(v).CN.EndsWith("_TM"))
                    return v.Substring(0, v.Length - 3);
            foreach (string v in entry.Properties["memberOf"])
                if (new PropertyValue(v).CN.StartsWith("TM-"))
                    return v.Substring(3);

            return "Fixemer No";
        }
    }
}
