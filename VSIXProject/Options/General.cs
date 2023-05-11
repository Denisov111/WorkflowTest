using System.ComponentModel;
using System.Runtime.InteropServices;
using VSIXProject.Enums;

namespace VSIXProject
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "VSIXProject", "General", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        [Category("My category")]
        [DisplayName("My Option")]
        [Description("An informative description.")]
        [DefaultValue(true)]
        public bool MyOption { get; set; } = true;

        [Category("My category")]
        [DisplayName("My Enum")]
        [Description("Select the value you want from the list.")]
        [DefaultValue(Numbers.First)]
        [TypeConverter(typeof(EnumConverter))]
        public Numbers MyEnum { get; set; } = Numbers.First;
    }
}
