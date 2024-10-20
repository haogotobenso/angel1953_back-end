using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class UserInfoDetailDto
    {
        public string Name {get;set;}

        public string Account {get;set;}

        public string Email {get;set;}

        public string School {get;set;}
        
        public string Class {get;set;}

        public string StudentId{get;set;}

        public string FBurl{get;set;}

        public string State{get;set;}

        public int BullyingerPoint{get;set;}

        public int BullyingerPost{get;set;}

        public List<TodoBackDto>? Todo{get;set;}
        
    }
    public class TodoBackDto()
    {
            public string TodoName{get;set;}
            public string TodoState{get;set;}
    }
    
}