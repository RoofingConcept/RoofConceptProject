using System;
using System.Collections.Generic;
using System.Text;

namespace RoofingConcept.Business.Results;

public class ServiceResult
{
 public bool Success { get; set; } 

    public string? Message { get; set; } = string.Empty;

    public string? Error { get; set; }
}
