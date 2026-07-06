namespace MyFirstWebAPI
{
    public class AttachmentOptions
    { 
            public string AllowedExtensions { get; set; }
            public int MaxSizeInMB { get; set; }
            public bool EnableCompression { get; set; }
      
    }
}
