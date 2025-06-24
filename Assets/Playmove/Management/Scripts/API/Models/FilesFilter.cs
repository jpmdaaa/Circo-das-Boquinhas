using System;

namespace Playmove.Management.API.Models
{
    [Serializable]
    public class FilesFilter
    {
        public long? ApplicationId { get; set; }
        public long? HistoricId { get; set; }
        public long? ClassroomId { get; set; }
        public long? PlayerId { get; set; }
        public long? GroupingId { get; set; }
    }
}