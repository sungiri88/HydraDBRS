using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGarb.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyHeader { get; set; }
        public string User { get; set; }
    }

    public class RulesInfo
    {
        public string AssetClass { get; set; }
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleCondition { get; set; }
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public bool IsAutoElementName { get; set; }
        public bool IsPreviousYear { get; set; }
        public string PreviousYearColumns { get; set; }
        public string CompanyHeader { get; set; }
        public bool IsGenericRuleInherited { get; set; }
    }
}
