using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchExample.Persistence.Interceptors
{
    public class CommandLoggingInterceptor : DbCommandInterceptor
    {
        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var data = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
                stopwatch.Stop();
                Console.WriteLine($"[SQL] ✅ {command.CommandText} ({stopwatch.ElapsedMilliseconds}ms)");
                return data;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"[SQL] ❌ ERROR in query: {command.CommandText}\n{ex.Message}");
                throw;
            }
        }
    }
}