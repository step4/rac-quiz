using System;
public interface ICommand
{
    bool CanExecute(object parameter);
    void Execute(object parameter);
}
