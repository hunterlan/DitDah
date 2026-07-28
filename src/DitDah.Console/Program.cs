using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

const ConsoleKey dotKey = ConsoleKey.J;
const ConsoleKey dashKey = ConsoleKey.K;

Console.WriteLine("Welcome to DitDah! Quick manual");
Console.WriteLine(
    "Press key \"j\" for dot, press \"k\" for dash, press \"spacebar\" for space, press \"Enter\" to submit.");

var inputs = Observable.Create<ConsoleKey>((observer) =>
    {
        var cts = new CancellationTokenSource();

        var keyboardReader = new Thread(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    var input = Console.ReadKey(intercept: true);

                    if (input.Key == ConsoleKey.Enter)
                    {
                        observer.OnCompleted();
                        break;
                    }

                    observer.OnNext(input.Key);
                }
            }
        );

        keyboardReader.Start();

        return Disposable.Create(cts.Cancel);
    }
).Publish().RefCount();

var morseSymbols = inputs
    .Where(c => c is dotKey or dashKey or ConsoleKey.Spacebar)
    .Select(c => c switch
        {
            dotKey => '.',
            dashKey => '-',
            _ => ' '
        }
    );

var sb = new StringBuilder();
morseSymbols.Subscribe(onNext: c => sb.Append(c));

morseSymbols.Subscribe(
    onNext: Console.Write,
    onCompleted: () => Console.WriteLine("\nGot your message!")
);

//TODO: Sound for morse.