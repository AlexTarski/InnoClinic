namespace InnoClinic.Documents.Business
{
    /// <summary>
    /// Specifies the type of file being uploaded (e.g., a photo of a person, an office, or other).
    /// The storage path is determined entirely by this value.
    public enum UploadFileType
    {
        PhotoDoctor,
        PhotoReceptionist,
        PhotoPatient,
        PhotoOffice
    }
}