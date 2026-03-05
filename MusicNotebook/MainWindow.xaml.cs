using MusicNotebook.NotebookDefinitions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicNotebook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        // Double-click the TextBlock: hide it, show the TextBox and set focus
        private void TabTitleLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                var container = FindAncestor<Grid>(fe);
                if (container != null)
                {
                    ChangeVisibility(container,true);
                }
            }
        }

        // Commit on LostFocus
        private void TabTitleEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                var container = FindAncestor<Grid>(fe);
                if (container != null)
                {
                    ChangeVisibility(container,false);
                }
            }
        }

        // Commit on Enter key
        private void TabTitleEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Move focus away so LostFocus handler runs and binding updates
                var scope = FocusManager.GetFocusScope((DependencyObject)sender);
                FocusManager.SetFocusedElement(scope, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // Cancel edit: revert binding by reloading the label value (binding two-way updated on property change,
                // so to cancel you'd need an explicit rollback — here we simply end editing without changing focus behavior).
                if (sender is FrameworkElement fe)
                {
                    var container = FindAncestor<Grid>(fe);
                    if (container != null)
                    {
                        ChangeVisibility(container,false);
                    }
                }
                e.Handled = true;
            }
        }

        static void ChangeVisibility(Grid container, bool editMode)
        {
            var editor = FindChild<TextBox>(container, "TabTitleEditor");
            var label = FindChild<Label>(container, "TabTitleLabel");
            if (editor != null && label != null)
            {
                label.Visibility = editMode ? Visibility.Collapsed : Visibility.Visible;
                editor.Visibility = editMode ? Visibility.Visible : Visibility.Collapsed;
                if (editMode)
                {
                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        editor.Focus();
                        Keyboard.Focus(editor);
                        editor.SelectAll();
                    }, DispatcherPriority.Input);

                }
            }
        }

        // Utility: find ancestor of specific type
        private static T? FindAncestor<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject? current = child;
            while (current != null)
            {
                if (current is T t) return t;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        // Utility: recursive child search by name and type
        private static T? FindChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t && (string.IsNullOrEmpty(name) || (child as FrameworkElement)?.Name == name))
                    return t;
                var result = FindChild<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }
    }
}