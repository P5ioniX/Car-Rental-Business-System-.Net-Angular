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
    
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.Rentals = new HashSet<Rental>();
        }
    
        public int Vehicle_ID { get; set; }
        public int Branch_ID { get; set; }
        public int Vehicle_Type_ID { get; set; }
        public int Mileage { get; set; }
        public byte[] Picture { get; set; }
        public bool Rentable { get; set; }
        public bool Available { get; set; }
    
        public virtual Branch Branch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual Vehicle_Types Vehicle_Types { get; set; }
    }
}