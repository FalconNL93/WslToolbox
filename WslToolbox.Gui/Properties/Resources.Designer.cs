//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WslToolbox.Gui.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WslToolbox.Gui.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to No.
        /// </summary>
        internal static string BOOL_NO {
            get {
                return ResourceManager.GetString("BOOL_NO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        internal static string BOOL_YES {
            get {
                return ResourceManager.GetString("BOOL_YES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string ERROR {
            get {
                return ResourceManager.GetString("ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only use alphanumeric characters and must contain 3 characters or more. Spaces are not allowed..
        /// </summary>
        internal static string ERROR_ONLY_ALPHANUMERIC {
            get {
                return ResourceManager.GetString("ERROR_ONLY_ALPHANUMERIC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not finish update.
        /// </summary>
        internal static string ERROR_UPDATE_GENERIC {
            get {
                return ResourceManager.GetString("ERROR_UPDATE_GENERIC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notice.
        /// </summary>
        internal static string NOTICE {
            get {
                return ResourceManager.GetString("NOTICE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Due to current restrictions in WSL CLI, installing an existing distribution is not possible. You can export an existing distro and import it back with a different name..
        /// </summary>
        internal static string NOTICE_INSTALL_EXISTING_DISTRIBUTION {
            get {
                return ResourceManager.GetString("NOTICE_INSTALL_EXISTING_DISTRIBUTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your current operating system build {0} is not recommended by {1}..
        /// </summary>
        internal static string OS_MINIMUM {
            get {
                return ResourceManager.GetString("OS_MINIMUM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Build {0} or higher is recommended..
        /// </summary>
        internal static string OS_MINIMUM_BUILD {
            get {
                return ResourceManager.GetString("OS_MINIMUM_BUILD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your current operating system build {0} is not supported by {1}..
        /// </summary>
        internal static string OS_NOT_SUPPORTED {
            get {
                return ResourceManager.GetString("OS_NOT_SUPPORTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Build {0} or higher is required..
        /// </summary>
        internal static string OS_NOT_SUPPORTED_BUILD_REQUIRED {
            get {
                return ResourceManager.GetString("OS_NOT_SUPPORTED_BUILD_REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string WARNING {
            get {
                return ResourceManager.GetString("WARNING", resourceCulture);
            }
        }
    }
}
