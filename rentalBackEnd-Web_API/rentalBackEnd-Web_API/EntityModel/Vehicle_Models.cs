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
    
    public partial class Vehicle_Models
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle_Models()
        {
            this.Vehicle_Types = new HashSet<Vehicle_Types>();
        }
    
        public int Model_ID { get; set; }
        public int Make_ID { get; set; }
        public string Model_Name { get; set; }
    
        public virtual Vehicle_Makes Vehicle_Makes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle_Types> Vehicle_Types { get; set; }
    }
}
