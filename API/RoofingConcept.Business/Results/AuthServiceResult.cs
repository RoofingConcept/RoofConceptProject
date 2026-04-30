using System;
using System.Collections.Generic;
using System.Text;

namespace RoofingConcept.Business.Results;

public class AuthServiceResult<T> where T : ServiceResult
{
    public T? Result { get; set; }
}

public class AuthServiceResult : ServiceResult
{
  
}
