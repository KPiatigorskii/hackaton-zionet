using System;

namespace ZionetCompetition.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomAttribute : Attribute
    {
        public string Label { get; set; }
        public bool Required { get; set; }

        public CustomAttribute(string label, bool required)
        {
            Label = label;
            Required = required;
        }
    }

}
