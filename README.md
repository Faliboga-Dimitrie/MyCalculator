Project Structure

This project consists of three classes and a .xaml file:

    MainWindow.xaml and MainWindow.xaml.cs were automatically generated as empty files by the Visual Studio IDE.

Class Descriptions

    MainWindow.xaml.cs – Responsible for handling all events, whether triggered by the UI or keyboard input.

    CalculatorEngine – Manages user input, handles the calculator's memory, and evaluates equations using Polish postfix notation.

    CalculatorManager – Stores global settings, manages certain UI changes, and creates a CalculatorEngine object.

    MainWindow.xaml – Defines the project's UI, with data binding set to CalculatorManager.
