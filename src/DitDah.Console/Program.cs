using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

var inputs = Observable.Create<char>((observer) =>
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

                    observer.OnNext(input.KeyChar);
                }
            }
        );
        
        keyboardReader.Start();

        return Disposable.Create(cts.Cancel);
    }
).Publish().RefCount();

var sb = new StringBuilder();

inputs.Subscribe(
    onNext: c =>
    {
        sb.Append(c);
        Console.Write(c);
    },
    onCompleted: () => Console.WriteLine("\nGot your message!")
);

inputs.Subscribe(c => Console.Write($"B:{c} "));