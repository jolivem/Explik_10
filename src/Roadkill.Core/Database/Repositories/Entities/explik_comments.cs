//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Roadkill.Core.Database.Repositories.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class explik_comments
    {
        public int Id { get; set; }
        public Nullable<int> PageId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> Rating { get; set; }
        public string ControlledBy { get; set; }
        public bool IsControlled { get; set; }
        public bool IsRejected { get; set; }
        public string Text { get; set; }
    }
}
