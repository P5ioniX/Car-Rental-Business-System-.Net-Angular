//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace rentalBackEnd_Web_API.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vehicle_Makes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle_Makes()
        {
            this.Vehicle_Models = new HashSet<Vehicle_Models>();
            this.Vehicle_Types = new HashSet<Vehicle_Types>();
        }
    
        public int Make_ID { get; set; }
        public string Make_Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle_Models> Vehicle_Models { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle_Types> Vehicle_Types { get; set; }
    }
}
