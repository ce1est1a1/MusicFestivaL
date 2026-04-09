using System.Collections.Generic;
using System.Xml.Serialization;

namespace MusicFestivalDeserializer
{
    [XmlRoot("FestivalData")]
    public class FestivalData
    {
        public FestivalData()
        {
            Festival = new Festival();
            Artists = new List<Artist>();
            Stages = new List<StageInfo>();
        }

        public Festival Festival { get; set; }

        [XmlArray("Artists")]
        [XmlArrayItem("Artist")]
        public List<Artist> Artists { get; set; }

        [XmlArray("Stages")]
        [XmlArrayItem("Stage")]
        public List<StageInfo> Stages { get; set; }
    }

    public class Festival
    {
        public Festival()
        {
            Venue = new VenueInfo();
            Contacts = new ContactsInfo();
            Schedule = new ScheduleInfo();
        }

        public string Name { get; set; }
        public string Theme { get; set; }
        public string Duration { get; set; }
        public decimal TicketPriceFrom { get; set; }
        public int ExpectedGuests { get; set; }
        public VenueInfo Venue { get; set; }
        public ContactsInfo Contacts { get; set; }
        public ScheduleInfo Schedule { get; set; }
    }

    public class Artist
    {
        public Artist()
        {
            Performance = new PerformanceInfo();
            ArtistStats = new ArtistStats();
        }

        public string StageName { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public bool Headliner { get; set; }
        public string PerformanceDay { get; set; }
        public PerformanceInfo Performance { get; set; }
        public ArtistStats ArtistStats { get; set; }
    }

    public class StageInfo
    {
        public StageInfo()
        {
            WorkTime = new StageWorkTime();
            Manager = new StageManager();
            Decor = new StageDecor();
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsOpenAir { get; set; }
        public string SoundSystem { get; set; }
        public string AccessLevel { get; set; }
        public StageWorkTime WorkTime { get; set; }
        public StageManager Manager { get; set; }
        public StageDecor Decor { get; set; }
    }

    public class VenueInfo
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string VenueType { get; set; }
    }

    public class ContactsInfo
    {
        public string Organizer { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }

    public class ScheduleInfo
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EntryStart { get; set; }
        public string AfterpartyStart { get; set; }
    }

    public class PerformanceInfo
    {
        public string Stage { get; set; }
        public string Time { get; set; }
        public int DurationMinutes { get; set; }
        public string SpecialSet { get; set; }
    }

    public class ArtistStats
    {
        public int FoundedYear { get; set; }
        public int StudioAlbums { get; set; }
        public string TopRelease { get; set; }
        public string Status { get; set; }
    }

    public class StageWorkTime
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Soundcheck { get; set; }
        public string BreakTime { get; set; }
    }

    public class StageManager
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Shift { get; set; }
    }

    public class StageDecor
    {
        public string LedScreens { get; set; }
        public string LaserShow { get; set; }
        public string SmokeMachines { get; set; }
        public string MainColor { get; set; }
    }
}
