namespace InnoClinic.Documents.Infrastructure
{
    public class AwsSettings
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string PhotoDoctorPath { get; set; }
        public string PhotoReceptionistPath { get; set; }
        public string PhotoPatientPath { get; set; }
        public string PhotoOfficePath { get; set; }
    }
}