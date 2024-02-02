using System.Data;
using System.Threading.Tasks;

using Npgsql;

namespace PgHintPlan
{
    public static class NpgsqlExtensions
    {
        /// <summary>
        /// Enable pg_hint_plan.
        /// </summary>
        /// <param name="connection">The <see cref="NpgsqlConnection"/> to enable pg_hint_plan on.</param>
        public static void EnablePgHintPlan(this NpgsqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var command = new NpgsqlCommand("SET pg_hint_plan.enable_hint=ON;", connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Enable pg_hint_plan.
        /// </summary>
        /// <param name="connection">The <see cref="NpgsqlConnection"/> to enable pg_hint_plan on.</param>
        public static async Task EnablePgHintPlanAsync(this NpgsqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            await using var command = new NpgsqlCommand("SET pg_hint_plan.enable_hint=ON;", connection);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Enable pg_hint_plan.
        /// </summary>
        /// <param name="dataSource">The <see cref="NpgsqlDataSource"/> to enable pg_hint_plan on.</param>
        public static void EnablePgHintPlan(this NpgsqlDataSource dataSource)
        {
            using var command = dataSource.CreateCommand("SET pg_hint_plan.enable_hint=ON;");
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Enable pg_hint_plan.
        /// </summary>
        /// <param name="dataSource">The <see cref="NpgsqlDataSource"/> to enable pg_hint_plan on.</param>
        public static async Task EnablePgHintPlanAsync(this NpgsqlDataSource dataSource)
        {
            await using var command = dataSource.CreateCommand("SET pg_hint_plan.enable_hint=ON;");
            await command.ExecuteNonQueryAsync();
        }
    }
}
