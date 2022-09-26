using System;

namespace FileUploadApp.Domain;

public class User : IHaveId<int>
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Username { get; set; }

    public string Passwhash { get; set; }
}