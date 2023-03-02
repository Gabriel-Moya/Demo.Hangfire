namespace HangfireProject.Jobs;

public class JobsHangfire
{
    public async Task FireAndForget()
    {
        await Task.Run(() =>
        {
            Console.WriteLine("Job Fire-and-forget!");
        });
    }

    public async Task Delayed()
    {
        await Task.Run(() =>
        {
            Console.WriteLine("Job delayed: Esse job irá rodar 1x após 1 minuto do start da aplicação");
        });
    }

    public async Task RecurringJobMethod()
    {
        var id = Guid.NewGuid().ToString("N").ToUpper()[..6];
        await Task.Run(() =>
        {
            Console.WriteLine($"{GetRandomString()} - A cada 1 minuto ele será executado");
        });
    }

    public async Task TimeConsumingMethod()
    {
        await Task.Run(async () =>
        {
            Console.WriteLine("Start método demorado");
            await Task.Delay(TimeSpan.FromMinutes(2));
            Console.WriteLine("Finish método demorado");
        });
    }

    public static string GetRandomString()
    {
        return Guid.NewGuid().ToString("N").ToUpper()[..6];
    }
}
