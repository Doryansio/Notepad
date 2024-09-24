using System.Collections.Generic;


namespace Notepad.Services
{
    public static class ExtensionsMethods
    {
        public static void Replace<T>(this List<T> list, T oldItem, T newItem)
        {
            var oldItemIndex = list.IndexOf(oldItem);
            list[oldItemIndex] = newItem;
        }

        public static void Replace<String>(string oldString, string NewString)
        {
            NewString = string.Empty;
            oldString = NewString;
        }

    }
}
