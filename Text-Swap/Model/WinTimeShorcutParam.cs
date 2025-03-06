namespace Text_Swap.Services
{
    public class WinTimeShorcutParam
    {
        public double ErrorRate { get; set; } // Pourcentage d'erreurs
        public  double ShortcutExecutionTime { get; set; } // Temps d'utilisation du raccourci (s)
        public  double ThinkingTime { get; set; } // Temps de réflexion moyen (s)
        public  double TypingSpeed { get; set; } // Caractères par seconde
    }
}