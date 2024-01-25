using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgHintPlan
{
    internal static class TagHelper
    {
        internal static string GenerateTag(string tag)
        {
            return $"PgHintPlan::{tag}";
        }

        internal static string GenerateTag(string tag, params string[] args)
        {
            var sb = new StringBuilder();

            sb.Append(tag);
            sb.Append("(");

            for (int i = 0; i < args.Length; i++)
            {
                sb.Append(args[i]);

                if (i < args.Length - 1)
                {
                    sb.Append(" ");
                }
            }

            sb.Append(")");

            return GenerateTag(sb.ToString());
        }

        internal static string GenerateTag(string property, bool value)
        {
            return GenerateTag("Set", property, value ? "on" : "off");
        }
    }
}
