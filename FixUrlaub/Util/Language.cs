using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class Language
    {
        public static readonly Language English = new Language(Environment.CurrentDirectory + "\\lang-EN.txt"),
                                        German = new Language(Environment.CurrentDirectory + "\\lang-DE.txt");

        public readonly string LangName,
            LogoTopLeft, 
            Day, 
            Days, 
            NameLine, 
            BornLine, 
            UserIDLine,
            DepartmentLine,
            YearTag, 
            RemainingVac, 
            Announcement, 
            From, 
            To, 
            YearVac, 
            SpecVac, 
            UnpaidVac, 
            Reason,
            EmployeeField,
            Date,
            Signature,
            SuperiorField,
            HRField,
            Submit,
            Close;

        /// <summary>
        /// Object that reads out a language-file and houses all the text that needs to be translated
        /// </summary>
        /// <param name="LangFilePath"></param>
        public Language(string LangFilePath)
        {
            string[] translations = File.ReadAllText(LangFilePath).Replace("<br/>", "\n").Split(';');

            int i = 0;

            LangName =          translations[i++];
            LogoTopLeft =       translations[i++];
            Day =               translations[i++];
            Days =              translations[i++];
            NameLine =          translations[i++];
            BornLine =          translations[i++];
            UserIDLine =        translations[i++];
            DepartmentLine =    translations[i++];
            YearTag =           translations[i++];
            RemainingVac =      translations[i++];
            Announcement =      translations[i++];
            From =              translations[i++];
            To =                translations[i++];
            YearVac =           translations[i++];
            SpecVac =           translations[i++];
            UnpaidVac =         translations[i++];
            Reason =            translations[i++];
            EmployeeField =     translations[i++];
            Date =              translations[i++];
            Signature =         translations[i++];
            SuperiorField =     translations[i++];
            HRField =           translations[i++];
            Submit =            translations[i++];
            Close =             translations[i++];
        }

        public override string ToString()
            => "FixUrlaub.Util.Language(" + LangName + ")";
    }
}
