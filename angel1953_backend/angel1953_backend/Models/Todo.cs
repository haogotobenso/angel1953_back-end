using System;
using System.Collections.Generic;

namespace angel1953_backend.Models
{
    public class Todo
    {
        public int TodoId {get;set;}
        public string Account {get;set;} = null!;
        public byte TodoThing {get;set;}
        public bool State {get;set;}


    }
}