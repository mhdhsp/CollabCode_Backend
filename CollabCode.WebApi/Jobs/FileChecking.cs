using Quartz;

namespace CollabCode.CollabCode.WebApi.Jobs
{
    public class FileChecking:IJob
    {
        public  Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("from job");
            return Task.CompletedTask;
        }
    }
}
