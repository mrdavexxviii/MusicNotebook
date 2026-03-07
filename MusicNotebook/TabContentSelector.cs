using MusicNotebook.NotebookDefinitions;
using System.Windows;
using System.Windows.Controls;

namespace MusicNotebook;

class TabContentSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (container is FrameworkElement fe)
        {
            if (item is ImagePage)
            {
                return (DataTemplate)fe.FindResource("ImagePageTemplate");
            }
            if (item is TextPage)
            {
                return (DataTemplate)fe.FindResource("TextPageTemplate");
            }
            if (item is TitlePage)
            {                
                return (DataTemplate)fe.FindResource("TitlePageTemplate");
            }
        }
        return base.SelectTemplate(item, container);
    }
}
