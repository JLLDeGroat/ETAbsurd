//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ETAbsurdV1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ETA_FactionThread
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
        public string Faction { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public string AuthorAvatar { get; set; }
        public string Voters { get; set; }
        public int Votes { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public System.DateTime Date { get; set; }
        public int Comments { get; set; }
        public string Tags { get; set; }
        public int Views { get; set; }
        public int Posts { get; set; }
        public Nullable<System.DateTime> Edited { get; set; }
        public string Edit { get; set; }
        public Nullable<System.DateTime> DateOfEdit { get; set; }
        public Nullable<int> EditAmounts { get; set; }
        public Nullable<System.DateTime> LastPost { get; set; }
        public Nullable<int> LastPostId { get; set; }
    }
}
