﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMR.Randomizer.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class asm {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal asm() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MMR.Randomizer.Resources.asm", typeof(asm).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] rom_patch {
            get {
                object obj = ResourceManager.GetObject("rom_patch", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///    &quot;DPAD_CONFIG&quot;: &quot;03809D58&quot;,
        ///    &quot;DPAD_TEXTURE&quot;: &quot;0380BE90&quot;,
        ///    &quot;EXT_MSG_DATA_FILE&quot;: &quot;0380A568&quot;,
        ///    &quot;EXT_MSG_TABLE&quot;: &quot;0380AE88&quot;,
        ///    &quot;EXT_MSG_TABLE_COUNT&quot;: &quot;0380A530&quot;,
        ///    &quot;EXT_OBJECTS&quot;: &quot;0380A588&quot;,
        ///    &quot;FONT_TEXTURE&quot;: &quot;0380CE90&quot;,
        ///    &quot;G_C_HEAP&quot;: &quot;03820000&quot;,
        ///    &quot;G_PAYLOAD_ADDR&quot;: &quot;03800000&quot;,
        ///    &quot;HASH_ICONS&quot;: &quot;03809DE4&quot;,
        ///    &quot;HUD_COLOR_CONFIG&quot;: &quot;03809EB8&quot;,
        ///    &quot;ITEM_OVERRIDE_COUNT&quot;: &quot;0380A4EC&quot;,
        ///    &quot;ITEM_OVERRIDE_ENTRIES&quot;: &quot;0380A608&quot;,
        ///    &quot;MISC_CONFIG&quot;: &quot;0380A2FC&quot;,
        ///    &quot;MMR_CONFIG&quot;: &quot;0380A320&quot;,
        ///    &quot;P [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string symbols {
            get {
                return ResourceManager.GetString("symbols", resourceCulture);
            }
        }
    }
}