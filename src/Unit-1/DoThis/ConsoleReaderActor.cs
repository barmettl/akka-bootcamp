using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    ///     Actor responsible for reading FROM the console.
    ///     Also responsible for calling <see cref="ActorSystem.Shutdown" />.
    /// </summary>
    internal class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        // in ConsoleReaderActor
        public const string StartCommand = "start";
        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

        // in ConsoleReaderActor
        protected override void OnReceive(object message)
        {


            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError)
            {
                _validationActor.Tell(message as Messages.InputError);
            }

            GetAndValidateInput();
        }

        #region Internal methods

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }


        /// <summary>
        ///     Reads input from console, validates it, then signals appropriate response
        ///     (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) &&
                String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // if user typed ExitCommand, shut down the entire actor
                // system (allows the process to exit)
                Context.System.Shutdown();
                return;
            }

            // otherwise, just hand message off to validation actor
            // (by telling its actor ref)
            _validationActor.Tell(message);
        }

        /// <summary>
        ///     Validates <see cref="message" />.
        ///     Currently says messages are valid if contain even number of characters.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool IsValid(string message)
        {
            var valid = message.Length%2 == 0;
            return valid;
        }

        #endregion
    }
}