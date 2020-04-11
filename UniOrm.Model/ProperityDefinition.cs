using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UniOrm.Model
{
    public class ProperityDefinition : MemberDifinitions
    {
       
        public override MemberTypes MemberType { get; set; }

        // Summary:
        //     Gets a value indicating whether the property can be written to.
        //
        // Returns:
        //     true if this property can be written to; otherwise, false.
        public bool CanWrite { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the property can be read.
        //
        // Returns:
        //     true if this property can be read; otherwise, false.
        public bool CanRead { get; set; }
        //
        // Summary:
        //     Gets the attributes for this property.
        //
        // Returns:
        //     The attributes of this property.
        public PropertyAttributes Attributes { get; set; }
        //
        // Summary:
        //     Gets the type of this property.
        //
        // Returns:
        //     The type of this property.
        public string ProperityType { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the property is the special name.
        //
        // Returns:
        //     true if this property is the special name; otherwise, false.
        public bool IsSpecialName { get; set; }
        public DateTime? Addtime { get; set; }
    }
}
