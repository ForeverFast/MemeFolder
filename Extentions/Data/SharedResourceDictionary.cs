using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemeFolder.Extentions.Data
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        /// <summary>
        /// Internal cache of loaded dictionaries 
        /// </summary>
        public static Dictionary<Uri, ResourceDictionary> SharedDictinaries = new Dictionary<Uri, ResourceDictionary>();

        /// <summary>
        /// Local member of the source uri
        /// </summary>
        private Uri _sourceUri;

        private static bool IsInDesignMode
        {
            get
            {
                return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
                                                                       typeof(DependencyObject)).Metadata.DefaultValue;
            }
        }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get { return _sourceUri; }
            set
            {
                _sourceUri = value;
                if (!SharedDictinaries.ContainsKey(value))
                {
                    try
                    {
                        //If the dictionary is not yet loaded, load it by setting
                        //the source of the base class
                        base.Source = value;
                    }
                    catch (Exception exp)
                    {
                        Debug.WriteLine(exp.Message);
                        //only throw exception @runtime to avoid "Exception has been 
                        //thrown by the target of an invocation."-Error@DesignTime
                        if (!IsInDesignMode)
                            throw;
                    }
                    // add it to the cache
                    SharedDictinaries.Add(value, this);
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache 
                    MergedDictionaries.Add(SharedDictinaries[value]);
                }
            }
        }
    }
}
