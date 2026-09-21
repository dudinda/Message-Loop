using MessageLoop.Common.Models.LongRun;
using MessageLoop.Web.Common.Services.Api;

namespace MessageLoop.Web.Common.Code.Extensions
{
    public static class LongRunApiExtensions
    {
        extension <T>(ILongRunApi<T> api)
            where T: LongRunItem, new() 
        {
            public async Task<LongRunResult> Poll(LongRunToken token, CancellationToken cncl)
            {
                var id = token.Id;
                var response = await api.GetLongRunStatus(id);
           
                while (response.Content.Status != TaskStatus.RanToCompletion &&
                       response.Content.Status != TaskStatus.Faulted && 
                       response.Content.Status != TaskStatus.Canceled)
                {
                    if (cncl.IsCancellationRequested)
                    {
                        await api.Abort(id);
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    response = await api.GetLongRunStatus(id);
                }

                if (response.Content.Status == TaskStatus.Faulted)
                {
                    throw new InvalidOperationException(response.Content.Exception);
                }

                cncl.ThrowIfCancellationRequested();
                return response.Content;
            }
        }
    }
}
