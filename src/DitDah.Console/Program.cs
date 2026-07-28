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

// First publisher - publishes ALL keystrokes
var inputs = Observable.Create<ConsoleKey>((observer) =>
    {
        var cts = new CancellationTokenSource();

        // Works in background - doesn't block main thread
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
    // Publish.RefCount allows to share the same observable sequence among multiple subscribers, and output will be deterministic.
).Publish().RefCount();

// Second publisher - publishes only morse symbols from pressing "j" or "k" keys
var morseSymbols = inputs
    .Where(c => c is dotKey or dashKey)
    .Select(c => c == dotKey ? '.' : '-');

// Third publisher and first subscriber of second publisher - listen to morse symbols and convert them to morse letters
// (basically return new string)
var letters = morseSymbols
    .Buffer(() => inputs.Throttle(letterPause))
    .Where(b => b.Count > 0)
    .Select(b => new string(b.ToArray()));

// Fourth publisher and second subscriber of second publisher - listen to morse symbols and publishes an empty whitespace string.
var words = morseSymbols
    .Throttle(wordPause)
    .Select(_ => " ");

// Fifth publisher and subscriber of third and fourth publishers - listen to morse symbols and publishes an empty whitespace string.
var output = letters.Merge(words);

// First publisher of fifth publisher - output result string and on finish outputs final messsage.
output.Subscribe(
    onNext: Console.Write,
    onCompleted: () => Console.WriteLine("\nGot your message!")
);

//TODO: Sound for morse.