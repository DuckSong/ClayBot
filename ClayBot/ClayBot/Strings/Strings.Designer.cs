﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClayBot.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClayBot.Strings.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to PVP.net Client.
        /// </summary>
        internal static string ClientText {
            get {
                return ResourceManager.GetString("ClientText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends (TM) Client.
        /// </summary>
        internal static string GameText {
            get {
                return ResourceManager.GetString("GameText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ESC: Quit, F12: Pause and show configuration menu.
        /// </summary>
        internal static string Instruction {
            get {
                return ResourceManager.GetString("Instruction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends directory.
        /// </summary>
        internal static string LolDirectoryLabel {
            get {
                return ResourceManager.GetString("LolDirectoryLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends directory is incorrect!.
        /// </summary>
        internal static string LolDirectoryNotFound {
            get {
                return ResourceManager.GetString("LolDirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends ID.
        /// </summary>
        internal static string LolIdLabel {
            get {
                return ResourceManager.GetString("LolIdLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your League of Legends ID is not set!.
        /// </summary>
        internal static string LolIdNull {
            get {
                return ResourceManager.GetString("LolIdNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends launcher is not found under specified directory! Please make sure League of Legends directory is set correctly!.
        /// </summary>
        internal static string LolLauncherNotFound {
            get {
                return ResourceManager.GetString("LolLauncherNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not recognize League of Legends locale! Please make sure League of Legends directory is set correctly!.
        /// </summary>
        internal static string LolLocaleNotFound {
            get {
                return ResourceManager.GetString("LolLocaleNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your locale ({0}) is not supported!.
        /// </summary>
        internal static string LolLocaleNotSupported {
            get {
                return ResourceManager.GetString("LolLocaleNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to League of Legends password.
        /// </summary>
        internal static string LolPasswordLabel {
            get {
                return ResourceManager.GetString("LolPasswordLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your League of Legends password is not set!.
        /// </summary>
        internal static string LolPasswordNull {
            get {
                return ResourceManager.GetString("LolPasswordNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LoL Patcher.
        /// </summary>
        internal static string PatcherText {
            get {
                return ResourceManager.GetString("PatcherText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to State: {0}, Retry: {1}.
        /// </summary>
        internal static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
        }
    }
}