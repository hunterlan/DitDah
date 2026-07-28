using System.Reactive.Linq;

var ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period: TimeSpan.FromSeconds(1)
);

ticks.Subscribe(tick => Console.WriteLine($"Tick {tick}") );

Console.ReadKey();