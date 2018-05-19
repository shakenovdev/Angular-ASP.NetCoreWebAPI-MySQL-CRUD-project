using DataAccessLayer;
using System;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public static class Process
    {
        public static ProcessResult Run(Action action)
        {
            try
            {
                action();
                return ProcessResult.Ok();
            } 
            catch(Exception ex)
            {
                return ProcessResult.Fail(ex.Message);
            }
        }

        public static async Task<ProcessResult> RunAsync(Func<Task> action)
        {
            try
            {
                await action();
                return ProcessResult.Ok();
            } 
            catch(Exception ex)
            {
                return ProcessResult.Fail(ex.Message);
            }
        }

        public static ProcessResult<T> Run<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return ProcessResult<T>.Ok(result);
            }
            catch (Exception ex)
            {
                return ProcessResult<T>.Fail(ex.Message);
            }
        }

        public static async Task<ProcessResult<T>> RunAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return ProcessResult<T>.Ok(result);
            }
            catch (Exception ex)
            {
                return ProcessResult<T>.Fail(ex.Message);
            }
        }

        public static async Task<ProcessResult> RunInTransactionAsync(Func<Task> action, ApplicationDbContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    await action();
                    transaction.Commit();
                    return ProcessResult.Ok();
                } 
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return ProcessResult.Fail(ex.Message);
                }
            }
        }
        
    }
}