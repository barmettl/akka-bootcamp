using System;

namespace WinTail
{

    public static class MessagesExtensions
    {
        public static bool Match<T>(object message,
            Action<Messages.ContinueProcessing> onContinue,
            Action<Messages.InputError> onInputError,
            Action<Messages.InputSuccess> onInputSuccess
            ) where T : Messages
        {
            if (message is Messages.ContinueProcessing)
            {
                onContinue((Messages.ContinueProcessing) message);
            }
            if (message is Messages.InputError)
            {
                onInputError((Messages.InputError)message);
            }
            if (message is Messages.InputSuccess)
            {
                onInputSuccess((Messages.InputSuccess)message);
            }
            return false;
        }
       
    }



    public class Messages
    {
        // in Messages.cs

        #region Neutral/system messages

        /// <summary>
        ///     Marker class to continue processing.
        /// </summary>
        public class ContinueProcessing
        {
        }

        #endregion

        #region Success messages

        // in Messages.cs
        /// <summary>
        ///     Base class for signalling that user input was valid.
        /// </summary>
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }

        #endregion

        #region Error messages

        /// <summary>
        ///     Base class for signalling that user input was invalid.
        /// </summary>
        public class InputError
        {
            public InputError(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }

        /// <summary>
        ///     User provided blank input.
        /// </summary>
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason)
            {
            }
        }

        /// <summary>
        ///     User provided invalid input (currently, input w/ odd # chars)
        /// </summary>
        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason)
            {
            }
        }

        #endregion
    }
}