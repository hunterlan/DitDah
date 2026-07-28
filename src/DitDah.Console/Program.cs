using System.Reactive.Disposables;
using System.Reactive.Linq;

const ConsoleKey dotKey = ConsoleKey.J;
const ConsoleKey dashKey = ConsoleKey.K;

var letterPause = TimeSpan.FromMilliseconds(600);
var wordPause = TimeSpan.FromMilliseconds(1500);

// International Morse code table: A-Z and 0-9.
var morseTable = new Dictionary<string, char>
{
    [".-"] = 'A',
    ["-..."] = 'B',
    ["-.-."] = 'C',
    ["-.."] = 'D',
    ["."] = 'E',
    ["..-."] = 'F',
    ["--."] = 'G',
    ["...."] = 'H',
    [".."] = 'I',
    [".---"] = 'J',
    ["-.-"] = 'K',
    [".-.."] = 'L',
    ["--"] = 'M',
    ["-."] = 'N',
    ["---"] = 'O',
    [".--."] = 'P',
    ["--.-"] = 'Q',
    [".-."] = 'R',
    ["..."] = 'S',
    ["-"] = 'T',
    ["..-"] = 'U',
    ["...-"] = 'V',
    [".--"] = 'W',
    ["-..-"] = 'X',
    ["-.--"] = 'Y',
    ["--.."] = 'Z',
    ["-----"] = '0',
    [".----"] = '1',
    ["..---"] = '2',
    ["...--"] = '3',
    ["....-"] = '4',
    ["....."] = '5',
    ["-...."] = '6',
    ["--..."] = '7',
    ["---.."] = '8',
    ["----."] = '9',
};

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

// Third publisher and first subscriber of second publisher - listen to morse symbols, convert them to morse char array,
// then - to letters
var letters = morseSymbols
    .Buffer(() => inputs.Throttle(letterPause))
    .Where(b => b.Count > 0)
    .Select(MorseToLetter);

// Fourth publisher and second subscriber of second publisher - listen to morse symbols and publishes an empty
// whitespace string.
var words = morseSymbols
    .Throttle(wordPause)
    .Select(_ => ' ');

// Fifth publisher and subscriber of third and fourth publishers - listen to letters or whitespace and then aggregates
// via scan to string.
var output = letters.Merge(words).Scan(string.Empty, (acc, c) => acc + c);

// First publisher of fifth publisher - output result string and on finish outputs final messsage.
output.Subscribe(
    onNext: msg => { Console.Write("\r"); Console.Write(msg); },
    onCompleted: () => Console.WriteLine("\nGot your message!")
);

//TODO: Sound for morse.

char MorseToLetter(IList<char> morseChars)
{
    var sequence = new string(morseChars.ToArray());

    // Unknown sequences decode to '?' so a bad key doesn't kill the pipeline.
    return morseTable.GetValueOrDefault(sequence, '?');
}