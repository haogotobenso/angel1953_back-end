using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Scase
{
    public int ScaseId{get;set;}
    public string PostUrl{get;set;}
    public byte Source{get;set;}
    public string Info{get;set;}
    public byte[]? SCimg{get;set;}
    public string? Account{get;set;}
    public DateTime Date{get;set;}
    public byte State{get;set;}
}
