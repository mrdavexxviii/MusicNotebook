using MusicNotebook.NotebookDefinitions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicNotebook;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new ViewModel();
        Loaded += (s, e) =>
        {
            if (DataContext is ViewModel vm)
            {
                vm.RequestEditPageName += OnRequestEditPageName;
            }
        };
        this.Closing += (s, e) =>
        {
            if (DataContext is ViewModel vm)
            {
                e.Cancel = !vm.CanOverwrite();
            }
        };
    }



    private void OnRequestEditPageName(INotebookPage page)
    {
        Task.Run(() => //Ensure small wait on non ui thread for tab to be created
        {
            Thread.Sleep(10);
            // Ensure UI selection happens on UI thread
            Dispatcher.BeginInvoke(() =>
            {
                ActivateEditorForItem(page);
               
            }, DispatcherPriority.Normal);
        });
    }

    private void ActivateEditorForItem(INotebookPage page)
    {
        // If TabItem is still null try again later (give layout a chance)
        if (Tabs.ItemContainerGenerator.ContainerFromItem(page) is not TabItem)
        {
            Dispatcher.BeginInvoke(() => ActivateEditorForItem(page), DispatcherPriority.Input);
            return;
        }

        // The header ContentPresenter hosts the ItemTemplate; search for it first
        var headerPresenter = FindContentPresenterByContent(Tabs, page);

        var searchRoot = FindTabHeader(Tabs, page);


        // Now toggle visibility + focus of the editor inside the header
        if (searchRoot != null)
        {
            ChangeVisibility(searchRoot, true);
        }
    }

    // Double-click the TextBlock: hide it, show the TextBox and set focus
    private void TabTitleLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement fe)
        {
            var container = FindAncestor<Grid>(fe);
            if (container != null)
            {
                ChangeVisibility(container, true);
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
                ChangeVisibility(container, false);
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
                    ChangeVisibility(container, false);
                }
            }
            e.Handled = true;
        }
    }

    static void ChangeVisibility(FrameworkElement container, bool editMode)
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

    // Find the ContentPresenter that hosts the header DataTemplate for the given item
    private static ContentPresenter? FindContentPresenterByContent(DependencyObject parent, object content)
    {
        if (parent == null) return null;
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is ContentPresenter cp && Equals(cp.Content, content))
                return cp;
            var found = FindContentPresenterByContent(child, content);
            if (found != null) return found;
        }
        return null;
    }

    private static FrameworkElement? FindTabHeader(DependencyObject parent, INotebookPage page)
    {
        if (parent == null) return null;
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is Grid fe && fe.DataContext == page && fe.Name == "TabTitleContainer")
                return fe;
            var result = FindTabHeader(child, page);
            if (result != null) return result;
        }
        return null;
    }

    private void Canvas_PreviewStylusDown(object sender, StylusDownEventArgs e)
    {
        if (sender is InkCanvas canvas)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Ctrl + Stylus tap: toggle eraser mode
                canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

        }
    }

    private void Canvas_PreviewStylusButtonDown(object sender, StylusButtonEventArgs e)
    {
        if (sender is InkCanvas canvas)
        {
            if (e.StylusButton.Name == "Barrel Switch")
            {
                canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;

            }
        }
    }

    private void Canvas_PreviewStylusButtonUp(object sender, StylusButtonEventArgs e)
    {
        if (sender is InkCanvas canvas)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }
    }

    private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is InkCanvas canvas)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Ctrl + Stylus tap: toggle eraser mode
                canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

        }
    }

    private void Canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is InkCanvas canvas)
        {
                canvas.EditingMode = InkCanvasEditingMode.Ink;
            

        }
    }

}