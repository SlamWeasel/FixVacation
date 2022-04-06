using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class Settings
    {
        private ColorTheme _theme;
        public ColorTheme Theme 
        { 
            get => _theme;
            set
            {
                _theme = value;
                UpdateConfigFile();
            }
        }
        private Language _currentLanguage;
        public Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                UpdateConfigFile();
            }
        }

        private string _sqlConnectionString;
        public string SqlConnectionString
        {
            get => _sqlConnectionString; 
            set
            {
                _sqlConnectionString = value;
                UpdateConfigFile();
            }
        }
        private string _usernameOverride;
        public string UsernameOverride
        {
            get => _usernameOverride;
            set
            {
                _usernameOverride = value;
                UpdateConfigFile();
            }
        }
        private string _directoryOverride;
        public string DirectoryOverride
        {
            get => _directoryOverride;
            set
            {
                _directoryOverride = value;
                UpdateConfigFile();
            }
        }


        public Settings()
        {
            DirectoryOverride = null;

            LoadConfig(Environment.CurrentDirectory + "\\config.txt");
        }

        private void LoadConfig(string Path)
        {
            if(DirectoryOverride == null)
            {
                // TODO: Load Config File from Path
            }
            else
            {
                // TODO: Load Config File from Override
            }
        }
        private void UpdateConfigFile()
        {
            // TODO: What the method name says
        }
    }
}
