using UnityEngine;

namespace UI
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] private HoverEventSender hoverableObject;

        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public string Text { get; set; }

        private void Start()
        {
            hoverableObject.PointerEntered += HoverableObjectOnPointerEntered;
        }

        private void HoverableObjectOnPointerEntered()
        {
            
        }
    }
}