using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using AssetsHandler.Models;

namespace WpfApp1.Extensions
{
    public static class WindowExtensions
    {
        public static T FindVisualChild<T>(this Window window, string name) where T : DependencyObject
        {
            return FindVisualChild<T>((DependencyObject)window, name);
        }

        public static T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && (element as FrameworkElement)?.Name == name)
                {
                    return element;
                }
                else
                {
                    T result = FindVisualChild<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public static DataTemplate GetDataTemplate(this Window window,int index)
        {
            switch (index)
            {
                case 0:
                    return window.Resources["Cash"] as DataTemplate;
                case 1:
                    return window.Resources["Bank"] as DataTemplate;
                case 2:
                    return window.Resources["Different"] as DataTemplate;
                case 3:
                    return window.Resources["RealEstate"] as DataTemplate;
                case 4:
                    return window.Resources["Inventory"] as DataTemplate;
                default:
                    return null;
            }
        }
        public static DataTemplate GetDataTemplate(this Window window, Asset asset)
        {
            switch (asset.GetType().Name)
            {
                case "Money":
                    return window.Resources["Cash"] as DataTemplate;
                case "BankMoney":
                    return window.Resources["Bank"] as DataTemplate;
                case "DifferentMoney":
                    return window.Resources["Different"] as DataTemplate;
                case "RealEstate":
                    return window.Resources["RealEstate"] as DataTemplate;
                case "Indentory":
                    return window.Resources["Inventory"] as DataTemplate;
                default:
                    return null;
            }
        }


    }

}

