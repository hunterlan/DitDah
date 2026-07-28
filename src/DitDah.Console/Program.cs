using System.Reactive.Disposables;
using System.Reactive.Linq;

const ConsoleKey dotKey = ConsoleKey.J;
const ConsoleKey dashKey = ConsoleKey.K;

var letterPause = TimeSpan.FromMilliseconds(600);
var wordPause = TimeSpan.FromMilliseconds(1500);

Console.WriteLine("Welcome to DitDah! Quick manual");
Console.WriteLine(
    "Press key \"j\" for dot, press \"k\" for dash, press \"Enter\" to submit."
);

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
    .Where(c => c is dotKey or dashKey)
    .Select(c => c == dotKey ? '.' : '-');

var letters = morseSymbols
    .Buffer(() => inputs.Throttle(letterPause))
    .Where(b => b.Count > 0)
    .Select(b => new string(b.ToArray()));

var words = morseSymbols
    .Throttle(wordPause)
    .Select(_ => " ");

var output = letters.Merge(words);

output.Subscribe(
    onNext: Console.Write,
    onCompleted: () => Console.WriteLine("\nGot your message!")
);

//TODO: Sound for morse.