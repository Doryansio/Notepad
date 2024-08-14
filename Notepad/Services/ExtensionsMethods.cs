﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.Services
{
    public static class ExtensionsMethods
    {
        public static void Replace<T>(this List<T> list, T oldItem,  T newItem)
        {
            var oldItemIndex = list.IndexOf(oldItem);
            list[oldItemIndex] = newItem;
        }
    }
}
