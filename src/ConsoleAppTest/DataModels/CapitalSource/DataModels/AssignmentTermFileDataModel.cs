using System;

namespace ConsoleAppTest.DataModels.CapitalSource.DataModels
{
    public class AssignmentTermFileDataModel
    {
        public Guid TemplateId { get; set; }
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public string UploadedByUserName { get; set; }
        public string UploadedByUserEmail { get; set; }
    }
}