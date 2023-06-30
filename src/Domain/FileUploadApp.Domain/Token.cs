﻿using System.Collections.Generic;

namespace FileUploadApp.Domain;

public class Token : IHaveId<string>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long Expires { get; set; }
    public string Id { get; set; }
    public string Role { get; set; }
    public IDictionary<string, string> Claims { get; set; }
}