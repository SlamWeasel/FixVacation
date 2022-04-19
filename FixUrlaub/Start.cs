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

namespace FixUrlaub
{
    internal class Start
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Bruh");
            ADUser me = new ADUser();

            try
            {
                //me = new ADUser("MHahn");
                //me = new ADUser(Environment.UserName);
                Console.WriteLine(me.FullName + " -> " + me.ID + " workin in " + me.Department + " is Leader(" + me.IsLeader.ToString() + ") and works for " + (me.Leader == null ? "" : me.Leader.FullName));

                ColorTheme colorTheme = new ColorTheme(ColorTheme.DefaultBackground, ColorTheme.DefaultForeground, ColorTheme.DefaultHighlight, new KeyValuePair<string, Color>("Alert", Color.Red), new KeyValuePair<string, Color>("Good", Color.FromArgb(0, 255, 0)));
                Console.WriteLine(new ColorTheme(colorTheme.ToString()).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (!File.Exists(Environment.CurrentDirectory + "\\lang-DE.txt"))
                File.WriteAllText(Environment.CurrentDirectory + "\\lang-DE.txt",
                    "Deutsch;Urlaubsantrag<br/>für Mitarbeiter;Tag;Tage;Name;geb. am;Kontroll-Nr.;Abteilung;Mir zustehender Urlaub für das Jahr; bereits genommen, es verbleibt ein Rest von;Heute beantrage ich den nachstehend aufgeführten Urlaub;vom;bis;Jahresurlaub;Sonderurlaub;Unbezahlter Urlaub;Begründung;Arbeitnehmer bzw. Antragsteller;Datum;Unterschrift;geprüft: Vorgesetzter;Lohn-/Personalbüro;Einreichen;Fenster schließen;Einstellungen;Ändern sie die Parameter des Programms, und sogar die Farbgebung;Kalender;Genehmigen;Sprache;Benutzerdef.;Farbschema;Hintergrundfarbe;Vordergrundfarbe;Detailfarbe;Konfiguration;Datenbank-Verbindungs-String-Override;Verzeichnis-Override");
            if (!File.Exists(Environment.CurrentDirectory + "\\lang-EN.txt"))
                File.WriteAllText(Environment.CurrentDirectory + "\\lang-EN.txt",
                    "English;Vacation Request<br/>for employees;Day;Days;Name;born on;User-ID;Department;My vacation for the year;already taken, there remain;Today, I request the following vacation time;from;to;Annual leave;Special leave;Unpaid vacation;Reason;Employee or Applicant;Date;Signature;checked: Superior;Human Resources;Submit;Exit Application;Settings;Change parameters of the program and even the color theme;Calendar;Approve;Language;Custom;Color Theme;Background Color;Foreground Color;Highlight Color;Configuration;Database-Connexion-String-Override;Directory-Override");

            VacMainForm v = new VacMainForm(me);
            Application.Run(v);

        }
    }
}
