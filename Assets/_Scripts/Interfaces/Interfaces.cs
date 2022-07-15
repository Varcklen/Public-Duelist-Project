using System.Threading.Tasks;

namespace Project.Interfaces
{
    /// <summary>
    /// Allows you to enable or disable elements using another element.
    /// </summary>
    internal interface IShowHide
    {
        public void Show();
        public void Hide();
    }

    internal interface IShowHideAsync
    {
        public Task Show();
        public Task Hide();
    }
}
