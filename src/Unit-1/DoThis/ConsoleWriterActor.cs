using System;
using Akka;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    ///     Actor responsible for serializing message writes to the console.
    ///     (write one message at a time, champ :)
    /// </summary>
    internal class ConsoleWriterActor : UntypedActor
    {
        // in ConsoleWriterActor.cs
        protected override void OnReceive(object message)
        {
            var handled = message.Match()
                .With<Messages.InputError>(msg =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg.Reason);
                })
                .With<Messages.InputSuccess>(msg =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(msg.Reason);
                })
                .With<string>(
                    msg => Console.WriteLine(
                        msg)).WasHandled;

            if (!handled)
            {
                throw new InvalidMessageException();
            }

            Console.ResetColor();
        }
    }
}