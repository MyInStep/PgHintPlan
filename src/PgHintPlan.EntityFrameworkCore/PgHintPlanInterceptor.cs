using System;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PgHintPlan.EntityFrameworkCore
{
    public class PgHintPlanInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            ManipulateCommand(command);

            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            ManipulateCommand(command);

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        internal static void ManipulateCommand(DbCommand command)
        {
            var sbHints   = new StringBuilder();
            var sbCommand = new StringBuilder();

            sbHints.AppendLine("/*+");

            foreach (var line in command.CommandText.Split(command.CommandText.Contains("\r\n") ? "\r\n" : "\n", StringSplitOptions.TrimEntries))
            {
                if (line.StartsWith(Constants.TagPrefix))
                {
                    var hint = line.Split("::")[1];

                    var hintType = hint.Split("(")[0];

                    switch (hintType)
                    {
                        case "Set":
                        case "Rows":
                        case "Parallel":

                            break;

                        case JoinMethods.HashJoin:
                        case JoinMethods.NoHashJoin:
                        default:

                            var tables = hint.Split("(")[1].Replace(")", "").Split(" ");
                            foreach (var table in tables)
                            {
                                // get table alias
                                var tableAlias = Regex.Match(command.CommandText, $@"""{table}""\sAS\s(\w)").Groups[1].Value;

                                hint = hint.Replace(table, tableAlias);
                            }

                            break;
                    }

                    sbHints.AppendLine(hint);
                }
                else
                {
                    sbCommand.AppendLine(line);
                }
            }

         
            sbHints.AppendLine("*/");
            sbHints.AppendLine(sbCommand.ToString());

            command.CommandText = sbHints.ToString();
        }
    }
}
