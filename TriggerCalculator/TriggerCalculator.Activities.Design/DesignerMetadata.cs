using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using TriggerCalculator.Activities.Design.Designers;
using TriggerCalculator.Activities.Design.Properties;

namespace TriggerCalculator.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TriggerCalculator), categoryAttribute);
            builder.AddCustomAttributes(typeof(TriggerCalculator), new DesignerAttribute(typeof(TriggerCalculatorDesigner)));
            builder.AddCustomAttributes(typeof(TriggerCalculator), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
