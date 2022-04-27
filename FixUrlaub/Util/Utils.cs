using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub.Util
{
    internal abstract class Utils
    {
        public static void AddHoverPointer(Control c)
        {
            c.MouseEnter += (sender, e) =>
            {
                try
                {
                    c.Parent.Cursor = Cursors.Hand;
                }
                catch { }
            };
            c.MouseLeave += (sender, e) =>
            {
                try
                {
                    c.Parent.Cursor = Cursors.Default;
                }
                catch { }
            };
        }

        public static Dictionary<string, VacationInfo> GetMyTeamsVacation(string user, DateRange searchArea)
        {
            Dictionary<string, VacationInfo> OUT = new Dictionary<string, VacationInfo>();

            using (SqlConnection cn = new SqlConnection(Settings.sqlConnectionString))
            using (SqlCommand cmd = new SqlCommand("" +
                "SELECT" +                                                                                          " \n" +
                "    (SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Sender]) AS [UserName]," +     " \n" +
                "    [StartDat]," +                                                                                 " \n" +
                "    [EndDat]," +                                                                                   " \n" +
                "    FORMAT([VacAmount], 'N', 'en-us') AS VacAmount" +                                              " \n" +
                "FROM [Jobs]" +                                                                                     " \n" +
                "WHERE" +                                                                                           " \n" +
                "    [Sender] IN" +                                                                                 " \n" +
                "        (SELECT [UserID] FROM [Users] WHERE [TeamID] =" +                                          " \n" +
                "            (SELECT [TeamID] FROM [Users] WHERE [UserName] = '" + user + "' " +                    " \n" +
                "            )" +                                                                                   " \n" +
                "		)" +                                                                                        " \n" +
                "    AND [Stage1Passed] = 1 " +                                                                     " \n" +
                "    AND [Aborted] = 0 " +                                                                          " \n" +
                "    AND ([StartDat] BETWEEN " + searchArea.ToSqlTimestampRange() + " OR [EndDat] BETWEEN " + searchArea.ToSqlTimestampRange() + ") \n" +
                "ORDER BY [JobID] ASC" +
                "", cn){ CommandTimeout = 600 })
            {
                cn.Open();
                using(SqlDataReader read = cmd.ExecuteReader())
                    while(read.Read())
                    {
                        string username = read.GetString(0);
                        DateTime start = read.GetDateTime(1);
                        DateTime end = read.GetDateTime(2);
                        float amount = float.Parse(read.GetString(3));

                        Console.WriteLine("Read a line");

                        if (OUT.ContainsKey(username))
                            OUT[username].TakenDays
                                .Add(new DateRange(
                                        start,
                                        end),
                                    amount);
                        else
                            OUT.Add(
                                username,
                                new VacationInfo(
                                    new DateRange(
                                        start,
                                        end),
                                    amount));
                    }
            }

            return OUT;
        }
    }
}