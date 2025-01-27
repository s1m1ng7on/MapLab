using System.Collections;

namespace MapLab.Web.Models.Shared
{
    public class MultiStepModalViewModel : IEnumerable<ModalViewModel>
    {
        private List<ModalViewModel>? _modals;

        public string Id { get; set; } = "modal" + Guid.NewGuid().ToString("N");

        public IEnumerator<ModalViewModel> GetEnumerator() => _modals.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
