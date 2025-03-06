using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.Model;

namespace Text_Swap.Services
{
    public static class ShortcutTimeService
    {
        private static  double _typingSpeed; // Caractères par seconde
        private static  double _errorRate; // Pourcentage d'erreurs
        private static  double _thinkingTime; // Temps de réflexion moyen (s)
        private static  double _shortcutExecutionTime; // Temps d'utilisation du raccourci (s)
        private static int _frequency; // frequence d'utilisation du raccourci

        static ShortcutTimeService()
        {
            LoadSettings();
        }
        public static void LoadSettings()
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                        .Build();
            var settings = config.GetSection("WinTimeShorcutParam");

            _typingSpeed = settings.GetValue<double>("TypingSpeedInSecond", 5);
            _errorRate = settings.GetValue<double>("ErrorRate", 0.02);
            _thinkingTime = settings.GetValue<double>("ThinkingTimeInSecond", 1);
            _shortcutExecutionTime = settings.GetValue<double>("ShortcutExecutionTime", 0.5);
            _frequency = settings.GetValue<int>("FrequencyShorcutUsage", 10);
        }

        public static TimeSpan CalculateTimeSaved(string replacement, int frequency)
        {
            // Temps moyen de frappe sans raccourci
            double manualTypingTime = (replacement.Length / _typingSpeed) + (_thinkingTime);

            // Ajout du temps perdu à cause des erreurs (temps de correction)
            double errorCorrectionTime = manualTypingTime * _errorRate;

            // Temps total de frappe
            double totalManualTime = manualTypingTime + errorCorrectionTime;

            // Temps gagné par utilisation du raccourci
            double timeSavedPerUse = totalManualTime - _shortcutExecutionTime;

            // Temps économisé total
            double totalTimeSaved = timeSavedPerUse * frequency;

            return TimeSpan.FromSeconds(totalTimeSaved);
        }

        public static List<ShortcutModel> ConvertToShortcutTimerModels(ObservableCollection<ShortcutModel> shortcuts)
        {
            return shortcuts.Select(s => new ShortcutModel
            {
                Trigger = s.Trigger,
                Replacement = s.Replacement,
                //TimeSaved = CalculateTimeSaved(s.Replacement, _frequency)
            }).ToList();
        }

        public static TimeSpan ConvertToShortcutTimerString(string? remplacement)
        {
            return CalculateTimeSaved(remplacement, _frequency);
        }
    }

}
