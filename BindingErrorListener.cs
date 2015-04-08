namespace WpfCommonLibrary
{
    #region Usings

    using Dependencies.Wpf;
    using FluentChecker;
    using System;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Listener for Wpf Binding Errors 
    /// </summary>
    public class BindingErrorListener : TraceListener, IBindingErrorListener
    {
        #region Properties

        private Action<string> logAction;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Set the input action as handler when the listeners is being notified. 
        /// </summary>
        /// <param name="action"> The action that will handle the event. </param>
        public void Listen(Action<string> action)
        {
            Check.IfIsNull(action).Throw<ArgumentNullException>(() => action);

            logAction = action;

            PresentationTraceSources.DataBindingSource.Listeners
                .Add(new BindingErrorListener {logAction = logAction});
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you
        /// create in the derived class.
        /// </summary>
        /// <param name="message"> A message to write. </param>
        public override void Write(string message)
        {
            logAction(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the
        /// derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message"> A message to write. </param>
        public override void WriteLine(string message)
        {
            logAction(message);
        }

        #endregion Methods
    }
}