using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pleer.Models
{
    public static class ErrorManager
    {
        public static void PlaybackError()
        {
            MessageBox.Show("Ошибка при попытке воспроизведения аудиофайла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
