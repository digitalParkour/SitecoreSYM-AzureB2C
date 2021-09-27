using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Sitecore.FakeDb.AutoFixture;

namespace SYMB2C.Foundation.Testing.Attributes
{
    public class AutoDbDataAttribute : AutoDataAttribute
    {
        public AutoDbDataAttribute()
        {
            this.Fixture.Customize(new AutoDbCustomization());
            this.Fixture.Customize(new AutoNSubstituteCustomization());
        }
    }
}
