using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UniOrm.Model
{
    public class FieldInfoDefinition: MemberDifinitions
    {
        
        public override  MemberTypes MemberType { get; set; }
       
        public bool IsFamily { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the potential visibility of this field is described
        //     by System.Reflection.FieldAttributes.Assembly; that is, the field is visible
        //     at most to other types in the same assembly, and is not visible to derived types
        //     outside the assembly.
        //
        // Returns:
        //     true if the visibility of this field is exactly described by System.Reflection.FieldAttributes.Assembly;
        //     otherwise, false.
        public bool IsAssembly { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the visibility of this field is described by
        //     System.Reflection.FieldAttributes.FamANDAssem; that is, the field can be accessed
        //     from derived classes, but only if they are in the same assembly.
        //
        // Returns:
        //     true if access to this field is exactly described by System.Reflection.FieldAttributes.FamANDAssem;
        //     otherwise, false.
        public bool IsFamilyAndAssembly { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the potential visibility of this field is described
        //     by System.Reflection.FieldAttributes.FamORAssem; that is, the field can be accessed
        //     by derived classes wherever they are, and by classes in the same assembly.
        //
        // Returns:
        //     true if access to this field is exactly described by System.Reflection.FieldAttributes.FamORAssem;
        //     otherwise, false.
        public bool IsFamilyOrAssembly { get; set; }
    
        //
        // Summary:
        //     Gets a value indicating whether the field can only be set in the body of the
        //     constructor.
        //
        // Returns:
        //     true if the field has the InitOnly attribute set; otherwise, false.
        public bool IsInitOnly { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the value is written at compile time and cannot
        //     be changed.
        //
        // Returns:
        //     true if the field has the Literal attribute set; otherwise, false.
        public bool IsLiteral { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field has the NotSerialized attribute.
        //
        // Returns:
        //     true if the field has the NotSerialized attribute set; otherwise, false.
        public bool IsNotSerialized { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the corresponding SpecialName attribute is set
        //     in the System.Reflection.FieldAttributes enumerator.
        //
        // Returns:
        //     true if the SpecialName attribute is set in System.Reflection.FieldAttributes;
        //     otherwise, false.
        public bool IsSpecialName { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the corresponding PinvokeImpl attribute is set
        //     in System.Reflection.FieldAttributes.
        //
        // Returns:
        //     true if the PinvokeImpl attribute is set in System.Reflection.FieldAttributes;
        //     otherwise, false.
        public bool IsPinvokeImpl { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the current field is security-critical or
        //     security-safe-critical at the current trust level.
        //
        // Returns:
        //     true if the current field is security-critical or security-safe-critical at the
        //     current trust level; false if it is transparent.
        public virtual bool IsSecurityCritical { get; set; }
        //
        // Summary:
        //     Gets the attributes associated with this field.
        //
        // Returns:
        //     The FieldAttributes for this field.
        public FieldAttributes Attributes { get; set; }
        //
        // Summary:
        //     Gets the type of this field object.
        //
        // Returns:
        //     The type of this field object.
        public string FieldType { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the current field is transparent at the current
        //     trust level.
        //
        // Returns:
        //     true if the field is security-transparent at the current trust level; otherwise,
        //     false.
        public virtual bool IsSecurityTransparent { get; set; }
       
        //
        // Summary:
        //     Gets a value that indicates whether the current field is security-safe-critical
        //     at the current trust level.
        //
        // Returns:
        //     true if the current field is security-safe-critical at the current trust level;
        //     false if it is security-critical or transparent.
        public virtual bool IsSecuritySafeCritical { get; set; }  
    }
}
