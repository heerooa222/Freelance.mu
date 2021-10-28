using System;
using System.Collections.Generic;

namespace Services.Freelancer.Models
{
    public class ProjectModel
    {
        public string IdProject { get; set; }
        public string AddedBy { get; set; }
        public UserModel AddedByUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionEntreprise { get; set; }
        public string DescriptionOffre { get; set; }
        public DateTime? Date { get; set; }
        public DateTime ApplyBefore { get; set; }
        public decimal? Price { get; set; }
        public bool IsOpen { get; set; }
        public string TimeString
        {
            get
            {
                if (Date == null) return "";
                var time = "";
                var timeSpan = (TimeSpan)(DateTime.Now - Date);
                if (timeSpan.Days >= 365) time = $"{timeSpan.Days / 365} years ago";
                else if (timeSpan.Days >= 30) time = $"{timeSpan.Days / 30} months ago";
                else if (timeSpan.Days < 30 && timeSpan.Days >= 1) time = $"{timeSpan.Days} days ago";
                else if (timeSpan.Hours <= 24 && timeSpan.Hours >= 1) time = $"{timeSpan.Hours} hours ago";
                else if (timeSpan.Minutes <= 60 && timeSpan.Minutes >= 1) time = $"{timeSpan.Minutes} mins ago";
                else if (timeSpan.Seconds <= 60 && timeSpan.Seconds >= 1) time = $"{timeSpan.Seconds} secs ago";
                else time = "Now";
                return time;
            }
        }

        public List<CompetenceModel> CompetenceRequis { get; set; } = new List<CompetenceModel>();
        public string SelectedCompetence { get; set; }
        public bool AlreadyApplied { get; set; } = false;
        public string MyStatus { get; set; }
        public int NombreCandidatures { get; set; }
    }
}