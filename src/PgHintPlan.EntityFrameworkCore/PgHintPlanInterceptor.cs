using System;
using System.Data.Common;
using System.Linq;
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
                    var hintParams = hint.Split("(")[1].Replace(")", "").Split(" ");

                    switch (hintType)
                    {
                        case "Set":

                            break;
                       
                        case ScanMethods.IndexScan:
                        case ScanMethods.IndexOnlyScan:
                        case ScanMethods.BitmapScan:

                            var onlytable = hintParams.FirstOrDefault();
                            var indexes = hintParams.Skip(1);
                           
                            // get table alias
                            var tabAlias = Regex.Match(command.CommandText, $@"""{onlytable}""\sAS\s(\w+)").Groups[1].Value;

                            var newSB =new StringBuilder();
                            newSB.Append(hintType);
                            newSB.Append("(");
                            newSB.Append(tabAlias);

                            foreach (var index in indexes)
                            {
                                newSB.Append(" ");
                                newSB.Append(tabAlias);
                                newSB.Append(index.TrimStart(onlytable.ToCharArray()));
                            }   

                            newSB.Append(")");
                            hint = newSB.ToString();
                            break;

                        case ScanMethods.IndexScanRegexp:
                        case ScanMethods.IndexOnlyScanRegexp:
                        case ScanMethods.BitmapScanRegexp:
                        case MiscMethods.Parallel:

                            var firsttable = hintParams.FirstOrDefault();
                            var tAlias = Regex.Match(command.CommandText, $@"""{firsttable}""\sAS\s(\w+)").Groups[1].Value;
                            hint = hint.Replace(firsttable, tAlias);

                            break;

                        case MiscMethods.Rows:

                            foreach (var table in hintParams.Take(..^1))
                            {
                                // get table alias
                                var tableAlias = Regex.Match(command.CommandText, $@"""{table}""\sAS\s(\w+)").Groups[1].Value;

                                hint = hint.Replace(table, tableAlias);
                            }

                            break;

                        default:

                            foreach (var table in hintParams)
                            {
                                // get table alias
                                var tableAlias = Regex.Match(command.CommandText, $@"""{table}""\sAS\s(\w+)").Groups[1].Value;

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
