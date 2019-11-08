using System;
using System.Windows.Input;

namespace StarWarsExplorer.Utilities
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public RelayCommand(Action execute)
			: this((p) => execute(), () => true) { }

		public RelayCommand(Action execute, Func<bool> canExecute)
			: this((p) => execute(), (b) => canExecute()) { }

		public RelayCommand(Action execute, Predicate<object> canExecute)
			: this((p) => execute(), canExecute) { }

		public RelayCommand(Action<object> execute, Func<bool> predicate)
			: this(execute, (p) => predicate()) { }

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public void Execute()
		{
			_execute(null);
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}
	}
}