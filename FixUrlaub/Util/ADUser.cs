﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class ADUser
    {
        public string Username,
                        FullName,
                        ID,
                        Department;
        public ADUser Leader;
        public bool IsLeader = false;
        public DateTime Birthday;
        public VacationInfo VI;
        

        public ADUser() : this("CFixemer") { }
        public ADUser(string _Username)
        {
            this.Username = _Username;

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal u = UserPrincipal.FindByIdentity(ctx, this.Username);

            PullUserData(u);
        }
        public ADUser(UserPrincipal u)
        {
            this.Username = u.Name;
            PullUserData(u);
        }

        /// <summary>
        /// Pulls the Data of the User from the ActiveDirectory by using a <see cref="UserPrincipal"/> created usually with a UserName
        /// </summary>
        /// <param name="u"></param>
        [HandleProcessCorruptedStateExceptions]
        private void PullUserData(UserPrincipal u)
        {
            this.FullName = u.DisplayName;
            this.ID = u.EmployeeId;
            if (u.GetUnderlyingObjectType() == typeof(DirectoryEntry))
            {
                using (DirectoryEntry entry = (DirectoryEntry)u.GetUnderlyingObject())
                {
                    if (entry.Properties["department"] != null)
                        this.Department = entry.Properties["department"].Value.ToString();

                    //Creates a new Instance of ADUser by using the "manager" Property to search for it in the AD.
                    //If there is no Manager put in, CFixemer is automatically put as Manager 
                    if (entry.Properties["manager"].Value != null)
                        this.Leader = new ADUser(
                            UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain),
                            IdentityType.DistinguishedName,
                            entry.Properties["manager"].Value.ToString()));
                    else if (u.Surname == "Fixemer")
                        this.Leader = null;
                    else
                        this.Leader = new ADUser();

                    if (entry.Properties["manager"] != null)
                        foreach (string v in entry.Properties["memberOf"])
                            if (new PropertyValue(v).CN == "Team_Manager")
                                this.IsLeader = true;
                }

                // TODO: Pull Data from SQL-Database
            }
        }
    }
}