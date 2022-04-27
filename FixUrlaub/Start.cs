using FixUrlaub.Masks;
using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FixUrlaub
{
    internal class Start
    {
        protected static string sqlConnectionString = @"Data Source=NT-WINSPEDTEST2\WINSPEDTEST2;Initial Catalog=FixVacation;User ID=gobabygo;Password=comeback";
        private static ADUser me = null;

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                me = new ADUser("JSchuler");
                //me = new ADUser(Environment.UserName);
                Console.WriteLine(me.FullName + " -> " + me.ID + " workin in " + me.Department + " is Leader(" + me.IsLeader.ToString() + ") and works for " + (me.Leader == null ? "" : me.Leader.FullName));

                ColorTheme colorTheme = new ColorTheme(ColorTheme.DefaultBackground, ColorTheme.DefaultForeground, ColorTheme.DefaultHighlight, new KeyValuePair<string, Color>("Alert", Color.Red), new KeyValuePair<string, Color>("Good", Color.FromArgb(0, 255, 0)));
                Console.WriteLine(new ColorTheme(colorTheme.ToString()).ToString());


                //Checks if the User already Exists in the SQL-Database
                using (SqlConnection cn = new SqlConnection(sqlConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "IF EXISTS(SELECT * FROM [Users] WHERE [UserName] = '" + me.Username + "')" + "\n" +
                    "BEGIN" + "\n" +
                    "   SELECT 'True' AS [Bool]" + "\n" +
                    "END" + "\n" +
                    "ELSE" + "\n" +
                    "BEGIN" + "\n" +
                    "   SELECT 'False' AS [Bool]" + "\n" +
                    "END"
                    , cn) { CommandTimeout = 600 })
                {
                    cn.Open();

                    using (SqlDataReader read = cmd.ExecuteReader())
                    {
                        read.Read();

                        //If the User does not already Exist
                        if (!bool.Parse(read.GetString(0)))
                        {
                            read.Close();
                            if (AddUserToDB(me, cn))
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (!File.Exists(Environment.CurrentDirectory + "\\lang-DE.txt"))
                File.WriteAllText(Environment.CurrentDirectory + "\\lang-DE.txt",
                    "Deutsch;Urlaubsantrag<br/>für Mitarbeiter;Tag;Tage;Name;geb. am;Kontroll-Nr.;Abteilung;Mir zustehender Urlaub für das Jahr; bereits genommen, es verbleibt ein Rest von;Heute beantrage ich den nachstehend aufgeführten Urlaub;vom;bis;Jahresurlaub;Sonderurlaub;Unbezahlter Urlaub;Begründung;Arbeitnehmer bzw. Antragsteller;Datum;Unterschrift;geprüft: Vorgesetzter;Lohn-/Personalbüro;Einreichen;Fenster schließen;Einstellungen;Ändern sie die Parameter des Programms, und sogar die Farbgebung;Kalender;Genehmigen;Sprache;Benutzerdef.;Farbschema;Hintergrundfarbe;Vordergrundfarbe;Detailfarbe;Konfiguration;Datenbank-Verbindungs-String-Override;Verzeichnis-Override;Erlauben;Ablehnen");
            if (!File.Exists(Environment.CurrentDirectory + "\\lang-EN.txt"))
                File.WriteAllText(Environment.CurrentDirectory + "\\lang-EN.txt",
                    "English;Vacation Request<br/>for employees;Day;Days;Name;born on;User-ID;Department;My vacation for the year;already taken, there remain;Today, I request the following vacation time;from;to;Annual leave;Special leave;Unpaid vacation;Reason;Employee or Applicant;Date;Signature;checked: Superior;Human Resources;Submit;Exit Application;Settings;Change parameters of the program and even the color theme;Calendar;Approve;Language;Custom;Color Theme;Background Color;Foreground Color;Highlight Color;Configuration;Database-Connexion-String-Override;Directory-Override;Allow;Deny");

            VacMainForm v = new VacMainForm(me);
            Application.Run(v);
        }

        /// <summary>
        /// Opens a Window for the User to put in Data and get added to the SQL-Database
        /// </summary>
        /// <param name="us"></param>
        /// <param name="con"></param>
        public static bool AddUserToDB(ADUser us, SqlConnection con)
        {
            ValuePair<DateTime?, string> birthTeam = new ValuePair<DateTime?, string>(DateTime.Today, us.GetTeamGroup());

            VacADLogin v = new VacADLogin(us, ref birthTeam)
            {
                Bounds = new Rectangle(800, 400, 200, 300)
            };

            v.SetDesktopLocation(v.Location.X, v.Location.Y);
            if (DialogResult.OK == v.ShowDialog())
            {
                using (SqlCommand cmdEx = new SqlCommand(
                    "INSERT INTO [Users] VALUES" +
                    "(" +
                    "" + us.ID + ", " +
                    "'" + us.Username + "', " +
                    "" + "(SELECT TOP 1 [TeamID] FROM [Teams] WHERE [TeamName] LIKE '" + birthTeam.Value2 + "'), " +
                    "" + "{ts '" + (birthTeam.Value1 ?? DateTime.MaxValue).ToString("yyyy-MM-dd") + "  00:00:00'}" +
                    ");"
                    , con))
                    _ = cmdEx.ExecuteNonQuery();

                me.Birthday = birthTeam.Value1 ?? DateTime.MaxValue;
            }
            else return true;

            return false;
        }
    }
}